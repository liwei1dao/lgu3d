using System;
using UnityEngine;

namespace lgu3d
{
    public enum SkillState
    {
        NoRelease,          //没有释放
        InRelease,          //释放中
        InCd,               //CD冷却中
        Reloading,          //装弹中
    }
    public enum SkillCDState
    {
        CdEnd,
        CdIn,
    }
    //技能释放类型
    public enum SkilReleaseType
    {
        Target,                     //目标
        Direction                   //方向
    }
    public interface ISkillMonitor
    {
        void OnReleaseEnd(ISkillBase skil);
        void OnCdEnd(ISkillBase skil);
    }
    public interface ISkillBase : IEntityCompBase
    {
        public SkillState GetState();
        public SkilReleaseType GetSkilReleaseType();
        IEntityBase GetHostEntity();
        ///技能释放接口
        void Release(IEntityBase target, params object[] agrs);
        void Release(Vector3 direction, params object[] agrs);
        void CdEnd();
    }
}