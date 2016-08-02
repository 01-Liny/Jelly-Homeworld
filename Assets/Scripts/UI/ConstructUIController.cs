using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConstructUIController : MonoBehaviour 
{
    public Text text;
    public float UIHeight;
    public MapManager m_MapManager;
    public TowerManager m_TowerManager;

    private Canvas m_Canvas;
    private Vector3 m_Pos;
    private GameObject m_Tower;
    private string state;

    [HideInInspector]public Vector2 m_MapPos;
    [HideInInspector]public Ray m_Ray;
    private void Awake()
    {
        m_Canvas=GetComponent<Canvas>();
        m_Pos=new Vector3();
        m_MapPos=new Vector2();
        m_Ray=new Ray();
    }

    private void Start()
    {
        m_Canvas.enabled=false;
        m_Pos.y=UIHeight;
    }
    
    //将UI移动至参数坐标
    public void MoveTo(Vector3 m_PosTemp)
    {
        m_Pos.x= m_PosTemp.x;
        m_Pos.z= m_PosTemp.z;
        transform.position=m_Pos;
    }

    //改变按钮内容
    public void ChangeButtonText(string content)
    {
        this.text.text=content;
    }

    public void ChangeState(string state)
    {
        this.state = state;
    }

    //更新地图坐标，参数来自上一次显示UI时的地图坐标
    public void UpdateMapPos(Vector2 m_MapPos)
    {
        this.m_MapPos.x=m_MapPos.x;
        this.m_MapPos.y=m_MapPos.y;
        Debug.Log("X:"+m_MapPos.x+" Y:"+m_MapPos.y);
    }

    //更新上一次显示UI时屏幕到鼠标的射线信息
    //由于DestroyStone函数作废，该函数也作废
    //但是该函数仍然在使用中
    public void UpdateCameraRay(Ray ray)
    {
        m_Ray.origin=ray.origin;
        m_Ray.direction=ray.direction;
    }

    //显示UI
    public void Enable()
    {
        //Debug.Log("Invoke ConstructUIController OnEnable");
        m_Canvas.enabled=true;
    }

    //隐藏UI
    public void Disable()
    {
        //Debug.Log("Invoke ConstructUIController OnDisable");
        m_Canvas.enabled=false;
    }

    public void SetTowerGameObject(GameObject m_Tower)
    {
        this.m_Tower = m_Tower;
    }

    public GameObject GetTowerGameObject()
    {
        return m_Tower;
    }

    public void OnClick()
    {
        //点击后，UI消失
        Disable();
        switch (state)
        {
            case "Tower":
                {
                    m_MapManager.ModifyMap((int)m_MapPos.x, (int)m_MapPos.y, MapType.Tower);
                    m_TowerManager.RandomInstantiateTower(transform.position);
                    m_TowerManager.RetrieveUpdatableTower();
                    UIRemainTowerCount.SubTowerCount();
                    break;
                }
            case "Update":
                {
                    m_TowerManager.RetrieveMergeableTower(m_Tower);
                    break;
                }
        }
    }
}
