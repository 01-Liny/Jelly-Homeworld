using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public FSM m_FSM;//总状态机
    public UISelectedArea m_SelectedArea;

    private void Update()
    {
        //如果点击鼠标左键
        if (Input.GetButtonDown("Fire1"))
        {
            //如果点击到UI则不触发事件
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
#endif

#if UNITY_ANDROID
            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }
#endif
            //转到该实体进行鼠标位置分析和接下来的行为
            //当前状态对应的鼠标事件
            m_FSM.OnClick();
            //m_SelectedArea.ClickConfirmed();
        }
    }
}
