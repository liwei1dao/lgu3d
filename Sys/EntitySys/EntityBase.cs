using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{
    public abstract class EntityBase : IEntityBase
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
        public virtual CP AddComp<CP>(params object[] agrs) where CP : class, IEntityCompBase, new()
        {
            CP Comp = new CP();
            Comp.Load(this, agrs);
            Comps.Add(Comp);
            return Comp;
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
        public void Destroy()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Destroy();
            }
        }

        public CoroutineTask StartCoroutine(IEnumerator routine)
        {
            return CoroutineModule.Instance.StartCoroutine(routine);
        }
        #endregion
    }

    public abstract class EntityBase<E> : IEntityBase<E> where E : EntityBase<E>, IEntityBase<E>
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
        public E Entity { get; set; }
        protected List<IEntityCompBase> Comps = new List<IEntityCompBase>();

        #region 框架函数
        public virtual void Init(IEntityBase entity)
        {
            throw new System.Exception("请使用 Load(E entity) 接口初始化");
        }
        public virtual void Init(E entity)
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
        public virtual CP AddComp<CP>(params object[] agrs) where CP : class, IEntityCompBase, new()
        {
            CP Comp = new CP();
            Comp.Load(this, agrs);
            Comps.Add(Comp);
            return Comp;
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
        public void Destroy()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Destroy();
            }
        }

        public CoroutineTask StartCoroutine(IEnumerator routine)
        {
            return CoroutineModule.Instance.StartCoroutine(routine);
        }
        #endregion
    }

    public abstract class EntityBase<E, D> : IEntityBase<E, D> where E : EntityBase<E, D>, IEntityBase<E, D> where D : EntityDataBase
    {
        public D Config { get; set; }
        /// <summary>
        /// 实体ID
        /// </summary>
        /// <value></value>
        public long EntityID { get; private set; }
        /// <summary>
        /// 实体对象
        /// </summary>
        /// <value></value>
        public E Entity { get; set; }
        protected List<IEntityCompBase> Comps = new List<IEntityCompBase>();

        #region 框架函数
        public virtual void Init(E entity, D config)
        {
            EntityID = Utils.GuidToLongID();
            Entity = entity;
            Config = config;
        }

        public virtual void Start()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Start();
            }
        }
        public virtual CP AddComp<CP>(params object[] agrs) where CP : class, IEntityCompBase, new()
        {
            CP Comp = new CP();
            Comp.Load(this, agrs);
            Comps.Add(Comp);
            return Comp;
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
        public void Destroy()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Destroy();
            }
        }

        public CoroutineTask StartCoroutine(IEnumerator routine)
        {
            return CoroutineModule.Instance.StartCoroutine(routine);
        }
        #endregion
    }


}
