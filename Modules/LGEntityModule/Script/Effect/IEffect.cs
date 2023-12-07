
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
        [LabelText("其他")]
        Other = 2,
    }


    /// <summary>
    /// 效果执行
    /// </summary>
    public interface IEffetExecution<E> : IReference where E : ILGEntity
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
        public E SourceEffetEntity { get; }
        /// <summary>
        /// 效果目标实体
        /// </summary>
        public E TargetEntity { get; }
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