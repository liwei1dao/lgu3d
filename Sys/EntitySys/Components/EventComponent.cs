using System;
using System.Linq;
using System.Collections.Generic;



/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace LG.EntitySys
{
    public class SubscribeSubject : LGEntity
    {
        public override void LGInit(params object[] agrs)
        {
            Name = agrs[0].GetHashCode().ToString();
        }

        public SubscribeSubject DisposeWith(LGEntity entity)
        {
            entity.SetChild(this);
            return this;
        }
    }
    public sealed class EventComponent : LGEntityComp
    {
        public override bool DefaultEnable { get; set; } = false;
        private Dictionary<Type, List<object>> TypeEvent2ActionLists = new Dictionary<Type, List<object>>();
        public static bool DebugLog { get; set; } = false;

        public new T Publish<T>(T TEvent) where T : class
        {
            if (TypeEvent2ActionLists.TryGetValue(typeof(T), out var actionList))
            {
                var tempList = actionList.ToArray();
                foreach (Action<T> action in tempList)
                {
                    action.Invoke(TEvent);
                }
            }
            return TEvent;
        }

        public new SubscribeSubject Subscribe<T>(Action<T> action) where T : class
        {
            var type = typeof(T);
            if (!TypeEvent2ActionLists.TryGetValue(type, out var actionList))
            {
                actionList = new List<object>();
                TypeEvent2ActionLists.Add(type, actionList);
            }
            actionList.Add(action);
            return Entity.AddChild<SubscribeSubject>(action);
        }

        public new void UnSubscribe<T>(Action<T> action) where T : class
        {
            if (TypeEvent2ActionLists.TryGetValue(typeof(T), out var actionList))
            {
                actionList.Remove(action);
            }
            var e = Entity.Find<SubscribeSubject>(action.GetHashCode().ToString());
            if (e != null)
            {
                LGEntity.Destroy(e);
            }
        }

    }

}