
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
    public interface IEffetExecution<E> where E : ILGEntity
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="entity"></param>
        void Execution(E entity);

    }


    /// <summary>
    /// 技能效果组件
    /// </summary>
    public interface ILGEntityEffetExecutionComp : ILGEntityComponent
    {

    }

}