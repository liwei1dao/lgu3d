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
    public abstract class EntityCompBase<E> : EntityCompBase where E : class, IEntityBase
    {
        public new E Entity { get; set; }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity);
            Entity = entity as E;
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
    public abstract class MonoEntityCompBase<E> : MonoEntityCompBase where E : class, IEntityBase
    {
        public new E Entity { get; set; }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity);
            Entity = entity as E;
        }
    }
}
