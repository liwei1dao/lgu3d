using System;

namespace lgu3d
{
    /// <summary>
    /// 实体事件
    /// </summary>
    public abstract class LGFsmState<S> : ILGFsmState<S> where S : Enum
    {
        public S StateTypes { get; set; }
        public string StateName { get; set; }
        public int Priority { get; set; }

        public virtual S GetConflictStateTypeses()
        {
            return default(S);
        }


        public void SetData(S stateTypes, string stateName, int priority)
        {
            StateTypes = stateTypes;
            StateName = stateName;
            Priority = priority;
        }

        public virtual bool TryEnter(ILGEntityFsmStateComp<S> comp)
        {
            if (comp.CheckConflictState(GetConflictStateTypeses()))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 状态进入
        /// </summary>
        public virtual void OnEnter(ILGEntityFsmStateComp<S> comp)
        {
        }
        /// <summary>
        /// 状态退出
        /// </summary>
        public virtual void OnExit(ILGEntityFsmStateComp<S> comp)
        {

        }
        /// <summary>
        /// 状态移除
        /// </summary>
        public virtual void OnRemoved(ILGEntityFsmStateComp<S> comp)
        {

        }

        public virtual void Clear()
        {

        }
    }

}