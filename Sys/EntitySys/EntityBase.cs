using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{
  public abstract class EntityBase : MonoBehaviour, IEntityBase
  {
    public EntityDataBase Config { get; set; }
    protected List<IEntityCompBase> MyComps = new List<IEntityCompBase>();
    #region 基础组件接口
    public IEntityBaseSkillReleaseComp SkillReleaseComp;          //技能释放组件
    public IEntityBaseSkillAcceptComp SkilllAcceptComp;           //技能承受组件
    #endregion

    #region 框架函数
    public virtual void Load(EntityDataBase config)
    {
      Config = config;
    }

    public virtual void Init()
    {
      for (int i = 0; i < MyComps.Count; i++)
      {
        MyComps[i].Init();
      }
    }
    public virtual CP AddComp<CP>(params object[] agrs) where CP : IEntityCompBase, new()
    {
      CP Comp = new CP();
      Comp.Load(this, agrs);
      MyComps.Add(Comp);
      return Comp;
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
    #endregion
  }



  public abstract class EntityBase<E, D> : EntityBase, IEntityBase<E, D> where E : EntityBase, IEntityBase<E, D> where D : EntityDataBase
  {
    public new D Config { get; set; }


    public virtual void Load(D config)
    {
      Config = config;
      base.Load(config);
    }
  }
}
