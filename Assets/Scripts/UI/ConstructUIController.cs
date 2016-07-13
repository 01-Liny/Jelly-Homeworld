using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConstructUIController : MonoBehaviour 
{
    public Text text;
    public float UIHeight;

    private Canvas m_Canvas;
    private Vector3 m_Pos;

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
    public void MoveTo(float x,float z)
    {
        m_Pos.x=x;
        m_Pos.z=z;
        transform.position=m_Pos;
    }

    //改变按钮内容
    public void ChangeButtonText(string content)
    {
        this.text.text=content;
    }

    //更新地图坐标，参数来自上一次显示UI时的地图坐标
    public void UpdateMapPos(Vector2 m_MapPos)
    {
        this.m_MapPos.x=m_MapPos.x;
        this.m_MapPos.y=m_MapPos.y;
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
}
