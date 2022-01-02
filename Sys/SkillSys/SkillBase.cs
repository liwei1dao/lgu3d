using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{



  public class SkillBase : ISkillBase
  {
    public EntityBase Entity { get; set; }
    public SkillDataBase Config { get; set; }
    public SkillCDBase Cd;
    public SkillState State;
    protected List<BulletBase> Bullets;

    public virtual void Load(EntityBase entity, SkillDataBase config)
    {
      Entity = entity;
      Config = config;
      Cd = new SkillCDBase(this, Config.CdTime);
      State = SkillState.NoRelease;
      Bullets = new List<BulletBase>();
    }

    public virtual void Init(params object[] _Agr)
    {

    }

    public virtual void Release(params object[] _Agr)
    {
      State = SkillState.InRelease;
      Entity.StartCoroutine(ReleaseAnim());
    }

    protected virtual IEnumerator ReleaseAnim()
    {
      yield return 1;

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
      State = SkillState.InCd;
      Cd.CdStart();
    }

    public virtual void CdEnd()
    {
      State = SkillState.NoRelease;
    }

    public virtual void RemoveBullet(BulletBase bullet)
    {
      Bullets.Remove(bullet);
    }
  }

  public abstract class SkillBase<E, D> : SkillBase, ISkillBase<E, D> where E : EntityBase where D : SkillDataBase
  {
    public new E Entity { get; set; }
    public new D Config { get; set; }
    public virtual void Load(E entity, D config)
    {
      base.Load(entity, config);
      Entity = entity;
      Config = config;
    }
  }
}