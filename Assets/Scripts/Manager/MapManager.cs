using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour 
{
    public static int mapRegionX = 50;
    public static int mapRegionY = 50;
    
    public enum MapType
    {
        Empty,//该位置为空
        Tower,//该位置有防御塔或者地基
        TeleportA,//该位置有传送门A
        TeleportB//该位置有传送门B
    }
    
    //存放地图位置信息
    private MapType[,] map;
    
    private void Start()
    {
        map=new MapType[mapRegionX,mapRegionY];
        for(int j=0;j<mapRegionX;j++)
        {
            for(int k=0;k<mapRegionY;k++)
            {
                if(j==0||j==mapRegionX-1||k==0||k==mapRegionY-1)
                {
                    map[j, k] = MapType.Tower;
                }
            }
        }
    }
    
    public bool SetMap(int posX,int posY,MapType mapType)
    {
        //如果该位置没有防御塔
        if(map[posX,posY]!=MapType.Tower)
        {
            map[posX, posY] = mapType;
            return true;
        }
        Debug.LogError("Map Error");
        return false;

    }
    
    public MapType[,] GetMap()
    {
        return map;
    }
}
