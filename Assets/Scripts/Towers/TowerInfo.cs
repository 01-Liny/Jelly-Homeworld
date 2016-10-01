using UnityEngine;
using System.Collections;


public enum TowerType //Deprecated
{
    Strike,         //单体，减甲
    Stun,           //单体，眩晕
    Mutiple,        //分裂攻击

    SourceBuffSlowdown,       //减速Buff
    SourceBuffRate,       //加攻速Buff
    SourceBuffDamage,     //加攻击Buff

    MaxCount
}

public enum TowerElem
{
    Strike, //破甲
    Stun,   //眩晕
    Slowdown,   //减速
    Rate,   //攻速
    Range,  //范围

    MaxCount,
    NULL    //为空 表示该类型的变量值为空 不含有任何元素 通常用来初始化
}



public enum TowerLevel
{
    One,
    Two,
    Three,
    Four,

    MaxLevel,
    Empty//表示没有这个参数，用于TakeDamage函数
}


public class TowerElemInfo
{
    public static int MAX_ELEM = 3;

    public static float[] basicFireRange;//基础攻击范围
    public static float[] basicFireDamage;//基础攻击伤害
    public static float[] basicFireRate;//基础攻击频率 每秒攻击几次


    public static float[] strikeArmor;//破甲 破甲值为具体数值
    public static float[] stunTime;//眩晕时间 单位秒
    public static float[] stunProbability;//眩晕几率 1~100
    public static float[] extraFireRate;//攻击频率

    public static float[] slowdownDegree;//减速幅度 百分比减速
    public static float[] slowdownTime;//减速时间

    public static float[] extraFireRange;//攻击范围
    public static float[] extraFireRangeOffset;//攻击范围元素造成的各属性削弱

    public static float[,] towerSize;//塔的尺寸

    //初始化防御塔数据
    public static void Init()
    {
        int temp = MAX_ELEM + 1;//从1开始算，初始的塔自带一个元素，值为1
        basicFireRange = new float[temp];
        basicFireDamage = new float[temp];
        basicFireRate = new float[temp];
        strikeArmor = new float[temp];
        stunTime = new float[temp];
        stunProbability = new float[temp];
        extraFireRate = new float[temp];
        slowdownDegree = new float[temp];
        slowdownTime = new float[temp];
        extraFireRange = new float[temp];
        extraFireRangeOffset = new float[temp];
        towerSize = new float[temp, 4];//4表示Scale:x,y,z  Position:y

        #region basicFireRange
        basicFireRange[0] = 0;
        basicFireRange[1] = 4;
        basicFireRange[2] = 4;
        basicFireRange[3] = 4;
        #endregion

        #region basicFireDamage
        basicFireDamage[0] = 0;
        basicFireDamage[1] = 20;
        basicFireDamage[2] = 40;
        basicFireDamage[3] = 80;
        #endregion

        #region basicFireRate
        basicFireRate[0] = 0;
        basicFireRate[1] = 1;
        basicFireRate[2] = 1;
        basicFireRate[3] = 1;
        #endregion

        #region strikeArmor
        strikeArmor[0] = 0;
        strikeArmor[1] = 10;
        strikeArmor[2] = 20;
        strikeArmor[3] = 40;
        #endregion

        #region stunTime
        stunTime[0] = 0;
        stunTime[1] = 0.5f;
        stunTime[2] = 0.5f;
        stunTime[3] = 0.5f;
        #endregion

        #region stunProbability
        stunProbability[0] = 0;
        stunProbability[1] = 30;
        stunProbability[2] = 50;
        stunProbability[3] = 70;
        #endregion

        #region fireRate
        extraFireRate[0] = 0;
        extraFireRate[1] = 1;
        extraFireRate[2] = 1.5f;
        extraFireRate[3] = 2;
        #endregion

        #region slowdownDegree
        slowdownDegree[0] = 0;
        slowdownDegree[1] = 0.15f;
        slowdownDegree[2] = 0.25f;
        slowdownDegree[3] = 0.4f;
        #endregion

        #region slowdownTime
        slowdownTime[0] = 0;
        slowdownTime[1] = 0.3f;
        slowdownTime[2] = 0.6f;
        slowdownTime[3] = 1;
        #endregion

        #region fireRange
        extraFireRange[0] = 0;
        extraFireRange[1] = 2;
        extraFireRange[2] = 4;
        extraFireRange[3] = 6;
        #endregion

        #region fireRangeOffset
        extraFireRangeOffset[0] = 1;
        extraFireRangeOffset[1] = 0.5f;
        extraFireRangeOffset[2] = 0.5f;
        extraFireRangeOffset[3] = 0.5f;
        #endregion

        #region towerSize
        //一个元素
        towerSize[1, 0] = 1f;
        towerSize[1, 1] = 1f;
        towerSize[1, 2] = 1f;
        towerSize[1, 3] = 0;

        //两个元素
        towerSize[2, 0] = 2f;
        towerSize[2, 1] = 1.5f;
        towerSize[2, 2] = 2f;
        towerSize[2, 3] = 0;

        //三个元素
        towerSize[3, 0] = 2f;
        towerSize[3, 1] = 2f;
        towerSize[3, 2] = 2f;
        towerSize[3, 3] = 0.5f;
        #endregion
    }
}

