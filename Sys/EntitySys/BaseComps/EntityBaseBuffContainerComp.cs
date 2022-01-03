
using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{

  public interface IEntityBaseBuffContainerComp<A> where A : Enum
  {
    void AddBuff(IBuffBase<A> buff);
    void RemoveBuff(IBuffBase<A> buff);
  }

  /// <summary>
  /// 实体bug容器组件
  /// </summary>
  public abstract class EntityBaseBuffContainerComp<E, A> : EntityCompBase<E>, IEntityBaseBuffContainerComp<A> where E : IEntityBase where A : Enum
  {
    protected List<IBuffBase<A>> Buffs;
    public override void Load(IEntityBase entity, params object[] agrs)
    {
      base.Load(entity, agrs);
      Buffs = new List<IBuffBase<A>>();
    }
    public virtual void AddBuff(IBuffBase<A> buff)
    {

    }
    public virtual void RemoveBuff(IBuffBase<A> buff)
    {
      Buffs.Remove(buff);
    }
    public virtual void UpDate(float time)
    {
      for (var i = 0; i < Buffs.Count; i++)
      {
        Buffs[i].Update(time);
      }
    }
  }

  /// <summary>
  /// 实体Buff容器组件
  /// </summary>
  /// <typeparam name="E"></typeparam>
  public abstract class MonoEntityBaseBuffContainerComp<E, A> : MonoEntityCompBase<E>, IEntityBaseBuffContainerComp<A> where E : MonoEntityBase where A : Enum
  {
    protected List<IBuffBase<A>> Buffs;
    public override void Load(E entity, params object[] agrs)
    {
      base.Load(entity, agrs);
      Buffs = new List<IBuffBase<A>>();
    }
    public abstract void AddBuff(IBuffBase<A> buff);

    public virtual void RemoveBuff(IBuffBase<A> buff)
    {
      Buffs.Remove(buff);
    }
    public virtual void UpDate(float time)
    {
      for (var i = 0; i < Buffs.Count; i++)
      {
        Buffs[i].Update(time);
      }
    }
    public override void Destroy()
    {
      Buffs.Clear();
      base.Destroy();
    }
  }
}