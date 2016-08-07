using UnityEngine;
using System.Collections;
using System.Linq;

public class ConstructTowerFSM : FSM
{
    public FSM m_FSM;
    public UISelectedArea m_UISelectedArea;
    public MapManager m_MapManager;
    public TowerManager m_TowerManager;
    public ConstructUIController m_ConstructUIController;
    public bool isOnPlayMode = false;
    private Canvas m_Canvas;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("ConstructTower", this);
    }

    public override void OnEnter(string prevState = "")
    {
        m_Canvas.enabled = true;
        UIRemainTowerCount.ResetTowerCount();
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
        //如果在生成怪物状态，不能建造塔
        if (isOnPlayMode)
            return;
        //如果在地图范围内
        if (m_UISelectedArea.IsOutRange() == false)
        {
            int posX, posY;
            //将游戏地图上的坐标转换为地图数组的下标
            Vector2 pos = m_UISelectedArea.GetClickMapPos();
            posX = (int)pos.x;
            posY = (int)pos.y;
            MapType mapType = m_MapManager.GetMap(posX, posY);
            //如果处于升级模式，则不能建造塔，也不能显示UI
            if (TowerManager.isOnUpdate)
            {
                GameObject m_MergeableTower = m_UISelectedArea.GetUpdateTower();
                //如果该塔可以合并
                if(m_MergeableTower!=null)
                {
                    pos = m_UISelectedArea.RealPosToMapPos(m_MergeableTower.transform.position);
                    posX = (int)pos.x;
                    posY = (int)pos.y;
                    m_MapManager.DeleteTower(posX, posY);
                    m_TowerManager.DestroyTower(m_MergeableTower);
                    m_TowerManager.UpdateTower(m_ConstructUIController.GetTowerGameObject());
                    //再次搜索
                    m_TowerManager.RetrieveUpdatableTower();
                }
            }
            else
            {
                switch (mapType)
                {
                    case MapType.Basic:
                        {
                            //如果还有剩余的建造塔数量
                            if(UIRemainTowerCount.remainTowerCount>0)
                            {
                                UpdateConstructUIConstroller("Tower");
                            }
                            break;
                        }
                    case MapType.Tower:
                        {
                            GameObject m_Tower = m_UISelectedArea.GetUpdateTower();
                            //如果该塔可以升级
                            if (m_Tower != null)
                            {
                                UpdateConstructUIConstroller("Update");
                                m_ConstructUIController.SetTowerGameObject(m_Tower);
                            }
                            break;
                        }
                }
            }
        }
    }

    public void UpdateConstructUIConstroller(string temp)
    {
        m_ConstructUIController.MoveTo(m_UISelectedArea.GetClickOffsetRealPos());
        m_ConstructUIController.UpdateMapPos(m_UISelectedArea.GetClickMapPos());
        m_ConstructUIController.UpdateCameraRay(m_UISelectedArea.GetCameraRay());
        m_ConstructUIController.ChangeButtonText(temp);
        m_ConstructUIController.ChangeState(temp);
        m_ConstructUIController.Enable();
    }
}