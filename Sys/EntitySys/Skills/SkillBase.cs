using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public interface ISkillBase
    {

        ///技能释放接口
        void Release(params object[] agrs);
        void CdEnd();
    }

    public abstract class SkillBase<E> : EntityCompBase<E>, ISkillBase where E : class, IEntityBase
    {
        public SkillState State { get; set; }
        public SkillCDBase Cd;
        protected List<IBulletBase> Bullets;

        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity);
            State = SkillState.NoRelease;
        }

        public virtual void Release(params object[] agrs)
        {
            State = SkillState.InRelease;
        }

        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
        }

    }

    public abstract class MonoSkillBase<E> : MonoEntityCompBase<E>, ISkillBase where E : class, IEntityBase
    {
        public SkillState State { get; set; }
        public SkillCDBase Cd;
        protected List<IBulletBase> Bullets;

        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity);
            State = SkillState.NoRelease;
        }

        public virtual void Release(params object[] agrs)
        {
            State = SkillState.InRelease;
        }

        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
        }

    }
}