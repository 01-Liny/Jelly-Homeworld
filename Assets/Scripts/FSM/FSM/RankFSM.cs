using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankFSM : FSM
{
    public FSM m_FSM;
    public ConfigManager m_ConfigManager;
    public Text m_Text;

    private Canvas m_Canvas;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("Rank", this);
    }

    public override void OnEnter(string prevState = "")
    {
        m_Canvas.enabled = true;
        m_Text.text = m_ConfigManager.GetUserRecordWithString();
    }

    public override void OnExit(string nextState = "")
    {
        //for (int i = 0; i < m_States.Count; i++)
        //{
        //    m_States.Values.ElementAt(i).OnExit();
        //}
        m_Canvas.enabled = false;
    }

    public override void OnClick()
    {
        
    }
}
