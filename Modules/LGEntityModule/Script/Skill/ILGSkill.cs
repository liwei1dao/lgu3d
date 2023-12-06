
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
    public enum SkillSpellType
    {
        [LabelText("主动技能")]
        Initiative,
        [LabelText("被动技能")]
        Passive,
    }

    /// <summary>
    /// LG技能对象
    /// </summary>
    public interface ILGSkill : ILGEntityComponent
    {
        public LGSkillState State { get; set; }


        void CdEnd();
    }


    /// <summary>
    /// 技能效果
    /// </summary>
    public interface ILGEffect
    {
        bool Apply(ILGEntity target);
    }
}