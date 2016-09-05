using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : MonoBehaviour 
{
    [Range(0, 100)]
    public float health;

    private Image m_Image;
    private Color m_Color;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        m_Color = new Color();
    }

    private void Update()
    {
        m_Color = m_Image.color;
        m_Color.r = (100-health) / 100.0f;
        m_Color.g = health / 100.0f;
        m_Color.b = 0;
        m_Image.color = m_Color;
    }
}
