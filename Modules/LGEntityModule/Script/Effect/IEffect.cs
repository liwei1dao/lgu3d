
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
    [LabelText("作用对象")]
    public enum LGSkillEffetTargetType
    {
        [LabelText("技能目标")]
        SkillTarget = 0,
        [LabelText("自身")]
        Self = 1,
    }

    /// <summary>
    /// 效果触发类型
    /// </summary>
    public enum EffectTriggerType
    {
        [LabelText("（空）")]
        None = 0,
        [LabelText("立即触发")]
        Instant = 1,
        [LabelText("条件触发")]
        Condition = 2,
        [LabelText("行动点触发")]
        Action = 3,
        [LabelText("子弹命中触发")]
        BulletHit = 4,
        [LabelText("子弹销毁触发")]
        BulletDestroy = 5,
    }

    /// <summary>
    /// 效果执行实体
    /// </summary>
    public interface IEffetExecution : ILGEntity
    {
        /// <summary>
        /// 状态类型
        /// </summary>
        public LGEffect Effect { get; }
        /// <summary>
        /// 效果来源技能
        /// </summary>
        public ILGSkill SourceEffetSkill { get; }
        /// <summary>
        /// 效果来源实体
        /// </summary>
        public ILGBattleEntity SourceEffetEntity { get; }
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="entity"></param>
        void Execution();

    }


    /// <summary>
    /// 技能效果组件
    /// </summary>
    public interface ILGEntityEffetExecutionComp : ILGEntityComponent
    {
        void AddEffect(ILGSkill source, LGEffect effect);
    }

}