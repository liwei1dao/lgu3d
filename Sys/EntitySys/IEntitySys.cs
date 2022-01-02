using System;

namespace lgu3d
{
  public interface IEntityBase
  {
    EntityDataBase Config { get; set; }
    void Load(EntityDataBase config);
    void Init();
    CP AddComp<CP>(params object[] agrs) where CP : IEntityCompBase, new();
    void RemoveComp(IEntityCompBase comp);
    void Destroy();
  }

  public interface IEntityCompBase
  {
    IEntityBase Entity { get; set; }
    void Load(IEntityBase entity, params object[] agrs);
    void Init();
    void Destroy();
  }

  public interface IEntityBase<E, D> : IEntityBase where E : IEntityBase<E, D> where D : EntityDataBase
  {
    new D Config { get; set; }

    void Load(D config);
  }

  public interface IEntityCompBase<E> : IEntityCompBase where E : IEntityBase
  {
    new E Entity { get; set; }
    void Load(E entity, params object[] agrs);
  }
}