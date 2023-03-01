using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{
    //子弹
    public interface IBulletBase
    {
        void LGInit(ISkillBase skill);
    }

    public abstract class BulletBase<S> : IBulletBase where S : class, ISkillBase
    {
        public S Skill;
        public BulletBase(S skill)
        {
            LGInit(skill);
        }

        public virtual void LGInit(ISkillBase skill)
        {
            Skill = skill as S;
        }
    }

    public abstract class MonoBulletBase<S> : MonoBehaviour where S : class, ISkillBase
    {
        public S Skill;
        public virtual void LGInit(ISkillBase skill)
        {
            Skill = skill as S;
        }
    }
}
