using UnityEngine;
using System.Collections;

public class BuffSlowdownLowLevelTower : BasicTower 
{
    public GameObject m_HighLevelTowerPrefab;
    private GameObject m_Instance;
    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.SourceBuffSlowdown;
        base.Init(towerLevel);
    }
    
    protected override void Fire()
    {
        m_BasicEnemyMinHealth.TakeDamage(realFireDamage);
        m_BasicEnemyMinHealth.TakeSlowdown(towerLevel);
    }
    
    public override void ResetLevel(TowerLevel towerLevel)
    {
        base.ResetLevel(towerLevel);
        if(this.towerLevel>TowerLevel.Two)
        {
            Destroy(gameObject);
            m_Instance=Instantiate(m_HighLevelTowerPrefab, transform.position, transform.rotation)as GameObject;
            m_Instance.GetComponent<BasicTower>().Init(TowerLevel.Three);
        }
    }
}
