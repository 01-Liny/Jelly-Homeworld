using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISection : MonoBehaviour
{
    private Text m_Title;
    private Text m_Content;

    private void Awake()
    {
        m_Title = GetComponent<Text>();
        m_Content = transform.FindChild("Content").GetComponent<Text>();
    }

    public void SetInfo(ProducerElemInfo info)
    {
        gameObject.name = info.gameObjectName;
        m_Title.text = info.title;
        m_Content.text = info.content;
    }

    public void SetPosY(float Y)
    {
        Vector3 temp = transform.localPosition;
        temp.x = 0;
        temp.y = Y;
        temp.z = 0;
        transform.localPosition = temp;
    }
}
