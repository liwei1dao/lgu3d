
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{

    /// <summary>
    /// LG实体对象
    /// </summary>
    public interface ILGEntity
    {
        void LGInit(ILGEntity entity);
        void LGStart();
        void LGUpdate(float time);
        void Activation();
        C LGAddComp<C>(C comp) where C : class, ILGEntityComponent;
        C LGGetComp<C>() where C : class, ILGEntityComponent;
        List<C> LGGetComps<C>() where C : class, ILGEntityComponent;
        C LGAddMissingComp<C>() where C : Component, ILGEntityComponent;
    }

    /// <summary>
    /// LG实体组件
    /// </summary>
    public interface ILGEntityComponent
    {
        /// <summary>
        /// 状态类型
        /// </summary>
        ILGEntity Entity { get; }
        void LGInit(ILGEntity entity);
        void LGStart();
        void LGUpdate(float time);
        void Activation();
    }

}