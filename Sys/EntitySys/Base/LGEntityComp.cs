using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace LG.EntitySys
{
    /// <summary>
    /// 基础实体对象
    /// </summary>
    public abstract class LGEntityComp
    {
        public LGEntity Entity { get; set; }
        public bool IsDisposed { get; set; }
        public Dictionary<long, LGEntity> Id2Children { get; private set; } = new Dictionary<long, LGEntity>();
        public virtual bool DefaultEnable { get; set; } = true;
        private bool enable = false;
        public bool Enable
        {
            set
            {
                if (enable == value) return;
                enable = value;
                if (enable) OnEnable();
                else OnDisable();
            }
            get
            {
                return enable;
            }
        }
        public bool Disable => enable == false;
        public T GetEntity<T>() where T : LGEntity
        {
            return Entity as T;
        }


        public virtual void LGInit(LGEntity entity, params object[] agrs)
        {
            Entity = entity;
        }

        public virtual void LGStart()
        {

        }
        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
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
            if (LGEntity.EnableLog) Log.Debug($"{GetType().Name}->Dispose");
            Enable = false;
            IsDisposed = true;
        }

        public static void Destroy(LGEntityComp entity)
        {
            try
            {
                entity.OnDestroy();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            entity.Dispose();
        }


        public T Publish<T>(T TEvent) where T : class
        {
            Entity.Publish(TEvent);
            return TEvent;
        }

        public void Subscribe<T>(Action<T> action) where T : class
        {
            Entity.Subscribe(action);
        }

        public void UnSubscribe<T>(Action<T> action) where T : class
        {
            Entity.UnSubscribe(action);
        }


    }

}