public class TowerInfo : MonoBehaviour
{
    public static float[,] fireRange;                  //攻击范围
    public static float[,] fireDamage;                 //攻击伤害
    public static float[,] fireRate;                   //多少秒攻击一次

    public static float[,] strikeArmor;                //减护甲 简单扣除
    public static float[,] stunTime;                   //眩晕时间 单位秒
    public static int[,] mutipleTargetCount;         //多重攻击目标个数
    public static float[,] sourceBuffSlow;             //百分比减速
    public static float[,] sourceBuffDamage;           //攻击加成 简单叠加
    public static float[,] sourceBuffFireRate;         //百分比加成
    public static float[,] debuffDuringTime;            //Debuff持续时间 秒

    public static float[,] towerLevelSize;              //塔随着等级的大小

    //初始化防御塔数据
    public static void Init()
    {
        fireRange = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        fireDamage = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        fireRate = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        strikeArmor = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        stunTime = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        mutipleTargetCount = new int[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        sourceBuffSlow = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        sourceBuffDamage = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        sourceBuffFireRate = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        debuffDuringTime = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];

        //后面四个值对应transform scale的x,y,x和position的y
        towerLevelSize = new float[(int)TowerLevel.MaxLevel, 4];

        //塔的Size
        {
            //一级
            towerLevelSize[(int)TowerLevel.One, 0] = 1.5f;
            towerLevelSize[(int)TowerLevel.One, 1] = 1.5f;
            towerLevelSize[(int)TowerLevel.One, 2] = 1.5f;
            towerLevelSize[(int)TowerLevel.One, 3] = 0;

            //二级
            towerLevelSize[(int)TowerLevel.Two, 0] = 2f;
            towerLevelSize[(int)TowerLevel.Two, 1] = 1.5f;
            towerLevelSize[(int)TowerLevel.Two, 2] = 2f;
            towerLevelSize[(int)TowerLevel.Two, 3] = 0;

            //三级
            towerLevelSize[(int)TowerLevel.Three, 0] = 2f;
            towerLevelSize[(int)TowerLevel.Three, 1] = 1.5f;
            towerLevelSize[(int)TowerLevel.Three, 2] = 2f;
            towerLevelSize[(int)TowerLevel.Three, 3] = 0;

            //四级
            towerLevelSize[(int)TowerLevel.Four, 0] = 2f;
            towerLevelSize[(int)TowerLevel.Four, 1] = 2f;
            towerLevelSize[(int)TowerLevel.Four, 2] = 2f;
            towerLevelSize[(int)TowerLevel.Four, 3] = 0f;
        }

        //单体，减甲塔
        {
            //一级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.One] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.One] = 20;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.One] = 1f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.One] = 5;
            debuffDuringTime[(int)TowerType.Strike, (int)TowerLevel.One] = 1f;

            //二级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.Two] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.Two] = 30;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.Two] = 1.5f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.Two] = 10;
            debuffDuringTime[(int)TowerType.Strike, (int)TowerLevel.Two] = 1f;

            //三级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.Three] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.Three] = 40;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.Three] = 2f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.Three] = 15;
            debuffDuringTime[(int)TowerType.Strike, (int)TowerLevel.Three] = 1f;

            //四级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.Four] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.Four] = 50;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.Four] = 2.5f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.Four] = 20;
            debuffDuringTime[(int)TowerType.Strike, (int)TowerLevel.Four] = 1f;
        }

        //单体，眩晕塔
        {
            //一级
            fireRange[(int)TowerType.Stun, (int)TowerLevel.One] = 2;
            fireDamage[(int)TowerType.Stun, (int)TowerLevel.One] = 20;
            fireRate[(int)TowerType.Stun, (int)TowerLevel.One] = 1f;
            stunTime[(int)TowerType.Stun, (int)TowerLevel.One] = 0.2f;

            //二级
            fireRange[(int)TowerType.Stun, (int)TowerLevel.Two] = 2;
            fireDamage[(int)TowerType.Stun, (int)TowerLevel.Two] = 30;
            fireRate[(int)TowerType.Stun, (int)TowerLevel.Two] = 1.5f;
            stunTime[(int)TowerType.Stun, (int)TowerLevel.Two] = 0.4f;

            //三级
            fireRange[(int)TowerType.Stun, (int)TowerLevel.Three] = 2;
            fireDamage[(int)TowerType.Stun, (int)TowerLevel.Three] = 40;
            fireRate[(int)TowerType.Stun, (int)TowerLevel.Three] = 2f;
            stunTime[(int)TowerType.Stun, (int)TowerLevel.Three] = 0.7f;

            //四级
            fireRange[(int)TowerType.Stun, (int)TowerLevel.Four] = 2;
            fireDamage[(int)TowerType.Stun, (int)TowerLevel.Four] = 50;
            fireRate[(int)TowerType.Stun, (int)TowerLevel.Four] = 2.5f;
            stunTime[(int)TowerType.Stun, (int)TowerLevel.Four] = 1f;
        }

        //分裂攻击塔
        {
            //一级
            fireRange[(int)TowerType.Mutiple, (int)TowerLevel.One] = 3;
            fireDamage[(int)TowerType.Mutiple, (int)TowerLevel.One] = 10;
            fireRate[(int)TowerType.Mutiple, (int)TowerLevel.One] = 1f;
            mutipleTargetCount[(int)TowerType.Mutiple, (int)TowerLevel.One] = 3;

            //二级
            fireRange[(int)TowerType.Mutiple, (int)TowerLevel.Two] = 3;
            fireDamage[(int)TowerType.Mutiple, (int)TowerLevel.Two] = 15;
            fireRate[(int)TowerType.Mutiple, (int)TowerLevel.Two] = 1.5f;
            mutipleTargetCount[(int)TowerType.Mutiple, (int)TowerLevel.Two] = 3;

            //三级
            fireRange[(int)TowerType.Mutiple, (int)TowerLevel.Three] = 3;
            fireDamage[(int)TowerType.Mutiple, (int)TowerLevel.Three] = 20;
            fireRate[(int)TowerType.Mutiple, (int)TowerLevel.Three] = 2f;
            mutipleTargetCount[(int)TowerType.Mutiple, (int)TowerLevel.Three] = 3;

            //四级
            fireRange[(int)TowerType.Mutiple, (int)TowerLevel.Four] = 3;
            fireDamage[(int)TowerType.Mutiple, (int)TowerLevel.Four] = 25;
            fireRate[(int)TowerType.Mutiple, (int)TowerLevel.Four] = 2.5f;
            mutipleTargetCount[(int)TowerType.Mutiple, (int)TowerLevel.Four] = 3;
        }

        //减速Buff塔
        {
            //一级
            fireRange[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.One] = 3f;
            fireDamage[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.One] = 6;
            fireRate[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.One] = 1f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.One] = 0.15f;
            debuffDuringTime[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.One] = 1f;

            //二级
            fireRange[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Two] = 3.5f;
            fireDamage[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Two] = 9;
            fireRate[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Two] = 1.5f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Two] = 0.25f;
            debuffDuringTime[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Two] = 1f;

