
using System;
using System.Linq;
using System.Collections.Generic;

namespace LG.EntitySys
{
    /// <summary>
    /// 实体状态
    /// </summary>
    public abstract class LGEntityState
    {
        public string StateName { get; set; }
        public int Priority { get; set; }
        public void SetData(string stateName, int priority)
        {
            StateName = stateName;
            Priority = priority;
        }
        public virtual bool TryEnter(ILGEntityFsmStateComp comp)
        {
            return true;
        }
        /// <summary>
        /// 状态进入
        /// </summary>
        public virtual void OnEnter(ILGEntityFsmStateComp comp)
        {
        }
        /// <summary>
        /// 状态退出
        /// </summary>
        public virtual void OnExit(ILGEntityFsmStateComp comp)
        {

        }
        /// <summary>
        /// 状态移除
        /// </summary>
        public virtual void OnRemoved(ILGEntityFsmStateComp comp)
        {

        }

        public virtual void Clear()
        {

        }
    }
}