/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using UnityEngine;
using TouchScript.Gestures;

namespace TouchScript.Examples.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        public ScreenTransformGesture TwoFingerMoveGesture;
        public ScreenTransformGesture ManipulationGesture;
        public float PanSpeed = 200f;
        public float RotationSpeed = 200f;
        public float ZoomSpeed = 10f;

        private Transform pivot;
        private Transform cam;

        private Camera m_Camera;

        private void Awake()
        {
            pivot = transform.Find("Pivot");
            cam = transform.Find("Pivot/Camera");
            m_Camera = cam.GetComponent<Camera>();
        }

        private void OnEnable()
        {
            TwoFingerMoveGesture.Transformed += twoFingerTransformHandler;
            ManipulationGesture.Transformed += manipulationTransformedHandler;
        }

        private void OnDisable()
        {
            TwoFingerMoveGesture.Transformed -= twoFingerTransformHandler;
            ManipulationGesture.Transformed -= manipulationTransformedHandler;
        }

        private void manipulationTransformedHandler(object sender, System.EventArgs e)
        {
            m_Camera.orthographicSize -= (ManipulationGesture.DeltaScale - 1f) * ZoomSpeed;
        }

        private void twoFingerTransformHandler(object sender, System.EventArgs e)
        {
            pivot.localPosition += pivot.rotation * TwoFingerMoveGesture.DeltaPosition * PanSpeed;
        }
    }
}