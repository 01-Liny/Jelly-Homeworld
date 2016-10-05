﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class ChooseMapFSM : FSM
{
    public FSM m_FSM;
    public MapManager m_MapManager;
    private Canvas m_Canvas;

    protected void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
        //注册到状态机
        m_FSM.Register("ChooseMap", this);
    }

    public override void OnEnter(string prevState = "")
    {
        m_Canvas.enabled = true;
        m_MapManager.ChooseMap(0);
        m_MapManager.ReGenerateStoneByMap();
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