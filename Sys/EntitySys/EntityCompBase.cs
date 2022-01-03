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
  public abstract class EntityCompBase<E> : EntityCompBase, IEntityCompBase<E> where E : IEntityBase
  {
    public new E Entity { get; set; }

    public virtual void Load(E entity, params object[] agrs)
    {
      Entity = entity;
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
  public abstract class MonoEntityCompBase<E> : MonoEntityCompBase, IMonoEntityCompBase<E> where E : MonoBehaviour, IMonoEntityBase
  {
    public new E Entity { get; set; }

    public virtual void Load(E entity, params object[] agrs)
    {
      Entity = entity;
      base.Load(entity, agrs);
    }
  }
}
