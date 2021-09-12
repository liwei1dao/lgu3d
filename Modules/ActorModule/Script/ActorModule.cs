using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 角色模块
  /// </summary>
  public class ActorModule : ManagerContorBase<ActorModule>
  {
    private ActorModuleFactoryComp FactoryComp;
    public override void Load(params object[] agrs)
    {
      ResourceComp = AddComp<Module_ResourceComp>();
      FactoryComp = AddComp<ActorModuleFactoryComp>();
      base.Load(agrs);
    }

    public void RegisterActor(int ActorType, IActorFactory ActorFactory)
    {
      FactoryComp.RegisterActor(ActorType, ActorFactory);
    }

    public void CreateActor<A>(int ActorId, Action<A> CallBack) where A : ActorBase
    {
      FactoryComp.CreateActor<A>(ActorId, CallBack);
    }
    public void CreateActor<A, D>(int ActorId, D Data, Action<A, D> CallBack) where A : ActorBase
    {
      FactoryComp.CreateActor<A, D>(ActorId, Data, CallBack);
    }
  }
}
