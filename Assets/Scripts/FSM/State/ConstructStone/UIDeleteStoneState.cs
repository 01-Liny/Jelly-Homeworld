using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDeleteStoneState : MonoBehaviour, IState
{
    public FSM FSMConstruct;

    private Image m_Image;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        //注册到状态机
        FSMConstruct.Register("DeleteStone", this);
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

    }
    #endregion
}
