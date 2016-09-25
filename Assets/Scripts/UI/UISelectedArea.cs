using UnityEngine;
using System.Collections;


public class UISelectedArea : MonoBehaviour 
{
	public GameObject m_Prefabs;
    public Camera m_Camera;

    public TowerManager m_TowerManager;
    public MapManager m_MapManager;
    public ConstructUIController m_ConstructUIController;
    public FSM fsm;

    private RaycastHit m_Hit;

    private Vector3 m_VecTemp=new Vector3();

    public bool isOutRange = false;
    private float size;
    private float halfSize;
    private int temp;
    private Ray ray;

    //专门记录要传递的地图位置信息
    private Vector2 m_MapPosTemp;


    private void Start()
    {
        size = MapManager.mapSize;
        halfSize = size / 2;
        m_MapPosTemp=new Vector2();
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

    public bool IsOutRange()
    {
        return isOutRange;
    }

    public Vector2 GetClickMapPos()
    {
         //将游戏地图上的坐标转换为地图数组的下标

         m_MapPosTemp.x=(int)(m_VecTemp.x / size);
         m_MapPosTemp.y=(int)(m_VecTemp.z / size);

         return m_MapPosTemp;
    }

    public Vector3 GetClickOffsetRealPos()
    {
        return m_VecTemp;
    }

    public Ray GetCameraRay()
    {
        return ray;
    }

    public Vector2 RealPosToMapPos(Vector3 m_RealPos)
    {
         m_MapPosTemp.x=(int)(m_RealPos.x / size);
         m_MapPosTemp.y=(int)(m_RealPos.z / size);

         return m_MapPosTemp;
    }

    //返回当前Ray穿过的可升级或可合并的塔
    public GameObject GetUpdateTower()
    {
        RaycastHit m_Hit;
        //以ray的方向和原点，发射一条射线，检测射线是否碰撞到LayerMask为Update的Collider
        //为什么不用加parent
        if (Physics.Raycast(ray.origin, ray.direction, out m_Hit, Mathf.Infinity, 1 << 11))
        {
            //return m_Hit.transform.parent.gameObject;
            //被搜索出来的是basicTower所在的gameobject 所以要加parent，取得Tower所在的gameobject
            return m_Hit.transform.parent.gameObject;
        }
        else
            return null;
    }

    //当鼠标左键被触发时，调用此函数
    //已弃用
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
                        //直接建造 不经过UI确认
                        //如果可以添加到地图
                        if (m_MapManager.ModifyMap(posX, posY, MapType.Basic))
                        {
                            m_TowerManager.RandomInstantiateStone(GetClickOffsetRealPos());
                        }
                        // m_ConstructUIController.MoveTo(m_VecTemp.x,m_VecTemp.z);
                        // m_ConstructUIController.UpdateMapPos(RealPosToMapPos(m_VecTemp));
                        // m_ConstructUIController.UpdateCameraRay(ray);
                        // m_ConstructUIController.ChangeButtonText("Stone");
                        // m_ConstructUIController.Enable();
                    }
                    else
                    {
                        m_ConstructUIController.Disable();
                    }
                    break;
                }
                case MapType.Basic://就是石头的意思
                {
                    if(fsm.GetCurrentState()=="Tower")
                    {
                        //写到一个函数里面
                        //m_ConstructUIController.MoveTo(m_VecTemp.x,m_VecTemp.z);
                        m_ConstructUIController.UpdateMapPos(RealPosToMapPos(m_VecTemp));
                        m_ConstructUIController.UpdateCameraRay(ray);
                        m_ConstructUIController.ChangeButtonText("Tower");
                        m_ConstructUIController.Enable();
                    }
                    else
                    {
                        m_ConstructUIController.Disable();
                        if(fsm.GetCurrentState()=="DeleteStone")
                        {
                            //直接删除 不经过UI确认
                            if (m_MapManager.GetMap(posX,posY)==MapType.Basic)
                            {
                                //先删除石头实体模型，再删除地图上的石头信息，防止石头模型实体未被删除，地图已被删除的情况，石头的Collider未覆盖整个石头建造区域
                                //由于边界的石头没有实体模型，所以同时可以防止边界石头的地图信息被删除
                                if(m_TowerManager.DestroyStone(ray))
                                    m_MapManager.DeleteStone(posX, posY);
                            }
                        }
                    }
                    break;
                }
                default:
                {
                    m_ConstructUIController.Disable();
                    break;
                }
            }
        }
    }
}
