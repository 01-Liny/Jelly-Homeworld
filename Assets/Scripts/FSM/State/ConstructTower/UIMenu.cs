using UnityEngine;
using System.Collections;

public class UIMenu : MonoBehaviour
{
    private Canvas m_Canvas;

    //记录上一次timeScale的值
    private float timeScaleTemp;

    private bool isSpeedUp = false;
    private bool isPause = false;

    private void Awake()
    {
        m_Canvas = GetComponent<Canvas>();

    }

    public void Enable()
    {
        m_Canvas.enabled = true;
        timeScaleTemp = GameManager.GetTimeScale();
        //暂停游戏
        GameManager.SetTimeScale(0);
    }

    public void Disable()
    {
        m_Canvas.enabled = false;
        //继续游戏 恢复到以前的游戏速度
        GameManager.SetTimeScale(timeScaleTemp);
    }

    public void ToggleSpeedUp()
    {
        if(isSpeedUp)
        {
            isSpeedUp = false;
            if(isPause==false)
            {
                GameManager.SetTimeScale(1);
            }
        }
        else
        {
            isSpeedUp = true;
            if (isPause == false)
            {
                GameManager.SetTimeScale(2);
            }
        }
    }

    public void TogglePause()
    {
        if(isPause)
        {
            isPause = false;
            if(isSpeedUp)
            {
                GameManager.SetTimeScale(2);
            }
            else
            {
                GameManager.SetTimeScale(1);
            }
        }
        else
        {
            isPause = true;
            GameManager.SetTimeScale(0);
        }
    }
}
