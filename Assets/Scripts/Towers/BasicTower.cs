using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTower : MonoBehaviour                                                     //该类应该为abstract
{
    public bool updateTower;
    public GameObject m_Hightlight;
    public GameObject m_Body;
    [SerializeField]public TowerType towerType;
    [SerializeField]public TowerLevel towerLevel;

    [SerializeField]protected float fireRange;//攻击范围
    [SerializeField]protected float fireDamage;//攻击伤害
    [SerializeField]protected float fireRate;//攻击频率
    [SerializeField]public float buffDamage;//攻击伤害加成
    [SerializeField]public float buffFireRate;//攻击频率加成

    [SerializeField]protected float realFireDamage;//总攻击伤害
    [SerializeField]protected float realFireRate;//总攻击频率
    [SerializeField]protected float realFireRateSpendSec;//总攻击一次需要多少秒

    protected SphereCollider m_AttackRangeCollider;//攻击范围collier

    protected float minEnemyHealth;//存储最小敌人血量，优先攻击最低血量的敌人
    protected BasicEnemy m_BasicEnemyTemp;//临时存放Enemy类信息
    protected BasicEnemy m_BasicEnemyMinHealth;//临时存放最小血量的Enemy信息
    protected Rigidbody m_RigidbodyEnemy;//临时存放被攻击的敌人的rigidbody信息


    protected float nextFire;//下次攻击的时间
    protected List<Collider> m_EnemyTriggerList = new List<Collider>();

    protected Material m_Material;
    //临时初始化函数                                                                           非正式代码，测试后删除
    // public virtual void Start()
    // {
    //     GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
    //     this.towerType = TowerType.Strike;
    //     this.towerLevel = TowerLevel.One;
    //     RereadTowerInfo();
    //     m_AttackRangeCollider = GetComponent<SphereCollider>();
    //     m_AttackRangeCollider.radius = fireRange;//设置攻击范围
    //     ResetMinEnemyHealth();
    //     RecalcInfo();
    // }

    public virtual void Awake()
    {
        //m_Hightlight.SetActive(false);
        //m_Material = m_Hightlight.GetComponent<Renderer>().material;

    }

    public void NoticeEnableUpdate()
    {
        Color temp = m_Material.color;
        temp.r = 255 / 255.0f;
        temp.g = 152 / 255.0f;
        temp.b = 0 / 255.0f;
        m_Material.color = temp;
    }

    public void NoticeEnableMerge()
    {
        Color temp = m_Material.color;
        temp.r = 3 / 255.0f;
        temp.g = 169 / 255.0f;
        temp.b = 244 / 255.0f;
        m_Material.color = temp;
    }

    protected virtual void ResetMinEnemyHealth()
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
        
        if(updateTower==true)
        {
            updateTower = false;
            UpdateTower();
        }

        m_Hightlight.transform.rotation = Quaternion.identity;
    }
    
    public virtual void UpdateTower()
    {
        //如果塔还没达到最高等级
        if(towerLevel+1!=TowerLevel.MaxLevel)
        {
            ResetLevel(towerLevel + 1);
        }
        else
        {
            Debug.LogError("TowerLevel reached Top!");
        }
    }

    protected void FixedUpdate()
    {
        //将塔的正面转向被攻击的敌人的位置
        if(m_RigidbodyEnemy!=null)
        {
            Vector3 relativePos = m_RigidbodyEnemy.position - transform.position;
            relativePos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,0.25f);
        }
    }

    //敌人出现在塔的攻击范围内，加入到敌人列表中
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!m_EnemyTriggerList.Contains(other))
            {
                m_EnemyTriggerList.Add(other);
            }
            Debug.Log("Enter");
        }
    }

    //敌人离开塔的攻击范围，将敌人从敌人列表中移除
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (m_EnemyTriggerList.Contains(other))
            {
                m_EnemyTriggerList.Remove(other);
            }
            //如果该塔攻击的敌人已经离开塔的范围，不再跟随该敌人旋转
            if (m_RigidbodyEnemy == other.GetComponent<Rigidbody>())
            {
                m_RigidbodyEnemy = null;
            }
            Debug.Log("Exit");
        }
    }


    //外部调用初始化
    public virtual void Init(TowerLevel towerLevel)
    {
        m_Material = m_Hightlight.GetComponent<Renderer>().material;
        Vector3 temp = m_Hightlight.transform.localScale;
        temp.Set(0.1f * MapManager.mapSize, 1, 0.1f * MapManager.mapSize);
        m_Hightlight.transform.localScale = temp;

        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        this.towerLevel = towerLevel;
        RereadTowerInfo();
        m_AttackRangeCollider = GetComponent<SphereCollider>();
        m_AttackRangeCollider.radius = fireRange;//设置攻击范围
        ResetMinEnemyHealth();
        RecalcFireInfo();
        m_Hightlight.SetActive(false);
        ResetLevel(towerLevel);
    }

    //重新设置塔等级
    public virtual void ResetLevel(TowerLevel towerLevel)
    {
        this.towerLevel = towerLevel;
        Vector3 temp = m_Body.transform.localScale;
        temp.Set(TowerInfo.towerLevelSize[(int)towerLevel, 0], TowerInfo.towerLevelSize[(int)towerLevel, 1], TowerInfo.towerLevelSize[(int)towerLevel, 2]);
        m_Body.transform.localScale = temp;
        temp = m_Body.transform.localPosition;
        temp.y = TowerInfo.towerLevelSize[(int)towerLevel, 3];
        m_Body.transform.localPosition = temp;
        RereadTowerInfo();
        m_AttackRangeCollider.radius = fireRange;
        RecalcFireInfo();
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
        if (Time.time > nextFire||m_RigidbodyEnemy==null)//当前时间超过下次攻击时间或者没有攻击目标时，开始搜索攻击目标
        {
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
                //转向要攻击的目标
                m_RigidbodyEnemy = m_BasicEnemyMinHealth.GetComponent<Rigidbody>();
                
                if(Time.time > nextFire)
                {
                    nextFire = Time.time + realFireRateSpendSec;//计算下一次攻击时间
                    //攻击
                    Debug.Log("Attack");
                    //不同的塔攻击方式也不同，转到Fire函数以方便子类重写
                    Fire();
                }
            }
        }
    }

    protected virtual void Fire()
    {
        //m_BasicEnemyMinHealth.TakeDamage(realFireDamage);
    }

    //重新计算总攻击伤害和频率
    public void RecalcFireInfo()
    {
        realFireDamage = fireDamage + buffDamage;
        realFireRate = fireRate * (1 + buffFireRate);
        realFireRateSpendSec = 1 / realFireRate;
    }

    //增加攻击伤害加成
    public void AddBuffDamage(float buffDamage)
    {
        this.buffDamage += buffDamage;
    }
    
    public void MinusBuffDamage(float buffDamage)
    {
        this.buffDamage -= buffDamage;
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
    
    public void ClearEnemyList()
    {
        m_EnemyTriggerList.Clear();
    }
}
