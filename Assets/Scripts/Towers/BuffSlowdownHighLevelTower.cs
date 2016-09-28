using UnityEngine;
using System.Collections;

public class BuffSlowdownHighLevelTower : BasicTower
{
    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.SourceBuffSlowdown;
        base.Init(towerLevel);
    }

    protected override void Fire()
    {
        //m_BasicEnemyMinHealth.TakeDamage(realFireDamage);
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //other.GetComponent<BasicEnemy>().TakeSlowdown(towerLevel);
            Debug.Log("Stay");
        }
    }
}
