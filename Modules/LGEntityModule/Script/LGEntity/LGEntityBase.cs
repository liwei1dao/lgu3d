using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{
    /// <summary>
    /// 基础实体对象
    /// </summary>
    public abstract class LGEntityBase : MonoBehaviour, ILGEntity

    {
        public ILGEntity Entity { get; set; }
        public LGEntityState State { get; set; }
        public string Camp { get; set; }
        protected List<ILGEntityComponent> Comps;

        public virtual void LGInit(ILGEntity entity)
        {
            Entity = entity;
            Comps = new List<ILGEntityComponent>();
            ILGEntityComponent[] comps = gameObject.GetComponentsInChildren<ILGEntityComponent>();
            this.LGAddComps(comps);
            State = LGEntityState.Loaded;
        }

        public virtual void LGStart()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGStart();
            }
            State = LGEntityState.Active;
        }
        //回收
        public virtual void Reclaim()
        {
            State = LGEntityState.Recycle;
            gameObject.SetActive(false);
        }
        //重置
        public virtual void LGReset()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGReset();
            }
            State = LGEntityState.Active;
            gameObject.SetActive(true);
        }

        protected virtual void LGAddComps(ILGEntityComponent[] comps)
        {
            Comps.AddRange(comps);
            foreach (var comp in comps)
            {
                comp.LGInit(Entity);
            }
            return;
        }

        public virtual C LGAddComp<C>(C comp, params object[] agrs) where C : class, ILGEntityComponent
        {
            comp.LGInit(this, agrs);
            Comps.Add(comp);
            return comp;
        }

        public virtual C LGGetComp<C>() where C : class, ILGEntityComponent
        {
            foreach (var item in Comps)
            {
                if (item is C)
                {
                    return item as C;
                }
            }
            return null;
        }
        public virtual C LGAddMissingComp<C>() where C : Component, ILGEntityComponent
        {
            foreach (var item in Comps)
            {
                if (item is C)
                {
                    return item as C;
                }
            }
            C comp = gameObject.AddMissingComponent<C>();
            comp.LGInit(Entity);
            Comps.Add(comp);
            return comp;
        }
    }
    public abstract class LGEntityBase<E> : LGEntityBase where E : LGEntityBase<E>
    {

        public new E Entity { get; set; }
        #region 框架函数
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
            Entity = entity as E;
        }
        #endregion
    }
    public abstract class LGEntityBase<E, D> : LGEntityBase where E : LGEntityBase<E, D> where D : class
    {
        public D Config;
        public new E Entity { get; set; }
        #region 框架函数
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
            Entity = entity as E;
        }
        #endregion
    }
}