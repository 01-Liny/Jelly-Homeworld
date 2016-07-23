using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour 
{
    public GameObject[] m_TowerPrefabs;
    public GameObject[] m_StonePrefabs;
    private  GameObject m_SelectedPrefab;
    private GameObject m_Instance;
    private BasicTower m_InstanceBasicTower;
    [SerializeField]private List<BasicTower> m_TowerList = new List<BasicTower>();
    [SerializeField]private List<GameObject> m_StoneList = new List<GameObject>();

    private RaycastHit m_Hit;

    public void RandomInstantiateTower(Vector3 m_Position)
    {
        if(m_TowerPrefabs.Length!=0)
        {
            m_SelectedPrefab = m_TowerPrefabs[Random.Range(0, m_TowerPrefabs.Length)];
            m_Position.y = 1.3f;
            m_Instance=Instantiate(m_SelectedPrefab, m_Position, Quaternion.identity)as GameObject;
            m_InstanceBasicTower = m_Instance.GetComponent<BasicTower>();
            m_InstanceBasicTower.Init(TowerLevel.One);//目前先定为一级，后续将会随机
            m_TowerList.Add(m_InstanceBasicTower);
        }
    }

    public void RandomInstantiateStone(Vector3 m_Position)
    {
        if(m_StonePrefabs.Length!=0)
        {
			m_SelectedPrefab=m_StonePrefabs[Random.Range(0, m_StonePrefabs.Length)];
            m_Position.y = 0.158f;
            m_Instance=Instantiate(m_SelectedPrefab,m_Position,Quaternion.identity)as GameObject;
            m_StoneList.Add(m_Instance);
        }
    }

    //已作废,留下备用
    public void DestroyStone(Ray ray)
    {
        //以ray的方向和原点，发射一条射线，检测射线是否碰撞到LayerMask为Stone的Collider
        if(Physics.Raycast(ray.origin, ray.direction, out m_Hit, Mathf.Infinity, 1 << 9))
        {
            //摧毁找到的石头
            Destroy(m_Hit.transform.gameObject);
        }
    }
}
