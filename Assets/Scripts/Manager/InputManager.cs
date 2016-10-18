using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TouchScript.Gestures;

namespace TouchScript.Examples.InputManager
{
    public class InputManager : MonoBehaviour
    {
        public FSM m_FSM;//总状态机
        public UISelectedArea m_SelectedArea;
        public Camera m_Camera;

        public ScreenTransformGesture SingleFingerMoveGesture;
        public ScreenTransformGesture TwoFingerScaleGesture;
        public TapGesture m_TapGesture;
        public PressGesture m_PressGesture;
        public float PanSpeed = 200f;
        public float ZoomSpeed = 10f;

        public Transform m_RangeTransformA;
        public Transform m_RangeTransformB;
        private Vector3 m_RangeA;
        private Vector3 m_RangeB;
        private float maxOrthographicSize;
        private float cameraY;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        private float screenRatio;

        private bool isMoved = false;
        private bool isTouchedUI = false;

        private float screenOffsetWidth;
        private float screenOffsetHeight;

        private Vector3 m_OriginCameraPos;
        private float originCameraOrthographicSize;


        private void Start()
        {
            m_OriginCameraPos = m_Camera.transform.position;
            originCameraOrthographicSize = m_Camera.orthographicSize;
            //pc上的摄像头视角大小不一样，注释值为pc上的数值
            screenOffsetWidth = Screen.width / 1.87f;//3.55
            screenOffsetHeight = Screen.height / 2.9f;//2
            screenRatio = (Screen.width*1.0f) / Screen.height;

            m_RangeA = m_RangeTransformA.position;
            m_RangeB = m_RangeTransformB.position;
            maxOrthographicSize = m_Camera.orthographicSize;
            cameraY = m_Camera.transform.localPosition.y;

            Vector3 temp = m_Camera.transform.localPosition;
            float viewSize = m_Camera.orthographicSize;
        }

        public void ResetCameraInfo()
        {
            m_Camera.transform.position= m_OriginCameraPos;
            m_Camera.orthographicSize=originCameraOrthographicSize;
        }

        private void Update()
        {
            //限制摄像头视角范围
            float viewSize = m_Camera.orthographicSize;

#if UNITY_ANDROID
            //手机处理方式不同
            minX = m_RangeA.x + screenRatio*3f * viewSize;
            maxX = m_RangeB.x - screenRatio*3f * viewSize;
#endif

#if UNITY_EDITOR
            minX = m_RangeA.x + screenRatio * viewSize;
            maxX = m_RangeB.x - screenRatio * viewSize;
#endif

            minY = m_RangeB.z + viewSize*1.4f- cameraY;
            maxY = m_RangeA.z - viewSize*1.4f- cameraY;

            Vector3 temp = m_Camera.transform.localPosition;
            temp.x=Mathf.Clamp(temp.x, minX, maxX);
            temp.z= Mathf.Clamp(temp.z, minY, maxY);
            m_Camera.transform.localPosition = temp;

            m_Camera.orthographicSize= Mathf.Clamp(m_Camera.orthographicSize, 2, maxOrthographicSize);
        }

        private void OnEnable()
        {
            SingleFingerMoveGesture.Transformed += SingleFingerTransformHandler;
            TwoFingerScaleGesture.Transformed += TwoFingerTransformHandler;
            m_PressGesture.Pressed += PressedHandler;
            m_TapGesture.Tapped += TappedHandler;
        }

        private void OnDisable()
        {
            SingleFingerMoveGesture.Transformed -= SingleFingerTransformHandler;
            TwoFingerScaleGesture.Transformed -= TwoFingerTransformHandler;
            m_PressGesture.Pressed -= PressedHandler;
            m_TapGesture.Tapped -= TappedHandler;
        }

        private void PressedHandler(object sender, System.EventArgs e)
        {
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
                isTouchedUI = true;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                isTouchedUI = true;
#endif
        }

        private void TappedHandler(object sender, System.EventArgs e)
        {
            if (isMoved|isTouchedUI)
            {
                isTouchedUI = false;
                isMoved = false;
                return;
            }

            //一般不会触发这一段代码
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
#endif

            m_FSM.OnClick();
        }

        private void SingleFingerTransformHandler(object sender, System.EventArgs e)
        {
            //传回来的值为x,y
            Vector3 temp = m_Camera.transform.localPosition;
            temp.x -= SingleFingerMoveGesture.DeltaPosition.x * PanSpeed;
            temp.z -= SingleFingerMoveGesture.DeltaPosition.y * PanSpeed;
            m_Camera.transform.localPosition = temp;
        }

