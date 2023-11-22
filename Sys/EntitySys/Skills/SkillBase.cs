using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace lgu3d
{
    public abstract class SkillBase<E, D> : EntityCompBase<E>, ISkillBase where E : class, IEntityBase where D : class
    {
        public D Config;
        public SkillState State;
        public SkillCDBase Cd;
        public SkillState GetState()
        {
            return State;
        }

        public virtual IEntityBase GetHostEntity()
        {
            return Entity;
        }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity);
            Cd.Skill = this;
            State = SkillState.NoRelease;
        }
        public virtual void Release(IEntityBase target, params object[] agrs)
        {
            throw new Exception("Release no Overload");
        }
        public virtual void Release(Vector3 direction, params object[] agrs)
        {
            throw new Exception("Release no Overload");
        }

        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
            Cd.CdStart();
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
        }
        /// <summary>
        /// 回收子弹
        /// </summary>
        /// <param name="bullet"></param>
        public virtual void ReclaimIBullet(IBullet bullet)
        {

        }
    }
    public abstract class MonoSkillBase<E> : MonoEntityCompBase<E>, ISkillBase where E : class, IEntityBase
    {
        public SkillState State;
        public SkillCDBase Cd;

        public SkillState GetState()
        {
            return State;
        }
        public virtual IEntityBase GetHostEntity()
        {
            return Entity;
        }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            Cd ??= new SkillCDBase();
            Cd.Skill = this;
            State = SkillState.NoRelease;
        }

        public virtual void Release(IEntityBase target, params object[] agrs)
        {
            throw new Exception("Release no Overload");
        }
        public virtual void Release(Vector3 direction, params object[] agrs)
        {
            throw new Exception("Release no Overload");
        }
        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
            Cd.CdStart();
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
        }
        /// <summary>
        /// 回收子弹
        /// </summary>
        /// <param name="bullet"></param>
        public virtual void ReclaimIBullet(IBullet bullet)
        {

        }
        protected virtual void Update()
        {
            Cd.Update(Time.deltaTime);
        }
    }
    public abstract class MonoSkillBase<E, D> : MonoEntityCompBase<E>, ISkillBase where E : class, IEntityBase where D : class
    {
        public D Config;
        public SkillState State;
        public SkillCDBase Cd;

        public SkillState GetState()
        {
            return State;
        }
        public virtual IEntityBase GetHostEntity()
        {
            return Entity;
        }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            Cd.Skill = this;
            State = SkillState.NoRelease;
        }

        public virtual void Release(params object[] agrs)
        {
            State = SkillState.InRelease;

        }
        public virtual void Release(IEntityBase target, params object[] agrs)
        {
            throw new Exception("Release no Overload");
        }
        public virtual void Release(Vector3 direction, params object[] agrs)
        {
            throw new Exception("Release no Overload");
        }
        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
            Cd.CdStart();
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
        }
        /// <summary>
        /// 回收子弹
        /// </summary>
        /// <param name="bullet"></param>
        public virtual void ReclaimIBullet(IBullet bullet)
        {

        }
        protected virtual void Update()
        {
            Cd.Update(Time.deltaTime);
        }

    }
}