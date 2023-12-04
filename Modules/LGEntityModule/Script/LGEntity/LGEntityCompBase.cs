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
    public abstract class LGEntityCompBase : MonoBehaviour, ILGEntityComponent
    {
        public ILGEntity Entity { get; set; }

        public virtual void LGInit(ILGEntity entity, params object[] agrs)
        {
            Entity = entity;
        }

        public virtual void LGStart()
        {

        }
        public virtual void LGReset()
        {

        }
    }

    public abstract class LGEntityCompBase<E> : EntityCompBase where E : class, IEntityBase
    {
        public new E Entity { get; set; }
        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            Entity = entity as E;
        }
    }

}