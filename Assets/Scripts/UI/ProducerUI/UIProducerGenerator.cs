using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class UIProducerGenerator : MonoBehaviour, IEndDragHandler,IBeginDragHandler
{
    public GameObject m_UISectionPrefab;
    public Transform m_ContentTransfer;
    private List<ProducerElemInfo> m_ProducerList=new List<ProducerElemInfo>();

    private float lastY=0;
    private bool isDrag=false;
    private bool isEnable = false;

    private void Start()
    {
        GameObject tempGameObject;
        float temp;
        Init();
        for(int i=0;i<m_ProducerList.Count;i++)
        {
            tempGameObject = Instantiate(m_UISectionPrefab)as GameObject;
            tempGameObject.transform.parent = m_ContentTransfer;
            tempGameObject.GetComponent<UISection>().SetInfo(m_ProducerList[i]);
            tempGameObject.GetComponent<UISection>().SetPosY(lastY);
            switch (m_ProducerList[i].count)
            {
                case 1:
                    temp = 208;
                    break;
                case 2:
                    temp = 253;
                    break;
                default:
                    temp = 208;
                    break;
            }
            lastY -= temp;       
        }
    }


    private void FixedUpdate()
    {
        if(isDrag||isEnable==false)
            return;

        Vector3 temp = m_ContentTransfer.localPosition;
        if (temp.y < -658)
        {
            temp.y=Mathf.Lerp(temp.y, -648, 0.25f);
        }
        else if(temp.y==3175)
        {
            return;
        }
        else if(temp.y > 3175)
        {
            temp.y = Mathf.Lerp(temp.y, 3175, 0.25f);
        }
        else
        {
            temp.y += 36.4f * Time.deltaTime;
        }

        m_ContentTransfer.localPosition = temp;  
    }

    public void Enable()
    {
        isEnable = true;
        Vector3 temp = m_ContentTransfer.localPosition;
        temp.y = -658;
        m_ContentTransfer.localPosition = temp;
    }

    public void Diable()
    {
        isEnable = false;
    }

    private void Init()
    {
        m_ProducerList.Add(new ProducerElemInfo("Lead Programmer", "游戏主程", "谢邦晟", 1));
        m_ProducerList.Add(new ProducerElemInfo("Programmer", "程序编写", "林毅\n谢邦晟", 2));
        m_ProducerList.Add(new ProducerElemInfo("Creative Designer", "创意设计", "林毅\n谢邦晟", 2));
        m_ProducerList.Add(new ProducerElemInfo("Character Designer", "人物设计", "许力丹", 1));
        m_ProducerList.Add(new ProducerElemInfo("Model Designer", "模型设计", "陈璐", 1));
        m_ProducerList.Add(new ProducerElemInfo("UI Designer", "界面设计", "曾文泽", 1));
        m_ProducerList.Add(new ProducerElemInfo("Map Designer", "地图设计", "待定", 1));
        m_ProducerList.Add(new ProducerElemInfo("Balance Tester", "平衡测试", "林毅", 1));
        m_ProducerList.Add(new ProducerElemInfo("Effects Designer", "特效设计", "谢邦晟", 1));
        m_ProducerList.Add(new ProducerElemInfo("Sound Designer", "音效设计", "谢邦晟", 1));
        //m_ProducerList.Add(new ProducerElemInfo("Character Designer", "人物设计", "许力丹", 1));
        //m_ProducerList.Add(new ProducerElemInfo("Character Designer", "人物设计", "许力丹", 1));
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }
}

public class ProducerElemInfo
{
    public string gameObjectName;
    public string title;
    public string content;
    public int count;//人数

    public ProducerElemInfo(string gameObjectName, string title, string content, int count)
    {
        this.gameObjectName = gameObjectName;
        this.title = title;
        this.content = content;
        this.count = count;
    }
}
