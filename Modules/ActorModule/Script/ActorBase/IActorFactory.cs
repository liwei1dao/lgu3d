using System;

namespace lgu3d
{
    /// <summary>
    /// 角色的制造工厂
    /// </summary>
    public interface IActorFactory
    {
        void Load(ActorModule Model);
        void Create(int Id,Action<ActorBase> CallBack);
    }
}
