using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseState : MonoBehaviour
{
    private Image m_Image;

    //private SpriteState m_ST;
    static public bool isPause = false;

    //public Sprite m_HightlightedSprite;
    //public Sprite m_PressSprite;
    public Color m_Color;
    public Color m_Color2;

    private void Awake()
    {
        m_Image = GetComponent<Image>();

        //m_Image.sprite = m_PressSprite;

        //m_ST = m_Button.spriteState;
        //m_ST.pressedSprite = m_PressSprite;
        //m_Button.spriteState = m_ST;
    }

    public void ToggleState()
    {
        if (isPause)
        {
            isPause = false;
            m_Image.color = m_Color;
            //m_Image.sprite = m_PressSprite;
            //m_ST.pressedSprite = m_PressSprite;
            //m_Button.spriteState = m_ST;
        }
        else
        {
            isPause = true;
            m_Image.color = m_Color2;
            //m_Image.sprite = m_HightlightedSprite;
            //m_ST.pressedSprite = m_HightlightedSprite;
            //m_Button.spriteState = m_ST;
        }
    }
}
