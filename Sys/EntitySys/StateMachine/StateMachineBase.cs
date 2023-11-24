using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public abstract class FiniteStateMachineBase<E, S> : MonoEntityCompBase<E>, IFiniteStateMachine<S> where E : class, IEntityBase where S : System.Enum
    {
        protected Dictionary<S, StateBase<S>> states = new();
        protected StateBase<S> currstate;
        [SerializeField]
        protected S State;

        public override void LGInit(IEntityBase entity, params object[] agrs)
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

        public virtual void AddState(S key, StateBase<S> state)
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
                StateBase<S> stateBase = states[key];
                currstate = stateBase;
                State = key;
                stateBase.OnEnter(agrs);
            }
            else
            {
                throw new NotImplementedException("no state:" + key);
            }
        }
        protected virtual void Update()
        {
            currstate?.OnUpdate();
        }

    }
}