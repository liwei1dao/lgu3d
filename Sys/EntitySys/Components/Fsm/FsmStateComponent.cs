using System;
using System.Linq;
using System.Collections.Generic;



/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace LG.EntitySys
{
    /// <summary>
    /// 状态管理
    /// </summary>
    public abstract class FsmStateComponent : LGEntityComp, ILGEntityFsmStateComp
    {
        protected Dictionary<string, List<ILGFsmState>> m_States = new();
        protected LinkedList<ILGFsmState> m_FsmStateBases = new();
        public override void LGInit(LGEntity entity, params object[] agrs)
        {
            base.LGInit(entity);
        }

        /// <summary>
        /// 根据状态名称获取状态
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        private ILGFsmState GetState(string stateName)
        {
            foreach (var aFsmStateBase in this.m_FsmStateBases)
            {
                if (aFsmStateBase.StateName == stateName)
                {
                    return aFsmStateBase;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取栈顶状态
        /// </summary>
        /// <returns></returns>
        public ILGFsmState GetCurrentFsmState()
        {
            return this.m_FsmStateBases.First?.Value;
        }
        /// <summary>
        /// 检查是否为栈顶状态
        /// </summary>
        /// <param name="aFsmStateBase"></param>
        /// <returns></returns>
        private bool CheckIsFirstState(ILGFsmState aFsmStateBase)
        {
            return aFsmStateBase == this.GetCurrentFsmState();
        }

        /// <summary>
        /// 是否会发生状态互斥，只要包含了conflictStateTypes的子集，就返回true
        /// </summary>
        /// <param name="conflictStateTypes">互斥的状态</param>
        /// <returns></returns>
        public abstract bool CheckConflictState(string conflictStateTypes);

        /// <summary>
        /// 是否完全包含某个状态，需要包含targetStateTypes一致的时候才会返回true
        /// </summary>
        /// <param name="targetStateTypes"></param>
        /// <returns></returns>
        public bool HasAbsoluteEqualsState(string targetStateTypes)
        {
            foreach (var state in this.m_States)
            {
                if (targetStateTypes == state.Key && state.Value.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 从状态机移除一个状态（指定名称），如果移除的是栈顶元素，需要对新的栈顶元素进行OnEnter操作
        /// </summary>
        /// <param name="stateName"></param>
        public void RemoveState(string stateName)
        {
            ILGFsmState temp = GetState(stateName);
            if (temp == null)
                return;

            bool theRemovedItemIsFirstState = this.CheckIsFirstState(temp);
            this.m_States[temp.StateName].Remove(temp);
            this.m_FsmStateBases.Remove(temp);
            temp.OnRemoved(this);
            ReferencePool.Release(temp);
            if (theRemovedItemIsFirstState)
            {
                this.GetCurrentFsmState()?.OnEnter(this);
            }
        }

        /// <summary>
        /// 切换状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行，切换成功返回成功，切换失败返回失败
        /// 这里的切换成功是指目标状态来到链表头部，插入到链表中或者插入失败都属于切换失败
        /// </summary>
        public bool ChangeState(ILGFsmState aFsmStateBase)
        {
            ILGFsmState tempFsmStateBase = this.GetState(aFsmStateBase.StateName);

            if (tempFsmStateBase != null)
            {
                //因为已有此状态，所以进行回收
                ReferencePool.Release(aFsmStateBase);
                this.InsertState(tempFsmStateBase, true);
                return CheckIsFirstState(tempFsmStateBase);
            }

            this.InsertState(aFsmStateBase);
            return CheckIsFirstState(aFsmStateBase);
        }

        /// <summary>
        /// 切换状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行，切换成功返回成功，切换失败返回失败
        /// 这里的切换成功是指目标状态来到链表头部，插入到链表中或者插入失败都属于切换失败
        /// </summary>
        /// <param name="stateTypes">状态类型</param>
        /// <param name="stateName">状态名称</param>
        /// <param name="priority">状态优先级</param>
        public bool ChangeState<T>(string stateName, int priority) where T : class, ILGFsmState, new()
        {
            ILGFsmState aFsmStateBase = this.GetState(stateName);

            if (aFsmStateBase != null)
            {
                this.InsertState(aFsmStateBase, true);
                return CheckIsFirstState(aFsmStateBase);
            }

            aFsmStateBase = ReferencePool.Acquire<T>();
            aFsmStateBase.SetData(stateName, priority);
            this.InsertState(aFsmStateBase);
            return CheckIsFirstState(aFsmStateBase);
        }
        /// <summary>
        /// 向状态机添加一个状态，如果当前已存在，说明需要把它提到同优先级状态的前面去，让他先执行
        /// </summary>
        /// <param name="fsmStateToInsert">目标状态</param>
        /// <param name="containsItSelf">是否包含自身</param>
        private void InsertState(ILGFsmState fsmStateToInsert, bool containsItSelf = false)
        {
            if (!fsmStateToInsert.TryEnter(this))
            {
                //如果没有目标状态，说明是新增的状态，但是没有成功添加，需要归还给引用池
                if (!containsItSelf)
                {
                    ReferencePool.Release(fsmStateToInsert);
                }

                return;
            }

            LinkedListNode<ILGFsmState> current = this.m_FsmStateBases.First;
            while (current != null)
            {
                if (fsmStateToInsert.Priority >= current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            ILGFsmState tempFirstState = this.GetCurrentFsmState();
            //如果包含自身，就看current是不是自己，如果是，就不对链表做改变，如果不是就提到current前面
            if (containsItSelf)
            {
                if (fsmStateToInsert.StateName == current.Value.StateName)
                {
                    return;
                }
                else
                {
                    m_FsmStateBases.Remove(fsmStateToInsert);
                    m_FsmStateBases.AddBefore(current, fsmStateToInsert);
                }
            }
            else //如果不包含自身，且current不为空，即代表非尾节点有自己的位置，就插入，否则代表所有结点优先级都大于自身，就直接插入链表最后面
            {
                if (current != null)
                {
                    this.m_FsmStateBases.AddBefore(current, fsmStateToInsert);
                }
                else
                {
                    this.m_FsmStateBases.AddLast(fsmStateToInsert);
                }

                if (this.m_States.TryGetValue(fsmStateToInsert.StateName, out var stateList))
                {
                    stateList.Add(fsmStateToInsert);
                }
                else
                {
                    this.m_States.Add(fsmStateToInsert.StateName, new List<ILGFsmState>() { fsmStateToInsert });
                }
            }

            //如果这个被插入的状态成为了链表首状态，说明发生了状态变化
            if (CheckIsFirstState(fsmStateToInsert))
            {
                //Log.Info($"打断了{tempFirstState?.StateName},开始{fsmStateToInsert.StateName}");
                tempFirstState?.OnExit(this);
                fsmStateToInsert.OnEnter(this);
            }
        }
    }
}