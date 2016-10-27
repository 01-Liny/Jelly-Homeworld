using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;//使用泛型的时候需要这个

public class MonsterPathFinding
{
    private int currentWayPoint = 0;
    private float speed = 1.0f;

    private static int m = 5, n = 6;			//定义地图的高和宽
    private int shortestPath = 1000;
    public static int stackSize;				//最短路径的数组大小（最小路径的栈的大小）
    public static int ListSize = 0;
    public static int[,] moveArray = new int[8, 2] { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }, { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };


    public class Node : ICloneable
    {
        public int x;
        public int y;
        public int pathLength;
        public int[,] markMapArray = new int[50, 50];

        public Node() { }

        public Node(int[,] mapArray)
        {
            for (int i = 0; i < m + 2; i++)
            {
                for (int j = 0; j < n + 2; j++)
                {
                    markMapArray[i, j] = mapArray[i, j];
                }
            }
        }
        //浅拷贝
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        //深拷贝
        public object DeepClone()
        {
            Node m_Temp = new Node();
            m_Temp.x = this.x;
            m_Temp.y = this.y;
            m_Temp.pathLength = this.pathLength;
            for (int i = 0; i < m + 2; i++)
            {
                for (int j = 0; j < n + 2; j++)
                {
                    m_Temp.markMapArray[i, j] = this.markMapArray[i, j];
                }
            }
            return m_Temp;
        }
        public bool whetherMove()
        {
            int tempX, tempY;
            for (int i = 0; i < 8; i++)
            {
                tempX = x + moveArray[i, 0];
                tempY = y + moveArray[i, 1];
                //Debug.Log("tempX:" + tempX + " tempY:" + tempY);
                if (markMapArray[tempX, tempY] == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool arriveEndPosition(int endX, int endY)
        {
            if (x == endX && y == endY)
            {
                return true;
            }
            return false;
        }

        public void move()
        {
            int tempX, tempY;
            for (int i = 0; i < 7; i++)
            {
                tempX = x + moveArray[i, 0];
                tempY = y + moveArray[i, 1];
                if (markMapArray[tempX, tempY] == 0)
                {
                    markMapArray[tempX, tempY] = 1;
                    x = tempX;
                    y = tempY;
                    if (i < 4)
                    {
                        pathLength += 3;
                    }
                    else
                    {
                        pathLength += 2;
                    }
                    return;
                    //return tempX*m + tempY;//如果mod完m之后等于0，tempY=m，tempX=（返回值-m）/m
                }
            }
        }

        public void copyMarkMapArray(int[,] markMapArray)
        {
            for (int i = 0; i < m + 2; i++)
            {
                for (int j = 0; j < n + 2; j++)
                {
                    this.markMapArray[i, j] = markMapArray[i, j];
                }
            }
        }
    }   

    public static Node[] m_pathArray = new Node[100];

    private Node m_firstNode, m_tempNode;
    private Stack<Node> m_pathStore;
    public static List<Node> m_ListPath;

    public void monsterPathFinding(int[,] mapArray, int starX, int starY, int endX, int endY)//寻路函数
    {
        bool findPath = false;
        m_pathStore = new Stack<Node>();//bfs过程中存储路径的栈

        //stack<Node> m_pathStore;//bfs过程中存储路径的栈
        //stack<Node> *m_tempStore;//bfs过程中存放最优路径的栈

        m_firstNode = new Node(mapArray);
        m_tempNode = new Node(mapArray);

        m_firstNode.x = starX;
        m_firstNode.y = starY;
        m_firstNode.markMapArray[starX, starY] = 1;
        m_firstNode.pathLength = 0;

        m_pathStore.Push(m_firstNode);
        //m_ListPath.Add(m_firstNode);
        //ListSize++;
        while (m_pathStore.Count != 0)
        {
            m_tempNode = (Node)(m_pathStore.Peek().DeepClone());
            //m_tempNode = (Node)m_ListPath[ListSize].DeepClone();
            if (m_tempNode.whetherMove())
            {
                m_tempNode.move();
                m_pathStore.Peek().copyMarkMapArray(m_tempNode.markMapArray);

                //......优化当pathLength等于他们之间斜线的距离时，那就直接跳出

                m_pathStore.Push(m_tempNode);
                if (m_tempNode.arriveEndPosition(endX, endY)==true)
                {
                    findPath = true;
                    if (m_tempNode.pathLength < shortestPath)
                    {
                        //记下最短的路径长度
                        shortestPath = m_tempNode.pathLength;
                        stackSize = m_pathStore.Count;

                        //方法一、
                        m_pathStore.CopyTo(m_pathArray, 0);//把栈里元素copy入数组，从数组0开始copy
                        m_ListPath = new List<Node>(m_pathStore.ToArray());
                        //方法二、
                        //存储下pathStore里的所有坐标
                        //m_tempStore = new Stack<Node>(m_pathStore);//用已有的stack再new一个stack时两个stack栈里的元素是相反的
                        //for (int j = m_tempStore.Count - 1; j >= 0; j--)
                        //{
                        //    m_pathArray[j] = m_tempStore.Peek();
                        //    m_tempStore.Pop();
                        //}
                        //for (int j = 0; j < stackSize; j++)//这里不能用j < m_tempStore.Count,因为m_tempStore.Count的个数一直在变（好...蠢）
                        //{
                        //    m_pathArray[j] = (Node)m_tempStore.Peek().DeepClone();
                        //    m_tempStore.Pop();
                        //}                      
                    }
                    m_pathStore.Pop();
                }
            }
            else
            {
                m_pathStore.Pop();
            }

        }
    }
 
}
