using UnityEngine;
using UnityEditor;

namespace EntitySys
{
    public interface IEntityComp 
    {
        void Load(IEntity entity);
    }

    #region EntityCompBase
    public abstract class EntityCompBase : IEntityComp
    {
        protected IEntity Entity;

        public void Load(IEntity entity)
        {
            Entity = entity;
        }
    }

    [RequireComponent(typeof(EntityBase))]
    public abstract class EntityCompBase<E> : EntityCompBase where E: EntityBase
    {
        protected new E Entity;

    }
    #endregion

    #region MonoEntityCompBase
    [RequireComponent(typeof(MonoEntityBase))]
    public abstract class MonoEntityCompBase :MonoBehaviour, IEntityComp
    {
        protected IEntity Entity;

        public virtual void Load(IEntity entity)
        {
            Entity = entity;
        }

        protected void Start()
        {
            transform.GetComponent<MonoEntityBase>().AddComp(this);
        }
    }

    [RequireComponent(typeof(MonoEntityBase))]
    public abstract class MonoEntityCompBase<E> : MonoEntityCompBase where E: class, IEntity
    {
        protected new E Entity;

        public override void Load(IEntity entity)
        {
            base.Load(entity);
            Entity = entity as E;
        }

    }
    #endregion
}