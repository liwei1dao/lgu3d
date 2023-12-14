using System;
using System.Collections.Generic;


namespace LG.EntitySys
{
    public sealed class GameObjectComponent : LGEntityComp
    {
        public UnityEngine.GameObject GameObject { get; private set; }
        public override void LGInit(LGEntity entity, params object[] agrs)
        {
            GameObject = new UnityEngine.GameObject(Entity.GetType().Name);
            var view = GameObject.AddComponent<ComponentView>();
            view.Type = GameObject.name;
            view.Component = this;
        }
    }
}