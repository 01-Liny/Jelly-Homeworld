using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Atom : MonoBehaviour
{
    public List<Material> m_MaterialList;
    //默认只有三个元素球    代码已写死
    public List<GameObject> m_AtomList;
    public float scale=0.5f;
    Vector3 temp;
    public float radius=1.28f;

    //记录当前可使用的球的下标
    private int atomIndex = 0;

    private void Start()
    {
        temp = new Vector3();
        //初始不显示元素球  改代码会导致元素球无法显示 unity的开关有延迟 坑
        //for (int i = 1; i < 3; i++)
        //{
        //    //m_AtomList[i].SetActive(false);
        //    m_AtomList[i].GetComponent<Renderer>().enabled = false;
        //}
    }

    private void FixedUpdate()
    {
        //自旋转
        transform.Rotate(0, -Time.deltaTime * 20f, 0);

        //不应该出现在FixedUpdate中，方便测试       测试代码！！！！！！！！！！！！！！！！！！！
        temp.Set(scale, scale, scale);
        m_AtomList[0].transform.localScale = temp;
        m_AtomList[1].transform.localScale = temp;
        m_AtomList[2].transform.localScale = temp;

        Vector3 temp1 = new Vector3(radius, 0, 0);
        m_AtomList[0].transform.localPosition = Quaternion.Euler(0, 0, 0) * temp1;
        m_AtomList[1].transform.localPosition = Quaternion.Euler(0, 120, 0) * temp1;
        m_AtomList[2].transform.localPosition = Quaternion.Euler(0, 240, 0) * temp1;
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
        //Vector3 temp = new Vector3(radius, 0, 0);
        //m_AtomList[0].transform.localPosition = Quaternion.Euler(0, 0, 0) * temp;
        //m_AtomList[1].transform.localPosition = Quaternion.Euler(0, 120, 0) * temp;
        //m_AtomList[2].transform.localPosition = Quaternion.Euler(0, 240, 0) * temp;
    }

    public void SetHeight(float height)
    {
        Vector3 temp= transform.localPosition;
        temp.y = height;
        transform.localPosition = temp;
    }

    public void AddElem(TowerElem towerElem)
    {
        Renderer temp = m_AtomList[atomIndex].GetComponent<Renderer>();
        temp.enabled = true;
        //依赖于塔列表赋值时的顺序   已写死
        temp.material = m_MaterialList[(int)towerElem];
        atomIndex++;
    }
}
