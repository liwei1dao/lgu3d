using System;
using UnityEngine;

namespace lgu3d
{
    public abstract class EntityCompBase : IEntityCompBase
    {
        public IEntityBase Entity { get; set; }

        public virtual void Load(IEntityBase entity, params object[] agrs)
        {
            Entity = entity;
        }

        public virtual void Init()
        {

        }

        public virtual void Destroy()
        {

        }
    }
    public abstract class EntityCompBase<E> : IEntityCompBase<E> where E : IEntityBase
    {
        public E Entity { get; set; }

        public virtual void Load(IEntityBase entity, params object[] agrs)
        {
            Entity = entity as E;
        }

        public virtual void Init()
        {

        }

        public virtual void Destroy()
        {

        }
    }

    public abstract class MonoEntityCompBase : MonoBehaviour, IEntityCompBase
    {
        public IEntityBase Entity { get; set; }

        public virtual void Load(IEntityBase entity, params object[] agrs)
        {
            Entity = entity;
        }

        public virtual void Destroy()
        {
            GameObject.Destroy(this);
        }

        public virtual void Init()
        {

        }
    }
    public abstract class MonoEntityCompBase<E> : MonoBehaviour, IEntityCompBase<E> where E : MonoEntityBase
    {
        public IEntityBase Entity { get; set; }

        public virtual void Load(IEntityBase entity, params object[] agrs)
        {
            Entity = entity;
        }

        public virtual void Destroy()
        {
            GameObject.Destroy(this);
        }

        public virtual void Init()
        {

        }
    }
}
