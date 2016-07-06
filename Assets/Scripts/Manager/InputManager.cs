using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    public UISelectedArea m_SelectedArea;

    private void Update()
    {
        //如果点击鼠标左键
        if (Input.GetButtonDown("Fire1"))
        {
            m_SelectedArea.ClickConfirmed();
        }
    }
}
