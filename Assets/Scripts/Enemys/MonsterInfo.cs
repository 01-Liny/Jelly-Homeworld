using UnityEngine;
using System.Collections;

public class MonsterInfo
{
    public static int[,] PrefabsPositon;
    public static float[,,] EnemyProperty;
   // public static int[] 

    public static void Init()
    {
        PrefabsPositon = new int[21, 3];
        EnemyProperty = new float[5, 7, 6];//[怪物关卡等级，怪物种类，怪物6种属性值]

        //restore
        //maxArmor
        //speed
        //isStun
        //isSlowdown
        //LimitAttackTimes
        //之后优化各属性加强
        #region EnemyProperty
        //回血快
        EnemyProperty[1, 1, 0] = 10;
        EnemyProperty[1, 1, 1] = 20;
        EnemyProperty[1, 1, 2] = 2;
        EnemyProperty[1, 1, 3] = 1;
        EnemyProperty[1, 1, 4] = 1;
        EnemyProperty[1, 1, 5] = 100;

        EnemyProperty[2, 1, 0] = 12;
        EnemyProperty[2, 1, 1] = 20;
        EnemyProperty[2, 1, 2] = 2;
        EnemyProperty[2, 1, 3] = 1;
        EnemyProperty[2, 1, 4] = 1; 
        EnemyProperty[2, 1, 5] = 100;

        EnemyProperty[3, 1, 0] = 14;
        EnemyProperty[3, 1, 1] = 20;
        EnemyProperty[3, 1, 2] = 2;
        EnemyProperty[3, 1, 3] = 1;
        EnemyProperty[3, 1, 4] = 1;
        EnemyProperty[3, 1, 5] = 100;

        EnemyProperty[4, 1, 0] = 16;
        EnemyProperty[4, 1, 1] = 20;
        EnemyProperty[4, 1, 2] = 2;
        EnemyProperty[4, 1, 3] = 1;
        EnemyProperty[4, 1, 4] = 1;
        EnemyProperty[4, 1, 5] = 100;

        //高护甲
        EnemyProperty[1, 2, 0] = 5;
        EnemyProperty[1, 2, 1] = 30;
        EnemyProperty[1, 2, 2] = 2;
        EnemyProperty[1, 2, 3] = 1;
        EnemyProperty[1, 2, 4] = 1;
        EnemyProperty[1, 2, 5] = 100;

        EnemyProperty[2, 2, 0] = 5;
        EnemyProperty[2, 2, 1] = 35;
        EnemyProperty[2, 2, 2] = 2;
        EnemyProperty[2, 2, 3] = 1;
        EnemyProperty[2, 2, 4] = 1;
        EnemyProperty[2, 2, 5] = 100;

        EnemyProperty[3, 2, 0] = 5;
        EnemyProperty[3, 2, 1] = 40;
        EnemyProperty[3, 2, 2] = 2;
        EnemyProperty[3, 2, 3] = 1;
        EnemyProperty[3, 2, 4] = 1;
        EnemyProperty[3, 2, 5] = 100;

        EnemyProperty[4, 2, 0] = 5;
        EnemyProperty[4, 2, 1] = 45;
        EnemyProperty[4, 2, 2] = 2;
        EnemyProperty[4, 2, 3] = 1;
        EnemyProperty[4, 2, 4] = 1;
        EnemyProperty[4, 2, 5] = 100;

        //免疫减速
        EnemyProperty[1, 3, 0] = 5;
        EnemyProperty[1, 3, 1] = 20;
        EnemyProperty[1, 3, 2] = 2;
        EnemyProperty[1, 3, 3] = 1;
        EnemyProperty[1, 3, 4] = 0;
        EnemyProperty[1, 3, 5] = 100;

        EnemyProperty[2, 3, 0] = 5;
        EnemyProperty[2, 3, 1] = 20;
        EnemyProperty[2, 3, 2] = 2;
        EnemyProperty[2, 3, 3] = 1;
        EnemyProperty[2, 3, 4] = 0;
        EnemyProperty[2, 3, 5] = 100;

        EnemyProperty[3, 3, 0] = 5;
        EnemyProperty[3, 3, 1] = 20;
        EnemyProperty[3, 3, 2] = 2;
        EnemyProperty[3, 3, 3] = 1;
        EnemyProperty[3, 3, 4] = 0;
        EnemyProperty[3, 3, 5] = 100;

        EnemyProperty[4, 3, 0] = 5;
        EnemyProperty[4, 3, 1] = 20;
        EnemyProperty[4, 3, 2] = 2;
        EnemyProperty[4, 3, 3] = 1;
        EnemyProperty[4, 3, 4] = 0;
        EnemyProperty[4, 3, 5] = 100;

        //限制受到攻击次数
        EnemyProperty[1, 4, 0] = 5;
        EnemyProperty[1, 4, 1] = 20;
        EnemyProperty[1, 4, 2] = 2;
        EnemyProperty[1, 4, 3] = 1;
        EnemyProperty[1, 4, 4] = 1;
        EnemyProperty[1, 4, 5] = 1;

        EnemyProperty[2, 4, 0] = 5;
        EnemyProperty[2, 4, 1] = 20;
        EnemyProperty[2, 4, 2] = 2;
        EnemyProperty[2, 4, 3] = 1;
        EnemyProperty[2, 4, 4] = 1;
        EnemyProperty[2, 4, 5] = 2;

        EnemyProperty[3, 4, 0] = 5;
        EnemyProperty[3, 4, 1] = 20;
        EnemyProperty[3, 4, 2] = 2;
        EnemyProperty[3, 4, 3] = 1;
        EnemyProperty[3, 4, 4] = 1;
        EnemyProperty[3, 4, 5] = 1;

        EnemyProperty[4, 4, 0] = 5;
        EnemyProperty[4, 4, 1] = 20;
        EnemyProperty[4, 4, 2] = 2;
        EnemyProperty[4, 4, 3] = 1;
        EnemyProperty[4, 4, 4] = 1;
        EnemyProperty[4, 4, 5] = 1;

        //移速快
        EnemyProperty[1, 5, 0] = 5;
        EnemyProperty[1, 5, 1] = 20;
        EnemyProperty[1, 5, 2] = 3;
        EnemyProperty[1, 5, 3] = 1;
        EnemyProperty[1, 5, 4] = 1;
        EnemyProperty[1, 5, 5] = 100;

        EnemyProperty[2, 5, 0] = 5;
        EnemyProperty[2, 5, 1] = 20;
        EnemyProperty[2, 5, 2] = 4;
        EnemyProperty[2, 5, 3] = 1;
        EnemyProperty[2, 5, 4] = 1;
        EnemyProperty[2, 5, 5] = 100;

        EnemyProperty[3, 5, 0] = 5;
        EnemyProperty[3, 5, 1] = 20;
        EnemyProperty[3, 5, 2] = 5;
        EnemyProperty[3, 5, 3] = 1;
        EnemyProperty[3, 5, 4] = 1;
        EnemyProperty[3, 5, 5] = 100;

        EnemyProperty[4, 5, 0] = 5;
        EnemyProperty[4, 5, 1] = 20;
        EnemyProperty[4, 5, 2] = 6;
        EnemyProperty[4, 5, 3] = 1;
        EnemyProperty[4, 5, 4] = 1;
        EnemyProperty[4, 5, 5] = 100;

        //免疫眩晕
        EnemyProperty[1, 6, 0] = 5;
        EnemyProperty[1, 6, 1] = 20;
        EnemyProperty[1, 6, 2] = 2;
        EnemyProperty[1, 6, 3] = 1;
        EnemyProperty[1, 6, 4] = 1;
        EnemyProperty[1, 6, 5] = 100;

        EnemyProperty[2, 6, 0] = 5;
        EnemyProperty[2, 6, 1] = 20;
        EnemyProperty[2, 6, 2] = 2;
        EnemyProperty[2, 6, 3] = 1;
        EnemyProperty[2, 6, 4] = 1;
        EnemyProperty[2, 6, 5] = 100;

        EnemyProperty[3, 6, 0] = 5;
        EnemyProperty[3, 6, 1] = 20;
        EnemyProperty[3, 6, 2] = 2;
        EnemyProperty[3, 6, 3] = 1;
        EnemyProperty[3, 6, 4] = 1;
        EnemyProperty[3, 6, 5] = 100;

        EnemyProperty[4, 6, 0] = 5;
        EnemyProperty[4, 6, 1] = 20;
        EnemyProperty[4, 6, 2] = 2;
        EnemyProperty[4, 6, 3] = 1;
        EnemyProperty[4, 6, 4] = 1;
        EnemyProperty[4, 6, 5] = 100;

        #endregion

        #region PrefabsPositon
        PrefabsPositon[1,0] = 1;
        PrefabsPositon[1,1] = 4;
        PrefabsPositon[1,2] = 0;

        PrefabsPositon[2,0] = 2;
        PrefabsPositon[2,1] = 0;
        PrefabsPositon[2,2] = 0;

        PrefabsPositon[3,0] = 3;
        PrefabsPositon[3,1] = 0;
        PrefabsPositon[3,2] = 0;

        PrefabsPositon[4,0] = 4;
        PrefabsPositon[4,1] = 0;
        PrefabsPositon[4,2] = 0;

        PrefabsPositon[5,0] = 5;
        PrefabsPositon[5,1] = 0;
        PrefabsPositon[5,2] = 0;

        PrefabsPositon[6,0] = 6;
        PrefabsPositon[6,1] = 0;
        PrefabsPositon[6,2] = 0;

        PrefabsPositon[7,0] = 6;
        PrefabsPositon[7,1] = 0;
        PrefabsPositon[7,2] = 0;

        PrefabsPositon[8,0] = 6;
        PrefabsPositon[8,1] = 0;
        PrefabsPositon[8,2] = 0;

        PrefabsPositon[9,0] = 6;
        PrefabsPositon[9,1] = 0;
        PrefabsPositon[9,2] = 0;

        PrefabsPositon[10,0] = 6;
        PrefabsPositon[10,1] = 0;
        PrefabsPositon[10,2] = 0;

        PrefabsPositon[11,0] = 6;
        PrefabsPositon[11,1] = 0;
        PrefabsPositon[11,2] = 0;

        PrefabsPositon[12,0] = 6;
        PrefabsPositon[12,1] = 0;
        PrefabsPositon[12,2] = 0;

        PrefabsPositon[13,0] = 6;
        PrefabsPositon[13,1] = 0;
        PrefabsPositon[13,2] = 0;

        PrefabsPositon[14,0] = 6;
        PrefabsPositon[14,1] = 0;
        PrefabsPositon[14,2] = 0;

        PrefabsPositon[15,0] = 6;
        PrefabsPositon[15,1] = 0;
        PrefabsPositon[15,2] = 0;

        PrefabsPositon[16,0] = 6;
        PrefabsPositon[16,1] = 0;
        PrefabsPositon[16,2] = 0;

        PrefabsPositon[17,0] = 6;
        PrefabsPositon[17,1] = 0;
        PrefabsPositon[17,2] = 0;

        PrefabsPositon[18,0] = 6;
        PrefabsPositon[18,1] = 0;
        PrefabsPositon[18,2] = 0;

        PrefabsPositon[19,0] = 6;
        PrefabsPositon[19,1] = 0;
        PrefabsPositon[19,2] = 0;

        PrefabsPositon[20,0] = 6;
        PrefabsPositon[20,1] = 0;
        PrefabsPositon[20,2] = 0;
        #endregion


    }

}
