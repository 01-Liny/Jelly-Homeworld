using UnityEngine;
using System.Collections;

public class TowerManager : MonoBehaviour 
{
    public  GameObject[] m_TowerPrefabs;
    private  GameObject m_SelectedPrefab;

    public void RandomInstantiate(Vector3 m_Position)
    {
        int prefabCount=m_TowerPrefabs.Length;
        if(m_TowerPrefabs.Length!=0)
        {
            m_SelectedPrefab = m_TowerPrefabs[Random.Range(0, m_TowerPrefabs.Length)];
            m_Position.y = 1;
            Instantiate(m_SelectedPrefab, m_Position, Quaternion.identity);
        }
    }
}
