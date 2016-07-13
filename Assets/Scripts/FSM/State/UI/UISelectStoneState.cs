using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectStoneState : MonoBehaviour ,IState
{
    public FSM FSMConstruct;
    public MapManager m_MapManager;
    public TowerManager m_TowerManger;
    public ConstructUIController m_ConstructUIController;
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
        m_Image.color=Color.white;
    }
    public void OnUpdate()
    {
        
    }

	public void OnStart()
	{
        Vector2 m_MapPos=m_ConstructUIController.m_MapPos;
        //如果可以添加到地图
        if(m_MapManager.SetMap((int)m_MapPos.x,(int)m_MapPos.y,MapType.Basic))
        {
            m_TowerManger.RandomInstantiateStone(m_ConstructUIController.transform.position);
        }
	}

    #endregion
}
