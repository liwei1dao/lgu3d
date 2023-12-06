using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        [LabelText("技能名称")]
        public bool Active { get; set; }
        protected List<ILGEntityComponent> Comps;

        public virtual void LGInit(ILGEntity entity)
        {
            Comps = new List<ILGEntityComponent>();
            ILGEntityComponent[] comps = gameObject.GetComponentsInChildren<ILGEntityComponent>();
            this.LGAddComps(comps);
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

        protected virtual void LGAddComps(ILGEntityComponent[] comps)
        {
            Comps.AddRange(comps);
            foreach (var comp in comps)
            {
                comp.LGInit(this);
            }
            return;
        }

        public virtual C LGAddComp<C>(C comp) where C : class, ILGEntityComponent
        {
            comp.LGInit(this);
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
            comp.LGInit(this);
            Comps.Add(comp);
            return comp;
        }
    }
    public abstract class LGEntityBase<E> : LGEntityBase where E : LGEntityBase<E>
    {
        public E Entity { get; set; }
        #region 框架函数
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
            Entity = entity as E;
        }
        #endregion
    }
}