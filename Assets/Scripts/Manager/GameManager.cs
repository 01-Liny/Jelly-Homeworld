using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour 
{
    public bool isSpeedUp=false;
    //声明委托
    public delegate void EnemyDiedEventHandler(object sender, EnemyDiedEventsArgs e);
    public static event EnemyDiedEventHandler EnemyDied;
    //定义
    public class EnemyDiedEventsArgs:EventArgs
    {
        public readonly Collider enemyCollider;
        //传递敌人的Collider信息
        public EnemyDiedEventsArgs(Collider enemyCollider)
        {
            this.enemyCollider = enemyCollider;
        }
    }

    //当敌人死亡时，调用该函数以发送敌人死亡信息到订阅者
    public static void OnEnemyDied(EnemyDiedEventsArgs e)
    {
        if (EnemyDied != null)
            EnemyDied(null, e);
    }

    //                          关卡重置的时候只要调用List.clear就行
    //整个游戏的初始化
    void Awake()
    {
        //防止手机熄灭屏幕
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        TowerInfo.Init();
        TowerElemInfo.Init();
        int[,] map = new int[7, 7]{{1, 1, 1, 1, 1, 1, 1},
                                     {1, 0, 1, 0, 0, 0, 1},
                                     {1, 0, 1, 0, 1, 0, 1},
                                     {1, 0, 0, 0, 0, 0, 1},
                                     {1, 0, 1, 1, 0, 0, 1},
                                     {1, 0, 0, 0, 1, 0, 1},
                                     {1, 1, 1, 1, 1, 1, 1}};
        //测试
        MapType[,] mapTemp = new MapType[7, 7];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                mapTemp[i, j] = (MapType)map[i, j];
            }
        }
        Point start = new Point(5, 3);
        Point end = new Point(5, 5);
        Point[] pointA = new Point[2] { new Point(1,12), new Point(1, 12) };
        Point[] pointB = new Point[2] { start, end };
        //AStar.Maze maze = new AStar.Maze(mapTemp, 7, 7, start, end, pointA, pointB);
        //maze.FindFinalPath();
        //Debug.Log("Print path:");
        //monsterPathFinding.monsterPathFinding(map, 5, 3, 5, 5);
    }

    static public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    static public float GetTimeScale()
    {
        return Time.timeScale;
    }

    void Update()
    {
        //if(isSpeedUp)
        //{
        //    Time.timeScale=0;
        //}
        //else
        //{
        //    Time.timeScale = 1;
        //}
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
