using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{
    public abstract class BulletBase
    {
        protected SkillBase Skill;
        public BulletBase(SkillBase _Skill, params object[] _Agr)
        {
            Skill = _Skill;
        }


        public virtual void Launch(params object[] _Agr)
        {
            
        }

        public virtual void Update(float time)
        {
            
        }

        public virtual void Destroy()
        {
            Skill.RemoveBullet(this);
        }
    }

    public abstract class BulletBase<S> : BulletBase where S: SkillBase
    {
        protected new S Skill;
        public BulletBase(S _Skill, params object[] _Agr)
            :base(_Skill, _Agr)
        {
            Skill = _Skill;
        }
    }

}
