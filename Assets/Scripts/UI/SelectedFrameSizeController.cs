using UnityEngine;
using System.Collections;

public class SelectedFrameSizeController : MonoBehaviour 
{
    public LineRenderer m_LineRenderer;
    public Transform m_OffsetTransform;
    private Vector3 m_VertexTemp = new Vector3();
    private int frameSize;
    private Vector3 offset=new Vector3();
    private void Start()
    {
        frameSize = MapManager.mapSize;
        
        //修改偏移量
        offset.Set(-frameSize / 2.0f, 0, -frameSize / 2.0f);
        m_OffsetTransform.localPosition = offset;
        
        //画线
        m_LineRenderer.SetVertexCount(5);
        
        m_LineRenderer.SetPosition(0, m_VertexTemp);
        m_LineRenderer.SetPosition(4, m_VertexTemp);
        
        m_VertexTemp.Set(frameSize, 0, 0);
        m_LineRenderer.SetPosition(1, m_VertexTemp);
        
        m_VertexTemp.Set(frameSize, 0, frameSize);
        m_LineRenderer.SetPosition(2, m_VertexTemp);
        
        m_VertexTemp.Set(0, 0, frameSize);
        m_LineRenderer.SetPosition(3, m_VertexTemp);
    }
}