        private void TwoFingerTransformHandler(object sender, System.EventArgs e)
        {
            float temp = (TwoFingerScaleGesture.DeltaScale - 1f) * ZoomSpeed;
            if(temp<0&& m_Camera.orthographicSize == maxOrthographicSize)
                return;
            m_Camera.orthographicSize -= (TwoFingerScaleGesture.DeltaScale - 1f) * ZoomSpeed;
        }
    }
}



    //    private void FixedUpdate()
    //    {
    //#if UNITY_ANDROID
    //        Touch touch1;
    //        if (Input.touchCount == 1)
    //        {
    //            touch1 = Input.GetTouch(0);
    //            //防止两个点触控时，先放开另外一个点，导致画面漂移
    //            if (isSingle == false)
    //            {
    //                isSingle = true;
    //                m_Origin1 = touch1.position;

    //            }

    //            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
    //                return;

    //            // Handle finger movements based on touch phase.
    //            switch (touch1.phase)
    //            {
    //                // Record initial touch position.
    //                case TouchPhase.Began:
    //                    {
    //                        m_Origin1 = touch1.position;
    //                        //m_Origin1 = GetTouchHitPos(touch1);

    //                        m_CameraOriginPos = m_Camera.transform.position;
    //                        break;
    //                    }
    //                case TouchPhase.Stationary:
    //                    break;
    //                // Determine direction by comparing the current touch position with the initial one.
    //                case TouchPhase.Moved:
    //                    {
    //                        isMoved = true;
    //                        Vector3 temp = m_CameraOriginPos;

    //                        //m_Drection1 = m_Origin1 - GetTouchHitPos(touch1);
    //                        m_Drection1 = m_Origin1 - touch1.position;
    //                        temp.x += (m_Drection1.x/ screenOffsetWidth) *m_Camera.orthographicSize;
    //                        temp.z +=( m_Drection1.y/screenOffsetHeight) *m_Camera.orthographicSize;
    //                        m_Camera.transform.position = temp;
    //                        break;
    //                    }
    //                case TouchPhase.Canceled:

    //                    break;

    //                // Report that a direction has been chosen when the finger is lifted.
    //                case TouchPhase.Ended:
    //                    if (isMoved == false)
    //                    {
    //                        m_FSM.OnClick();
    //                    }
    //                    isMoved = false;
    //                    break;
    //            }
    //        }
    //        else if (Input.touchCount == 2)
    //        {
    //            isSingle = false;
    //            touch1 = Input.GetTouch(0);
    //            Touch touch2 = Input.GetTouch(1);

    //            // Handle finger movements based on touch phase.
    //            switch (touch1.phase)
    //            {
    //                // Record initial touch position.
    //                case TouchPhase.Began:
    //                    m_Origin1 = touch1.position;
    //                    break;

    //                // Determine direction by comparing the current touch position with the initial one.
    //                case TouchPhase.Moved:
    //                    m_Drection1 = m_Origin1 - touch1.position;
    //                    break;

    //                // Report that a direction has been chosen when the finger is lifted.
    //                case TouchPhase.Ended:
    //                    break;
    //            }

    //            switch (touch2.phase)
    //            {
    //                // Record initial touch position.
    //                case TouchPhase.Began:
    //                    m_Origin2 = touch2.position;
    //                    distance = (m_Origin1 - m_Origin2).magnitude;
    //                    break;

    //                // Determine direction by comparing the current touch position with the initial one.
    //                case TouchPhase.Moved:
    //                    float distanceTemp = (touch1.position - touch2.position).magnitude;
    //                    m_Drection2 = m_Origin2 - touch2.position;
    //                    //说明是相向的
    //                    if (distance > distanceTemp)
    //                    {
    //                        m_Camera.orthographicSize += (m_Drection1.magnitude + m_Drection2.magnitude) / 1000;
    //                    }
    //                    else if (distance < distanceTemp)
    //                    {
    //                        m_Camera.orthographicSize -= (m_Drection1.magnitude + m_Drection2.magnitude) / 1000;
    //                    }
    //                    break;

    //                // Report that a direction has been chosen when the finger is lifted.
    //                case TouchPhase.Ended:

    //                    break;
    //            }
    //        }
    //#endif

    //        //安卓开发模式下，同时含有UNITY_ANDROID,UNITY_EDITOR
    //#if UNITY_EDITOR && !UNITY_ANDROID
    //        if (Input.GetButtonDown("Fire1"))
    //        {
    //            if (EventSystem.current.IsPointerOverGameObject())
    //            {
    //                return;
    //            }
    //            m_FSM.OnClick();
    //        }
    //#endif

    //        //        //如果点击鼠标左键
    //        //        if (Input.GetButtonDown("Fire1"))
    //        //        {
    //        //            //如果点击到UI则不触发事件
    //        //#if UNITY_EDITOR
    //        //            //if (EventSystem.current.IsPointerOverGameObject())
    //        //            //{
    //        //            //    //return;
    //        //            //}
    //        //            if (m_SelectedArea.isClickUI())
    //        //            {
    //        //                return;
    //        //            }
    //        //#endif



    //        //#if UNITY_ANDROID
    //        //            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
    //        //            {
    //        //                //return;
    //        //            }
    //        //#endif
    //        //            //转到该实体进行鼠标位置分析和接下来的行为
    //        //            //当前状态对应的鼠标事件
    //        //            m_FSM.OnClick();
    //        //            //m_SelectedArea.ClickConfirmed();


    //        // }
    //    }

