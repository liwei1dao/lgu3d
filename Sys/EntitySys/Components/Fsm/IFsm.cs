using System;
using LG;

namespace LG.EntitySys
{

    /// <summary>
    /// 状态对象
    /// </summary>
    public interface ILGFsmState : IReference
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        string StateName { get; }
        /// <summary>
        /// 状态的优先级
        /// </summary>
        int Priority { get; }

        void SetData(string stateName, int priority);
        bool TryEnter(ILGEntityFsmStateComp comp);
        void OnEnter(ILGEntityFsmStateComp comp);
        void OnExit(ILGEntityFsmStateComp comp);
        void OnRemoved(ILGEntityFsmStateComp comp);
    }

    /// <summary>
    /// 状态管理
    /// </summary>
    public interface ILGEntityFsmStateComp
    {
        /// <summary>
        /// 获取栈顶状态
        /// </summary>
        /// <returns></returns>
        public ILGFsmState GetCurrentFsmState();
        /// <summary>
        /// 是否会发生状态互斥，只要包含了conflictStateTypes的子集，就返回true
        /// </summary>
        /// <param name="conflictStateTypes">互斥的状态</param>
        /// <returns></returns>
        bool CheckConflictState(string conflictStateTypes);
        /// <summary>
        /// 从状态机移除一个状态（指定名称），如果移除的是栈顶元素，需要对新的栈顶元素进行OnEnter操作
        /// </summary>
        /// <param name="stateName"></param>
        void RemoveState(string stateName);

        /// <summary>
        /// 切换状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行，切换成功返回成功，切换失败返回失败
        /// 这里的切换成功是指目标状态来到链表头部，插入到链表中或者插入失败都属于切换失败
        /// </summary>
        bool ChangeState(ILGFsmState state);
    }
}
