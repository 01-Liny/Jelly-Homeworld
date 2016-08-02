using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectStoneState : MonoBehaviour, IState
{
    public FSM FSMConstruct;
    //public MapManager m_MapManager;
    //public TowerManager m_TowerManager;
    //public ConstructUIController m_ConstructUIController;
    private Image m_Image;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        //注册到状态机
        FSMConstruct.Register("Stone", this);
    }

    #region IState Members

    public void OnEnter(string prevState)
    {
        m_Image.color = Color.green;
    }

    public void OnExit(string nextState)
    {
        m_Image.color = Color.white;
    }

    public void OnUpdate()
    {

    }

    public void OnTrigger()
    {
        //建造石头时不再触发确认UI，已作废
        // Vector2 m_MapPos=m_ConstructUIController.m_MapPos;
        // //如果可以添加到地图
        // if(m_MapManager.ModifyMap((int)m_MapPos.x,(int)m_MapPos.y,MapType.Basic))
        // {
        //     m_TowerManager.RandomInstantiateStone(m_ConstructUIController.transform.position);
        // }
    }
    #endregion
}
