using UnityEngine;
using System.Collections;

public class UISelectedArea : MonoBehaviour 
{
	public GameObject m_Prefabs;
    public Camera m_Camera;
    public RaycastHit m_Hit;
    public TowerManager m_TowerManager;

    private Vector3 m_VecTemp=new Vector3();

    public void Update()
    {
        //从摄像头的坐标到鼠标在屏幕上的坐标的方向向量
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        
        //以ray的方向和原点，发射一条射线，检测射线是否碰撞到LayerMask为Plane的Collider
        if(Physics.Raycast(ray.origin, ray.direction, out m_Hit, Mathf.Infinity, 1 << 8))
        {
            m_VecTemp = m_Hit.point;
            m_VecTemp.y = 100;
            Debug.DrawLine(m_Hit.point, m_VecTemp, Color.blue);
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        }
        //给选中框做位置偏移
        m_VecTemp.x=(int)(m_Hit.point.x+0.5f*Mathf.Sign(m_Hit.point.x));
        m_VecTemp.z=(int)(m_Hit.point.z+0.5f*Mathf.Sign(m_Hit.point.z));
        m_VecTemp.y = 0.1f;

        m_Prefabs.transform.position = m_VecTemp;
        
        //如果点击鼠标左键
        if (Input.GetButtonDown("Fire1"))
        {
            m_TowerManager.RandomInstantiate(m_VecTemp);
        }
    }
}
