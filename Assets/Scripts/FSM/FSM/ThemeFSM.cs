using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ThemeFSM : FSM
{
    public FSM m_FSM;
    public AudioManager m_AudioManager;
    public MapManager m_MapManager;
    private Canvas m_Canvas;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("Theme", this);
    }
    
    public override void OnEnter(string prevState = "")
    {
        m_Canvas.enabled = true;
        m_AudioManager.StartBGM();
        m_AudioManager.StartThemeMusic();
        m_MapManager.ClearStone();
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