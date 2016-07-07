using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour 
{
    public UISelectedArea m_SelectedArea;

    private void Update()
    {
        //如果点击鼠标左键
        if (Input.GetButtonDown("Fire1"))
        {
            //如果点击到UI则不触发事件
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //return;
            }
            m_SelectedArea.ClickConfirmed();
            Global_Variables.fsm.Test();
        }
    }
}
