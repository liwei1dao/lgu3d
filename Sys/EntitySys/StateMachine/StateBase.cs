using System;
namespace lgu3d
{
    /// <summary>
    /// 状态机基础状态对象
    /// </summary>
    public abstract class StateBase<S> : IStateBase<S> where S : System.Enum
    {

        public virtual void LGInit(IFiniteStateMachine<S> stateMachine, IEntityBase entity, params object[] agrs)
        {

        }
        public virtual void OnEnter(params object[] agrs)
        {

        }
        public virtual void OnUpdate()
        {

        }

        public virtual void OnExit()
        {

        }


    }
}