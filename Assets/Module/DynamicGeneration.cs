using UnityEngine;
using System.Collections;

public class DynamicGeneration : MonoBehaviour
{
    public bool isChangeModule = false;
    public bool isChangeMotion = false;
    public GameObject[] m_Modules;

    private GameObject m_CurrentModule;
    private Animation m_Animation;
    private Animator m_Animator;
    private int temp = 1;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(isChangeModule)
        {
            isChangeModule = false;
            if (temp == 1)
                temp = 0;
            else
                temp = 1;
            NewInit(temp);
        }
        
        if(isChangeMotion)
        {
            isChangeMotion = false;
            m_Animator.SetTrigger("Died");
        }
	}

    private void NewInit(int index)
    {
        Destroy(m_CurrentModule);
        m_CurrentModule = Instantiate(m_Modules[index], Vector3.zero, Quaternion.identity) as GameObject;
        m_Animator = m_CurrentModule.GetComponent<Animator>();
    }
}
