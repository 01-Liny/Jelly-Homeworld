using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectTowerState : MonoBehaviour ,IState
{
	public FSM FSMConstruct;
    public MapManager m_MapManager;
    public TowerManager m_TowerManager;
    public ConstructUIController m_ConstructUIController;
    private Image m_Image;

    private void Start()
    {
        m_Image = GetComponent<Image>();
        FSMConstruct.Register("Tower", this);
    }

    #region IState Members

    public void OnEnter(string prevState)
    {
        m_Image.color = Color.green;
    }
    public void OnExit(string nextState)
    {
        m_Image.color=Color.white;
    }
    public void OnUpdate()
    {
        
    }

	public void OnStart()
	{
        Vector2 m_MapPos=m_ConstructUIController.m_MapPos;
        //如果可以添加到地图
        if(m_MapManager.ModifyMap((int)m_MapPos.x,(int)m_MapPos.y,MapType.Tower))
        {
            //摧毁石头
            //m_TowerManger.DestroyStone(m_ConstructUIController.m_Ray);
            //建造防御塔
            m_TowerManager.RandomInstantiateTower(m_ConstructUIController.transform.position);
        }
	}

    #endregion
}
