﻿using UnityEngine;
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

    //private float pauseStartTime;
    //private float pauseTime = 0;

    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private float walkSpeed;


    //private bool[] isSlowdown;//是否受到减速
    //protected float slowdownRate = 0.0f;//减速百分比
    //private float slowdownTime;//减速失效时间

    //减速时间数组
    private float[] statusTime = new float[5];

    //减速等级
    private int slowDownLevel;

    private MonsterPathFinding.Node[] m_pathArray;
    //private List<MonsterPathFinding.Node> m_ListPath;
    private List<Point> m_ListPath;

    public float isSlowDown = 1;

    //眩晕时间，减速时间
    public void setFireStatus(float fireStunTime,int slowDownLevel)
    {
        //pauseTime = fireStunTime;
        //slowdownRate = TowerElemInfo.slowdownDegree[slowDownLevel];
        //slowdownTime = fireSlowdownTime;
        //if(fireStunTime!=0f)
        statusTime[4] = Time.time + fireStunTime;
        statusTime[slowDownLevel] = Time.time + TowerElemInfo.slowdownTime[slowDownLevel];
    }

    public void setIsSlownDown(float isSlowDown)
    {
        this.isSlowDown = isSlowDown;
    }

    //怪物被眩晕
    //public void Pause(float pauseTime)
    //{
    //    pauseStartTime = Time.time;
    //    this.pauseTime = pauseTime;
    //    walkSpeed = this.speed;
    //    changeSpeed(0);
    //}

    //改变速度
    private void changewalkSpeed(float walkSpeed)
    {
        this.walkSpeed = walkSpeed;
    }

    public void changeSpeed(float speed)
    {
        this.speed = speed;
        walkSpeed = speed;
    }

    // Use this for initialization
    void Start()
    {
        walkSpeed = speed;
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
        //m_ListPath = MonsterPathFinding.m_ListPath;
        m_ListPath = AStar.m_ListPath;

        //设置起始点的位置
        Vector3 temp = transform.position;
        temp.Set(m_ListPath[0].X, 1, m_ListPath[0].Y);
        transform.position = temp;
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

        //for (int i = 0; i < MonsterPathFinding.stackSize - 1; i++)
        //{
        //    temp1.Set(m_ListPath[i].x, 1, m_ListPath[i].y);
        //    temp2.Set(m_ListPath[i + 1].x, 1, m_ListPath[i + 1].y);
        //    Debug.DrawLine(temp1, temp2, Color.blue);
        //}

        for (int i = 4; i >= 0; i--)
        {
            if (Time.time < statusTime[i])
            {
                if (i == 4)
                {
                    changewalkSpeed(0);
                }
                else
                {
                    if (isSlowDown == 1)
                    {
                        changewalkSpeed(speed * (1 - TowerElemInfo.slowdownDegree[i]));
                    }
                }
                break;
            }
            else if (Time.time > statusTime[i])
            {
                walkSpeed = speed;
            }
        }

        //if (isPause)
        //{
        //    isPause = false;
        //    Pause(1);
        //}

        ////暂停后恢复原来速度
        //if (Time.time - pauseStartTime >= pauseTime)
        //{
        //    changeSpeed(walkSpeed);
        //}

        //for (int i = 0; i < AStar.m_ListPath.Count - 1; i++)
        //{
        //    if (m_ListPath[i + 1].X == -1)
        //    {
        //        i++;
        //        continue;
        //    }
        //    temp1.Set(m_ListPath[i].X, 1, m_ListPath[i].Y);
        //    temp2.Set(m_ListPath[i + 1].X, 1, m_ListPath[i + 1].Y);
        //    Debug.DrawRay(temp1, Vector3.up, Color.red);
        //    Debug.DrawLine(temp1, temp2, Color.blue);
        //}
        //startPosition.Set(m_ListPath[currentWayPoint].X, 1, m_ListPath[currentWayPoint].Y);

        ////碰到x，y为-1，-1的点时，把list下标往后移两个单位
        //if (m_ListPath[currentWayPoint + 1].X == -1)
        //{
        //    currentWayPoint += 2;
        //    Vector3 temp = transform.position;
        //    temp.Set(m_ListPath[currentWayPoint].X, 1, m_ListPath[currentWayPoint].Y);
        //    transform.position = temp;
        //}
        endPosition.Set(m_ListPath[currentWayPoint + 1].X, 1, m_ListPath[currentWayPoint + 1].Y);
        newDirection = (endPosition - transform.position);

        rotation = Quaternion.LookRotation(newDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.25f);
        //gameObject.transform.(endPosition);
        //gameObject.transform.Translate(Vector3.forward);

        //float x = newDirection.x;
        //float y = newDirection.y;
        //float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
        //transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        //pathLength = Vector3.Distance(startPosition, endPosition);
        //totalTimeForPath = pathLength / speed;//路径上总共需要花费的时间
        //currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        //transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
        //RotateIntoMoveDirection();
        rb.velocity = transform.forward * walkSpeed;
        if (Vector3.Distance(transform.position, endPosition) <= 0.5f)
        {
            if (currentWayPoint < AStar.m_ListPath.Count - 2)
            {
                currentWayPoint++;
                //lastWaypointSwitchTime = Time.time;
                // TODO: Rotate into move direction
            }
            else
            {
                walkSpeed = Mathf.Lerp(walkSpeed, 0, 0.25f);
                Destroy(this.gameObject);
                UIPlayerHealth.TakeHealth(UIGameLevel.level * 2);
                
                //walkSpeed = speed;
            }
        }
    }
}
