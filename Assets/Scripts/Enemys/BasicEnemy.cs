using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemy : MonoBehaviour 
{
    public bool GoDie = false;
    [SerializeField]protected float health = 100;//血量
    [SerializeField]protected float armor = 20;//护甲
    [SerializeField]protected float slowdownRate = 0.0f;//减速百分比

    [SerializeField]
    private bool[] isStrikeArmor;//是否受到减甲
    [SerializeField]
    private bool[] isSlowdown;//是否受到减速

    [SerializeField]
    private float[] strikeArmorTime;//减甲失效时间
    [SerializeField]
    private float[] slowdownTime;//减速失效时间


    private bool isDied = false;
    private float damageTemp;
    private new Collider collider;

    // Use this for initialization
    void Start()
    {
        collider = GetComponent<Collider>();
        isStrikeArmor = new bool[(int)TowerLevel.MaxLevel];
        isSlowdown = new bool[(int)TowerLevel.MaxLevel];
        for (int i = 0; i < (int)TowerLevel.MaxLevel; i++)
        {
            isStrikeArmor[i] = false;
            isSlowdown[i] = false;
        }
        strikeArmorTime = new float[(int)TowerLevel.MaxLevel];
        slowdownTime = new float[(int)TowerLevel.MaxLevel];
    }

    // Update is called once per frame
    void Update()
    {
        if (GoDie == true)
        {
            Died();
            GoDie = false;
        }
        if (health <= 0 && isDied == false)
        {
            isDied = true;
            Died();
        }

        for (int i = 0; i < (int)TowerLevel.MaxLevel; i++)
        {
            //当debuff效果结束时
            if (Time.time >= strikeArmorTime[i] && isStrikeArmor[i] == true)
            {
                isStrikeArmor[i] = false;
                armor += TowerInfo.strikeArmor[(int)TowerType.Strike, i];
            }
            if (Time.time >= slowdownTime[i] && isSlowdown[i]==true)
            {
                isSlowdown[i] = false;
                slowdownRate -= TowerInfo.sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, i];
            }
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

    //受到伤害的时候调用，strikeArmorLevel表示破甲等级，默认没有受到减甲；slowdownLevel表示减速等级，默认没有受到减速；debuffDuringTime表示本次debuff持续时间，默认为1s
    public void TakeDamage(float fireDamage, float debuffDuringTime = 1f, TowerLevel strikeArmorLevel = TowerLevel.Empty, TowerLevel slowdownLevel = TowerLevel.Empty)
    {
        if (strikeArmorLevel != TowerLevel.Empty)
        {
            //如果在减甲效果内被同一等级的减甲塔再次施加效果，则只刷新时间
            strikeArmorTime[(int)strikeArmorLevel] = Time.time + debuffDuringTime;
            //不会再次扣除护甲
            if (isStrikeArmor[(int)strikeArmorLevel] == false)
            {
                isStrikeArmor[(int)strikeArmorLevel] = true;
                armor -= TowerInfo.strikeArmor[(int)TowerType.Strike, (int)strikeArmorLevel];
            }
        }

        //减速也是同理
        if (slowdownLevel != TowerLevel.Empty)
        {
            slowdownTime[(int)slowdownLevel] = Time.time + debuffDuringTime;
            if (isSlowdown[(int)slowdownLevel] == false)
            {
                isSlowdown[(int)slowdownLevel] = true;
                slowdownRate += TowerInfo.sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, (int)slowdownLevel];
            }
        }

        TakeHealth(fireDamage);
    }

    //计算扣血量
    protected void TakeHealth(float fireDamage)
    {
        if(armor>=0)
        {
            damageTemp = fireDamage * (1 - ((armor * 0.06f) / (1 + 0.06f * armor)));
        }    
        else
        {
            damageTemp = fireDamage * (1 + (2 - Mathf.Pow(0.94f, -armor)));
        }
        health -= damageTemp;

        Debug.Log("Damage:" + damageTemp + " Health:" + health);
    }

    public float GetHealth()
    {
        return health;
    }

}
