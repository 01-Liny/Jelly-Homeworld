using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public enum MapType
{
    Empty,//该位置为空
    Basic,//该位置有地基
    Tower,//该位置有防御塔
    TeleportA,//该位置有传送门A
    TeleportB//该位置有传送门B 
}

public class MapManager : MonoBehaviour
{
    public TowerManager m_TowerManager;
    public static int mapSize = 3;
    public static int mapRegionX = 15;//实际要减2，外围有围墙
    public static int mapRegionY = 11;//实际要减2，外围有围墙
    public int currentMapIndex = 0;//现在读取的地图下标，默认为0

    //临时代码                                                     &&&&&&&&&
    public MonsterManager m_MonsterManager;

    //存放地图位置信息
    private MapType[,] map;
    private MapType[,,] mapFileCache;//存放读取地图文件的临时数组
    private Point start;
    private Point end = new Point(5, 5);
    private Point[] pointA;
    private Point[] pointB;

    private int availableMapCount;//可用地图数组数量
    private string filePath;
    private FileInfo fileInfo;

    private int[,] mapTemp;

    //寻路实体
    private AStar.Maze m_Maze;

    //测试代码
    public bool isGenerateStone = false;
    public bool isModifyMap = false;
    public bool isResetStone = false;
    private List<Point> m_ListPath;
    private Vector3 temp1 = new Vector3();
    private Vector3 temp2 = new Vector3();

    public void FixedUpdate()
    {
        if (isGenerateStone)
        {
            isGenerateStone = false;
            GenerateStoneByMap();
        }
        if (isModifyMap)
        {
            isModifyMap = false;
            ModifyMapFile();
            SaveMapFile();
        }
        if (isResetStone)
        {
            isResetStone = false;
            ResetStone();
        }

        m_ListPath = AStar.m_ListPath;

        for (int i = 0; i < AStar.m_ListPath.Count - 1; i++)
        {
            if (m_ListPath[i + 1].X == -1)
            {
                i++;
                continue;
            }
            temp1.Set(m_ListPath[i].X, 1, m_ListPath[i].Y);
            temp2.Set(m_ListPath[i + 1].X, 1, m_ListPath[i + 1].Y);
            Debug.DrawRay(temp1, Vector3.up, Color.red);
            Debug.DrawLine(temp1, temp2, Color.blue);
        }
    }

    //初始化地图信息，默认值为0（Empty），地图边缘有一圈围墙，建筑类型为石头（Basic）
    private void Awake()
    {
        map = new MapType[mapRegionX, mapRegionY];
        mapFileCache = new MapType[5, mapRegionX, mapRegionY];//最多5份地图
        mapTemp = new int[mapRegionX, mapRegionY];

        start = new Point(1, 9);
        end = new Point(13, 1);
        pointA = new Point[2] { new Point(1, 5), new Point(7, 9)};
        pointB = new Point[2] { new Point(7, 1), new Point(13, 5)};

        InitMap();
        m_Maze = new AStar.Maze(map, mapRegionY, mapRegionX, start, end, pointA, pointB);
        //m_Maze.FindFinalPath();
        //monsterPathFinding.monsterPathFinding(mapTemp, 5, 3, 5, 5);
        Debug.Log("Path Finding Successed");

        availableMapCount = 0;
        filePath = Application.persistentDataPath + "//map.txt";
        fileInfo = new FileInfo(filePath);
        LoadMapFile();
        //ChooseMapFile(0);
    }

    public void InitMap()
    {
        for (int j = 0; j < mapRegionX; j++)
        {
            for (int k = 0; k < mapRegionY; k++)
            {
                //初始化传送门
                if ((j == pointA[0].X && k == pointA[0].Y) || (j == pointA[1].X && k == pointA[1].Y))
                {
                    map[j, k] = MapType.TeleportA;
                    continue;
                }
                if ((j == pointB[0].X && k == pointB[0].Y) || (j == pointB[1].X && k == pointB[1].Y))
                {
                    map[j, k] = MapType.TeleportB;
                    continue;
                }

                if (j == 0 || j == mapRegionX - 1 || k == 0 || k == mapRegionY - 1)
                {
                    map[j, k] = MapType.Basic;
                    mapTemp[j, k] = (int)MapType.Basic;
                }
                else
                {
                    map[j, k] = MapType.Empty;
                }
            }
        }
    }

