using UnityEngine;
using System.Collections.Generic;

public enum FSM_ConstructeState
{
    Stone,
    Tower
}

public class FSM : MonoBehaviour 
{
    protected Dictionary<string, IState> m_States;
    protected string currentStateName;
    protected IState currentStateObject;

    private void Start()
    {
        currentStateName = null;
    }

    public void Register(string stateName,IState stateObject)
    {
        if(!m_States.ContainsKey(stateName))
        {
            m_States.Add(stateName, stateObject);
        }
        else
        {
            Debug.Log("Already have this state");
        }
    }

    public void Update()
    {
        if(currentStateName!=null)
        currentStateObject.OnUpdate();
    }

    public void Test()
    {
        Debug.Log("invoke test");
    }
}
