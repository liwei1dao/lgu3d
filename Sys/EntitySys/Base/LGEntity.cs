using System;
using System.Collections.Generic;


/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace LG.EntitySys
{
    /// <summary>
    /// 基础实体对象
    /// </summary>
    public abstract partial class LGEntity
    {
        public long Id { get; set; }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
            }
        }
        public long InstanceId { get; set; }
        public LGEntity Parent { get; protected set; }
        protected List<LGEntity> Children;
        public Dictionary<long, LGEntity> Id2Children { get; private set; } = new Dictionary<long, LGEntity>();
        public Dictionary<Type, List<LGEntity>> Type2Children { get; private set; } = new Dictionary<Type, List<LGEntity>>();
        public Dictionary<Type, LGEntityComp> Components { get; set; } = new Dictionary<Type, LGEntityComp>();

        public virtual void LGInit(params object[] agrs)
        {

        }

        public virtual void LGStart()
        {

        }

        public virtual void OnSetParent(LGEntity preParent, LGEntity nowParent)
        {

        }

        public virtual void LGUpdate(float time)
        {

        }

        public virtual void OnDestroy()
        {

        }

        private void Dispose()
        {
            if (EnableLog) Log.Debug($"{GetType().Name}->Dispose");
            if (Children.Count > 0)
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    Destroy(Children[i]);
                }
                Children.Clear();
                Type2Children.Clear();
            }
            Parent?.RemoveChild(this);
            foreach (var component in Components.Values)
            {
                component.Enable = false;
                LGEntityComp.Destroy(component);
            }
            Components.Clear();
            InstanceId = 0;
            if (Master.Entities.ContainsKey(GetType()))
            {
                Master.Entities[GetType()].Remove(this);
            }
        }

        #region 组件
        public T GetParent<T>() where T : LGEntity
        {
            return Parent as T;
        }
        public T Add<T>(params object[] agrs) where T : LGEntityComp
        {
            return AddComponent<T>(agrs);
        }

        public virtual T AddComponent<T>(params object[] agrs) where T : LGEntityComp
        {
            var component = Activator.CreateInstance<T>();
            component.Entity = this;
            component.LGInit(this, agrs);
            Components.Add(typeof(T), component);
            Master.AllComponents.Add(component);
            component.LGStart();
            return component;
        }

        public void RemoveComponent<T>() where T : LGEntityComp
        {
            var component = Components[typeof(T)];
            if (component.Enable) component.Enable = false;
            LGEntityComp.Destroy(component);
            Components.Remove(typeof(T));
        }

        public bool HasComponent<T>() where T : LGEntityComp
        {
            return Components.TryGetValue(typeof(T), out var component);
        }
        public LGEntityComp GetComponent(Type componentType)
        {
            if (this.Components.TryGetValue(componentType, out var component))
            {
                return component;
            }
            return null;
        }
        public T GetComponent<T>() where T : LGEntityComp
        {
            if (Components.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }
            return null;
        }

        public bool TryGet<T>(out T component) where T : LGEntityComp
        {
            if (Components.TryGetValue(typeof(T), out var c))
            {
                component = c as T;
                return true;
            }
            component = null;
            return false;
        }


        #endregion

        #region 子实体
        private void SetParent(LGEntity parent)
        {
            LGEntity preParent = Parent;
            preParent?.RemoveChild(this);
            this.Parent = parent;
            OnSetParent(preParent, parent);
        }


        public virtual E AddEntity<E>() where E : LGEntity
        {
            E entity = default(E);
            Children.Add(entity);
            entity.LGInit(this);
            return entity;
        }

        public LGEntity AddChild(Type entityType, object initData)
        {
            var entity = NewEntity(entityType);
            if (EnableLog) Log.Debug($"AddChild {this.GetType().Name}, {entityType.Name}={entity.Id}");
            SetupEntity(entity, this, initData);
            return entity;
        }
        public LGEntity AddChild(Type entityType)
        {
            var entity = NewEntity(entityType);
            if (EnableLog) Log.Debug($"AddChild {this.GetType().Name}, {entityType.Name}={entity.Id}");
            SetupEntity(entity, this);
            return entity;
        }
        public void SetChild(LGEntity child)
        {
            Children.Add(child);
            Id2Children.Add(child.Id, child);
            if (!Type2Children.ContainsKey(child.GetType())) Type2Children.Add(child.GetType(), new List<LGEntity>());
            Type2Children[child.GetType()].Add(child);
            child.SetParent(this);
        }

        public T AddChild<T>() where T : LGEntity
        {
            return AddChild(typeof(T)) as T;
        }

        public T AddChild<T>(object initData) where T : LGEntity
        {
            return AddChild(typeof(T), initData) as T;
        }
        public virtual void RemoveChild(LGEntity child)
        {
            Children.Remove(child);
            if (Type2Children.ContainsKey(child.GetType())) Type2Children[child.GetType()].Remove(child);
        }
        public LGEntity Find(string name)
        {
            foreach (var item in Children)
            {
                if (item.name == name) return item;
            }
            return null;
        }

        public T Find<T>(string name) where T : LGEntity
        {
            if (Type2Children.TryGetValue(typeof(T), out var chidren))
            {
                foreach (var item in chidren)
                {
                    if (item.name == name) return item as T;
                }
            }
            return null;
        }
        #endregion

        #region 事件
        public T Publish<T>(T TEvent) where T : class
        {
            var eventComponent = GetComponent<EventComponent>();
            if (eventComponent == null)
            {
                return TEvent;
            }
            eventComponent.Publish(TEvent);
            return TEvent;
        }

        public SubscribeSubject Subscribe<T>(Action<T> action) where T : class
        {
            var eventComponent = GetComponent<EventComponent>();
            if (eventComponent == null)
            {
                eventComponent = AddComponent<EventComponent>();
            }
            return eventComponent.Subscribe(action);
        }

        public SubscribeSubject Subscribe<T>(Action<T> action, LGEntity disposeWith) where T : class
        {
            var eventComponent = GetComponent<EventComponent>();
            if (eventComponent == null)
            {
                eventComponent = AddComponent<EventComponent>();
            }
            return eventComponent.Subscribe(action).DisposeWith(disposeWith);
        }

        public void UnSubscribe<T>(Action<T> action) where T : class
        {
            var eventComponent = GetComponent<EventComponent>();
            if (eventComponent != null)
            {
                eventComponent.UnSubscribe(action);
            }
        }
        #endregion
    }

}