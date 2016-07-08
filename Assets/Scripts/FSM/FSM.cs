using UnityEngine;
using System.Collections.Generic;

public enum FSM_ConstructeState
{
    Stone,
    Tower
}

public class FSM : MonoBehaviour 
{
    protected Dictionary<string, IState> m_States=new Dictionary<string, IState>();
    protected string currentStateName;

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
        m_States[currentStateName].OnUpdate();
    }

    public void ChangeState(string newStateName)
    {
        if(currentStateName!=null)
        {
            m_States[currentStateName].OnExit();
        }
        if(m_States.ContainsKey(newStateName))
        {
            m_States[newStateName].OnEnter();
            currentStateName = newStateName;
        }
        else
            Debug.LogError("No Exist State :"+newStateName+",Cannot Change State");
    }

    public void Test()
    {
        Debug.Log("invoke test");
    }
}
