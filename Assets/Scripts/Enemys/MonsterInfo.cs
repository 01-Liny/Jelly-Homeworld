using UnityEngine;
using System.Collections;

public class MonsterInfo
{
    public static int[,] PrefabsPositon;
    public static float[,,] EnemyProperty;
    public static float[] EnemyHealth;//每关怪物血量
    private static float[] restoreArray;
    private static float[] armorArray;
    // public static int[] 

    public static void Init()
    {
        PrefabsPositon = new int[21, 3];
        EnemyProperty = new float[5, 7, 6];//[怪物关卡等级，怪物种类，怪物6种属性值]
        EnemyHealth = new float[21];//每关怪物血量
        restoreArray = new float[4] { 2, 2.5f, 3, 3.5f };
        armorArray = new float[4] { 10, 14, 22, 32 };
        //restore
        //maxArmor
        //speed
        //isStun
        //isSlowdown
        //LimitAttackTimes
        //之后优化各属性加强
        #region EnemyProperty
        EnemyHealth[1] = 50;
        EnemyHealth[2] = 100;
        EnemyHealth[3] = 200;
        EnemyHealth[4] = 400;
        EnemyHealth[5] = 600;
        EnemyHealth[6] = 800;
        EnemyHealth[7] = 1200;
        EnemyHealth[8] = 1600;
        EnemyHealth[9] = 2000;
        EnemyHealth[10] = 2400;
        EnemyHealth[11] = 2800;
        EnemyHealth[12] = 3200;
        EnemyHealth[13] = 3600;
        EnemyHealth[14] = 4000;
        EnemyHealth[15] = 4400;
        EnemyHealth[16] = 4800;
        EnemyHealth[17] = 5400;
        EnemyHealth[18] = 6200;
        EnemyHealth[19] = 7000;
        EnemyHealth[20] = 100000;
        #endregion

        #region EnemyProperty
        //回血快
        EnemyProperty[1, 1, 0] = 4;
        EnemyProperty[1, 1, 1] = armorArray[0];
        EnemyProperty[1, 1, 2] = 2;
        EnemyProperty[1, 1, 3] = 1;
        EnemyProperty[1, 1, 4] = 1;
        EnemyProperty[1, 1, 5] = 100;

        EnemyProperty[2, 1, 0] = 5;
        EnemyProperty[2, 1, 1] = armorArray[1];
        EnemyProperty[2, 1, 2] = 2;
        EnemyProperty[2, 1, 3] = 1;
        EnemyProperty[2, 1, 4] = 1; 
        EnemyProperty[2, 1, 5] = 100;

        EnemyProperty[3, 1, 0] = 6;
        EnemyProperty[3, 1, 1] = armorArray[2];
        EnemyProperty[3, 1, 2] = 2;
        EnemyProperty[3, 1, 3] = 1;
        EnemyProperty[3, 1, 4] = 1;
        EnemyProperty[3, 1, 5] = 100;

        EnemyProperty[4, 1, 0] = 8;
        EnemyProperty[4, 1, 1] = armorArray[3];
        EnemyProperty[4, 1, 2] = 2;
        EnemyProperty[4, 1, 3] = 1;
        EnemyProperty[4, 1, 4] = 1;
        EnemyProperty[4, 1, 5] = 100;

        //高护甲
        EnemyProperty[1, 2, 0] = restoreArray[0];
        EnemyProperty[1, 2, 1] = armorArray[0] + 3;
        EnemyProperty[1, 2, 2] = 2;
        EnemyProperty[1, 2, 3] = 1;
        EnemyProperty[1, 2, 4] = 1;
        EnemyProperty[1, 2, 5] = 100;

        EnemyProperty[2, 2, 0] = restoreArray[1];
        EnemyProperty[2, 2, 1] = armorArray[1] + 3;
        EnemyProperty[2, 2, 2] = 2;
        EnemyProperty[2, 2, 3] = 1;
        EnemyProperty[2, 2, 4] = 1;
        EnemyProperty[2, 2, 5] = 100;

        EnemyProperty[3, 2, 0] = restoreArray[2];
        EnemyProperty[3, 2, 1] = armorArray[2] + 3;
        EnemyProperty[3, 2, 2] = 2;
        EnemyProperty[3, 2, 3] = 1;
        EnemyProperty[3, 2, 4] = 1;
        EnemyProperty[3, 2, 5] = 100;

        EnemyProperty[4, 2, 0] = restoreArray[3];
        EnemyProperty[4, 2, 1] = armorArray[3] + 3;
        EnemyProperty[4, 2, 2] = 2;
        EnemyProperty[4, 2, 3] = 1;
        EnemyProperty[4, 2, 4] = 1;
        EnemyProperty[4, 2, 5] = 100;

        //免疫减速
        EnemyProperty[1, 3, 0] = restoreArray[0];
        EnemyProperty[1, 3, 1] = armorArray[0];
        EnemyProperty[1, 3, 2] = 2;
        EnemyProperty[1, 3, 3] = 1;
        EnemyProperty[1, 3, 4] = 0;
        EnemyProperty[1, 3, 5] = 100;

        EnemyProperty[2, 3, 0] = restoreArray[1];
        EnemyProperty[2, 3, 1] = armorArray[1];
        EnemyProperty[2, 3, 2] = 2;
        EnemyProperty[2, 3, 3] = 1;
        EnemyProperty[2, 3, 4] = 0;
        EnemyProperty[2, 3, 5] = 100;

        EnemyProperty[3, 3, 0] = restoreArray[2];
        EnemyProperty[3, 3, 1] = armorArray[2];
        EnemyProperty[3, 3, 2] = 2;
        EnemyProperty[3, 3, 3] = 1;
        EnemyProperty[3, 3, 4] = 0;
        EnemyProperty[3, 3, 5] = 100;

        EnemyProperty[4, 3, 0] = restoreArray[3];
        EnemyProperty[4, 3, 1] = armorArray[3];
        EnemyProperty[4, 3, 2] = 2;
        EnemyProperty[4, 3, 3] = 1;
        EnemyProperty[4, 3, 4] = 0;
        EnemyProperty[4, 3, 5] = 100;

        //限制受到攻击次数
        EnemyProperty[1, 4, 0] = restoreArray[0];
        EnemyProperty[1, 4, 1] = armorArray[0];
        EnemyProperty[1, 4, 2] = 2;
        EnemyProperty[1, 4, 3] = 1;
        EnemyProperty[1, 4, 4] = 1;
        EnemyProperty[1, 4, 5] = 4;

        EnemyProperty[2, 4, 0] = restoreArray[1];
        EnemyProperty[2, 4, 1] = armorArray[1];
        EnemyProperty[2, 4, 2] = 2;
        EnemyProperty[2, 4, 3] = 1;
        EnemyProperty[2, 4, 4] = 1;
        EnemyProperty[2, 4, 5] = 3;

        EnemyProperty[3, 4, 0] = restoreArray[2];
        EnemyProperty[3, 4, 1] = armorArray[2];
        EnemyProperty[3, 4, 2] = 2;
        EnemyProperty[3, 4, 3] = 1;
        EnemyProperty[3, 4, 4] = 1;
        EnemyProperty[3, 4, 5] = 2;

        EnemyProperty[4, 4, 0] = restoreArray[3];
        EnemyProperty[4, 4, 1] = armorArray[3];
        EnemyProperty[4, 4, 2] = 2;
        EnemyProperty[4, 4, 3] = 1;
        EnemyProperty[4, 4, 4] = 1;
        EnemyProperty[4, 4, 5] = 1;

        //移速快
        EnemyProperty[1, 5, 0] = restoreArray[0];
        EnemyProperty[1, 5, 1] = armorArray[0];
        EnemyProperty[1, 5, 2] = 3;
        EnemyProperty[1, 5, 3] = 1;
        EnemyProperty[1, 5, 4] = 1;
        EnemyProperty[1, 5, 5] = 100;

        EnemyProperty[2, 5, 0] = restoreArray[1];
        EnemyProperty[2, 5, 1] = armorArray[1];
        EnemyProperty[2, 5, 2] = 4;
        EnemyProperty[2, 5, 3] = 1;
        EnemyProperty[2, 5, 4] = 1;
        EnemyProperty[2, 5, 5] = 100;

        EnemyProperty[3, 5, 0] = restoreArray[2];
        EnemyProperty[3, 5, 1] = armorArray[2];
        EnemyProperty[3, 5, 2] = 5;
        EnemyProperty[3, 5, 3] = 1;
        EnemyProperty[3, 5, 4] = 1;
        EnemyProperty[3, 5, 5] = 100;

        EnemyProperty[4, 5, 0] = restoreArray[3];
        EnemyProperty[4, 5, 1] = armorArray[3];
        EnemyProperty[4, 5, 2] = 6;
        EnemyProperty[4, 5, 3] = 1;
        EnemyProperty[4, 5, 4] = 1;
        EnemyProperty[4, 5, 5] = 100;

        //免疫眩晕
        EnemyProperty[1, 6, 0] = restoreArray[0];
        EnemyProperty[1, 6, 1] = armorArray[0];
        EnemyProperty[1, 6, 2] = 2;
        EnemyProperty[1, 6, 3] = 0;
        EnemyProperty[1, 6, 4] = 1;
        EnemyProperty[1, 6, 5] = 100;

        EnemyProperty[2, 6, 0] = restoreArray[1];
        EnemyProperty[2, 6, 1] = armorArray[1];
        EnemyProperty[2, 6, 2] = 2;
        EnemyProperty[2, 6, 3] = 0;
        EnemyProperty[2, 6, 4] = 1;
        EnemyProperty[2, 6, 5] = 100;

        EnemyProperty[3, 6, 0] = restoreArray[2];
        EnemyProperty[3, 6, 1] = armorArray[2];
        EnemyProperty[3, 6, 2] = 2;
        EnemyProperty[3, 6, 3] = 0;
        EnemyProperty[3, 6, 4] = 1;
        EnemyProperty[3, 6, 5] = 100;

        EnemyProperty[4, 6, 0] = restoreArray[3];
        EnemyProperty[4, 6, 1] = armorArray[3];
        EnemyProperty[4, 6, 2] = 2;
        EnemyProperty[4, 6, 3] = 0;
        EnemyProperty[4, 6, 4] = 1;
        EnemyProperty[4, 6, 5] = 100;
        #endregion

        //1回血快，2免疫减甲，3免疫减速
        //4限制次数攻击，5跑得快，6免疫眩晕
        #region PrefabsPositon
        PrefabsPositon[1,0] = 1;
        PrefabsPositon[1,1] = 0;
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

        PrefabsPositon[8,0] = 1;
        PrefabsPositon[8,1] = 2;
        PrefabsPositon[8,2] = 0;

        PrefabsPositon[9,0] = 1;
        PrefabsPositon[9,1] = 3;
        PrefabsPositon[9,2] = 0;

        PrefabsPositon[10,0] = 1;
        PrefabsPositon[10,1] = 4;
        PrefabsPositon[10,2] = 0;

        PrefabsPositon[11,0] = 1;
        PrefabsPositon[11,1] = 5;
        PrefabsPositon[11,2] = 0;

        PrefabsPositon[12,0] = 6;
        PrefabsPositon[12,1] = 1;
        PrefabsPositon[12,2] = 0;

        PrefabsPositon[13,0] = 2;
        PrefabsPositon[13,1] = 2;
        PrefabsPositon[13,2] = 0;

        PrefabsPositon[14,0] = 2;
        PrefabsPositon[14,1] = 3;
        PrefabsPositon[14,2] = 0;

        PrefabsPositon[15,0] = 2;
        PrefabsPositon[15,1] = 4;
        PrefabsPositon[15,2] = 0;

        PrefabsPositon[16,0] = 2;
        PrefabsPositon[16,1] = 5;
        PrefabsPositon[16,2] = 0;

        PrefabsPositon[17,0] = 6;
        PrefabsPositon[17,1] = 2;
        PrefabsPositon[17,2] = 0;

        PrefabsPositon[18,0] = 3;
        PrefabsPositon[18,1] = 4;
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
