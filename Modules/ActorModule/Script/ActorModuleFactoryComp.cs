using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 角色工厂组件
    /// </summary>
    public class ActorModuleFactoryComp : ModelCompBase<ActorModule>
    {
        private Dictionary<int, IActorFactory> ActorModelFactorys = new Dictionary<int, IActorFactory>();
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl);
            foreach (var item in ActorModelFactorys)
            {
                item.Value.Load(MyModule);
            }
            LoadEnd();
        }

        public void RegisterActor(int ActorType, IActorFactory ActorFactory)
        {
            ActorModelFactorys[ActorType] = ActorFactory;
            if (State > ModelCompBaseState.Close)
            {
                ActorModelFactorys[ActorType].Load(MyModule);
            }
        }


        public void CreateActor<A>(int ActorId,Action<A> CallBack) where A: ActorBase
        {
            int ActorType = ActorId / 10000;
            if (ActorModelFactorys.ContainsKey(ActorType))
            {
                ActorModelFactorys[ActorType].Create(ActorId, (actor)=> {
                    if (CallBack != null)
                        CallBack(actor as A);
                });
            }
            else
            {
                Debug.LogError("角色工厂没有注册 = "+ ActorType);
            }
        }

        public void CreateActor<A,D>(int ActorId,D Data, Action<A,D> CallBack) where A : ActorBase
        {
            CreateActor<A>(ActorId, (actor) =>
            {
                if (CallBack != null)
                    CallBack(actor, Data);
            });
        }

    }
}
