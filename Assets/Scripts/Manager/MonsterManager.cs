using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour
{

    //模型数组
    public GameObject[] EnemyPrefabs;
    public FSM m_ConstructTowerFSM;

    //怪物出生地
    private Vector3 m_Position = Vector3.zero;

    //怪物出生间隔
    [SerializeField]
    private float interval;

    public GameObject m_Instance;

    BasicEnemy m_InstanceBasicEnemy;

    private int MonsterNum;
    private float startTime;

    private bool isSendMessage = false;//防止多次发送建造模式信息

    private int InstantiateMonster;//初始化怪物类型

    //用于退出关卡时，一次性删除所有怪物
    [SerializeField]
    private List<BasicEnemy> m_EnemyList = new List<BasicEnemy>();
    //用于清除所有怪物时，防止怪物继续生成,进入关卡时要设置为true，已交给FSM完成
    public bool isClearMode = false;
    public BasicEnemy m_FocusedEnemy = null;

    //----------------可能要删掉-------------
    //public void setCreatMonster(Vector3 m_Position, int MonsterNum,int interval)
    //{
    //    this.m_Position = m_Position;
    //    this.MonsterNum = MonsterNum;
    //    this.interval = interval;
    //}


    IEnumerator InstantiateAfterDelay(GameObject prefabs, int InstantiateMonster, float delay, int gameLevel)
    {
        yield return new WaitForSeconds(delay);
        if (isClearMode)
            yield break;
        m_Instance = Instantiate(prefabs, m_Position, Quaternion.identity) as GameObject;
        m_InstanceBasicEnemy = m_Instance.GetComponent<BasicEnemy>();
        m_EnemyList.Add(m_InstanceBasicEnemy);
        m_InstanceBasicEnemy.Init(MonsterInfo.EnemyProperty[(int)(gameLevel / 5.0) + 1, InstantiateMonster, 0],
            MonsterInfo.EnemyProperty[(int)(gameLevel / 6.0) + 1, InstantiateMonster, 1],
            MonsterInfo.EnemyProperty[(int)(gameLevel / 6.0) + 1, InstantiateMonster, 2],
            MonsterInfo.EnemyProperty[(int)(gameLevel / 6.0) + 1, InstantiateMonster, 3],
            MonsterInfo.EnemyProperty[(int)(gameLevel / 6.0) + 1, InstantiateMonster, 4],
            MonsterInfo.EnemyProperty[(int)(gameLevel / 6.0) + 1, InstantiateMonster, 5], 
            MonsterInfo.EnemyHealth[gameLevel]);
    }


    //生成指定数量的怪物
    public void CreatMonster(int gameLevel, int enemyNum, int interval)
    {
        isSendMessage = false;
        this.interval = 0;

        int MonsterTypeNum = 0;//计算此关有几种怪物

        for (int i = 0; i < 3; i++)
        {
            if (MonsterInfo.PrefabsPositon[gameLevel, i] != 0)
            {
                MonsterTypeNum++;
            }
        }

        while (MonsterNum < enemyNum)
        {
            InstantiateMonster = MonsterInfo.PrefabsPositon[gameLevel, Random.Range(0, MonsterTypeNum)];
            StartCoroutine(InstantiateAfterDelay(EnemyPrefabs[InstantiateMonster], this.InstantiateMonster, this.interval, gameLevel)); // wait for one second
            MonsterNum++;
            this.interval += interval;
        }
    }

    // Use this for initialization
    void Start()
    {
        MonsterNum = 0;
        startTime = Time.time;
        MonsterInfo.Init();
        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (i < MonsterNum)
        //{
        //    if (Time.time - startTime >= interval)
        //    {
        //        isSendMessage = false;
        //        startTime = Time.time;
        //        //CreatMonster();
        //        i++;
        //    }
        //}

        //出现下一关的UI
        if (MonsterNum == 0 && isSendMessage == false)
        {
            isSendMessage = true;
            //防止出现在怪物被销毁后，NextLevelUI还跳出来的情况
            if(isClearMode==false)
                m_ConstructTowerFSM.ChangeState("Construct");
        }
    }

    public void RemoveEnemy(object sender, GameManager.EnemyDiedEventsArgs e)
    {
        //怪物数量扣1
        MonsterNum--;
    }


    //删除所有怪物,并停止怪物生成
    public void ClearAll()
    {
        isClearMode = true;
        MonsterNum = 0;
        for (int i = 0; i < m_EnemyList.Count; i++)
        {
            Destroy(m_EnemyList[i]);
        }
        m_EnemyList.Clear();
    }
}
