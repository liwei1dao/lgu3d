﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public abstract class SkillBase<E, D> : EntityCompBase<E>, ISkillBase where E : class, IEntityBase where D : class
    {
        public D Config;
        public SkilReleaseType ReleaseType;
        public SkillState State;
        public SkillCDBase Cd;
        protected List<IBulletBase> Bullets;
        public SkillState GetState()
        {
            return State;
        }
        public SkilReleaseType GetSkilReleaseType()
        {
            return ReleaseType;
        }
        public virtual void Init(IEntityBase entity, params object[] agrs)
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

    }
    public abstract class MonoSkillBase<E> : MonoEntityCompBase<E>, ISkillBase where E : class, IEntityBase
    {
        public SkilReleaseType ReleaseType;
        public SkillState State;
        public SkillCDBase Cd;
        protected List<IBulletBase> Bullets;

        public SkilReleaseType GetSkilReleaseType()
        {
            return ReleaseType;
        }
        public SkillState GetState()
        {
            return State;
        }
        public virtual void Init(IEntityBase entity, params object[] agrs)
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

        protected virtual void Update()
        {
            Cd.Update(Time.deltaTime);
        }
    }
    public abstract class MonoSkillBase<E, D> : MonoEntityCompBase<E>, ISkillBase where E : class, IEntityBase where D : class
    {
        public D Config;
        public SkilReleaseType ReleaseType;
        public SkillState State;
        public SkillCDBase Cd;
        protected List<IBulletBase> Bullets;

        public SkilReleaseType GetSkilReleaseType()
        {
            return ReleaseType;
        }
        public SkillState GetState()
        {
            return State;
        }
        public virtual void Init(IEntityBase entity, params object[] agrs)
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

        protected virtual void Update()
        {
            Cd.Update(Time.deltaTime);
        }

    }
}