using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConstructState : MonoBehaviour
{
    private Button m_Button;
    private Image m_Image;

    //private SpriteState m_ST;
	static public bool isStone=false;

    //public Sprite m_HightlightedSprite;
    //public Sprite m_PressSprite;
    public Color m_Color;
    public Color m_Color2;

    private void Awake()
    {
        m_Button=GetComponent<Button>();
        m_Image = GetComponent<Image>();

        //m_Image.sprite = m_PressSprite;

        //m_ST = m_Button.spriteState;
        //m_ST.pressedSprite = m_PressSprite;
        //m_Button.spriteState = m_ST;
    }

    public void ToggleState()
    {
        if(isStone)
        {
            isStone = false;
            m_Image.color = m_Color;
            //m_Image.sprite = m_PressSprite;
            //m_ST.pressedSprite = m_PressSprite;
            //m_Button.spriteState = m_ST;
        }
        else
        {
            isStone = true;
            m_Image.color = m_Color2;
            //m_Image.sprite = m_HightlightedSprite;
            //m_ST.pressedSprite = m_HightlightedSprite;
            //m_Button.spriteState = m_ST;
        }
    }
}
