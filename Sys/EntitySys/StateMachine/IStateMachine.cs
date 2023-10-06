using Google.Protobuf.WellKnownTypes;
using System;

namespace lgu3d
{
    /// <summary>
    /// 有限状态机接口类型
    /// </summary>
    public interface IStateBase<S> where S : System.Enum
    {
        void LGInit(IFiniteStateMachine<S> stateMachine, IEntityBase entity, params object[] agrs);
        void OnEnter(params object[] agrs);
        void OnUpdate();
        void OnExit();
    }
    /// <summary>
    /// 有限状态机接口类型
    /// </summary>
    public interface IFiniteStateMachine<S> : IEntityCompBase where S : System.Enum
    {
        void AddState(S Key, StateBase<S> State);
        void EnterState(S Key, params object[] agrs);
    }
}