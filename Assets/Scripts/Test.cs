using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TouchScript.Gestures;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour 
{

    public FSM m_FSM;//总状态机
    public UISelectedArea m_SelectedArea;
    public Camera m_Camera;

    public TapGesture m_TapGesture;
    public float PanSpeed = 200f;
    public float ZoomSpeed = 10f;

    private bool isMoved = false;

    private bool isTouched = false;
    private int firstTouchId;

    private Vector3 m_CameraOriginPos;
    private Vector2 m_Origin1;

    private Vector2 m_Drection1;
    private Vector2 m_Origin2;
    private Vector2 m_Drection2;


    private float distance;

    private bool isSingle = false;


    private Ray ray;
    private RaycastHit m_Hit;
    private Vector2 m_HitPos;

    private float screenOffsetWidth;
    private float screenOffsetHeight;

    private void Start()
    {
        //pc上的摄像头视角大小不一样，注释值为pc上的数值
        screenOffsetWidth = Screen.width / 1.87f;//3.55
        screenOffsetHeight = Screen.height / 2.9f;//2
    }

    private void OnEnable()
    {

        m_TapGesture.Tapped += TappedHandler;
    }

    private void OnDisable()
    {
        m_TapGesture.Tapped -= TappedHandler;
    }

    private void TappedHandler(object sender, System.EventArgs e)
    {
        if (isMoved)
        {
            isMoved = false;
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        m_FSM.OnClick();
    }
    //[Range(0, 100)]
    //public float health;

    //private float screenOffsetWidth;
    //private float screenOffsetHeight;
    //public Text text;

    //private Image m_Image;
    //private Color m_Color;

    //private void Start()
    //{
    //    //m_Image = GetComponent<Image>();
    //    //m_Color = new Color();
    //    //Vector3 t1 = new Vector3(1, 2, 3);
    //    //Vector2 t2 = (Vector2)t1;

    //    screenOffsetWidth = Screen.width;
    //    screenOffsetHeight = Screen.height;
    //    //text.GetComponent<Text>();

    //    text.text += screenOffsetWidth + " , " + screenOffsetHeight + " , " + Screen.dpi;
    //}

    //private void Update()
    //{
    //    //m_Color = m_Image.color;
    //    //m_Color.r = (100-health) / 100.0f;
    //    //m_Color.g = health / 100.0f;
    //    //m_Color.b = 0;
    //    //m_Image.color = m_Color;
    //}
}
