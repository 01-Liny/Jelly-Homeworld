using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour {

    public GameObject m_Instance;

    private Vector3 m_Position = Vector3.zero;
    private int MonsterNum;
    private float startTime;
    private int interval;
    private static int i;

    public void setCreatMonster(Vector3 m_Position, int MonsterNum,int interval)
    {
        this.m_Position = m_Position;
        this.MonsterNum = MonsterNum;
        this.interval = interval;
    }

    private void CreatMonster()
    {
        Instantiate(m_Instance, m_Position, Quaternion.identity);
    }

    // Use this for initialization
    void Start()
    {
        i = 0;
        MonsterNum = 0;
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(i < MonsterNum)
        {
            if (Time.time - startTime >= interval)
            {
                startTime = Time.time;
                CreatMonster();
                i++;
            }
        }
    }
}
