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
    public UIRangeIndicator m_UIRangeIndicator;
    public MonsterManager m_MonsterManager;
    public bool isOnPlayMode = false;
    public Canvas m_StartLevelCanvas;
    private Canvas m_Canvas;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("ConstructTower", this);
    }

    public override void OnEnter(string prevState = "")
    {
        ChangeState("Construct");
        m_Canvas.enabled = true;
        m_StartLevelCanvas.enabled = true;
        UIRemainTowerCount.ResetTowerCount();
        UIGameLevel.ResetLevel();
        //防止无法生成怪物的情况
        m_MonsterManager.isClearMode = false;
    }

    public override void OnExit(string nextState = "")
    {  
        //for (int i = 0; i < m_States.Count; i++)
        //{
        //    m_States.Values.ElementAt(i).OnExit();
        //}
        m_StartLevelCanvas.enabled = false;
        m_Canvas.enabled = false;
        m_TowerManager.ClearAll();
        m_MonsterManager.ClearAll();
    }

    public override void OnClick()
    {
        GameObject m_SelectedTower = m_UISelectedArea.GetSelectedTower();
        //如果在生成怪物状态，不能建造塔，只能显示UI
        if (currentStateName == "Play")
        {
            m_UIRangeIndicator.ShowTowerRangeIndicator(m_SelectedTower);
            return;
        }    
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
                    GameObject m_UpdatableTower = m_ConstructUIController.GetTowerGameObject();
                    pos = m_UISelectedArea.RealPosToMapPos(m_MergeableTower.transform.position);
                    posX = (int)pos.x;
                    posY = (int)pos.y;
                    m_MapManager.DeleteTower(posX, posY);
                    //取得升级元素
                    TowerElem updateElem = m_MergeableTower.GetComponent<Tower>().GetUpdateElem();
                    m_TowerManager.DestroyTower(m_MergeableTower);
                    m_TowerManager.UpdateTower(m_UpdatableTower, updateElem);
                    //显示升级后的塔的范围
                    m_UIRangeIndicator.ShowTowerRangeIndicator(m_UpdatableTower);
                    //再次搜索
                    m_TowerManager.RetrieveUpdatableTower();                
                }
            }
            else
            {
                switch (mapType)
                {
                    case MapType.Empty:
                        {
                            //隐藏UI
                            m_ConstructUIController.Disable();
                            m_UIRangeIndicator.Disable();
                            break;
                        }
                    case MapType.Basic:
                        {
                            //隐藏范围指示器
                            m_UIRangeIndicator.Disable();
                            //如果还有剩余的建造塔数量，显示建造UI
                            if (UIRemainTowerCount.remainTowerCount>0)
                            {
                                UpdateConstructUIConstroller("Tower");
                            }
                            else
                            {
                                m_ConstructUIController.Disable();
                            }
                            break;
                        }
                    case MapType.Tower:
                        {
                            //显示范围指示器
                            m_UIRangeIndicator.ShowTowerRangeIndicator(m_SelectedTower);

                            GameObject m_Tower = m_UISelectedArea.GetUpdateTower();
                            //如果该塔可以升级
                            if (m_Tower != null)
                            {
                                UpdateConstructUIConstroller("Update");
                                m_ConstructUIController.SetTowerGameObject(m_Tower);
                            }
                            else
                            {
                                m_ConstructUIController.Disable();
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