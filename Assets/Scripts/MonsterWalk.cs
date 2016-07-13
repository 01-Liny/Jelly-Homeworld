using UnityEngine;
using System.Collections;
using System.Collections.Generic;//使用泛型的时候需要这个

public class MonsterWalk : MonoBehaviour
{
    private Vector3 temp1, temp2, startPosition, endPosition, newDirection;
    private Quaternion rotation;
    private float pathLength, totalTimeForPath, currentTimeOnPath;
    private Rigidbody rb;
    private int currentWayPoint = 0;
    private float lastWaypointSwitchTime;
    private float speed = 1.0f;
    private MonsterPathFinding.Node[] m_pathArray;
    private List<MonsterPathFinding.Node> m_ListPath;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        temp1 = new Vector3();
        temp2 = new Vector3();
        startPosition = new Vector3();
        endPosition = new Vector3();
        //for (int i = stackSize-1; i >=0; i--)
        //{
        //    Debug.Log(m_pathArray[i].x);
        //    Debug.Log(m_pathArray[i].y);
        //}

        lastWaypointSwitchTime = Time.time;
        m_pathArray = MonsterPathFinding.m_pathArray;
        m_ListPath = MonsterPathFinding.m_ListPath;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (m_pathArray[0]!= null)
        //{
        //    for (int i = 0; i < MonsterPathFinding.stackSize - 1; i++)
        //    {
        //        temp1.Set(m_pathArray[i].x, 1, m_pathArray[i].y);
        //        temp2.Set(m_pathArray[i + 1].x, 1, m_pathArray[i + 1].y);
        //        Debug.DrawLine(temp1, temp2, Color.blue);
        //    }
        //    startPosition.Set(m_pathArray[currentWayPoint].x, 1, m_pathArray[currentWayPoint].y);
        //    endPosition.Set(m_pathArray[currentWayPoint + 1].x, 1, m_pathArray[currentWayPoint + 1].y);
        //    newDirection = (endPosition - startPosition);

        //    rotation = Quaternion.LookRotation(newDirection);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.25f);
        //    //gameObject.transform.(endPosition);
        //    //gameObject.transform.Translate(Vector3.forward);

        //    //float x = newDirection.x;
        //    //float y = newDirection.y;
        //    //float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
        //    //transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        //    pathLength = Vector3.Distance(startPosition, endPosition);
        //    totalTimeForPath = pathLength / speed;//路径上总共需要花费的时间
        //    currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        //    transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
        //    //RotateIntoMoveDirection();
        //    if (transform.position == endPosition)
        //    {
        //        if (currentWayPoint < MonsterPathFinding.stackSize - 2)
        //        {
        //            currentWayPoint++;
        //            lastWaypointSwitchTime = Time.time;
        //            // TODO: Rotate into move direction
        //        }
        //    }
        //}
        if (m_ListPath[0] != null)
        {
            for (int i = 0; i < MonsterPathFinding.stackSize - 1; i++)
            {
                temp1.Set(m_ListPath[i].x, 1, m_ListPath[i].y);
                temp2.Set(m_ListPath[i + 1].x, 1, m_ListPath[i + 1].y);
                Debug.DrawLine(temp1, temp2, Color.blue);
            }
            startPosition.Set(m_ListPath[currentWayPoint].x, 1, m_ListPath[currentWayPoint].y);
            endPosition.Set(m_ListPath[currentWayPoint + 1].x, 1, m_ListPath[currentWayPoint + 1].y);
            newDirection = (endPosition - startPosition);

            rotation = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.25f);
            //gameObject.transform.(endPosition);
            //gameObject.transform.Translate(Vector3.forward);

            //float x = newDirection.x;
            //float y = newDirection.y;
            //float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
            //transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

            pathLength = Vector3.Distance(startPosition, endPosition);
            totalTimeForPath = pathLength / speed;//路径上总共需要花费的时间
            currentTimeOnPath = Time.time - lastWaypointSwitchTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
            //RotateIntoMoveDirection();
            if (transform.position == endPosition)
            {
                if (currentWayPoint < MonsterPathFinding.stackSize - 2)
                {
                    currentWayPoint++;
                    lastWaypointSwitchTime = Time.time;
                    // TODO: Rotate into move direction
                }
            }
        }
    }
}
