using UnityEngine;
using System.Collections;

public class UIDirectionControl : MonoBehaviour 
{
    Quaternion m_ParentRotation;
    private void Start()
    {
        m_ParentRotation = transform.parent.rotation;
    }
    private void Update()
    {
        transform.rotation = m_ParentRotation;
    }
}
