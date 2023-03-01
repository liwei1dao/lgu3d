using System;

namespace lgu3d
{
    [Serializable]
    public class SkillCDBase
    {
        private ISkillBase Skill;
        public float CdTime;
        public float CurrCd;
        public float Progress;
        public SkillCDState State;

        public void CdStart()
        {
            CurrCd = CdTime;
            State = SkillCDState.CdIn;
        }

        public void Update(float time)
        {
            CurrCd -= time;
            Progress = CurrCd / CdTime;
            if (Progress <= 0)
            {
                CurrCd = 0;
                Progress = 0;
                CdEnd();
            }
        }

        public void CdEnd()
        {
            State = SkillCDState.CdEnd;
            Skill.CdEnd();
        }

    }
}
