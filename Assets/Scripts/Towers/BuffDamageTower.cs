using UnityEngine;
using System.Collections.Generic;

public class BuffDamageTower : BasicTower 
{
    private List<BasicTower> m_TowerList = new List<BasicTower>();
    private BasicTower m_TowerTemp;
    
    public override void Init(TowerLevel towerLevel)
    {
        towerType = TowerType.SourceBuffDamage;
        base.Init(towerLevel);
        buffDamage += TowerInfo.sourceBuffDamage[(int)towerType, (int)towerLevel];
        RecalcFireInfo();
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(other.tag=="Tower")
        {
            m_TowerTemp = other.GetComponentInParent<BasicTower>();
            if(!m_TowerList.Contains(m_TowerTemp))
            {
                //增加buff效果并重新计算塔的攻击伤害和频率
                m_TowerList.Add(m_TowerTemp);
                m_TowerTemp.buffDamage += TowerInfo.sourceBuffDamage[(int)towerType, (int)towerLevel];
                m_TowerTemp.RecalcFireInfo();
            }
            Debug.Log("Tower Enter");
        }
    }
    
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if(other.tag=="Tower")
        {
            m_TowerTemp = other.GetComponentInParent<BasicTower>();
            if(m_TowerList.Contains(m_TowerTemp))
            {
                m_TowerList.Remove(m_TowerTemp);
            }
            Debug.Log("Tower Exit");
        }
    }
    
    public override void ResetLevel(TowerLevel towerLevel)
    {
        for (int i = 0; i < m_TowerList.Count; i++)
        {
            m_TowerList[i].buffDamage -= TowerInfo.sourceBuffDamage[(int)towerType, (int)this.towerLevel];
            m_TowerList[i].buffDamage += TowerInfo.sourceBuffDamage[(int)towerType, (int)towerLevel];
            m_TowerList[i].RecalcFireInfo();
        }
        base.ResetLevel(towerLevel);
    }

    //当塔被合并时，原有塔将会被销毁，被销毁的塔的buff效果也会实效
    private void OnDestroy()
    {
        for (int i = 0; i < m_TowerList.Count; i++)
        {
            m_TowerList[i].buffDamage -= TowerInfo.sourceBuffDamage[(int)towerType, (int)towerLevel];
            m_TowerList[i].RecalcFireInfo();
        }
    }
}
