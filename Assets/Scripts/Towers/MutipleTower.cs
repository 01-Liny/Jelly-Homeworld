using UnityEngine;
using System.Collections;

public class MutipleTower : BasicTower 
{
    [SerializeField]protected int mutipleTargetCount;//多重攻击目标个数
    protected new BasicEnemy[] m_BasicEnemyMinHealth;//临时存放最小血量的多个Enemy信息
    
    public override void Start()
    {
        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        this.towerType = TowerType.Mutiple;
        this.towerLevel = TowerLevel.One;
        RereadTowerInfo();
        m_AttackRangeCollider = GetComponent<SphereCollider>();
        m_AttackRangeCollider.radius = fireRange;//设置攻击范围
        ResetMinEnemyHealth();
        RecalcInfo();
    }
    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.Mutiple;
        base.Init(towerLevel);
    }
    
    protected override void RereadTowerInfo()
    {
        base.RereadTowerInfo();
        mutipleTargetCount = TowerInfo.mutipleTargetCount[(int)towerType, (int)towerLevel];
        m_BasicEnemyMinHealth = new BasicEnemy[mutipleTargetCount];
    }
    
    protected override void ResetMinEnemyHealth()
    {
        minEnemyHealth = 500000;
        for(int i=0;i<mutipleTargetCount;i++)
        {
            m_BasicEnemyMinHealth[i] = null;
        }
    }

    protected override void TryFire()
    {
        if (Time.time > nextFire||m_RigidbodyEnemy==null)//当前时间超过下次攻击时间或者没有攻击目标时，开始搜索攻击目标
        {
            ResetMinEnemyHealth();
            //找到最小血量的敌人
            for (int i = 0; i < m_EnemyTriggerList.Count; i++)
            {
                m_BasicEnemyTemp = m_EnemyTriggerList[i].GetComponent<BasicEnemy>();
                minEnemyHealth = GetEnemyMinHealth();
                if(minEnemyHealth>m_BasicEnemyTemp.GetHealth())
                {
                    PushNext(m_BasicEnemyTemp);
                    //m_BasicEnemyMinHealth = m_BasicEnemyTemp;
                }
            }
            if(m_BasicEnemyMinHealth[0]!=null)
            {
                //转向要攻击的目标
                m_RigidbodyEnemy = m_BasicEnemyMinHealth[0].GetComponent<Rigidbody>();
                
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

    protected float GetEnemyMinHealth()
    {

        for (int i = mutipleTargetCount - 1; i >= 0; i--)
        {
            if (m_BasicEnemyMinHealth[i] != null)
                return m_BasicEnemyMinHealth[i].GetHealth();
            else
            {
                //如果有空的位置
                return 500000;
            }
        }
        //如果没有数据
        return 500000;
    }
    //加入最小血量的塔到数组，血量过大的塔将会被移除
    protected void PushNext(BasicEnemy m_BasicEnemyTemp)
    {
        for(int i=0;i<mutipleTargetCount;i++)
        {
            if(m_BasicEnemyMinHealth[i]==null)
            {
                m_BasicEnemyMinHealth[i] = m_BasicEnemyTemp;
                return;
            }
            if(m_BasicEnemyMinHealth[i].GetHealth()>m_BasicEnemyTemp.GetHealth())
            {
                for(int j=mutipleTargetCount-2;j>=i;j--)
                {
                    m_BasicEnemyMinHealth[j + 1] = m_BasicEnemyMinHealth[j];
                }
                m_BasicEnemyMinHealth[i] = m_BasicEnemyTemp;
            }
        }
    }
    
    protected override void Fire()
    {
        for(int i=0;i<mutipleTargetCount;i++)
        {
            if(m_BasicEnemyMinHealth[i]!=null)
            {
                m_BasicEnemyMinHealth[i].TakeDamage(realFireDamage);
            }
        }
    }
}
