using UnityEngine;
using System.Collections;

public class StunTower : BasicTower 
{
	public override void Start()
    {
        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        this.towerType = TowerType.Stun;
        this.towerLevel = TowerLevel.One;
        RereadTowerInfo();
        m_AttackRangeCollider = GetComponent<SphereCollider>();
        m_AttackRangeCollider.radius = fireRange;//设置攻击范围
        ResetMinEnemyHealth();
        RecalcInfo();
    }
	public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.Stun;
        base.Init(towerLevel);
    }
	
	protected override void Fire()
    {
        m_BasicEnemyMinHealth.TakeDamage(realFireDamage);
        m_BasicEnemyMinHealth.TakeStun(towerLevel);
    }
}
