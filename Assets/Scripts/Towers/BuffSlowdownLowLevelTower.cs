using UnityEngine;
using System.Collections;

public class BuffSlowdownLowLevelTower : BasicTower 
{
    public GameObject m_HighLevelTowerPrefab;
    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.SourceBuffSlowdown;
        base.Init(towerLevel);
    }
    
    protected override void Fire()
    {
        m_BasicEnemyMinHealth.TakeDamage(realFireDamage, TowerLevel.Empty, towerLevel);
    }
    
    public override void ResetLevel(TowerLevel towerLevel)
    {
        base.ResetLevel(towerLevel);
        if(this.towerLevel>TowerLevel.Two)
        {
            Destroy(this);
            Instantiate(m_HighLevelTowerPrefab, transform.position, transform.rotation);
        }
    }
    //未测试
}
