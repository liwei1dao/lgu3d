using System.Collections;
using System.Collections.Generic;
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
        /// <summary>
        /// 实体对象
        /// </summary>
        /// <value></value>
        public IEntityBase Entity { get; set; }
        /// <summary>
        /// 组件列表
        /// </summary>
        protected List<IEntityCompBase> Comps;
        #region 框架函数
        public virtual void Init(IEntityBase entity)
        {
            EntityID = Utils.GuidToLongID();
            Entity = entity;
            Comps = new List<IEntityCompBase>();
        }

        public virtual void Start()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Start();
            }
        }
        public virtual CP AddComp<CP>(CP comp, params object[] agrs) where CP : class, IEntityCompBase
        {
            comp.Init(Entity, agrs);
            Comps.Add(comp);
            return comp;
        }

        public virtual CP GetComp<CP>() where CP : class, IEntityCompBase
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
    public abstract class MonoEntityBase<E> : MonoEntityBase where E : MonoEntityBase<E>
    {
        public new E Entity { get; set; }
        #region 框架函数
        public override void Init(IEntityBase entity)
        {
            throw new System.Exception("请使用 Load(E entity) 接口初始化");
        }
        public virtual void Init(E entity)
        {
            base.Init(entity);
            Entity = entity;
            Comps = new List<IEntityCompBase>();
        }
        #endregion
    }

    public abstract class MonoEntityBase<E, D> : MonoEntityBase where E : MonoEntityBase<E, D> where D : EntityDataBase
    {
        public D Config { get; set; }
        public new E Entity { get; set; }
        #region 框架函数
        public override void Init(IEntityBase entity)
        {
            throw new System.Exception("请使用 Init(E entity, D config) 接口初始化");
        }
        public virtual void Init(E entity, D config)
        {
            base.Init(entity);
            Entity = entity;
            Config = config;
        }
        #endregion
    }
}