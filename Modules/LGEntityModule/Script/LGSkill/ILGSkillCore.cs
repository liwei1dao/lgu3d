
using System;
using System.Collections.Generic;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using Sirenix.OdinInspector;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{
    [LabelText("技能CD状态")]
    public enum LGSkillCDState
    {
        CdEnd,
        CdIn,
    }
    [LabelText("技能释放状态")]
    public enum LGSkillState
    {
        [LabelText("未释放")]
        NoRelease,          //没有释放
        [LabelText("释放中")]
        InRelease,          //释放中
        [LabelText("CD冷却中")]
        InCd,               //CD冷却中
    }

    [LabelText("子弹目标类型")]
    public enum LGBulletTarget
    {
        [LabelText("无目标类型")]
        None,
        [LabelText("方向")]
        Direction,
        [LabelText("坐标")]
        Point,
        [LabelText("对象")]
        Object,
    }

    [LabelText("子弹轨道类型")]
    public enum LGBulletTrack
    {
        [LabelText("无轨道")]
        None,
        [LabelText("直线")]
        StraightLine,
        [LabelText("抛物线")]
        Parabola,
    }

    [LabelText("子弹触发类型")]
    public enum LGBulletTrigger
    {
        [LabelText("距离触发")]
        DistanceTrigger,

        [LabelText("碰撞检测")]
        CollisionTrigger,
    }

    [LabelText("技能效果生效提交类型")]
    public enum ConditionEventType
    {
        [LabelText("（空）")]
        None = 0,
        [LabelText("当生命值低于x")]
        WhenHPLower = 1,
        [LabelText("当生命值低于百分比x")]
        WhenHPPctLower = 2,
        [LabelText("当x秒内没有受伤")]
        WhenInTimeNoDamage = 3,
        [LabelText("当间隔x秒")]
        WhenIntervalTime = 4,
    }

    /// <summary>
    /// LG技能对象
    /// </summary>
    public interface ILGSkill
    {
        public LGSkillState State { get; set; }
        void CdEnd();
    }
    /// <summary>
    /// LG技能CD
    /// </summary>
    public interface ILGSkillCD
    {
        float Progress();
    }

    /// <summary>
    /// 子弹对象
    /// </summary>
    public interface ILGSkillBullet
    {
        void Launch(ILGSkill skill, params object[] agrs);
    }

    /// <summary>
    /// 技能效果
    /// </summary>
    public interface ILGEffect
    {
        bool Apply(ILGEntity target);
    }
}