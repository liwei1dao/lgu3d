using System;

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
    public interface ISkillMonitor 
    {
        void OnReleaseEnd(ISkillBase skil);
        void OnCdEnd(ISkillBase skil);
    }
    public interface ISkillBase
    {
        public SkillState GetState ();
        void Init(IEntityBase entity,  params object[] agrs);
        ///技能释放接口
        void Release(params object[] agrs);
        void CdEnd();
    }
}