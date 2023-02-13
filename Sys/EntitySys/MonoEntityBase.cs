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
        [LGAttributeRename("实体ID", false)] public long EntityID;
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
        public virtual void LGInit(IEntityBase entity)
        {
            EntityID = Utils.GuidToLongID();
            Entity = entity;
            Comps = new List<IEntityCompBase>();
        }

        public virtual void LGStart()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGStart();
            }
        }
        public virtual CP LGAddComp<CP>(CP comp, params object[] agrs) where CP : class, IEntityCompBase
        {
            comp.LGInit(Entity, agrs);
            Comps.Add(comp);
            return comp;
        }

        public virtual CP LGGetComp<CP>() where CP : class, IEntityCompBase
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
        public virtual void LGRemoveComp(IEntityCompBase comp)
        {
            Comps.Remove(comp);
            comp.LGDestroy();
        }
        public virtual void LGDestroy()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGDestroy();
            }
            GameObject.Destroy(gameObject);
        }
        public new CoroutineTask LGStartCoroutine(IEnumerator routine)
        {
            return CoroutineModule.Instance.StartCoroutineTask(routine);
        }
        #endregion
    }
    public abstract class MonoEntityBase<E> : MonoEntityBase where E : MonoEntityBase<E>
    {
        public new E Entity { get; set; }
        #region 框架函数
        public override void LGInit(IEntityBase entity)
        {
            base.LGInit(entity);
            Entity = entity as E;
        }
        public virtual void Init(E entity)
        {
            base.LGInit(entity);
            Entity = entity;

        }
        #endregion
    }

    public abstract class MonoEntityBase<E, D> : MonoEntityBase where E : MonoEntityBase<E, D> where D : EntityDataBase
    {
        [SerializeField] protected D Config;

        public new E Entity { get; set; }
        #region 框架函数
        public override void LGInit(IEntityBase entity)
        {
            base.LGInit(entity);
            Entity = entity as E;
        }
        public virtual void Init(E entity, D config)
        {
            base.LGInit(entity);
            Entity = entity;
            Config = config;
        }
        #endregion
    }
}