    public void ResetStone()
    {
        InitMap();
        m_TowerManager.ClearStoneList();
        m_TowerManager.ClearTeleportList();
        GenerateStoneByMap();
    }

    public void ReGenerateStoneByMap()
    {
        m_TowerManager.ClearStoneList();
        m_TowerManager.ClearTeleportList();
        GenerateStoneByMap();
        //monsterPathFinding.monsterPathFinding(mapTemp, 5, 3, 5, 5);
        Debug.Log("Path Finding Successed");
    }

    //是非可以建造制定建筑，如果可以则修改地图信息并返回true，不行则为false
    public bool ModifyMap(int posX, int posY, MapType mapType)
    {
        //通过建筑类型执行分支
        switch (map[posX, posY])
        {
            case MapType.Empty:
                {
                    //石头只能在空地上建造
                    if (mapType == MapType.Basic)
                    {
                        map[posX, posY] = mapType;
                        m_Maze.ChangeMazeArray(map);
                        if (m_Maze.FindFinalPath() == false)
                        {
                            map[posX, posY] = MapType.Empty;
                            m_Maze.ChangeMazeArray(map);
                            m_Maze.FindFinalPath();
                            Debug.Log(AStar.m_ListPath.Count);
                            return false;
                        }
                        Debug.Log(AStar.m_ListPath.Count);
                        return true;
                    }
                    break;
                }
            case MapType.Basic:
                {
                    //塔只能在石头上建造
                    if (mapType == MapType.Tower)
                    {
                        map[posX, posY] = mapType;
                        return true;
                    }
                    break;
                }
        }
        Debug.LogError("Map Error");
        return false;
    }

    public bool DeleteStone(int posX, int posY)
    {
        if (map[posX, posY] == MapType.Basic)
        {
            map[posX, posY] = MapType.Empty;
            m_Maze.ChangeMazeArray(map);
            //删除石头不可能造成寻路出错问题，不用判断返回值
            m_Maze.FindFinalPath();
            Debug.Log(AStar.m_ListPath.Count);
            return true;
        }
        return false;
    }

    public bool DeleteTower(int posX, int posY)
    {
        if (map[posX, posY] == MapType.Tower)
        {
            map[posX, posY] = MapType.Basic;
            return true;
        }
        return false;
    }

    public void GenerateStoneByMap()
    {
        Vector3 m_VecTemp = new Vector3();
        for (int j = 0; j < mapRegionX; j++)
        {
            for (int k = 0; k < mapRegionY; k++)
            {
                m_VecTemp.x = j * mapSize;
                m_VecTemp.z = k * mapSize;
                if (map[j, k] == MapType.Basic)
                {
                    //不属于边界的石头才会生成
                    //先注释，方便测试                                            正式程序需要解除注释
                    //if (!(j == 0 || j == mapRegionX - 1 || k == 0 || k == mapRegionY - 1))
                    {
                        m_TowerManager.RandomInstantiateStone(m_VecTemp);
                    }
                }
                //生成传送门
                if (map[j, k] == MapType.TeleportA || map[j, k] == MapType.TeleportB)
                {
                    m_TowerManager.RandomInstantiateTeleport(map[j, k], m_VecTemp);
                }
            }
        }
    }

