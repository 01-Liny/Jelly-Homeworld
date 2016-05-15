using UnityEngine;
using System.Collections;

public class StunTower : BasicTower 
{
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
