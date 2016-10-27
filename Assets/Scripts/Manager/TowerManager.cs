using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public AudioManager m_AudioManager;
    public const int MaxTowerCount = 50;//测试代码 临时修改
    static public int remainTowerCount = MaxTowerCount;
    static public bool isOnUpdate = false;
    public GameObject[] m_TowerModules;
    public GameObject m_Tower;

    public GameObject[] m_StonePrefabs;
    public GameObject m_TeleportAPrefab;
    public GameObject m_TeleportBPrefab;
    private GameObject m_SelectedPrefab;
    private GameObject m_Instance;
    private Tower m_InstanceTower;
    [SerializeField]
    private List<Tower> m_TowerList = new List<Tower>();
    [SerializeField]
    private List<GameObject> m_StoneList = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_TeleportList = new List<GameObject>();

    private RaycastHit m_Hit;
    private int[,] towerLevelRandomMap;

    private void Start()
    {
        towerLevelRandomMap = new int[4, 4] {
            {100,100,100,100 } ,
            {75,100,100,100 } ,
            {50,75,100,100 } ,
            { 25,50,75,100} };
    }

    public void RandomInstantiateTower(Vector3 m_Position)
    {
        m_Position.y = 1.3f;
        m_Instance = Instantiate(m_Tower, m_Position, Quaternion.identity) as GameObject;
        m_InstanceTower = m_Instance.GetComponent<Tower>();
        m_InstanceTower.Init((TowerElem)Random.Range(0, (int)TowerElem.MaxCount), m_TowerModules);//随机添加一个元素
        //m_InstanceTower.Init(TowerElem.Stun, m_TowerModules);
        m_TowerList.Add(m_InstanceTower);
    }

    public void RandomInstantiateStone(Vector3 m_Position)
    {
        if (m_StonePrefabs.Length != 0)
        {
            m_SelectedPrefab = m_StonePrefabs[Random.Range(0, m_StonePrefabs.Length)];
            m_Position.y = 0.158f;
            m_Instance = Instantiate(m_SelectedPrefab, m_Position, Quaternion.identity) as GameObject;
            m_StoneList.Add(m_Instance);
        }
    }

    public void RandomInstantiateTeleport(MapType mapType, Vector3 m_Position)
    {
        if (mapType == MapType.TeleportA)
            m_SelectedPrefab = m_TeleportAPrefab;
        else
            m_SelectedPrefab = m_TeleportBPrefab;
        m_Position.y = 1f;
        m_Instance = Instantiate(m_SelectedPrefab, m_Position, Quaternion.identity) as GameObject;
        m_TeleportList.Add(m_Instance);
    }

    //删除石头模型实体
    public bool DestroyStone(Ray ray)
    {
        //以ray的方向和原点，发射一条射线，检测射线是否碰撞到LayerMask为Stone的Collider
        if (Physics.Raycast(ray.origin, ray.direction, out m_Hit, Mathf.Infinity, 1 << 9))
        {
            //Debug.Log("Casted");
            //将石头从石头列表中移除
            if (m_StoneList.Remove(m_Hit.transform.parent.gameObject))
                //Debug.Log("Removed From Stone List");
            //摧毁找到的石头
            Destroy(m_Hit.transform.parent.gameObject);
            return true;
        }
        return false;
    }

    public void ClearStoneList()
    {
        for (int i = 0; i < m_StoneList.Count; i++)
        {
            Destroy(m_StoneList[i]);
        }
        m_StoneList.Clear();
    }

    public void ClearTeleportList()
    {
        for (int i = 0; i < m_TeleportList.Count; i++)
        {
            Destroy(m_TeleportList[i]);
        }
        m_TeleportList.Clear();
    }

    public void ClearTowerList()
    {
        for (int i = 0; i < m_TowerList.Count; i++)
        {
            Destroy(m_TowerList[i]);
        }
        m_TowerList.Clear();
    }

    public void ClearAll()
    {
        ClearStoneList();
        ClearTeleportList();
        ClearTowerList();
    }

    public void RetrieveUpdatableTower()
    {
        DisableHightlight();

        for (int i = 0; i < m_TowerList.Count; i++)
        {
            //如果已经被激活说明已被之前的塔搜索过，跳过该塔
            if (m_TowerList[i].m_BasicTower.m_Hightlight.activeSelf == true)
                continue;
            //如果塔已经达到最大等级，则不能升级
            if ((int)m_TowerList[i].GetElemCount() >= 3)
                continue;
            for (int j = 0; j < m_TowerList.Count; j++)
            {
                if (i == j)
                    continue;
                if (m_TowerList[i].GetElemCount() == m_TowerList[j].GetElemCount())
                {
                    //能进入这个条件的塔，元素数量只有可能是1或2
                    //数量为1的直接标记

                    //写成这样 方便查阅 ref:KISS
                    if (m_TowerList[i].GetElemCount() == 1)
                    {
                        m_TowerList[i].m_BasicTower.NoticeEnableUpdate();
                        m_TowerList[i].m_BasicTower.m_Hightlight.SetActive(true);
                    }
                    if (m_TowerList[i].GetElemCount() == 2 && m_TowerList[j].GetMultipleElem() != TowerElem.NULL)
                    {
                        m_TowerList[i].m_BasicTower.NoticeEnableUpdate();
                        m_TowerList[i].m_BasicTower.m_Hightlight.SetActive(true);
                    }
                }
            }
        }
    }

    public void RetrieveMergeableTower(GameObject m_GameObject)
    {
        //进入升级塔模式          考虑是否需要创建状态机
        isOnUpdate = true;
        Tower m_UpdateTower = m_GameObject.GetComponent<Tower>();
        DisableHightlight();

        for (int i = 0; i < m_TowerList.Count; i++)
        {
            //要升级的塔自己不会高亮
            if (m_TowerList[i] != m_UpdateTower)
            {
                //元素一样多的塔才能升级
                if (m_UpdateTower.GetElemCount() == m_TowerList[i].GetElemCount())
                {
                    //能进入这个条件的塔，元素数量只有可能是1或2
                    //数量为1的直接标记

                    //写成这样 方便查阅 ref:KISS
                    if (m_UpdateTower.GetElemCount() == 1)
                    {
                        m_TowerList[i].m_BasicTower.NoticeEnableMerge();
                        m_TowerList[i].m_BasicTower.m_Hightlight.SetActive(true);
                    }
                    if (m_UpdateTower.GetElemCount() == 2 && m_TowerList[i].GetMultipleElem() != TowerElem.NULL)
                    {
                        m_TowerList[i].m_BasicTower.NoticeEnableMerge();
                        m_TowerList[i].m_BasicTower.m_Hightlight.SetActive(true);
                    }
                }
            }
        }
    }

    public void DisableHightlight()
    {
        for (int i = 0; i < m_TowerList.Count; i++)
        {
            m_TowerList[i].m_BasicTower.m_Hightlight.SetActive(false);
        }
    }

    public bool DestroyTower(GameObject m_DeleteTower)
    {
        Tower m_Temp = m_DeleteTower.GetComponent<Tower>();
        //将塔从塔列表中移除
        if (m_TowerList.Remove(m_Temp))
        {
            Destroy(m_DeleteTower);
            return true;
        }
        return false;
    }

    public void UpdateTower(GameObject m_UpdateTower, GameObject m_MergeableTower, bool isEnableReward)
    {
        Tower m_Mergeable = m_MergeableTower.GetComponent<Tower>();
        Tower m_Temp = m_UpdateTower.GetComponent<Tower>();
        m_AudioManager.PlayUpgrade();

        //在可奖励模式下，两个一样的新塔进行升级 会奖励一座新塔
        if (isEnableReward == true &&
            m_Mergeable.levelSign == UIGameLevel.level &&
            m_Mergeable.levelSign == m_Temp.levelSign &&
            m_Mergeable.GetElemCount() == m_Temp.GetElemCount() &&
            m_Mergeable.GetUpdateElem() == m_Temp.GetUpdateElem())
        {
            UIRemainTowerCount.AddTowerCount();
        }

        //得到升级元素并进行升级
        m_Temp.AddElem(m_Mergeable.GetUpdateElem());
        //退出升级模式
        isOnUpdate = false;
    }
}
