using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTower : MonoBehaviour                                                     //该类应该为abstract
{
    public GameObject m_Hightlight;
    public GameObject m_ForDetect;
    protected Material m_Material;
    public GameObject m_Body;
    public GameObject m_SelectedParticles;
    public GameObject m_RangeParticles;//每个塔都有可能变成范围塔
    public Transform m_FirePointTransform;

    [SerializeField]
    protected float fireRange;//攻击范围
    [SerializeField]
    protected float fireDamage;//攻击伤害
    [SerializeField]
    protected float fireRate;//攻击频率 每秒攻击几次
    [SerializeField]
    protected float fireRateSpendSec;//攻击频率 每次攻击花费几秒
    [SerializeField]
    protected float fireStrikeArmor;//破甲 破甲值为具体数值
    [SerializeField]
    protected float fireStunTime;//眩晕时间 单位秒
    [SerializeField]
    protected float fireStunProbability;//眩晕几率 1-1
    [SerializeField]
    protected float fireSlowdownDegree;//减速幅度 百分比减速
    [SerializeField]
    protected float fireSlowdownTime;//减速时间
    [SerializeField]
    protected bool isFireRange;//是否范围攻击

    protected SphereCollider m_AttackRangeCollider;//攻击范围collier

    protected float minEnemyHealth;//存储最小敌人血量，优先攻击最低血量的敌人
    protected BasicEnemy m_BasicEnemyTemp;//临时存放Enemy类信息
    protected BasicEnemy m_BasicEnemyMinHealth;//临时存放最小血量的Enemy信息
    protected Rigidbody m_RigidbodyEnemy;//临时存放被攻击的敌人的rigidbody信息

    protected float nextFire;//下次攻击的时间
    protected List<Collider> m_EnemyTriggerList = new List<Collider>();

    //元素信息
    [SerializeField]
    private int[] towerElemCount=new int[(int)TowerElem.MaxCount];
    //元素总共的个数
    private int elemCount;

    private Atom m_Atom;

    public void Init()
    {
        //初始化塔下方的Hightlight大小，并隐藏Hightlight
        m_Material = m_Hightlight.GetComponent<Renderer>().material;
        Vector3 temp = m_Hightlight.transform.localScale;
        temp.Set(0.1f * MapManager.mapSize, 1, 0.1f * MapManager.mapSize);
        m_ForDetect.transform.localScale = temp;
        m_Hightlight.transform.localScale = temp;
        m_Hightlight.SetActive(false);

        GameManager.EnemyDied += RemoveEnemy;//注册为订阅者
        m_AttackRangeCollider = GetComponent<SphereCollider>();

        //初始化元素数量表为空
        for (int i = 0; i < (int)TowerElem.MaxCount; i++)
        {
            towerElemCount[i] = 0;
        }
        elemCount = 0;
        //默认不是范围攻击
        isFireRange = false;

        m_Atom= transform.FindChild("Atoms").GetComponent<Atom>();
    }

    public void AddElem(TowerElem towerElem)
    {
        towerElemCount[(int)towerElem]++;
        elemCount++;
        m_Atom.AddElem(towerElem);
        //如果加入的是范围元素
        if (towerElem == TowerElem.Range)
        {
            //改变攻击方式
            isFireRange = true;
        }
    }

    public void RecalcInfo()
    {
        //含有范围攻击的塔属性会被削弱
        float offset = TowerElemInfo.extraFireRangeOffset[towerElemCount[(int)TowerElem.Range]];

        //根据现有元素数量决定当前塔的基础属性
        fireRange = TowerElemInfo.basicFireRange[elemCount];

        fireDamage = 0;
        for(int i=0;i<(int)TowerElem.MaxCount; i++)
        {
            fireDamage += TowerElemInfo.basicFireDamage[i] * towerElemCount[i];
        }
        
        fireRate = TowerElemInfo.basicFireRate[elemCount];

        fireRate += TowerElemInfo.extraFireRate[towerElemCount[(int)TowerElem.Rate]]* offset;
        fireRange += TowerElemInfo.extraFireRange[towerElemCount[(int)TowerElem.Range]];

        fireStrikeArmor = TowerElemInfo.strikeArmor[towerElemCount[(int)TowerElem.Strike]]* offset;
        fireStunTime = TowerElemInfo.stunTime[towerElemCount[(int)TowerElem.Stun]]* offset;
        fireStunProbability= TowerElemInfo.stunProbability[towerElemCount[(int)TowerElem.Stun]];
        fireSlowdownDegree = TowerElemInfo.slowdownDegree[towerElemCount[(int)TowerElem.Slowdown]]* offset;
        fireSlowdownTime = TowerElemInfo.slowdownTime[towerElemCount[(int)TowerElem.Slowdown]];

        //重新调整塔的攻击范围
        m_AttackRangeCollider.radius = fireRange;
        if (isFireRange)
            m_RangeParticles.GetComponent<ParticleSystem>().startSpeed = fireRange;


        //攻击一次需要多少秒
        fireRateSpendSec = 1 / fireRate;

        //重新计算塔的尺寸
        Vector3 temp = m_Body.transform.localScale;
        temp.Set(TowerElemInfo.towerSize[elemCount, 0], TowerElemInfo.towerSize[elemCount, 1], TowerElemInfo.towerSize[elemCount, 2]);
        m_Body.transform.localScale = temp;
        temp = m_Body.transform.localPosition;
        temp.y = TowerElemInfo.towerSize[elemCount, 3];
        m_Body.transform.localPosition = temp;

        //计算元素球高度
        m_Atom.SetHeight(TowerElemInfo.atomHeight[elemCount]);
    }

    public void NoticeEnableUpdate()
    {
        Color temp = m_Material.color;
        temp.r = 255 / 255.0f;
        temp.g = 152 / 255.0f;
        temp.b = 0 / 255.0f;
        //现在升级的塔的底座不会显示Hightlight提示
        //因为不一定要升级所有的塔，这要看玩家自己的取舍
        //但由于之前的升级塔判断规则，是否能升级需要Hightlight的启用
        //所以转成透明的material

        //测试阶段，先测试功能完整性，完成后移除注释

        //temp.a = 0;
        m_Material.color = temp;
    }

    public void NoticeEnableMerge()
    {
        Color temp = m_Material.color;
        temp.r = 3 / 255.0f;
        temp.g = 169 / 255.0f;
        temp.b = 244 / 255.0f;
        m_Material.color = temp;
    }

    private void ResetMinEnemyHealth()
    {
        minEnemyHealth = 500000;
        m_BasicEnemyMinHealth = null;
    }

    private void FixedUpdate()
    {
        //当敌人列表不为空时，尝试攻击
        if (m_EnemyTriggerList.Count > 0)
        {
            TryFire();
        }

        //将塔的正面转向被攻击的敌人的位置 
        //范围塔只会自旋转     
        if (m_RigidbodyEnemy != null && isFireRange == false)
        {
            Vector3 relativePos = m_RigidbodyEnemy.position - transform.position;
            relativePos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            m_Body.transform.rotation = Quaternion.Lerp(m_Body.transform.rotation, rotation, 0.25f);
        }

        //范围塔只会自旋转
        if (isFireRange)
        {
            //自旋转
            Quaternion autoRotation = Quaternion.AngleAxis(Time.deltaTime * 10f, Vector3.up);
            m_Body.transform.rotation = autoRotation * m_Body.transform.rotation;
        }

        m_Hightlight.transform.rotation = Quaternion.identity;
    }

    //攻击
    private void TryFire()
    {
        //当前时间超过下次攻击时间，开始搜索攻击目标
        //当变成范围塔时，m_RigidbodyEnemy失效
        if (isFireRange == false)
        {
            ResetMinEnemyHealth();
            //找到最小血量的敌人
            for (int i = 0; i < m_EnemyTriggerList.Count; i++)
            {
                m_BasicEnemyTemp = m_EnemyTriggerList[i].GetComponent<BasicEnemy>();
                //发现被要求集火的敌人
                if(m_BasicEnemyTemp.isFocused==true)
                {
                    m_BasicEnemyMinHealth = m_BasicEnemyTemp;
                    break;
                }
                if (minEnemyHealth > m_BasicEnemyTemp.GetHealth())
                {
                    minEnemyHealth = m_BasicEnemyTemp.GetHealth();
                    m_BasicEnemyMinHealth = m_BasicEnemyTemp;
                }
            }
            //虽然逻辑上不可能进入这个函数后，发现没有敌人
            //但是还是保险起见
            if (m_BasicEnemyMinHealth != null)
            {
                //转向要攻击的目标
                m_RigidbodyEnemy = m_BasicEnemyMinHealth.GetComponent<Rigidbody>();
            }
        }

        //如果达到攻击间隔，开始攻击
        if (Time.time > nextFire)
        {
            //如果是范围塔，攻击每个进入范围的敌人
            if (isFireRange)
            {
                for (int i = 0; i < m_EnemyTriggerList.Count; i++)
                {
                    m_BasicEnemyTemp = m_EnemyTriggerList[i].GetComponent<BasicEnemy>();
                    Fire(m_BasicEnemyTemp);
                }
                m_RangeParticles.GetComponent<ParticleSystem>().Stop();
                m_RangeParticles.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                //虽然逻辑上不可能进入这个函数后，发现没有敌人
                //但是还是保险起见
                if (m_BasicEnemyMinHealth != null)
                {
                    Fire(m_BasicEnemyMinHealth);
                }
            }
            nextFire = Time.time + fireRateSpendSec;//计算下一次攻击时间
        }
    }

    private void Fire(BasicEnemy m_FireTarget)
    {
        GameObject temp;
        //有一定几率触发眩晕
        int random = Random.Range(1, 101);
        if(!isFireRange)
        {
            temp = Instantiate(m_SelectedParticles, m_FirePointTransform.position, transform.rotation) as GameObject;
            if (random <= fireStunProbability)
            {
                temp.GetComponent<Bullet>().Init(fireDamage, fireStrikeArmor, fireStunTime, towerElemCount[(int)TowerElem.Slowdown], transform.position, m_FireTarget);
            }
            else
            {
                temp.GetComponent<Bullet>().Init(fireDamage, fireStrikeArmor, 0, towerElemCount[(int)TowerElem.Slowdown], transform.position, m_FireTarget);
            }
        }
        else
        {
            if (random <= fireStunProbability)
            {
                m_FireTarget.TakeDamage(fireDamage, fireStrikeArmor, fireStunTime, towerElemCount[(int)TowerElem.Slowdown], Vector3.zero, transform.position);
            }
            else
            {
                m_FireTarget.TakeDamage(fireDamage, fireStrikeArmor, 0, towerElemCount[(int)TowerElem.Slowdown], Vector3.zero, transform.position);
            }   
        }
    }

    //敌人出现在塔的攻击范围内，加入到敌人列表中
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!m_EnemyTriggerList.Contains(other))
            {
                m_EnemyTriggerList.Add(other);
            }
            Debug.Log("Enter");
        }
    }

    //敌人离开塔的攻击范围，将敌人从敌人列表中移除
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (m_EnemyTriggerList.Contains(other))
            {
                m_EnemyTriggerList.Remove(other);
            }
            //如果该塔攻击的敌人已经离开塔的范围，不再跟随该敌人旋转
            //当变成范围塔时，m_RigidbodyEnemy失效
            if (m_RigidbodyEnemy == other.GetComponent<Rigidbody>() && isFireRange == false)
            {
                m_RigidbodyEnemy = null;
            }
            Debug.Log("Exit");
        }
    }

    //作为EnemyDied事件的订阅者，当敌人死亡时将会受到来自GameManager发送的关于敌人死亡的信息
    public void RemoveEnemy(object sender, GameManager.EnemyDiedEventsArgs e)
    {
        //将死亡的敌人从敌人列表中移除
        if (m_EnemyTriggerList.Contains(e.enemyCollider))
        {
            m_EnemyTriggerList.Remove(e.enemyCollider);
            Debug.Log("Enemy Died And Removed");
        }
    }

    public void ClearEnemyList()
    {
        m_EnemyTriggerList.Clear();
    }

    public float GetTowerRange()
    {
        return fireRange;
    }

    private void OnDestroy()
    {
        GameManager.EnemyDied -= RemoveEnemy;//停止订阅
    }
}
