using System;
using System.Collections;
using UnityEngine;

namespace lgu3d
{
  public interface IEntityBase
  {
    IEntityBase Entity { get; set; }
    void Load(IEntityBase entity);
    void Init();
    CP AddComp<CP>(params object[] agrs) where CP : class, IEntityCompBase, new();

    CP GetComp<CP>() where CP : class, IEntityCompBase;

    void RemoveComp(IEntityCompBase comp);
    CoroutineTask StartCoroutine(IEnumerator routine);
    void Destroy();
  }

  public interface IEntityBase<E, D> : IEntityBase where E : IEntityBase<E, D> where D : EntityDataBase
  {
    D Config { get; set; }
    new E Entity { get; set; }
    void Load(E entity, D config);
  }

  public interface IMonoEntityBase : IEntityBase
  {
    new IMonoEntityBase Entity { get; set; }
    void Load(IMonoEntityBase entity);
    new CP AddComp<CP>(params object[] agrs) where CP : Component, IMonoEntityCompBase;
    new CP GetComp<CP>() where CP : Component, IMonoEntityCompBase;
  }
  public interface IMonoEntityBase<E, D> : IEntityBase where E : MonoBehaviour, IMonoEntityBase where D : EntityDataBase
  {
    D Config { get; set; }
    new E Entity { get; set; }
    void Load(E entity, D config);
    new CP AddComp<CP>(params object[] agrs) where CP : Component, IMonoEntityCompBase<E>;
    new CP GetComp<CP>() where CP : Component, IMonoEntityCompBase<E>;
  }

  public interface IEntityCompBase
  {
    IEntityBase Entity { get; set; }
    void Load(IEntityBase entity, params object[] agrs);
    void Init();
    void Destroy();
  }
  public interface IEntityCompBase<E> : IEntityCompBase where E : IEntityBase
  {
    new E Entity { get; set; }
    void Load(E entity, params object[] agrs);
  }
  public interface IMonoEntityCompBase : IEntityCompBase
  {
    new IMonoEntityBase Entity { get; set; }
    void Load(IMonoEntityBase entity, params object[] agrs);
  }
  public interface IMonoEntityCompBase<E> : IMonoEntityCompBase where E : MonoBehaviour, IMonoEntityBase
  {
    new E Entity { get; set; }
    void Load(E entity, params object[] agrs);
  }
}