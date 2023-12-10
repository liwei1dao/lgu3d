using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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
    public abstract class LGEntityBase : ILGEntity
    {

        public bool Active { get; set; }
        public ILGEntity Parent { get; protected set; }
        protected ILGEntityModule module;
        protected List<ILGEntity> Childs;
        protected List<ILGEntityComponent> Comps;

        public virtual void LGInit(ILGEntityModule module, ILGEntity parent)
        {
            Parent = parent;
            Comps = new List<ILGEntityComponent>();
        }

        public virtual void LGStart()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGStart();
            }
            Active = true;
        }

        public virtual void LGUpdate(float time)
        {
            if (!Active)
                return;
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].LGUpdate(time);
            }
        }

        //重置
        public virtual void Activation()
        {
            for (int i = 0; i < Comps.Count; i++)
            {
                Comps[i].Activation();
            }
        }

        /// <summary>
        /// 回收清理
        /// </summary>
        public virtual void Clear()
        {

        }

        protected virtual void LGAddComps(ILGEntityComponent[] comps)
        {
            Comps.AddRange(comps);
            foreach (var comp in comps)
            {
                comp.LGInit(this);
            }
            foreach (var comp in comps)
            {
                comp.LGStart();
            }
            return;
        }
        public virtual C LGAddComp<C>() where C : class, ILGEntityComponent
        {
            C comp = default(C);
            Comps.Add(comp);
            comp.LGInit(this);
            comp.LGStart();
            return comp;
        }
        public virtual C LGAddComp<C>(C comp) where C : class, ILGEntityComponent
        {
            Comps.Add(comp);
            comp.LGInit(this);
            comp.LGStart();
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
        public virtual List<C> LGGetComps<C>() where C : class, ILGEntityComponent
        {
            List<C> comps = new();
            foreach (var item in Comps)
            {
                if (item is C)
                {
                    comps.Add(item as C);
                }
            }
            return comps;
        }
        public virtual C LGAddMissingComp<C>() where C : class, ILGEntityComponent
        {
            foreach (var item in Comps)
            {
                if (item is C)
                {
                    return item as C;
                }
            }
            C comp = default;
            this.LGAddComp<C>(comp);
            return comp;
        }

        public virtual E LGAddEntity<E>() where E : class, ILGEntity
        {
            E entity = default(E);
            Childs.Add(entity);
            entity.LGInit(module, this);
            entity.LGStart();
            return entity;
        }

        public E LGGetEntity<E>() where E : class, ILGEntity
        {
            foreach (ILGEntity entity in Childs)
            {
                if (entity is E)
                {
                    return entity as E;
                }
            }
            return null;
        }

        public virtual void LGRemoveEntity(ILGEntity entity)
        {
            Childs.Remove(entity);
            return;
        }
    }

}