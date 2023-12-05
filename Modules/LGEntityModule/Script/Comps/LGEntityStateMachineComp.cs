using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{

    /// <summary>
    /// 有限状态机接口类型
    /// </summary>
    public interface ILGStateBase<S> where S : System.Enum
    {
        void LGInit(ILGEntityStateMachineComp<S> stateMachine, ILGEntity entity, params object[] agrs);
        void OnEnter(params object[] agrs);
        void OnUpdate();
        void OnExit();
    }
    /// <summary>
    /// 有限状态机接口类型
    /// </summary>
    public interface ILGEntityStateMachineComp<S> : ILGEntityComponent where S : System.Enum
    {
        void AddState(S Key, LGStateBase<S> State);
        void EnterState(S Key, params object[] agrs);
        //重置状态
        void ResetState(params object[] agrs);
    }

    /// <summary>
    /// 状态机基础状态对象
    /// </summary>
    public abstract class LGStateBase<S> : ILGStateBase<S> where S : System.Enum
    {
        protected ILGEntityStateMachineComp<S> StateMachine;
        public virtual void LGInit(ILGEntityStateMachineComp<S> stateMachine, ILGEntity entity, params object[] agrs)
        {
            StateMachine = stateMachine;
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

    public abstract class LGEntityStateMachineComp<E, S> : LGEntityCompBase<E>, ILGEntityStateMachineComp<S> where E : class, ILGEntity where S : System.Enum
    {
        protected Dictionary<S, LGStateBase<S>> states = new();
        protected LGStateBase<S> currstate;
        [SerializeField]
        protected S State;

        public override void LGInit(ILGEntity entity, params object[] agrs)
        {
            base.LGInit(entity);
            foreach (var item in states)
            {
                item.Value.LGInit(this, entity, agrs);
            }
        }

        public virtual void ResetState(params object[] agrs)
        {

        }

        public virtual void AddState(S key, LGStateBase<S> state)
        {
            if (!states.ContainsKey(key))
            {
                states.Add(key, state);
            }
        }

        public virtual void EnterState(S key, params object[] agrs)
        {
            currstate?.OnExit();
            if (states.ContainsKey(key))
            {
                LGStateBase<S> stateBase = states[key];
                currstate = stateBase;
                State = key;
                stateBase.OnEnter(agrs);
            }
            else
            {
                throw new NotImplementedException("no state:" + key);
            }
        }
        public override void LGUpdate(float time)
        {
            currstate?.OnUpdate();
        }

    }
}