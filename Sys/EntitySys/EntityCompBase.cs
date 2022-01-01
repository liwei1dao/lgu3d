using System;

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

    public virtual void Updata(float time)
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

}
