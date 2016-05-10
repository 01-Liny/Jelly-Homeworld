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

        GameManager.OnEnemyDied(new GameManager.EnemyDiedEventsArgs(collider));
        Debug.Log("Died");
		//死亡后自我销毁
        Destroy(this.gameObject);
    }
	
	public void TakeDamage(float fireDamage)
	{
        health -= fireDamage;
        Debug.Log("Health:" + health);
    }

    public float GetHealth()
    {
        return health;
    }
	
}
