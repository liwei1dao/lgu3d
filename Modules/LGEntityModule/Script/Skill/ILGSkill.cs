
using System;
using System.Collections.Generic;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using Sirenix.OdinInspector;
using UnityEngine;

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

    [LabelText("技能目标输入类型")]
    public enum SkillTargetInputType
    {
        [LabelText("自动选择")]
        Auto = 0,
        [LabelText("目标实体")]
        Entity = 1,
        [LabelText("目标方向")]
        Direction = 2,
        [LabelText("目标点")]
        Point = 3,
        [LabelText("动态点")]
        DynamicPoints = 4,
    }


    [LabelText("弹道类型")]
    public enum SkillBallisticsType
    {
        [LabelText("无弹道")]
        None = 0,
        [LabelText("直线弹道")]
        StraightLine = 1,
        [LabelText("抛物线弹道")]
        Parabola = 2,
    }

    /// <summary>
    /// LG技能对象
    /// </summary>
    public interface ILGSkill : ILGEntityComponent
    {
        public LGSkillState State { get; set; }

        /// <summary>
        /// 技能条件检测
        /// </summary>
        /// <returns></returns>
        bool Condition();
        /// <summary>
        /// 无参数释放
        /// </summary>
        void Release();
        /// <summary>
        /// 目标实体释放
        /// </summary>
        /// <param name="target"></param>
        void Release_Entity(ILGEntity target);
        /// <summary>
        /// 目标实体释放
        /// </summary>
        /// <param name="target"></param>
        void Release_Direction(Vector3 Direction);
        /// <summary>
        /// 目标实体释放
        /// </summary>
        /// <param name="target"></param>
        void Release_Point(Vector3 Point);
        /// <summary>
        /// 目标实体释放
        /// </summary>
        /// <param name="target"></param>
        void Release_DynamicPoints(Transform Point);

        ILGBullet CreateBullet();

        void ReleaseEnd();
        void CdEnd();
    }

    /// <summary>
    /// 子弹对象
    /// </summary>
    public interface ILGBullet
    {

    }

}