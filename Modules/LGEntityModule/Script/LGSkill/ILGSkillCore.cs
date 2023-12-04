
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

    [LabelText("技能类型")]
    public enum LGSkillSpellType
    {
        [LabelText("主动技能")]
        Initiative,
        [LabelText("被动技能")]
        Passive,
    }
    [LabelText("技能目标阵营")]
    public enum LGSkillTargetFindType
    {
        [LabelText("自身")]
        Self = 0,
        [LabelText("己方")]
        SelfTeam = 1,
        [LabelText("敌方")]
        EnemyTeam = 2,
    }
    /// <summary>
    /// LG技能对象
    /// </summary>
    public interface ILGSkill
    {
        public LGSkillState State { get; set; }
    }
    /// <summary>
    /// LG技能CD
    /// </summary>
    public interface ILGSkillCD
    {
        float Progress();
    }

    /// <summary>
    /// 技能检索器
    /// </summary>
    public interface ILGSkillTargetFinder
    {
        List<ILGEntity> Finds(List<ILGEntity> targets);
    }

    /// <summary>
    /// 技能效果
    /// </summary>
    public interface ILGSkillEffect
    {
        //触发生效技能效果
        bool OnTriggerApplyEffect(ILGEntity target);
    }

}