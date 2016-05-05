using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemy : MonoBehaviour 
{
    public bool GoDie = false;
    [SerializeField]protected float health = 100;
    [SerializeField]protected float armor = 10;

    private bool isDied = false;
    private new Collider collider;
	private List<Collider> towerTriggerList = new List<Collider>();
    // Use this for initialization
    void Start () 
	{
        collider=GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(GoDie==true)
		{
            Died();
            GoDie = false;
        }
		if(health<=0&&isDied==false)
		{
            isDied = true;
            Died();
        }
	}
	
	//敌人死后
	protected void Died()
	{
		//防止在敌人死亡的过程中出现在新的塔的范围内
        collider.enabled = false;
		
        for(int i=0;i<towerTriggerList.Count;i++)
		{
			//通过塔的collider找到BasicTower脚本，调用RemoveMyselfFromEnemyList函数将敌人从列表中移除
            towerTriggerList[i].GetComponent<BasicTower>().RemoveMyselfFromEnemyList(collider);
        }
        Debug.Log("Died");
		//死亡后自我销毁
        Destroy(this.gameObject);
    }
	
	public void TakeDamage(float fireDamage)
	{
        health -= fireDamage;
        Debug.Log("Health:" + health);
    }
	
	//敌人出现在塔的攻击范围内，加入到塔列表中
    protected void OnTriggerEnter(Collider other)
    {
        if(!towerTriggerList.Contains(other))
        {
            towerTriggerList.Add(other);
        }
        Debug.Log("Tower Enter");
    }
    
    //敌人离开塔的攻击范围，将塔从塔列表中移除
    protected void OnTriggerExit(Collider other)
    {
        if(towerTriggerList.Contains(other))
        {
            towerTriggerList.Remove(other);
        }
        Debug.Log("Tower Exit");
    }
	
}
