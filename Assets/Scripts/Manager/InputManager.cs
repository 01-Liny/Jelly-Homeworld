using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public FSM m_FSM;//总状态机
    public UISelectedArea m_SelectedArea;
    public Camera m_Camera;

    private bool isTouched = false;
    private int firstTouchId;

    private Vector3 m_CameraOriginPos;
    private Vector2 m_Origin1;
  
    private Vector2 m_Drection1;
    private Vector2 m_Origin2;
    private Vector2 m_Drection2;


    private float distance;

    private bool isSingle = false;
    private bool isMoved = false;

    private Ray ray;
    private RaycastHit m_Hit;
    private Vector2 m_HitPos;

    private float screenOffsetWidth;
    private float screenOffsetHeight;

    private void Start()
    {
        //pc上的摄像头视角大小不一样，注释值为pc上的数值
        screenOffsetWidth = Screen.width / 1.87f;//3.55
        screenOffsetHeight = Screen.height / 2.9f;//2
    }

    private void FixedUpdate()
    {
#if UNITY_ANDROID
        Touch touch1;
        if (Input.touchCount == 1)
        {
            touch1 = Input.GetTouch(0);
            //防止两个点触控时，先放开另外一个点，导致画面漂移
            if (isSingle == false)
            {
                isSingle = true;
                m_Origin1 = touch1.position;
                
            }

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            // Handle finger movements based on touch phase.
            switch (touch1.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    {
                        m_Origin1 = touch1.position;
                        //m_Origin1 = GetTouchHitPos(touch1);

                        m_CameraOriginPos = m_Camera.transform.position;
                        break;
                    }
                case TouchPhase.Stationary:
                    break;
                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    {
                        isMoved = true;
                        Vector3 temp = m_CameraOriginPos;

                        //m_Drection1 = m_Origin1 - GetTouchHitPos(touch1);
                        m_Drection1 = m_Origin1 - touch1.position;
                        temp.x += (m_Drection1.x/ screenOffsetWidth) *m_Camera.orthographicSize;
                        temp.z +=( m_Drection1.y/screenOffsetHeight) *m_Camera.orthographicSize;
                        m_Camera.transform.position = temp;
                        break;
                    }
                case TouchPhase.Canceled:

                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    if (isMoved == false)
                    {
                        m_FSM.OnClick();
                    }
                    isMoved = false;
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            isSingle = false;
            touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Handle finger movements based on touch phase.
            switch (touch1.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    m_Origin1 = touch1.position;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    m_Drection1 = m_Origin1 - touch1.position;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    break;
            }

            switch (touch2.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    m_Origin2 = touch2.position;
                    distance = (m_Origin1 - m_Origin2).magnitude;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    float distanceTemp = (touch1.position - touch2.position).magnitude;
                    m_Drection2 = m_Origin2 - touch2.position;
                    //说明是相向的
                    if (distance > distanceTemp)
                    {
                        m_Camera.orthographicSize += (m_Drection1.magnitude + m_Drection2.magnitude) / 1000;
                    }
                    else if (distance < distanceTemp)
                    {
                        m_Camera.orthographicSize -= (m_Drection1.magnitude + m_Drection2.magnitude) / 1000;
                    }
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:

                    break;
            }
        }
#endif

        //安卓开发模式下，同时含有UNITY_ANDROID,UNITY_EDITOR
#if UNITY_EDITOR && !UNITY_ANDROID
        if (Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            m_FSM.OnClick();
        }
#endif

        //        //如果点击鼠标左键
        //        if (Input.GetButtonDown("Fire1"))
        //        {
        //            //如果点击到UI则不触发事件
        //#if UNITY_EDITOR
        //            //if (EventSystem.current.IsPointerOverGameObject())
        //            //{
        //            //    //return;
        //            //}
        //            if (m_SelectedArea.isClickUI())
        //            {
        //                return;
        //            }
        //#endif



        //#if UNITY_ANDROID
        //            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //            {
        //                //return;
        //            }
        //#endif
        //            //转到该实体进行鼠标位置分析和接下来的行为
        //            //当前状态对应的鼠标事件
        //            m_FSM.OnClick();
        //            //m_SelectedArea.ClickConfirmed();


        // }
    }


    public void OnPointerDown(PointerEventData data)
    {
        if (isTouched == false)
        {
            isTouched = true;
            firstTouchId = data.pointerId;
            m_Origin1 = data.position;
            m_CameraOriginPos = m_Camera.transform.position;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (isTouched)
        {
            Vector3 temp = m_CameraOriginPos;
            Vector2 m_Drection = data.position - m_Origin1;
            temp.x += m_Drection.x;
            temp.z += m_Drection.y;
            m_Camera.transform.position = temp;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId == firstTouchId)
        {
            isTouched = false;

        }
    }
}