            //三级
            fireRange[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Three] = 4f;
            fireDamage[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Three] = 12;
            fireRate[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Three] = 2f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Three] = 0.4f;
            debuffDuringTime[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Three] = 1f;

            //四级
            fireRange[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Four] = 4.5f;
            fireDamage[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Four] = 15;
            fireRate[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Four] = 2.5f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Four] = 0.55f;
            debuffDuringTime[(int)TowerType.SourceBuffSlowdown, (int)TowerLevel.Four] = 1f;
        }

        //加攻速Buff
        {
            //一级
            fireRange[(int)TowerType.SourceBuffRate, (int)TowerLevel.One] = 3f;
            fireDamage[(int)TowerType.SourceBuffRate, (int)TowerLevel.One] = 6;
            fireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.One] = 1f;
            sourceBuffFireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.One] = 0.2f;

            //二级
            fireRange[(int)TowerType.SourceBuffRate, (int)TowerLevel.Two] = 3.5f;
            fireDamage[(int)TowerType.SourceBuffRate, (int)TowerLevel.Two] = 9;
            fireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.Two] = 1.5f;
            sourceBuffFireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.Two] = 0.4f;

            //三级
            fireRange[(int)TowerType.SourceBuffRate, (int)TowerLevel.Three] = 4f;
            fireDamage[(int)TowerType.SourceBuffRate, (int)TowerLevel.Three] = 12;
            fireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.Three] = 2f;
            sourceBuffFireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.Three] = 0.6f;

            //四级
            fireRange[(int)TowerType.SourceBuffRate, (int)TowerLevel.Four] = 4.5f;
            fireDamage[(int)TowerType.SourceBuffRate, (int)TowerLevel.Four] = 15;
            fireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.Four] = 2.5f;
            sourceBuffFireRate[(int)TowerType.SourceBuffRate, (int)TowerLevel.Four] = 0.8f;
        }

        //加攻击Buff
        {
            //一级
            fireRange[(int)TowerType.SourceBuffDamage, (int)TowerLevel.One] = 3f;
            fireDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.One] = 6;
            fireRate[(int)TowerType.SourceBuffDamage, (int)TowerLevel.One] = 1f;
            sourceBuffDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.One] = 10f;

            //二级
            fireRange[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Two] = 3.5f;
            fireDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Two] = 9;
            fireRate[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Two] = 1.5f;
            sourceBuffDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Two] = 15f;

            //三级
            fireRange[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Three] = 4f;
            fireDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Three] = 12;
            fireRate[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Three] = 2f;
            sourceBuffDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Three] = 20f;

            //四级
            fireRange[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Four] = 4.5f;
            fireDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Four] = 15;
            fireRate[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Four] = 2.5f;
            sourceBuffDamage[(int)TowerType.SourceBuffDamage, (int)TowerLevel.Four] = 25f;
        }
    }

}
