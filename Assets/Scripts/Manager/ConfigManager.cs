using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigManager : MonoBehaviour
{
    public JsonManager m_JsonManager;

    private Configuration m_Configuration;
    private List<UserRecord> m_UserRecord;

    private bool isEnableAdd = false;
    private int insertIndex=0;

    private int rankMaxCount = 7;

    public void Start()
    {
        if (m_JsonManager.m_Configuration != null)
        {
            m_Configuration = m_JsonManager.m_Configuration;
            m_UserRecord = m_Configuration.m_UserRecord;
        }          
    }

    //刚刚结算的游戏的分数是否大于已有的排行榜上的分数，大于的话返回true表示可以添加到排行榜
    public bool IsEnableAdd(int score)
    {
        //放在前面可以保证排序从大到小
        for (int i=0;i<m_UserRecord.Count;i++)
        {
            if(score>=m_UserRecord[i].score)
            {
                insertIndex = i;
                isEnableAdd = true;
                return true;
            }
        }

        //放在后面可以保证排序从大到小
        //如果排行榜记录数量没有达到最大值rankMaxCount，则直接添加
        if (m_UserRecord.Count < rankMaxCount)
        {
            insertIndex = m_UserRecord.Count;
            isEnableAdd = true;
            return true;
        }

        isEnableAdd = false;
        return false;
    }

    //必须经过isEnableAdd才能调用该函数
    public void Add(string name,int score)
    {
        if (isEnableAdd == false)
            return;
        UserRecord m_Temp = new UserRecord();
        m_Temp.name = name;
        m_Temp.score = score;
        m_UserRecord.Insert(insertIndex, m_Temp);
        //超过rankMaxCount个的时候，去掉分数最低的
        if (m_UserRecord.Count> rankMaxCount)
        {
            m_UserRecord.RemoveAt(rankMaxCount);
        }
    }

    //调用JsonManager的函数实现
    public void Save()
    {
        m_JsonManager.SaveConfig();
    }

    public string GetUserRecordWithString()
    {
        string temp="";
        for(int i=0;i<m_UserRecord.Count;i++)
        {
            temp += m_UserRecord[i].name + "\t" + m_UserRecord[i].score + "\n\n";
        }
        return temp;
    }
}

public class UserRecord
{
    public string name;
    public int score;
}

public class Configuration
{
    //记录游戏是否是玩家第一次运行，用来判断是否展示开场动画
    public bool isFirstRun;
    public List<UserRecord> m_UserRecord;

    public Configuration()
    {
        isFirstRun = true;
        m_UserRecord = new List<UserRecord>();
    }
}
