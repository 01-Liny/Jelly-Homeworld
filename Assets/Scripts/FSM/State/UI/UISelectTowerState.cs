using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectTowerState : MonoBehaviour ,IState
{
	public FSM FSMConstruct;
    private Image m_Image;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        FSMConstruct.Register("Tower", this);
    }

    #region IState Members

    public void OnEnter(string prevState)
    {
        m_Image.color = Color.green;
    }
    public void OnExit(string nextState)
    {
        m_Image.color=Color.white;
    }
    public void OnUpdate()
    {
        
    }

	public void OnStart()
	{

	}

    #endregion
}