    //读取地图文件，并把地图信息赋值到临时地图数组
    public void LoadMapFile()
    {
        if (!fileInfo.Exists)//如果不存在地图文件，则初始化一个地图文件，包含5个默认地图
        {
            //StreamWriter m_SW;
            //m_SW=fileInfo.CreateText();
            //m_SW.WriteLine("0");
            availableMapCount = 0;
            for (int i = 0; i < 5; i++)
                NewMapFile();
            SaveMapFile();
            //m_SW.Close();
            //m_SW.Dispose();
        }
        else
        {
            StreamReader m_SR;
            m_SR = fileInfo.OpenText();
            availableMapCount = Int32.Parse(m_SR.ReadLine());

            string temp;
            for (int i = 0; i < availableMapCount; i++)
            {
                m_SR.ReadLine();//每个数组前会空一行
                for (int j = 0; j < mapRegionX; j++)
                {
                    temp = m_SR.ReadLine();
                    for (int k = 0; k < mapRegionY; k++)
                    {
                        mapFileCache[i, j, k] = (MapType)(int)temp[k] - 48;
                    }
                }
            }
            m_SR.Close();
            m_SR.Dispose();
        }
    }

    //加载哪个临时地图数组到现有地图数组
    private bool ChooseMapFile(int mapIndex)
    {
        //范围内才能加载
        if (availableMapCount > mapIndex)
        {
            currentMapIndex = mapIndex;
            for (int j = 0; j < mapRegionX; j++)
            {
                for (int k = 0; k < mapRegionY; k++)
                {
                    map[j, k] = mapFileCache[mapIndex, j, k];
                }
            }
            m_Maze.ChangeMazeArray(map);
            m_Maze.FindFinalPath();
            return true;
        }
        else
        {
            return false;
        }
    }

    //临时代码 被按钮调用
    public void ChooseMap(int mapIndex)
    {
        ChooseMapFile(mapIndex);
    }

    //保存当前地图数组到临时地图数组
    public bool ModifyMapFile()
    {
        //在范围内才可以修改
        if (availableMapCount > currentMapIndex)
        {
            for (int j = 0; j < mapRegionX; j++)
            {
                for (int k = 0; k < mapRegionY; k++)
                {
                    mapFileCache[currentMapIndex, j, k] = map[j, k];
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ModifyMap()
    {
        ModifyMapFile();
    }

    //添加新的地图数组到临时地图数组
    public bool NewMapFile()
    {
        //地图数量不能超过5
        if (availableMapCount < 5)
        {
            for (int j = 0; j < mapRegionX; j++)
            {
                for (int k = 0; k < mapRegionY; k++)
                {
                    mapFileCache[availableMapCount, j, k] = map[j, k];
                }
            }
            availableMapCount++;
            return true;
        }
        else
        {
            return false;
        }
    }

    //以临时地图数组写入地图文件
    public void SaveMapFile()
    {
        File.Create(filePath).Close();
        StreamWriter m_SW;
        m_SW = fileInfo.AppendText();

        m_SW.WriteLine(availableMapCount);
        string temp = "";
        for (int i = 0; i < availableMapCount; i++)
        {
            m_SW.WriteLine();//每个数组前会空一行
            for (int j = 0; j < mapRegionX; j++)
            {
                temp = "";
                for (int k = 0; k < mapRegionY; k++)
                {
                    temp += ((int)(mapFileCache[i, j, k])).ToString();
                    // temp.Remove(k);
                    // temp.Insert(k,((int)(mapFileCache[i,j,k])).ToString());
                    //Debug.Log(((int)(mapFileCache[i,j,k])).ToString());
                }
                //Debug.Log("Temp:"+temp);
                m_SW.WriteLine(temp);
            }
        }
        m_SW.Close();
        m_SW.Dispose();
    }


    //根据参数地图位置信息，返回该位置的建筑类型
    public MapType GetMap(int posX, int posY)
    {
        try
        {
            return map[posX, posY];
        }
        catch(Exception ex)
        {
            Debug.LogError("Out of map range");
            return MapType.Empty;
        }
    }

    //返回所有地图信息
    public MapType[,] GetMap()
    {
        return map;
    }

    //临时代码                             &&&&&&&&&&
    public void CreateMonster()
    {
        m_MonsterManager.CreatMonster(UIGameLevel.level,5,1);
    }
}
