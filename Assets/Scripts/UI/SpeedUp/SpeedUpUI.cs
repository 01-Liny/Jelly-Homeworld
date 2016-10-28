using UnityEngine;
using System.Collections;

public class SpeedUpUI : MonoBehaviour
{
    private Animator m_Animator;
    private bool isOn=false;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Toggle()
    {
        if(isOn)
        {
            isOn = false;
            m_Animator.SetInteger("State", 2);
        }
        else
        {
            isOn = true;
            m_Animator.SetInteger("State", 1);
        }
    }
}
