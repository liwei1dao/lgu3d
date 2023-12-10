using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{

    public interface ILGBattleEntity : ILGEntity
    {

    }

    /// <summary>
    /// 战斗实体
    /// </summary>
    public abstract class LGBattleEntity : LGEntityBase, ILGBattleEntity
    {
        public T Publish<T>(T TEvent) where T : class
        {
            var eventComponent = LGGetComp<ILGEntityEventComp>();
            if (eventComponent == null)
            {
                return TEvent;
            }
            eventComponent.Publish(TEvent);
            return TEvent;
        }

        public void Subscribe<T>(Action<T> action) where T : class
        {
            var eventComponent = LGGetComp<ILGEntityEventComp>();
            if (eventComponent == null)
            {
                eventComponent = LGAddComp<ILGEntityEventComp>();
            }
            eventComponent.Subscribe(action);
        }

        public void UnSubscribe<T>(Action<T> action) where T : class
        {
            var eventComponent = LGGetComp<ILGEntityEventComp>();
            if (eventComponent != null)
            {
                eventComponent.UnSubscribe(action);
            }
        }
    }

}
