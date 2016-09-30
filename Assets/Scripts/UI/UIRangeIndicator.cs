using UnityEngine;
using System.Collections;

public class UIRangeIndicator : MonoBehaviour 
{
    #region Generate Range Indicator
    public enum Axis { X, Y, Z };

    [SerializeField]
    [Tooltip("The number of lines that will be used to draw the circle. The more lines, the more the circle will be \"flexible\".")]
    [Range(0, 1000)]
    private int _segments = 60;

    [SerializeField]
    [Tooltip("The radius of the circle.")]
    private float _radius = 10;

    [SerializeField]
    [Tooltip("The offset will be applied in the direction of the axis.")]
    private float _offset = 0;

    [SerializeField]
    [Tooltip("The axis about which the circle is drawn.")]
    private Axis _axis = Axis.Z;

    [SerializeField]
    [Tooltip("If checked, the circle will be rendered again each time one of the parameters change.")]
    private bool _checkValuesChanged = true;

    private int _previousSegmentsValue;
    private float _previousHorizRadiusValue;
    private float _previousVertRadiusValue;
    private float _previousOffsetValue;
    private Axis _previousAxisValue;

    private LineRenderer _line;

    void Start()
    {
        _line = gameObject.GetComponent<LineRenderer>();

        _line.SetVertexCount(_segments + 1);
        _line.useWorldSpace = false;

        UpdateValuesChanged();

        CreatePoints();

        MotionStart();
    }

    void Update()
    {
        if (_checkValuesChanged)
        {
            if (_previousSegmentsValue != _segments ||
                _previousHorizRadiusValue != _radius ||
                _previousVertRadiusValue != _radius ||
                _previousOffsetValue != _offset ||
                _previousAxisValue != _axis)
            {
                CreatePoints();
            }

            UpdateValuesChanged();
        }
    }

    void UpdateValuesChanged()
    {
        _previousSegmentsValue = _segments;
        _previousHorizRadiusValue = _radius;
        _previousVertRadiusValue = _radius;
        _previousOffsetValue = _offset;
        _previousAxisValue = _axis;
    }

    void CreatePoints()
    {

        if (_previousSegmentsValue != _segments)
        {
            _line.SetVertexCount(_segments + 1);
        }

        float x;
        float y;
        float z = _offset;

        float angle = 0f;

        for (int i = 0; i < (_segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;

            switch (_axis)
            {
                case Axis.X:
                    _line.SetPosition(i, new Vector3(z, y, x));
                    break;
                case Axis.Y:
                    _line.SetPosition(i, new Vector3(y, z, x));
                    break;
                case Axis.Z:
                    _line.SetPosition(i, new Vector3(x, y, z));
                    break;
                default:
                    break;
            }

            angle += (360f / _segments);
        }
    }
    #endregion

    #region Motion
    private BasicTower m_BasicTower;
    private float targetRange;

    private Vector3 targetPosition;

    //开始时调用
    private void MotionStart()
    {
        targetPosition = new Vector3();
        _line.enabled = false;
    }

    private void FixedUpdate()
    {
        //距离大于0.01f时才进行移动，防止无限Lerp
        if ((targetPosition - transform.position).magnitude >= 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.25f);
        }

        //距离大于0.01f时才进行范围修改，防止无限Lerp
        if (Mathf.Abs(_radius-targetRange)>=0.01f)
        {
            _radius = Mathf.Lerp(_radius, targetRange, 0.25f);
        }
    }

    private void MoveTo(Vector3 m_PosTemp)
    {
        targetPosition.x = m_PosTemp.x;
        targetPosition.z = m_PosTemp.z;
        //如果移动的时候UI原本处于隐藏状态，那么不使用Lerp移动，直接移动到目标位置
        //切换游戏场景时UI才会被隐藏，一般处于半径为0的状态
        if(_line.enabled==false)
            transform.position = targetPosition;
    }


    public void ShowTowerRangeIndicator(GameObject m_GameObject)
    {
        if (m_GameObject == null)
            return;
        m_BasicTower = m_GameObject.GetComponent<Tower>().m_BasicTower;
        targetRange = m_BasicTower.GetTowerRange();
        MoveTo(m_GameObject.transform.position);
        Enable();
    }

    public void Enable()
    {
        _line.enabled = true;
    }

    public void Disable()
    {
        //再次Enable时，才会有动画效果
        //不隐藏UI，只把范围半径调成0
        //在地图上会有个小点，但平时会被地基所遮盖
        targetRange = 0;
        //_line.enabled = false;
    }

    //半径调成0在地图上还是会有个小点，实现隐藏函数供调用
    public void Invisible()
    {
        _line.enabled = false;
    }
    #endregion
}
