using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    public GameObject[] m_TowerModules;//从TowerManager赋值过来的塔模型的列表
    public BasicTower m_BasicTower;

    private int[] towerElemCount;
    private GameObject m_CurrentModule;

    [SerializeField]
    private TowerElem currentModuleID;
    [SerializeField]
    private int elemCount = 0;//所有元素数量 最大3个

    private TowerElem singleElem = TowerElem.NULL;//用于升级别的塔时用的元素
    private TowerElem multipleElem = TowerElem.NULL;//用于升级别的塔时用的元素

    public void Init(TowerElem towerElem, GameObject[] m_TowerModules)
    {
        this.m_TowerModules = m_TowerModules;
        towerElemCount = new int[(int)TowerElem.MaxCount];
        for (int i = 0; i < (int)TowerElem.MaxCount; i++)
        {
            towerElemCount[i] = 0;
        }
        //初始加入一个元素
        singleElem = towerElem;
        currentModuleID = towerElem;
        elemCount++;
        towerElemCount[(int)towerElem] = 1;
        InitBasicTower();
    }

    public void AddElem(TowerElem towerElem)
    {
        elemCount++;
        towerElemCount[(int)towerElem]++;

        //如果加入的元素后，同类元素数量大于等于2，表示当前该塔拥有相同元素，可用于升级3级塔
        if (towerElemCount[(int)towerElem] >= 2)
        {
            multipleElem = towerElem;
            //如果同种元素大于等于2，而且塔的模型不是该种元素的模型
            if (currentModuleID != towerElem)
            {
                //换模型
                Destroy(m_CurrentModule);
                currentModuleID = towerElem;
                InitBasicTower();
                return;
            }
        }
        m_BasicTower.AddElem(towerElem);
        m_BasicTower.RecalcInfo();
    }

    //根据currentModuleID生成该种类的塔
    private void InitBasicTower()
    {
        m_CurrentModule = Instantiate(m_TowerModules[(int)currentModuleID], transform.position, transform.rotation) as GameObject;
        //BasicTower附在Tower上
        m_CurrentModule.transform.parent = transform;

        m_BasicTower = m_CurrentModule.GetComponent<BasicTower>();
        m_BasicTower.Init();
        for (int i = 0; i < (int)TowerElem.MaxCount; i++)
        {
            for (int j = 0; j < towerElemCount[i]; j++)
                m_BasicTower.AddElem((TowerElem)i);//添加元素
        }
        m_BasicTower.RecalcInfo();//重新计算塔信息
    }

    public int GetElemCount()
    {
        return elemCount;
    }

    public TowerElem GetMultipleElem()
    {
        return multipleElem;
    }

    public TowerElem GetUpdateElem()
    {
        if (elemCount == 1)
        {
            return singleElem;
        }
        if (elemCount == 2 && multipleElem != TowerElem.NULL)
        {
            return multipleElem;
        }
        Debug.LogError("GetUpdateElem() Error invalid invoke");
        return TowerElem.NULL;
    }

    //在销毁Tower脚本的同时，也会销毁整个gameObject
    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
