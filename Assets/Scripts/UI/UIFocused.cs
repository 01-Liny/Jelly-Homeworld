using UnityEngine;
using System.Collections;

public class UIFocused : MonoBehaviour
{
    public MonsterManager m_MonsterManager;
    public GameObject m_Signed;

    private void Update()
    {
        if(m_MonsterManager.m_FocusedEnemy!=null)
        {
            m_Signed.SetActive(true);
            this.transform.position = m_MonsterManager.m_FocusedEnemy.transform.position;
        }
        else
        {
            m_Signed.SetActive(false);
        }
    }
}
