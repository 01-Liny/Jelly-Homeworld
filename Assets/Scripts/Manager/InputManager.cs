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
                return;
            }
            //转到该实体进行鼠标位置分析和接下来的行为
            m_SelectedArea.ClickConfirmed();
        }
    }
}
