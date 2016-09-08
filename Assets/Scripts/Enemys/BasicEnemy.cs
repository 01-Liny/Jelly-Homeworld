using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour 
{
    public bool GoDie = false;
    [SerializeField]
    protected float maxHealth = 100;//最初的血量
    [SerializeField]protected float health = 100;//血量
    [SerializeField]protected float armor = 20;//护甲
    [SerializeField]protected float slowdownRate = 0.0f;//减速百分比
    [SerializeField]
    protected int restore = 5;//回血

    [SerializeField]
    private bool[] isStrikeArmor;//是否受到减甲
    [SerializeField]
    private bool[] isSlowdown;//是否受到减速

    [SerializeField]
    private float[] strikeArmorTime;//减甲失效时间
    [SerializeField]
    private float[] slowdownTime;//减速失效时间
    [SerializeField]
    private Slider slider;

    private bool isDied = false;
    private float damageTemp;
    private Collider m_Collider;

    // Use this for initialization
    void Start()
    {
        m_Collider = GetComponent<Collider>();
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

    void FixedUpdate()
    {
        if (health < maxHealth)
        {
            health += restore * Time.deltaTime;
            health = health > maxHealth ? maxHealth : health;
        }
    }

    //敌人死后
    protected void Died()
    {
        //防止在敌人死亡的过程中出现在新的塔的范围内
        m_Collider.enabled = false;

        GameManager.OnEnemyDied(new GameManager.EnemyDiedEventsArgs(m_Collider));
        Debug.Log("Died");
        //死亡后自我销毁
        Destroy(this.gameObject);
    }

    //受到伤害的时候调用，strikeArmorLevel表示破甲等级，默认没有受到减甲；
    public void TakeDamage(float fireDamage, TowerLevel strikeArmorLevel = TowerLevel.Empty)
    {
        if (strikeArmorLevel != TowerLevel.Empty)
        {
            //如果在减甲效果内被同一等级的减甲塔再次施加效果，则只刷新时间
            strikeArmorTime[(int)strikeArmorLevel] = Time.time + TowerInfo.debuffDuringTime[(int)TowerType.Strike, (int)strikeArmorLevel];
            //不会再次扣除护甲
            if (isStrikeArmor[(int)strikeArmorLevel] == false)
            {
                isStrikeArmor[(int)strikeArmorLevel] = true;
                armor -= TowerInfo.strikeArmor[(int)TowerType.Strike, (int)strikeArmorLevel];
            }
        }

        TakeHealth(fireDamage);
    }
    
    public void TakeSlowdown(TowerLevel slowdownLevel)
    {
            slowdownTime[(int)slowdownLevel] = Time.time + TowerInfo.debuffDuringTime[(int)TowerType.SourceBuffSlowdown, (int)slowdownLevel];
            if (isSlowdown[(int)slowdownLevel] == false)
            {
                isSlowdown[(int)slowdownLevel] = true;
                slowdownRate += TowerInfo.sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, (int)slowdownLevel];
            }
    }

    //计算扣血量
    protected void TakeHealth(float fireDamage)
    {
        //护甲大于0和小于0时的计算方式不一样
        if(armor>=0)
        {
            damageTemp = fireDamage * (1 - ((armor * 0.06f) / (1 + 0.06f * armor)));
        }    
        else
        {
            damageTemp = fireDamage * (1 + (2 - Mathf.Pow(0.94f, -armor)));
        }
        health -= damageTemp;
        slider.value = health / maxHealth;
        UIScore.Add(damageTemp);
        Debug.Log("Damage:" + damageTemp + " Health:" + health);
    }

    public void TakeStun(TowerLevel towerLevel)
    {

    }


    public float GetHealth()
    {
        return health;
    }

}
