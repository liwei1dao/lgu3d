using System;
using UnityEngine;

namespace lgu3d
{
  public abstract class EntityCompBase : IEntityCompBase
  {
    public IEntityBase Entity { get; set; }

    public virtual void Load(IEntityBase entity, params object[] agrs)
    {
      Entity = entity;
    }

    public virtual void Init()
    {

    }

    public virtual void Destroy()
    {

    }
  }
  public abstract class EntityCompBase<E> : EntityCompBase where E : class, IEntityBase
  {
    public new E Entity { get; set; }

    public override void Load(IEntityBase entity, params object[] agrs)
    {
      Entity = entity as E;
      base.Load(entity);
    }
  }

  public abstract class MonoEntityCompBase : MonoBehaviour, IMonoEntityCompBase
  {
    public IMonoEntityBase Entity { get; set; }
    IEntityBase IEntityCompBase.Entity { get; set; }

    public virtual void Load(IMonoEntityBase entity, params object[] agrs)
    {
      Entity = entity;
    }

    public virtual void Load(IEntityBase entity, params object[] agrs)
    {
      throw new NotImplementedException();
    }
    public virtual void Destroy()
    {
      GameObject.Destroy(this);
    }

    public virtual void Init()
    {

    }
  }
  public abstract class MonoEntityCompBase<E> : MonoEntityCompBase where E : MonoBehaviour, IMonoEntityBase
  {
    public new E Entity { get; set; }

    public override void Load(IMonoEntityBase entity, params object[] agrs)
    {
      Entity = entity as E;
      base.Load(entity, agrs);
    }
  }
}
