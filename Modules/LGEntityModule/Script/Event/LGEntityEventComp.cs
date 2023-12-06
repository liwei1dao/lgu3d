using System;
using System.Collections.Generic;
using System.Linq;

namespace lgu3d
{
    /// <summary>
    /// 事件管理
    /// </summary>
    public abstract class LGEntityEventComp<E> : LGEntityCompBase<E>, ILGEntityEventComp where E : class, ILGEntity
    {
        private Dictionary<Type, List<object>> TypeEvent2ActionLists = new();
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TEvent"></param>
        /// <returns></returns>
        public virtual T Publish<T>(T TEvent) where T : class
        {
            if (TypeEvent2ActionLists.TryGetValue(typeof(T), out var actionList))
            {
                var tempList = actionList.ToArray();
                foreach (Action<T> action in tempList.Cast<Action<T>>())
                {
                    action.Invoke(TEvent);
                }
            }
            return TEvent;
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public virtual void Subscribe<T>(Action<T> action) where T : class
        {
            var type = typeof(T);
            if (!TypeEvent2ActionLists.TryGetValue(type, out var actionList))
            {
                actionList = new List<object>();
                TypeEvent2ActionLists.Add(type, actionList);
            }
            actionList.Add(action);
            return;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public virtual void UnSubscribe<T>(Action<T> action) where T : class
        {
            if (TypeEvent2ActionLists.TryGetValue(typeof(T), out var actionList))
            {
                actionList.Remove(action);
            }
        }

    }
}