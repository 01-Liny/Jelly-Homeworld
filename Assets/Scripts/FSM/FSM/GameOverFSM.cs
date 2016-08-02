using UnityEngine;
using System.Collections;
using System.Linq;

public class GameOverFSM : FSM
{
    public FSM m_FSM;
    private Canvas m_Canvas;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("GameOver", this);
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

    }
}