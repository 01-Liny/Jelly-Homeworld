using UnityEngine;
using System.Collections;

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

    // Use this for initialization
    protected void Start () 
	{
        attackRangeCollider = GetComponent<SphereCollider>();
        attackRangeCollider.radius = fireRange;//设置攻击范围
        RecalcInfo();
    }
    
    protected void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            Fire();
        }
    }
    
    protected void Fire()
    {
        if (Time.time > nextFire)//当前时间超过下次攻击时间，表示可以开始攻击
        {
            nextFire = Time.time + fireRate;//计算下一次攻击时间
            Debug.Log("FireDamge:"+this.realFireDamage);
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
    

}
