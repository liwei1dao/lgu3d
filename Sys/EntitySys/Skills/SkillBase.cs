using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{

    public interface ISkillMonitor 
    {
        void OnReleaseEnd(ISkillBase skil);
        void OnCdEnd(ISkillBase skil);
    }
    public interface ISkillBase
    {
        void LGInit(IEntityBase entity, ISkillMonitor monitor = null, params object[] agrs);
        ///技能释放接口
        void Release(params object[] agrs);
        void CdEnd();
    }

    public abstract class SkillBase<E, D> : EntityCompBase<E>, ISkillBase where E : class, IEntityBase  where D : class
    {
        public D Config;
        public SkillState State { get; set; }
        public SkillCDBase Cd;
        protected List<IBulletBase> Bullets;
        protected ISkillMonitor Monitor;

        public virtual void LGInit(IEntityBase entity, ISkillMonitor monitor = null, params object[] agrs)
        {
            base.LGInit(entity);
            State = SkillState.NoRelease;
            Monitor = monitor;
        }

        public virtual void Release(params object[] agrs)
        {
            State = SkillState.InRelease;
        }

        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
            Monitor?.OnReleaseEnd(this);
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
            Monitor?.OnCdEnd(this);
        }

    }

    public abstract class MonoSkillBase<E, D> : MonoEntityCompBase<E>, ISkillBase where E : class, IEntityBase where D : class
    {
        public D Config;
        public SkillState State { get; set; }
        public SkillCDBase Cd;
        protected List<IBulletBase> Bullets;
        protected ISkillMonitor Monitor;

        public virtual void LGInit(IEntityBase entity, ISkillMonitor monitor = null, params object[] agrs)
        {
            base.LGInit(entity);
            State = SkillState.NoRelease;
            Monitor = monitor;
        }

        public virtual void Release(params object[] agrs)
        {
            State = SkillState.InRelease;
        }

        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
            Monitor?.OnReleaseEnd(this);
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
            Monitor?.OnCdEnd(this);
        }

    }
}