using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{
  public abstract class EntityBase : IEntityBase
  {
    public IEntityBase Entity { get; set; }
    protected List<IEntityCompBase> MyComps = new List<IEntityCompBase>();

    #region 框架函数
    public virtual void Load(IEntityBase entity)
    {
      Entity = entity;
    }

    public virtual void Init()
    {
      for (int i = 0; i < MyComps.Count; i++)
      {
        MyComps[i].Init();
      }
    }
    public virtual CP AddComp<CP>(params object[] agrs) where CP : class, IEntityCompBase, new()
    {
      CP Comp = new CP();
      Comp.Load(this, agrs);
      MyComps.Add(Comp);
      return Comp;
    }

    public virtual CP GetComp<CP>() where CP : class, IEntityCompBase
    {
      foreach (var item in MyComps)
      {
        if (item is CP)
        {
          return item as CP;
        }
      }
      return null;
    }

    public virtual void RemoveComp(IEntityCompBase comp)
    {
      MyComps.Remove(comp);
      comp.Destroy();
    }
    public void Destroy()
    {
      for (int i = 0; i < MyComps.Count; i++)
      {
        MyComps[i].Destroy();
      }
    }

    public CoroutineTask StartCoroutine(IEnumerator routine)
    {
      return CoroutineModule.Instance.StartCoroutine(routine);
    }
    #endregion
  }


  public abstract class EntityBase<E> : EntityBase, IEntityBase<E> where E : EntityBase, IEntityBase<E>
  {
    public new E Entity { get; set; }

    public virtual void Load(E entity)
    {
      Entity = entity;
      base.Load(entity);
    }
  }

  public abstract class EntityBase<E, D> : EntityBase, IEntityBase<E, D> where E : EntityBase, IEntityBase<E, D> where D : EntityDataBase
  {
    public D Config { get; set; }
    public new E Entity { get; set; }

    public virtual void Load(E entity, D config)
    {
      Entity = entity;
      Config = config;
      base.Load(entity);
    }
  }

  public abstract class MonoEntityBase : MonoBehaviour, IMonoEntityBase
  {
    public IMonoEntityBase Entity { get; set; }
    IEntityBase IEntityBase.Entity { get; set; }
    protected List<IEntityCompBase> MyComps = new List<IEntityCompBase>();
    #region 框架函数
    public virtual void Load(IMonoEntityBase entity)
    {
      Entity = entity;
    }

    public void Load(IEntityBase entity)
    {

    }

    public virtual void Init()
    {
      for (int i = 0; i < MyComps.Count; i++)
      {
        MyComps[i].Init();
      }
    }
    public CP AddComp<CP>(params object[] agrs) where CP : Component, IMonoEntityCompBase
    {
      CP comp = gameObject.AddMissingComponent<CP>();
      comp.Load(Entity, agrs);
      MyComps.Add(comp);
      return comp;
    }

    CP IEntityBase.AddComp<CP>(params object[] agrs)
    {
      throw new System.NotImplementedException();
    }

    public CP GetComp<CP>() where CP : Component, IMonoEntityCompBase
    {
      foreach (var item in MyComps)
      {
        if (item is CP)
        {
          return item as CP;
        }
      }
      return null;
    }

    CP IEntityBase.GetComp<CP>()
    {
      throw new System.NotImplementedException();
    }
    public virtual void RemoveComp(IEntityCompBase comp)
    {
      MyComps.Remove(comp);
      comp.Destroy();
    }
    public void Destroy()
    {
      for (int i = 0; i < MyComps.Count; i++)
      {
        MyComps[i].Destroy();
      }
      GameObject.Destroy(this);
    }
    public new CoroutineTask StartCoroutine(IEnumerator routine)
    {
      return CoroutineModule.Instance.StartCoroutine(routine);
    }

    #endregion
  }


  public abstract class MonoEntityBase<E> : MonoEntityBase, IMonoEntityBase<E> where E : MonoEntityBase, IMonoEntityBase<E>
  {
    public new E Entity { get; set; }
    public virtual void Load(E entity)
    {
      Entity = entity;
      base.Load(entity);
    }

    public new CP AddComp<CP>(params object[] agrs) where CP : Component, IMonoEntityCompBase<E>
    {
      CP comp = gameObject.AddMissingComponent<CP>();
      comp.Load(Entity, agrs);
      MyComps.Add(comp);
      return comp;
    }

    CP IEntityBase.AddComp<CP>(params object[] agrs)
    {
      throw new System.NotImplementedException();
    }

    public new CP GetComp<CP>() where CP : Component, IMonoEntityCompBase<E>
    {
      foreach (var item in MyComps)
      {
        if (item is CP)
        {
          return item as CP;
        }
      }
      return null;
    }

    CP IEntityBase.GetComp<CP>()
    {
      throw new System.NotImplementedException();
    }
  }

  public abstract class MonoEntityBase<E, D> : MonoEntityBase, IMonoEntityBase<E, D> where E : MonoEntityBase, IMonoEntityBase<E, D> where D : EntityDataBase
  {
    public D Config { get; set; }
    public new E Entity { get; set; }
    public virtual void Load(E entity, D config)
    {
      Entity = entity;
      Config = config;
      base.Load(entity);
    }

    public new CP AddComp<CP>(params object[] agrs) where CP : Component, IMonoEntityCompBase<E>
    {
      CP comp = gameObject.AddMissingComponent<CP>();
      comp.Load(Entity, agrs);
      MyComps.Add(comp);
      return comp;
    }

    CP IEntityBase.AddComp<CP>(params object[] agrs)
    {
      throw new System.NotImplementedException();
    }

    public new CP GetComp<CP>() where CP : Component, IMonoEntityCompBase<E>
    {
      foreach (var item in MyComps)
      {
        if (item is CP)
        {
          return item as CP;
        }
      }
      return null;
    }

    CP IEntityBase.GetComp<CP>()
    {
      throw new System.NotImplementedException();
    }
  }
}
