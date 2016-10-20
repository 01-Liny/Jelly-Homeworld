using UnityEngine;
using System.Collections;

public class UIPlayState : MonoBehaviour, IState
{
    public FSM FSMConstruct;

    private Canvas m_Canvas;

    private void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        FSMConstruct.Register("Play", this);
    }

    #region IState Members
    public void OnEnter(string prevState)
    {
        m_Canvas.enabled = false;
        //重置剩余可用塔
        UIRemainTowerCount.ResetTowerCount();

        UIGameLevel.AddLevel();
    }
    public void OnExit(string nextState)
    {
        m_Canvas.enabled = true;
    }
    public void OnUpdate()
    {

    }

    public void OnTrigger()
    {

    }
    #endregion
}
