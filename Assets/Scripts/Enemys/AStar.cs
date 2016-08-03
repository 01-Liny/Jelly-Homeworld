using UnityEngine;
using System.Collections;
using System.Collections.Generic;//使用泛型的时候需要这个
using System.Linq;

public class AStar
{
    public static List<Point>m_ListPath = new List<Point>();
    public class Maze
    {
        public const int OBLIQUE = 14;
        public const int STEP = 10;
        public int[,] MazeArray { get; private set; }
        private List<Point> CloseList;
        private List<Point> OpenList;
        private int weight,height;
        private Point[] pointA{ get; set; }
        private Point[] pointB { get; set; }
        private Point parent;
        private Point start;
        private Point end;

        public Maze(MapType[,] maze,int width,int height,Point start,Point end, Point[] pointA,Point[] pointB)
        {
            MazeArray = new int[height, width];
            this.pointA = pointA;
            this.pointB = pointB;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    this.MazeArray[i,j] = (int)maze[i,j];
                }
            }
            this.weight= width;
            this.height= height;
            this.start = start;
            this.end = end;
            OpenList = new List<Point>(MazeArray.Length);
            CloseList = new List<Point>(MazeArray.Length);
        }

        //修改地图的函数
        public void ChangeMazeArray(MapType[,] maze)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < weight; j++)
                {
                    this.MazeArray[i,j] = (int)maze[i,j];
                }
            }
        }

        public bool FindFinalPath()
        {
            FindOncePath(start, pointA[0], false);
            if (parent == null)
                return false;
            else
                Addm_ListPath();
            FindOncePath(pointA[1], pointB[0], false);
            if (parent == null)
                return false;
            else
                Addm_ListPath();
            FindOncePath(pointB[1], end, false);
            if (parent == null)
                return false;
            else
                Addm_ListPath();
            return true;
        }

        //一次寻路的函数,因为路径找的时候是从end到start，所以已经把函数中所有的end的地方和start的地方互换了
        public void FindOncePath(Point start, Point end, bool IsIgnoreCorner)
        {
            OpenList.Add(end);
            while (OpenList.Count != 0)
            {
                //找出F值最小的点
                Point tempStart = ListHelp.MinPoint(ref OpenList);
                OpenList.RemoveAt(0);
                CloseList.Add(tempStart);
                //找出它相邻的点
                List<Point> surroundPoints = SurroundPoints(tempStart, IsIgnoreCorner);
                foreach (Point point in surroundPoints)
                {
                    if (ListHelp.Exists(OpenList, point))
                        //如果已经在开始列表里了，计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
                        FoundPoint(tempStart, point);
                    else
                        //如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
                        NotFoundPoint(tempStart, start, point);
                }
                if (ListHelp.Get(OpenList, start) != null)
                {
                    parent = ListHelp.Get(OpenList, start);
                    return;
                }
            }
            parent = ListHelp.Get(OpenList, start);
        }

        //把路径加入m_ListPath中
        public void Addm_ListPath()
        {
            while (parent != null)
            {
                m_ListPath.Add(parent);
                parent = parent.ParentPoint;
            }
        }

        private void FoundPoint(Point tempStart, Point point)
        {
            int G = CalcG(tempStart, point);
            if (G < point.G)
            {
                point.ParentPoint = tempStart;
                point.G = G;
                point.CalcF();
            }
        }

        private void NotFoundPoint(Point tempStart, Point end, Point point)
        {
            point.ParentPoint = tempStart;
            point.G = CalcG(tempStart, point);
            point.H = CalcH(end, point);
            point.CalcF();
            OpenList.Add(point);
        }

        private int CalcG(Point start, Point point)
        {
            int G = (System.Math.Abs(point.X - start.X) + System.Math.Abs(point.Y - start.Y)) == 1 ? STEP : OBLIQUE;
            int parentG = point.ParentPoint != null ? point.ParentPoint.G : 0;
            return G + parentG;
        }

        private int CalcH(Point end, Point point)
        {
            int step = System.Math.Abs(point.X - end.X) + System.Math.Abs(point.Y - end.Y);
            return step * STEP;
        }

        //获取某个点周围可以到达的点
        public List<Point> SurroundPoints(Point point, bool IsIgnoreCorner)
        {
            List<Point> surroundPoints = new List<Point>(9);

            for (int x = point.X - 1; x <= point.X + 1; x++)
                for (int y = point.Y - 1; y <= point.Y + 1; y++)
                {
                    if (CanReach(point, x, y, IsIgnoreCorner))
                        ListHelp.Add(surroundPoints, x, y);
                }
            return surroundPoints;
        }

        //在二维数组对应的位置不为障碍物
        private bool CanReach(int x, int y)
        {
            Debug.Log(x+" "+ y);
            return MazeArray[x, y] == 0;
        }

        private bool CanReach(Point start, int x, int y, bool IsIgnoreCorner)
        {
            if (!CanReach(x, y) || ListHelp.Exists(CloseList, x, y))
                return false;
            else
            {
                //判断是否是上下左右走
                if (System.Math.Abs(x - start.X) + System.Math.Abs(y - start.Y) == 1)
                    return true;
                //如果是斜方向移动, 判断是否 "拌脚"
                else
                {
                    if (CanReach(start.X, y) || CanReach(x, start.Y))
                        return true;
                    else
                        return IsIgnoreCorner;
                }
            }
        }
    }




}
