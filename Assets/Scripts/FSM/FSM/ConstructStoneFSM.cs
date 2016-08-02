using UnityEngine;
using System.Collections;
using System.Linq;

public class ConstructStoneFSM : FSM
{
    public FSM m_FSM;
    public UISelectedArea m_UISelectedArea;
    public MapManager m_MapManager;
    public TowerManager m_TowerManager;
    private Canvas m_Canvas;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("ConstructStone", this);
    }

    public override void OnEnter(string prevState = "")
    {
        m_Canvas.enabled = true;
    }

    public override void OnExit(string nextState = "")
    {
        for (int i = 0; i < m_States.Count; i++)
        {
            m_States.Values.ElementAt(i).OnExit();
        }
        m_Canvas.enabled = false;
    }

    public override void OnClick()
    {
        //如果在地图范围内
        if (m_UISelectedArea.IsOutRange()==false)
        {
            int posX, posY;
            //将游戏地图上的坐标转换为地图数组的下标
            Vector2 pos = m_UISelectedArea.GetClickMapPos();
            posX = (int)pos.x;
            posY = (int)pos.y;
            MapType mapType = m_MapManager.GetMap(posX, posY);
            switch(mapType)
            {
                case MapType.Empty:
                    {
                        if(currentStateName=="Stone")
                        {
                            //直接建造 不经过UI确认
                            //如果可以添加到地图
                            if (m_MapManager.ModifyMap(posX, posY, MapType.Basic))
                            {
                                m_TowerManager.RandomInstantiateStone(m_UISelectedArea.GetClickOffsetRealPos());
                            }
                        }
                        break;
                    }
                case MapType.Basic:
                    {
                        if(currentStateName=="DeleteStone")
                        {
                            //直接删除 不经过UI确认
                            if (m_MapManager.GetMap(posX, posY) == MapType.Basic)
                            {
                                //先删除石头实体模型，再删除地图上的石头信息，防止石头模型实体未被删除，地图已被删除的情况，石头的Collider未覆盖整个石头建造区域
                                //由于边界的石头没有实体模型，所以同时可以防止边界石头的地图信息被删除
                                if (m_TowerManager.DestroyStone(m_UISelectedArea.GetCameraRay()))
                                    m_MapManager.DeleteStone(posX, posY);
                            }
                        }
                        break;
                    }
            }
        }
    }
}