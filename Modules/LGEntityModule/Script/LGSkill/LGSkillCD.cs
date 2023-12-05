using System;

namespace lgu3d
{
    public class LGSkillCD
    {
        private ILGSkill Skill;
        public float CdTime;
        public float CurrCd;
        public float Progress;
        public LGSkillCDState State;

        public LGSkillCD(ILGSkill skill)
        {
            this.Skill = skill;
        }

        public void CdStart(float cd)
        {
            CurrCd = CdTime;
            State = LGSkillCDState.CdIn;
        }

        public void Update(float time)
        {
            if (State == LGSkillCDState.CdIn)
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
        }

        public void CdEnd()
        {
            Skill.CdEnd();
        }

    }
}
