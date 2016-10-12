using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : MonoBehaviour 
{
    [Range(0, 100)]
    public float health;

    private float screenOffsetWidth;
    private float screenOffsetHeight;
    public Text text;

    private Image m_Image;
    private Color m_Color;

    private void Start()
    {
        //m_Image = GetComponent<Image>();
        //m_Color = new Color();
        //Vector3 t1 = new Vector3(1, 2, 3);
        //Vector2 t2 = (Vector2)t1;

        screenOffsetWidth = Screen.width;
        screenOffsetHeight = Screen.height;
        //text.GetComponent<Text>();

        text.text += screenOffsetWidth + " , " + screenOffsetHeight + " , " + Screen.dpi;
    }

    private void Update()
    {
        //m_Color = m_Image.color;
        //m_Color.r = (100-health) / 100.0f;
        //m_Color.g = health / 100.0f;
        //m_Color.b = 0;
        //m_Image.color = m_Color;
    }
}
