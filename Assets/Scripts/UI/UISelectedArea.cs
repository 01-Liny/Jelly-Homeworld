using UnityEngine;
using System.Collections;


public class UISelectedArea : MonoBehaviour 
{
	public GameObject m_Prefabs;
    public Camera m_Camera;
    public RaycastHit m_Hit;
    public TowerManager m_TowerManager;
    public MapManager m_MapManager;
    public ConstructUIController m_ConstructUIController;
    public FSM fsm;

    private Vector3 m_VecTemp=new Vector3();

    private bool isOutRange = false;
    private float size;
    private float halfSize;
    private int temp;
    private Ray ray;

    private void Start()
    {
        size = MapManager.mapSize;
        halfSize = size / 2;
    }

    public void Update()
    {
        //从摄像头的坐标到鼠标在屏幕上的坐标的方向向量
        ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        
        //以ray的方向和原点，发射一条射线，检测射线是否碰撞到LayerMask为Plane的Collider
        if(Physics.Raycast(ray.origin, ray.direction, out m_Hit, Mathf.Infinity, 1 << 8))
        {
            m_VecTemp = m_Hit.point;
            m_VecTemp.y = 100;
            Debug.DrawLine(m_Hit.point, m_VecTemp, Color.blue);
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        }

        //给选中框做位置偏移
        m_VecTemp.x=((int)((m_Hit.point.x+halfSize*Mathf.Sign(m_Hit.point.x))/size))*size;
        
        m_VecTemp.z=((int)((m_Hit.point.z+halfSize*Mathf.Sign(m_Hit.point.z))/size))*size;

        m_VecTemp.y = 0.1f;
        //判断是非超出地图范围,用mapRegionY判断m_VecTemp.z是正确的
        if(m_VecTemp.x>=0&&m_VecTemp.y<(MapManager.mapRegionX*size)
        &&m_VecTemp.z>=0&&m_VecTemp.z<(MapManager.mapRegionY*size))
        {
            isOutRange = false;
            m_Prefabs.transform.position = m_VecTemp;
        }
        else
        {
            isOutRange = true;
        }    
        
    }

    //当鼠标左键被触发时，调用此函数
    public void ClickConfirmed()
    {
        //如果在地图范围内
        if (isOutRange==false)
        {
            //将游戏地图上的坐标转换为地图数组的下标
            int posX,posY;
            posX=(int)(m_VecTemp.x / size);
            posY=(int)(m_VecTemp.z / size);

            MapType mapType=m_MapManager.GetMap(posX,posY);

            //只有该位置有石头或防御塔的时候才会显示ConstructUI
            switch (mapType)
            {
                case MapType.Empty:
                {
                    if(fsm.GetCurrentState()=="Stone")
                    {
                        m_ConstructUIController.enabled=true;
                    }
                    break;
                }
                case MapType.Basic://就是石头的意思
                {
                    if(fsm.GetCurrentState()=="Tower")
                    {
                        m_ConstructUIController.enabled=true;
                    }
                    break;
                }
                //default:
            }

            /*
            if()
            {
                m_ConstructUIController.enabled = false;
            }
            else
            {
                {
                    //将游戏地图上的坐标转换为地图数组的下标
                    if (m_MapManager.SetMap((int)(m_VecTemp.x / size), (int)(m_VecTemp.z / size), MapType.Tower))
                        m_TowerManager.RandomInstantiateTower(m_VecTemp);
                }
            }
            */
        }
    }
}
