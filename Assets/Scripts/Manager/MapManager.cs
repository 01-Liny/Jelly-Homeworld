using UnityEngine;
using System.IO;
using System.Collections;
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
    public static int mapSize = 2;
    public static int mapRegionX = 7;//实际要减1，外围有围墙
    public static int mapRegionY = 8;//实际要减1，外围有围墙

    //存放地图位置信息
    private MapType[,] map;
    private MapType[,,] mapFileCache;//存放读取地图文件的临时数组
    private int availableMapCount;//可用地图数组数量
    private string filePath;
    private FileInfo fileInfo;

    private int[,] mapTemp;

    //寻路实体
    private MonsterPathFinding monsterPathFinding;

    //测试代码
    public bool GenerateStone=false;
    public bool isModifyMap=false;

    public void FixedUpdate()
    {
        if(GenerateStone)
        {
            GenerateStone=false;
            GenerateStoneByMap();
        }
        if(isModifyMap)
        {
            isModifyMap=false;
            ModifyMapFile(0);
            SaveMapFile();
        }
    }

    //初始化地图信息，默认值为0（Empty），地图边缘有一圈围墙，建筑类型为石头（Basic）
    private void Awake()
    {
        monsterPathFinding = new MonsterPathFinding();
        map = new MapType[mapRegionX, mapRegionY];
        mapFileCache=new MapType[5,mapRegionX, mapRegionY];//最多5份地图
        mapTemp=new int[mapRegionX, mapRegionY];
        for (int j = 0; j < mapRegionX; j++)
        {
            for (int k = 0; k < mapRegionY; k++)
            {
                if (j == 0 || j == mapRegionX - 1 || k == 0 || k == mapRegionY - 1)
                {
                    map[j, k] = MapType.Basic;
                    mapTemp[j,k]=(int)MapType.Basic;
                }
            }
        }
        //monsterPathFinding.monsterPathFinding(mapTemp, 5, 3, 5, 5);
        Debug.Log("Path Finding Successed");

        availableMapCount=0;
        filePath=Application.persistentDataPath+"//map.txt";
        fileInfo=new FileInfo(filePath);
        LoadMapFile();
        ChooseMapFile(0);
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
                if(mapType==MapType.Basic)
                {
                    map[posX, posY] = mapType;
                    return true;
                }
                break;
            }
            case MapType.Basic:
            {
                //塔只能在石头上建造
                if(mapType==MapType.Tower)
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

    public void GenerateStoneByMap()
    {
        Vector3 m_VecTemp=new Vector3();
        for (int j = 0; j < mapRegionX; j++)
        {
            for (int k = 0; k < mapRegionY; k++)
            {
                if(map[j,k]==MapType.Basic)
                {
                    m_VecTemp.x=j*mapSize;
                    m_VecTemp.z=k*mapSize;
                    m_TowerManager.RandomInstantiateStone(m_VecTemp);
                }
            }
        }
    }

    //读取地图文件，并把地图信息赋值到临时地图数组
    public void LoadMapFile()
    {
        if(!fileInfo.Exists)//如果不存在地图文件，则初始化一个地图文件，不包含任何地图
        {
            StreamWriter m_SW;
            m_SW=fileInfo.CreateText();
            m_SW.WriteLine("0");
            availableMapCount=0;
            m_SW.Close();
            m_SW.Dispose();
        }
        else
        {
            StreamReader m_SR;
            m_SR=fileInfo.OpenText();
            availableMapCount=Int32.Parse(m_SR.ReadLine());

            string temp;
            for(int i=0;i<availableMapCount;i++)
            {
                m_SR.ReadLine();//每个数组前会空一行
                for(int j=0;j<mapRegionX;j++)
                {
                    temp=m_SR.ReadLine();
                    for(int k=0;k<mapRegionY;k++)
                    {
                        mapFileCache[i,j,k]=(MapType)(int)temp[k]-48;
                    }
                }
            }
            m_SR.Close();
            m_SR.Dispose();
        }
    }

    //加载哪个临时地图数组到现有地图数组
    public bool ChooseMapFile(int mapIndex)
    {
        //范围内才能加载
        if(availableMapCount>mapIndex)
        {
            for(int j=0;j<mapRegionX;j++)
            {
                for(int k=0;k<mapRegionY;k++)
                {
                    map[j,k]=mapFileCache[mapIndex,j,k];
                }
            }
            GenerateStoneByMap();
            return true;
        }
        else
        {
            return false;
        }
    }

    //保存当前地图数组到临时地图数组
    public bool ModifyMapFile(int mapIndex)
    {
        //在范围内才可以修改
        if(availableMapCount>mapIndex)
        {
            for(int j=0;j<mapRegionX;j++)
            {
                for(int k=0;k<mapRegionY;k++)
                {
                    mapFileCache[mapIndex,j,k]=map[j,k];
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //添加新的地图数组到临时地图数组
    public bool NewMapFile()
    {
        //地图数量不能超过5
        if(availableMapCount<5)
        {
            for(int j=0;j<mapRegionX;j++)
            {
                for(int k=0;k<mapRegionY;k++)
                {
                    mapFileCache[availableMapCount,j,k]=map[j,k];
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
        m_SW=fileInfo.AppendText();

        m_SW.WriteLine(availableMapCount);
        string temp="";
        for (int i = 0; i < availableMapCount; i++)
        {
            m_SW.WriteLine();//每个数组前会空一行
            for (int j = 0; j < mapRegionX; j++)
            {
                temp="";
                for (int k = 0; k < mapRegionY; k++)
                {
                    temp+=((int)(mapFileCache[i,j,k])).ToString();
                    // temp.Remove(k);
                    // temp.Insert(k,((int)(mapFileCache[i,j,k])).ToString());
                    Debug.Log(((int)(mapFileCache[i,j,k])).ToString());
                }
                Debug.Log("Temp:"+temp);
                m_SW.WriteLine(temp);
            }
        }
        m_SW.Close();
        m_SW.Dispose();
    }

    //根据参数地图位置信息，返回该位置的建筑类型
    public MapType GetMap(int posX,int posY)
    {
        return map[posX,posY];
    }

    //返回所有地图信息
    public MapType[,] GetMap()
    {
        return map;
    }
}
