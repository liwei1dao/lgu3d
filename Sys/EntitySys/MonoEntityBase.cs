using System.Collections;
using UnityEngine;

namespace lgu3d
{
    public abstract class MonoEntityBase : MonoBehaviour, IEntityBase
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        /// <value></value>
        public long EntityID { get; private set; }
        public IEntityBase Entity { get; set; }
        IEntityBase IEntityBase.Entity { get; set; }
        protected List<IEntityCompBase> Comps;
        #region 框架函数
        public virtual void Init(IEntityBase entity)
        {
            EntityID = Utils.GuidToLongID();
            Entity = entity as IMonoEntityBase;
            Comps = new List<IEntityCompBase>();
        }

        public virtual void Start()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Start();
            }
        }
        public virtual CP AddComp<CP>(params object[] agrs) where CP : Component, IEntityCompBase
        {
            CP comp = gameObject.AddMissingComponent<CP>();
            comp.Load(Entity, agrs);
            Comps.Add(comp);
            return comp;
        }

        public virtual CP GetComp<CP>() where CP : Component, IEntityCompBase
        {
            foreach (var item in Comps)
            {
                if (item is CP)
                {
                    return item as CP;
                }
            }
            return null;
        }
        public virtual void RemoveComp(IEntityCompBase comp)
        {
            Comps.Remove(comp);
            comp.Destroy();
        }
        public virtual void Destroy()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Destroy();
            }
            GameObject.Destroy(gameObject);
        }
        public new CoroutineTask StartCoroutine(IEnumerator routine)
        {
            return CoroutineModule.Instance.StartCoroutineTask(routine);
        }
        #endregion
    }
    public abstract class MonoEntityBase<E> : MonoEntityBase, IEntityBase<E> where E : MonoEntityBase, IEntityBase<E>
    {
        public new E Entity { get; set; }
        public virtual void Load(E entity)
        {
            base.Load(entity);
        }
    }

    public abstract class MonoEntityBase<E, D> : MonoEntityBase, IEntityBase<E, D> where E : MonoEntityBase, IEntityBase<E, D> where D : EntityDataBase
    {
        public D Config { get; set; }
        public new E Entity { get; set; }
        public virtual void Load(E entity, D config)
        {
            Entity = entity;
            Config = config;
            base.Load(entity);
        }
    }
}