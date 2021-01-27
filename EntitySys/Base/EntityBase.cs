using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySys {

    public interface IEntity {
        void OnBirth();
        void OnDeath();
        void AddComp(IEntityComp comp);
    }

    #region EntityBase
    public abstract class EntityBase : IEntity
    {
        protected List<IEntityComp> comps;
        public virtual void AddComp(IEntityComp comp)
        {
            comps.Add(comp);
        }

        public virtual void OnBirth() {
            comps = new List<IEntityComp>();
        }

        public abstract void OnDeath();
    }
    public abstract class EntityBase<C> : EntityBase where C: EntityConifgBase
    {
        public C Config;
        public virtual void OnBirth(C conifg) {
            Config = conifg;
            OnBirth();
        }
    }

    #endregion

    #region MonoEntityBase
    public abstract class MonoEntityBase :MonoBehaviour,IEntity
    {
        protected List<IEntityComp> comps;

        public virtual void AddComp(IEntityComp comp)
        {
            comp.Load(this);
            comps.Add(comp);
        }

        public void OnBirth()
        {
            comps = new List<IEntityComp>();
        }

        public void OnDeath()
        {

        }

    }
    public abstract class MonoEntityBase<C> : MonoEntityBase where C : EntityConifgBase
    {
        public C Config;

        public virtual void OnBirth(C conifg)
        {
            Config = conifg;
            OnBirth();
        }

        protected virtual void Awake()
        {
            OnBirth(Config);
        }
    }


    #endregion
}
