using UnityEngine;
using System.Collections;

public class StrikeTower : BasicTower 
{
    [SerializeField]protected float strikeArmor;//减甲

    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.Strike;
        base.Init(towerLevel);
    }

    protected override void RereadTowerInfo()
    {
        base.RereadTowerInfo();
        strikeArmor = TowerInfo.strikeArmor[(int)towerType, (int)towerLevel];//实际不用这些数据
    }

    protected override void Fire()
    {
        m_BasicEnemyMinHealth.TakeDamage(realFireDamage,1f,towerLevel,TowerLevel.Empty);
    }
}
