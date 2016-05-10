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

    protected SphereCollider attackRangeCollider;//攻击范围collier

    private float nextFire;//下次攻击的时间
    private List<Collider> enemyTriggerList = new List<Collider>();
    private float minEnemyHealth;//存储最小敌人血量，优先攻击最低血量的敌人
    private BasicEnemy basicEnemyTemp;//临时存放Enemy类信息
    private BasicEnemy basicEnemyMinHealth;//临时存放最小血量的Enemy信息
    private Rigidbody rigidbodyEnemy;//临时存放被攻击的敌人的rigidbody信息

    //临时初始化函数                                                                           非正式代码，测试后删除
    public void Start()
    {
        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        this.towerType = TowerType.Strike;
        this.towerLevel = TowerLevel.One;
        RereadTowerInfo();
        attackRangeCollider = GetComponent<SphereCollider>();
        attackRangeCollider.radius = fireRange;//设置攻击范围
        ResetMinEnemyHealth();
        RecalcInfo();
    }

    protected void ResetMinEnemyHealth()
    {
        minEnemyHealth = 500000;
        basicEnemyMinHealth = null;
    }

    protected void Update()
    {
        //当敌人列表不为空时，尝试攻击
        if (enemyTriggerList.Count > 0)
        {
            TryFire();
        }
    }

    protected void FixedUpdate()
    {
        //将塔的正面转向被攻击的敌人的位置
        if(rigidbodyEnemy!=null)
        {
            Vector3 relativePos = rigidbodyEnemy.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation,0.25f);
        }
    }

    //敌人出现在塔的攻击范围内，加入到敌人列表中
    protected void OnTriggerEnter(Collider other)
    {
        if (!enemyTriggerList.Contains(other))
        {
            enemyTriggerList.Add(other);
        }
        Debug.Log("Enter");
    }

    //敌人离开塔的攻击范围，将敌人从敌人列表中移除
    protected void OnTriggerExit(Collider other)
    {
        if (enemyTriggerList.Contains(other))
        {
            enemyTriggerList.Remove(other);
        }
        //如果该塔攻击的敌人已经离开塔的范围，不再跟随该敌人旋转
        if(rigidbodyEnemy==other.GetComponent<Rigidbody>())
        {
            rigidbodyEnemy = null;
        }
        Debug.Log("Exit");
    }


    //外部调用初始化
    public virtual void Init(TowerLevel towerLevel)
    {
        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        this.towerLevel = towerLevel;
        RereadTowerInfo();
        attackRangeCollider = GetComponent<SphereCollider>();
        attackRangeCollider.radius = fireRange;//设置攻击范围
        ResetMinEnemyHealth();
        RecalcInfo();
    }

    //重新设置塔等级
    public void ResetLevel(TowerLevel towerLevel)
    {
        this.towerLevel = towerLevel;
        RereadTowerInfo();
    }

    //重新读取塔数据                                                                   该函数应加abstract，非正式代码，测试后删除
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
            for (int i = 0; i < enemyTriggerList.Count; i++)
            {
                basicEnemyTemp = enemyTriggerList[i].GetComponent<BasicEnemy>();
                if(minEnemyHealth>basicEnemyTemp.GetHealth())
                {
                    minEnemyHealth = basicEnemyTemp.GetHealth();
                    basicEnemyMinHealth = basicEnemyTemp;
                }
            }
            if(basicEnemyMinHealth!=null)
            {
                //攻击
                Debug.Log("Attack");
                basicEnemyMinHealth.TakeDamage(realFireDamage);
                rigidbodyEnemy = basicEnemyMinHealth.GetComponent<Rigidbody>();
            }
        }
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

    //用于敌人死亡时调用，将敌人从检测到敌人的塔的敌人列表中移除。防止塔攻击死亡敌人
    //public void RemoveMyselfFromEnemyList(Collider other)
    //{
    //    Debug.Log(enemyTriggerList.Count);
    //    if (enemyTriggerList.Contains(other))
    //    {
    //        enemyTriggerList.Remove(other);
    //    }
    //    Debug.Log(enemyTriggerList.Count);
    //}

    //
    public void RemoveEnemy(object sender,GameManager.EnemyDiedEventsArgs e)
    {
        if (enemyTriggerList.Contains(e.enemyCollider))
        {
            enemyTriggerList.Remove(e.enemyCollider);
            Debug.Log("Enemy Died And Removed");
        }   
    }
}
