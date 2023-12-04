
using UnityEngine;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{

    public enum LGEntityState
    {
        //未加载
        NoLoad = 0,
        Loaded = 1,
        //活跃的
        Active = 2,
        //回收
        Recycle = 3,
    }

    /// <summary>
    /// LG实体对象
    /// </summary>
    public interface ILGEntity
    {
        public ILGEntity Entity { get; }
        string Camp { get; set; }
        void LGInit(ILGEntity entity);
        void LGStart();
        void LGReset();
        void Reclaim();
        C LGAddComp<C>(C comp, params object[] agrs) where C : class, ILGEntityComponent;
        C LGGetComp<C>() where C : class, ILGEntityComponent;
        C LGAddMissingComp<C>() where C : Component, ILGEntityComponent;
    }

    /// <summary>
    /// LG实体组件
    /// </summary>
    public interface ILGEntityComponent
    {
        void LGInit(ILGEntity entity, params object[] agrs);
        void LGStart();
        void LGReset();
    }
}