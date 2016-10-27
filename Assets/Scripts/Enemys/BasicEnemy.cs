using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BasicEnemy : MonoBehaviour 
{
    public GameObject m_SmogPrefab;
    public bool GoDie = false;
    public bool isFocused = false;//用来判断是否被标记为攻击目标
    public GameObject m_HitParticlesObject;
    private ParticleSystem m_HitParticles;

    [SerializeField]
    protected float isStun = 0;
    [SerializeField]
    protected float maxHealth = 100;//最初的血量
    [SerializeField]
    protected float health = 100;//当前血量
    [SerializeField]
    protected float maxArmor = 20;//最初的护甲
    [SerializeField]
    protected float armor = 20;//当前的护甲


    [SerializeField]
    protected float stunTime = 0;//眩晕时间


    [SerializeField]
    protected float restore = 5;//回血


    [SerializeField]
    private bool[] isSlowdown;//是否受到减速
    [SerializeField]
    protected float slowdownRate = 0.0f;//减速百分比
    [SerializeField]
    private float slowdownTime;//减速失效时间


    [SerializeField]
    private float LimitAttackTimes = 0;//限制别被攻击的次数
    private float CountAttackTimes = 0;//记录这段时间内被攻击的次数
    private float intervalAttack = 1;//1秒内受到一定次数的攻击

    [SerializeField]
    private Slider slider;
    private Image m_Image;

    private float startTime;//记录时间来判断一秒内攻击的次数

    private bool isDied = false;
    private float damageTemp;
    private Collider m_Collider;
    private MonsterWalk m_MonsterWalk;

    // Use this for initialization
    void Start() 
    {
        m_Collider = GetComponent<Collider>();
        m_MonsterWalk = GetComponent<MonsterWalk>();
        m_HitParticles=m_HitParticlesObject.GetComponent<ParticleSystem>();
        //isSlowdown = new bool[(int)TowerLevel.MaxLevel];
        //for (int i = 0; i < (int)TowerLevel.MaxLevel; i++)
        //{
        //    isSlowdown[i] = false;
        //}
        startTime = Time.time;

        m_Image = slider.transform.FindChild("Fill Area/Fill").GetComponent<Image>();
        health = maxHealth;
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

    }

    public void Init(float restore, float maxArmor, float speed, float isStun,float isSlownDown,float LimitAttackTimes,float EnemyHealth)
    {       
        m_MonsterWalk = GetComponent<MonsterWalk>();
        this.restore = restore;
        this.maxArmor = maxArmor;
        m_MonsterWalk.changeSpeed(speed);
        m_MonsterWalk.setIsSlownDown(isSlownDown);
        this.isStun = isStun;
        this.LimitAttackTimes = LimitAttackTimes;
        maxHealth = EnemyHealth;
    }

    void FixedUpdate()
    {

        if (Time.time - startTime >= intervalAttack)
        {
            CountAttackTimes = 0;
            intervalAttack += 1;
        }

        //怪物回血
        if (health < maxHealth)
        {
            health += restore * Time.deltaTime;
            health = health > maxHealth ? maxHealth : health;
        }
        slider.value = health / maxHealth;

        Color m_Color = m_Image.color;
        m_Color.r = (1- health / maxHealth);
        m_Color.g = health / maxHealth;
        m_Image.color = m_Color;
    }


    //敌人死后
    protected void Died()
    {
        //防止在敌人死亡的过程中出现在新的塔的范围内
        m_Collider.enabled = false;

        Vector3 temp = transform.position;
        temp.y = 0;
        Instantiate(m_SmogPrefab, temp, Quaternion.identity);

        //GameManager.OnEnemyDied(new GameManager.EnemyDiedEventsArgs(m_Collider));
        //Debug.Log("Died");
        //死亡后自我销毁
        Destroy(this.gameObject);
    }

    //受到伤害的时候调用，fireStrikeArmor表示破甲数值
    public void TakeDamage(float fireDamage, float fireStrikeArmor, float fireStunTime, int slowDownLevel,Vector3 m_HitPoint,Vector3 m_AttackerPotision)
    {
        if(m_HitPoint!= Vector3.zero && m_AttackerPotision!= Vector3.zero)
        {
            Vector3 relativePos = m_AttackerPotision- m_HitPoint ;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            m_HitParticlesObject.transform.position = m_HitPoint;
            m_HitParticlesObject.transform.rotation = rotation;
            m_HitParticles.Stop();
            m_HitParticles.Play();
        }
        stunTime = (isStun == 1 ? fireStunTime : 0);
        slowdownRate = TowerElemInfo.slowdownDegree[slowDownLevel];
        slowdownTime = TowerElemInfo.slowdownTime[slowDownLevel];
        //传眩晕时间，减速时间给MonsterWalk
        m_MonsterWalk.setFireStatus(stunTime, slowDownLevel);
        //判断是否免疫破甲
        if (armor % 2 == 1)
        {
            armor -= 1;
        }
        else
        {
            armor = maxArmor - fireStrikeArmor;
        }
        
        if (CountAttackTimes < LimitAttackTimes)
        {
            TakeHealth(fireDamage);
            CountAttackTimes++;
        }
        else
        {
            return;
        }
    }
    
    //public void TakeSlowdown(TowerLevel slowdownLevel)
    //{
    //        slowdownTime[(int)slowdownLevel] = Time.time + TowerInfo.debuffDuringTime[(int)TowerType.SourceBuffSlowdown, (int)slowdownLevel];
    //        if (isSlowdown[(int)slowdownLevel] == false)
    //        {
    //            isSlowdown[(int)slowdownLevel] = true;
    //            slowdownRate += TowerInfo.sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, (int)slowdownLevel];
    //        }
    //}

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
        UIScore.Add(damageTemp);
        //Debug.Log("Damage:" + damageTemp + " Health:" + health);
    }

    public void TakeStun(TowerLevel towerLevel)
    {

    }


    public float GetHealth()
    {
        return health;
    }

    //在销毁BasicEnemy脚本的同时，也会销毁整个gameObject,还会通知所有订阅者，也就是所有的塔
    protected void OnDestroy()
    {
        GameManager.OnEnemyDied(new GameManager.EnemyDiedEventsArgs(m_Collider));
        Destroy(this.gameObject);
    }
}
