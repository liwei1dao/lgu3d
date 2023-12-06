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

        public virtual void LGInit(ILGEntity entity)
        {
            Entity = entity;
        }

        public virtual void LGStart()
        {

        }

        public virtual void LGUpdate(float time)
        {

        }
        public virtual void Activation()
        {

        }
    }

    public abstract class LGEntityCompBase<E> : LGEntityCompBase where E : class, ILGEntity
    {
        public new E Entity { get; set; }
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
            Entity = entity as E;
        }
    }

}