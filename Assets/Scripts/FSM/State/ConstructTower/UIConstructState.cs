using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIConstructState : MonoBehaviour ,IState
{
    public FSM FSMConstruct;

    private Canvas m_Canvas;
    public ConstructUIController m_ConstructUIController;

    private void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        FSMConstruct.Register("Construct", this);
    }

    #region IState Members
    public void OnEnter(string prevState)
    {
        m_Canvas.enabled = true;
        UIRemainTowerCount.ResetTowerCount();
    }
    public void OnExit(string nextState)
    {
        m_Canvas.enabled = false;
        m_ConstructUIController.Hide();
    }
    public void OnUpdate()
    {

    }

    public void OnTrigger()
    {

    }
    #endregion
}
