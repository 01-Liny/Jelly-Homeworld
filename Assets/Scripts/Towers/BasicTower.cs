using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTower : MonoBehaviour                                                     //该类应该为abstract
{
    [SerializeField]public TowerType towerType;
    [SerializeField]public TowerLevel towerLevel;

    [SerializeField]protected float fireRange;//攻击范围
    [SerializeField]protected float fireDamage;//攻击伤害
    [SerializeField]protected float fireRate;//攻击频率
    [SerializeField]protected float buffDamage;//攻击伤害加成
    [SerializeField]protected float buffFireRate;//攻击频率加成

    [SerializeField]protected float realFireDamage;//总攻击伤害
    [SerializeField]protected float realFireRate;//总攻击频率
    [SerializeField]protected float realFireRateSpendSec;//总攻击一次需要多少秒

    protected SphereCollider m_AttackRangeCollider;//攻击范围collier

    protected float minEnemyHealth;//存储最小敌人血量，优先攻击最低血量的敌人
    protected BasicEnemy m_BasicEnemyTemp;//临时存放Enemy类信息
    protected BasicEnemy m_BasicEnemyMinHealth;//临时存放最小血量的Enemy信息
    protected Rigidbody m_RigidbodyEnemy;//临时存放被攻击的敌人的rigidbody信息


    private float nextFire;//下次攻击的时间
    private List<Collider> m_EnemyTriggerList = new List<Collider>();


    //临时初始化函数                                                                           非正式代码，测试后删除
    public void Start()
    {
        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        this.towerType = TowerType.Strike;
        this.towerLevel = TowerLevel.One;
        RereadTowerInfo();
        m_AttackRangeCollider = GetComponent<SphereCollider>();
        m_AttackRangeCollider.radius = fireRange;//设置攻击范围
        ResetMinEnemyHealth();
        RecalcInfo();
    }

    protected void ResetMinEnemyHealth()
    {
        minEnemyHealth = 500000;
        m_BasicEnemyMinHealth = null;
    }

    protected void Update()
    {
        //当敌人列表不为空时，尝试攻击
        if (m_EnemyTriggerList.Count > 0)
        {
            TryFire();
        }
    }

    protected void FixedUpdate()
    {
        //将塔的正面转向被攻击的敌人的位置
        if(m_RigidbodyEnemy!=null)
        {
            Vector3 relativePos = m_RigidbodyEnemy.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,0.25f);
        }
    }

    //敌人出现在塔的攻击范围内，加入到敌人列表中
    protected void OnTriggerEnter(Collider other)
    {
        if (!m_EnemyTriggerList.Contains(other))
        {
            m_EnemyTriggerList.Add(other);
        }
        Debug.Log("Enter");
    }

    //敌人离开塔的攻击范围，将敌人从敌人列表中移除
    protected void OnTriggerExit(Collider other)
    {
        if (m_EnemyTriggerList.Contains(other))
        {
            m_EnemyTriggerList.Remove(other);
        }
        //如果该塔攻击的敌人已经离开塔的范围，不再跟随该敌人旋转
        if(m_RigidbodyEnemy==other.GetComponent<Rigidbody>())
        {
            m_RigidbodyEnemy = null;
        }
        Debug.Log("Exit");
    }


    //外部调用初始化
    public virtual void Init(TowerLevel towerLevel)
    {
        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        this.towerLevel = towerLevel;
        RereadTowerInfo();
        m_AttackRangeCollider = GetComponent<SphereCollider>();
        m_AttackRangeCollider.radius = fireRange;//设置攻击范围
        ResetMinEnemyHealth();
        RecalcInfo();
    }

    //重新设置塔等级
    public void ResetLevel(TowerLevel towerLevel)
    {
        this.towerLevel = towerLevel;
        RereadTowerInfo();
    }

    //重新读取塔数据
    protected virtual void RereadTowerInfo()
    {
        fireRange = TowerInfo.fireRange[(int)towerType, (int)towerLevel];
        fireDamage = TowerInfo.fireDamage[(int)towerType, (int)towerLevel];
        fireRate = TowerInfo.fireRate[(int)towerType, (int)towerLevel];
    }

    //攻击
    protected virtual void TryFire()
    {
        if (Time.time > nextFire)//当前时间超过下次攻击时间，表示可以开始攻击
        {
            nextFire = Time.time + realFireRateSpendSec;//计算下一次攻击时间
            ResetMinEnemyHealth();
            //找到最小血量的敌人
            for (int i = 0; i < m_EnemyTriggerList.Count; i++)
            {
                m_BasicEnemyTemp = m_EnemyTriggerList[i].GetComponent<BasicEnemy>();
                if(minEnemyHealth>m_BasicEnemyTemp.GetHealth())
                {
                    minEnemyHealth = m_BasicEnemyTemp.GetHealth();
                    m_BasicEnemyMinHealth = m_BasicEnemyTemp;
                }
            }
            if(m_BasicEnemyMinHealth!=null)
            {
                //攻击
                Debug.Log("Attack");
                //不同的塔攻击方式也不同，转到Fire函数以方便子类重写
                Fire();
                m_RigidbodyEnemy = m_BasicEnemyMinHealth.GetComponent<Rigidbody>();
            }
        }
    }

    protected virtual void Fire()
    {
        m_BasicEnemyMinHealth.TakeDamage(realFireDamage);
    }

    //重新计算总攻击伤害和频率
    public void RecalcInfo()
    {
        realFireDamage = fireDamage + buffDamage;
        realFireRate = fireRate * (1 + buffFireRate);
        realFireRateSpendSec = 1 / realFireRate;
    }

    //重置攻击伤害和频率加成
    public void ResetInfo()
    {
        buffDamage = 0;
        buffFireRate = 0;
    }

    //增加攻击伤害加成
    public void AddBuffDamage(float buffDamage)
    {
        this.buffDamage += buffDamage;
    }


    //增加攻击频率加成
    public void AddBuffFireRate(float buffFireRate)
    {
        this.buffFireRate += buffFireRate;
    }

    //作为EnemyDied事件的订阅者，当敌人死亡时将会受到来自GameManager发送的关于敌人死亡的信息
    public void RemoveEnemy(object sender,GameManager.EnemyDiedEventsArgs e)
    {
        //将死亡的敌人从敌人列表中移除
        if (m_EnemyTriggerList.Contains(e.enemyCollider))
        {
            m_EnemyTriggerList.Remove(e.enemyCollider);
            Debug.Log("Enemy Died And Removed");
        }   
    }
}
