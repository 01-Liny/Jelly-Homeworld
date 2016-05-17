using UnityEngine;
using System.Collections;

public class BuffRateTower : BasicTower 
{
	public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.SourceBuffRate;
        base.Init(towerLevel);
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //if()
    }
}
