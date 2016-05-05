using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTower : MonoBehaviour 
{
    [SerializeField]protected float fireRange;//攻击范围
    [SerializeField]protected float fireDamage;//攻击伤害
    [SerializeField]protected float fireRate;//攻击频率
    [SerializeField]protected float buffDamage;//攻击伤害加成
    [SerializeField]protected float buffFireRate;//攻击频率加成

    protected float realFireDamage;//总攻击伤害
    protected float realFireRate;//总攻击频率
    protected SphereCollider attackRangeCollider;//攻击范围collier
    
    private float nextFire;//下次攻击的时间
    private List<Collider> enemyTriggerList = new List<Collider>();

    // Use this for initialization
    protected void Start () 
	{
        attackRangeCollider = GetComponent<SphereCollider>();
        attackRangeCollider.radius = fireRange;//设置攻击范围
        RecalcInfo();
    }
    
    protected void Update()
    {
        //当敌人列表不为空时，尝试攻击
        if(enemyTriggerList.Count>0)
        {
            TryFire();
        }
    }
    
    //敌人出现在塔的攻击范围内，加入到敌人列表中
    protected void OnTriggerEnter(Collider other)
    {
        if(!enemyTriggerList.Contains(other))
        {
            enemyTriggerList.Add(other);
        }
        Debug.Log("Enter");
    }
    
    //敌人离开塔的攻击范围，将敌人从敌人列表中移除
    protected void OnTriggerExit(Collider other)
    {
        if(enemyTriggerList.Contains(other))
        {
            enemyTriggerList.Remove(other);
        }
        Debug.Log("Exit");
    }
    
    //攻击
    protected void TryFire()
    {
        if (Time.time > nextFire)//当前时间超过下次攻击时间，表示可以开始攻击
        {
            nextFire = Time.time + fireRate;//计算下一次攻击时间
            for(int i=0;i<enemyTriggerList.Count;i++)
            {
                Debug.Log("Attack");
                enemyTriggerList[i].GetComponent<BasicEnemy>().TakeDamage(realFireDamage);         
            }
        }
    }
    
    //重新计算总攻击伤害和频率
    public void RecalcInfo()
    {
        realFireDamage = fireDamage + buffDamage;
        realFireRate = fireRate + buffFireRate;
    }
    
    //重置攻击伤害和频率加成
    public void ResetInfo()
    {
        buffDamage = 0;
        buffFireRate=0;
    }
    
    //增加攻击伤害加成
    public void AddBuffDamage(float buffDamage)
    {
        this.buffDamage += buffDamage;
    }
    
    
    //增加攻击频率加成
    public void AddBuffFireRate(float buffFireRate)
    {
        this.buffFireRate += buffFireRate;
    }
    
    //用于敌人死亡时调用，将敌人从检测到敌人的塔的敌人列表中移除。防止塔攻击死亡敌人
    public void RemoveMyselfFromEnemyList(Collider other)
    {
        Debug.Log(enemyTriggerList.Count);
        if(enemyTriggerList.Contains(other))
        {
            enemyTriggerList.Remove(other);
        }
        Debug.Log(enemyTriggerList.Count);
    }
}
