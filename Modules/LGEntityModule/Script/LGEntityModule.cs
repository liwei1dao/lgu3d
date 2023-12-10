using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using System.Collections.Generic;


namespace lgu3d
{
    /// <summary>
    /// 游戏实体技能 模块
    /// </summary>
    public abstract class LGEntityModule<C> : ManagerContorBase<C>, ILGEntityModule where C : ManagerContorBase<C>, new()
    {

        protected List<ILGEntity> entitys = new();

        public override void Load(params object[] agrs)
        {
            ResourceComp = AddComp<Module_ResourceComp>();
            SoundComp = AddComp<Module_SoundComp>();
            GameObjectPoolComp = AddComp<Module_GameObjectPoolComp>();
            base.Load(agrs);
        }

        public E CreateEntity<E>() where E : class, ILGEntity, new()
        {
            E entity = ReferencePool.Acquire<E>();
            entity.LGInit(this, null);
            entitys.Add(entity);
            return entity;
        }

        public void RemoveEntity(ILGEntity entity)
        {
            entitys.Remove(entity);
            ReferencePool.Release(entity);
        }

        public void Update(float time)
        {
            foreach (var entity in entitys)
            {
                entity.LGUpdate(time);
            }
        }
    }
}