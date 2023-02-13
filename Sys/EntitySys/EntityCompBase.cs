using System;
using UnityEngine;

namespace lgu3d
{
    public abstract class EntityCompBase : IEntityCompBase
    {
        public IEntityBase Entity { get; set; }

        public virtual void LGInit(IEntityBase entity, params object[] agrs)
        {
            Entity = entity;
        }

        public virtual void LGStart()
        {

        }

        public virtual void LGDestroy()
        {

        }
    }
    public abstract class EntityCompBase<E> : EntityCompBase where E : IEntityBase
    {
        public new E Entity { get; set; }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            throw new System.Exception("请使用 Init(E entity, params object[] agrs) 接口初始化");
        }
        public virtual void Init(E entity, params object[] agrs)
        {
            base.LGInit(entity);
            Entity = entity;
        }
    }

    public abstract class MonoEntityCompBase : MonoBehaviour, IEntityCompBase
    {
        public IEntityBase Entity { get; set; }

        public virtual void LGInit(IEntityBase entity, params object[] agrs)
        {
            Entity = entity;
        }

        public virtual void LGStart()
        {

        }

        public virtual void LGDestroy()
        {

        }
    }
    public abstract class MonoEntityCompBase<E> : MonoEntityCompBase where E : IEntityBase
    {
        public new E Entity { get; set; }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            throw new System.Exception("请使用 Init(E entity, params object[] agrs) 接口初始化");
        }
        public virtual void Init(E entity, params object[] agrs)
        {
            base.LGInit(entity);
            Entity = entity;
        }
    }
}
