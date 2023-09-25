using System;
using UnityEngine;

namespace lgu3d
{
    public enum SkillState
    {
        NoRelease,
        InRelease,
        InCd,
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
    public interface ISkillBase
    {
        public SkillState GetState();
        public SkilReleaseType GetSkilReleaseType();

        void Init(IEntityBase entity, params object[] agrs);
        ///技能释放接口
        void Release(IEntityBase target, params object[] agrs);
        void Release(Vector3 direction, params object[] agrs);
        void CdEnd();
    }
}