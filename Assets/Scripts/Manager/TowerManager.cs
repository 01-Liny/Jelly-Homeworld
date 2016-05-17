using UnityEngine;
using System.Collections;

public class TowerManager : MonoBehaviour 
{
    public  GameObject[] m_TowerPrefabs;
    private  GameObject m_SelectedPrefab;
    private GameObject m_Instance;
    private BasicTower m_InstanceBasicTower;

    public void RandomInstantiate(Vector3 m_Position)
    {
        int prefabCount=m_TowerPrefabs.Length;
        if(m_TowerPrefabs.Length!=0)
        {
            m_SelectedPrefab = m_TowerPrefabs[Random.Range(0, m_TowerPrefabs.Length)];
            m_Position.y = 1;
            m_Instance=Instantiate(m_SelectedPrefab, m_Position, Quaternion.identity)as GameObject;
            m_InstanceBasicTower = m_Instance.GetComponent<BasicTower>();
            m_InstanceBasicTower.Init(TowerLevel.One);//目前先定为一级，后续将会随机
        }
    }
}
