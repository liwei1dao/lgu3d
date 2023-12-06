using System;

namespace lgu3d
{
    /// <summary>
    /// 状态管理
    /// </summary>
    public interface ILGEntityEventComp : ILGEntityComponent
    {
        T Publish<T>(T TEvent) where T : class;
        void Subscribe<T>(Action<T> action) where T : class;
        void UnSubscribe<T>(Action<T> action) where T : class;
    }
}
