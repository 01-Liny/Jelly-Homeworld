using UnityEngine;
using System.Collections;

public class StrikeTower : BasicTower 
{

    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.Strike;
        base.Init(towerLevel);
    }

    protected override void Fire()
    {
        m_BasicEnemyMinHealth.TakeDamage(realFireDamage,towerLevel);
    }
}
