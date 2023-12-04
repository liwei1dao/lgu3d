using System;

namespace lgu3d
{
    [Serializable]
    public class LGSkillCD
    {
        public float CdTime;
        public float CurrCd;
        public float Progress;
        public LGSkillCDState State;

        public void CdStart()
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

        }

    }
}
