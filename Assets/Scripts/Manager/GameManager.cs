using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour 
{
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

    //整个游戏的初始化
    void Awake()
    {
        TowerInfo.Init();
    }
}
