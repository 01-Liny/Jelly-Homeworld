using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour {

    public GameObject m_Instance;
    public FSM m_ConstructTowerFSM;

    private Vector3 m_Position = Vector3.zero;
    public int MonsterNum;
    private float startTime;
    private int interval;
    private static int i;

    private bool isSendMessage = false;//防止多次发送建造模式信息

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

        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(i < MonsterNum)
        { 
            if (Time.time - startTime >= interval)
            {
                isSendMessage = false;
                startTime = Time.time;
                CreatMonster();
                i++;
            }
        }

        if(i==0&&isSendMessage==false)
        {
            isSendMessage = true;
            m_ConstructTowerFSM.ChangeState("Construct");
        }
    }

    public void RemoveEnemy(object sender,GameManager.EnemyDiedEventsArgs e)
    {
        //怪物数量扣1
        MonsterNum--;
        i --;
    }
}
