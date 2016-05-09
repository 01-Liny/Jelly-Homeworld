using UnityEngine;
using System.Collections;

public enum TowerType
{
    Strike,         //单体，减甲
    Stun,           //单体，眩晕
    Mutiple,        //分裂攻击

    SourceBuffSlow,       //减速Buff
    SourceBuffRate,       //加攻速Buff
    SourceBuffDamage,     //加攻击Buff

    MaxCount
}

public enum TowerLevel
{
    One,
    Two,
    Three,
    Four,

    MaxLevel
}

public class TowerInfo : MonoBehaviour
{
    public static float[,] fireRange;                  //攻击范围
    public static float[,] fireDamage;                 //攻击伤害
    public static float[,] fireRate;                   //多少秒攻击一次

    public static float[,] strikeArmor;                //减护甲 简单扣除
    public static float[,] stunTime;                   //眩晕时间 单位秒
    public static float[,] mutipleTargetCount;         //多重攻击目标个数
    public static float[,] sourceBuffSlow;             //百分比减速
    public static float[,] sourceBuffDamage;           //攻击加成 简单叠加
    public static float[,] sourceBuffFireRate;         //百分比加成

    //初始化防御塔数据
    public static void Ini()
    {
        fireRange = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        fireDamage = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        fireRate = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        strikeArmor = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        stunTime = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        mutipleTargetCount = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        sourceBuffSlow = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        sourceBuffDamage = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];
        sourceBuffFireRate = new float[(int)TowerType.MaxCount, (int)TowerLevel.MaxLevel];

        //单体，减甲塔
        {
            //一级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.One] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.One] = 20;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.One] = 1f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.One] = 5;

            //二级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.Two] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.Two] = 30;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.Two] = 1.5f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.Two] = 10;

            //三级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.Three] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.Three] = 40;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.Three] = 2f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.Three] = 15;

            //四级
            fireRange[(int)TowerType.Strike, (int)TowerLevel.Four] = 2;
            fireDamage[(int)TowerType.Strike, (int)TowerLevel.Four] = 50;
            fireRate[(int)TowerType.Strike, (int)TowerLevel.Four] = 2.5f;
            strikeArmor[(int)TowerType.Strike, (int)TowerLevel.Four] = 20;
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
            fireRange[(int)TowerType.SourceBuffSlow, (int)TowerLevel.One] = 3f;
            fireDamage[(int)TowerType.SourceBuffSlow, (int)TowerLevel.One] = 6;
            fireRate[(int)TowerType.SourceBuffSlow, (int)TowerLevel.One] = 1f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlow, (int)TowerLevel.One] = 0.15f;

            //二级
            fireRange[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Two] = 3.5f;
            fireDamage[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Two] = 9;
            fireRate[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Two] = 1.5f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Two] = 0.25f;

            //三级
            fireRange[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Three] = 4f;
            fireDamage[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Three] = 12;
            fireRate[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Three] = 2f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Three] = 0.4f;

            //四级
            fireRange[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Four] = 4.5f;
            fireDamage[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Four] = 15;
            fireRate[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Four] = 2.5f;
            sourceBuffSlow[(int)TowerType.SourceBuffSlow, (int)TowerLevel.Four] = 0.55f;
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
