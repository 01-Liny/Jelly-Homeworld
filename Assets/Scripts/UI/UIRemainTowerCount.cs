using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIRemainTowerCount : MonoBehaviour 
{
    static public int remainTowerCount { get; private set; }
    private static Text text;

    private void Start()
    {
        remainTowerCount = TowerManager.TowerCount;
        text = GetComponent<Text>();
    }

    public static void AddTowerCount()
    {
        remainTowerCount++;
        text.text = remainTowerCount.ToString();
    }

    public static void SubTowerCount()
    {
        remainTowerCount--;
        text.text = remainTowerCount.ToString();
    }

    public static void ResetTowerCount()
    {
        remainTowerCount = TowerManager.TowerCount;
        text.text = remainTowerCount.ToString();
    }

    //给UI使用
    public void Reset()
    {
        remainTowerCount = TowerManager.TowerCount;
        text.text = remainTowerCount.ToString();
    }
}
