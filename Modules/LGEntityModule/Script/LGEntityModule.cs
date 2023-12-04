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
        protected Dictionary<string, List<ILGEntity>> campEntitys;
        public override void Load(params object[] agrs)
        {
            campEntitys = new Dictionary<string, List<ILGEntity>>();
            base.Load(agrs);
        }

        public virtual void AddLGEntity(ILGEntity entity)
        {
            if (!campEntitys.ContainsKey(entity.Camp))
            {
                campEntitys[entity.Camp] = new List<ILGEntity>();
            }
            campEntitys[entity.Camp].Add(entity);
        }

        public virtual void RemoveLGEntity(ILGEntity entity)
        {
            campEntitys[entity.Camp].Remove(entity);
        }
    }
}