using UnityEngine;
using System.Collections.Generic;


public class FSM : MonoBehaviour, IState, IInput
{
    protected Dictionary<string, IState> m_States=new Dictionary<string, IState>();
    [SerializeField]protected string currentStateName;

    protected virtual void Start()
    {
        //currentStateName = null;
        if (currentStateName != "")
            m_States[currentStateName].OnEnter();
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
        if(currentStateName!="")
        m_States[currentStateName].OnUpdate();
    }

    public string GetCurrentState()
    {
        return currentStateName;
    }

    public void ChangeState(string newStateName)
    {
        if(m_States.ContainsKey(newStateName))
        {
            if (currentStateName != "")
            {
                m_States[currentStateName].OnExit();
            }
            m_States[newStateName].OnEnter();
            currentStateName = newStateName;
        }
        else
            Debug.LogError("No Exist State :"+newStateName+",Cannot Change State");
    }

    #region IState Members
    public virtual void OnEnter(string prevState = "")
    {
        
    }

    public virtual void OnExit(string nextState = "")
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnTrigger()
    {
        if (currentStateName != "")
            m_States[currentStateName].OnTrigger();
        else
            Debug.LogError("Cannot OnStart.currentStateName not exist");
    }
    #endregion

    #region IInput Members
    public virtual void OnClick()
    {
        if (currentStateName != "")
        {
            IInput m_Temp = m_States[currentStateName] as IInput;
            m_Temp.OnClick();
        }
            
    }
    #endregion
}
