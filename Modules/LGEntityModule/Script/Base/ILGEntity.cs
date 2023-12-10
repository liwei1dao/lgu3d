
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
    public interface ILGEntity : IReference
    {
        ILGEntity Parent { get; }
        void LGInit(ILGEntityModule module, ILGEntity parent);
        void LGStart();
        void LGUpdate(float time);
        void Activation();
        C LGAddComp<C>(C comp) where C : class, ILGEntityComponent;
        C LGGetComp<C>() where C : class, ILGEntityComponent;
        List<C> LGGetComps<C>() where C : class, ILGEntityComponent;
        C LGAddMissingComp<C>() where C : class, ILGEntityComponent;
        E LGAddEntity<E>() where E : class, ILGEntity;

        E LGGetEntity<E>() where E : class, ILGEntity;

        void LGRemoveEntity(ILGEntity entity);
    }
    public interface IMonoLGEntity : ILGEntity
    {
        public GameObject gameObject { get; set; }
    }

}