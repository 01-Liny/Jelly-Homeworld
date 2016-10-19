using UnityEngine;
using System.Collections;

public class ProducerFSM : FSM
{
    public FSM m_FSM;   
    public UIProducerGenerator m_UIProducerGenerator;
    private Canvas m_Canvas;


    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("Producer", this);
    }

    public override void OnEnter(string prevState = "")
    {
        m_Canvas.enabled = true;
        m_UIProducerGenerator.Enable();
    }

    public override void OnExit(string nextState = "")
    {
        m_Canvas.enabled = false;
        m_UIProducerGenerator.Diable();
    }
}
