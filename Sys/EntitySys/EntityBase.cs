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
        public long EntityID { get; protected set; }
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
        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Init(IEntityBase entity)
        {
            EntityID = Utils.GuidToLongID();
            Entity = entity;
            Comps = new List<IEntityCompBase>();
        }

        /// <summary>
        /// 启动函数
        /// </summary>
        public virtual void Start()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Start();
            }
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="CP"></typeparam>
        public virtual CP AddComp<CP>(CP comp, params object[] agrs) where CP : class, IEntityCompBase
        {
            comp.Init(this, agrs);
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

    public abstract class EntityBase<E> : EntityBase where E : EntityBase<E>
    {
        /// <summary>
        /// 实体对象
        /// </summary>
        /// <value></value>
        public new E Entity { get; set; }

        #region 框架函数
        public override void Init(IEntityBase entity)
        {
            base.Init(entity);
            Entity = entity as E;
        }
        public virtual void Init(E entity)
        {
            base.Init(entity);
            Entity = entity;
        }

        #endregion
    }

    public abstract class EntityBase<E, D> : EntityBase where E : EntityBase<E, D> where D : EntityDataBase
    {
        public D Config { get; set; }

        /// <summary>
        /// 实体对象
        /// </summary>
        /// <value></value>
        public new E Entity { get; set; }


        #region 框架函数
        public override void Init(IEntityBase entity)
        {
            base.Init(entity);
            Entity = entity as E;
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
