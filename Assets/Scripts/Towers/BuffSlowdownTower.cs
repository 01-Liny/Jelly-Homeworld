using UnityEngine;
using System.Collections;

public class BuffSlowdownTower : BasicTower 
{
    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.SourceBuffSlowdown;
        base.Init(towerLevel);
    }
    
    protected override void Fire()
    {
        m_BasicEnemyMinHealth.TakeDamage(realFireDamage, TowerLevel.Empty, towerLevel);
    }
    
    //                              未完成！！！！
}
