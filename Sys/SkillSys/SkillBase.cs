using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  public class SkillBase : ISkillBase
  {
    public string SkillId { get; set; }
    public IEntityBase Entity { get; set; }
    public SkillDataBase Config { get; set; }
    public SkillState State { get; set; }
    public SkillCDBase Cd;
    protected List<IBulletBase> Bullets;

    public SkillBase(string skillId, IEntityBase entity, SkillDataBase config)
    {
      SkillId = skillId;
      Entity = entity;
      Config = config;
      Cd = new SkillCDBase(this, Config.CdTime);
      State = SkillState.NoRelease;
      Bullets = new List<IBulletBase>();
    }

    public virtual void Init(params object[] agrs)
    {

    }

    public virtual void Release(params object[] agrs)
    {
      State = SkillState.InRelease;
    }

    public virtual void Update(float time)
    {
      for (int i = 0; i < Bullets.Count; i++)
      {
        Bullets[i].Update(time);
      }
      if (State == SkillState.InCd)
        Cd.Update(time);
    }

    public virtual void ReleaseEnd()
    {
      if (Config.CdTime > 0)
      {
        State = SkillState.InCd;
        Cd.CdStart();
      }
      else
      {
        State = SkillState.NoRelease;
      }
    }

    public virtual void CdEnd()
    {
      State = SkillState.NoRelease;
    }
    public virtual IBulletBase AddBullet(IBulletBase bullet)
    {
      Bullets.Add(bullet);
      return bullet;
    }
    public virtual void RemoveBullet(IBulletBase bullet)
    {
      Bullets.Remove(bullet);
    }
  }

  public abstract class SkillBase<E, D> : SkillBase, ISkillBase<E, D> where E : IEntityBase where D : SkillDataBase
  {

    public new E Entity { get; set; }
    public new D Config { get; set; }
    protected SkillBase(string skillId, E entity, D config) : base(skillId, entity, config)
    {
      Entity = entity;
      Config = config;
    }
  }
}