using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{

  public abstract class BulletBase : IBulletBase
  {
    public SkillBase Skill { get; set; }
    public BulletState State { get; set; }

    public BulletBase(SkillBase skill)
    {
      Skill = skill;
      State = BulletState.WaitLaunch;
    }

    /// <summary>
    /// 发射
    /// </summary>
    /// <param name="agrs"></param>
    public virtual void Launch(params object[] agrs)
    {
      State = BulletState.Launching;
    }

    /// <summary>
    /// 起效
    /// </summary>
    /// <param name="agrs"></param>
    public virtual void TakeEffect(params object[] agrs)
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="time"></param>
    public virtual void BulletUpdate(float time)
    {

    }

    public virtual void Destroy()
    {
      State = BulletState.Destroyed;
      Skill.RemoveBullet(this);
    }
  }

  public abstract class BulletBase<S> : BulletBase where S : SkillBase
  {
    protected new S Skill;
    public BulletBase(S skill)
        : base(skill)
    {
      Skill = skill;
    }
  }

  public abstract class MonoBulletBase : MonoBehaviour, IBulletBase
  {

    public SkillBase Skill { get; set; }
    public BulletState State { get; set; }

    public virtual void Load(SkillBase skill)
    {
      Skill = skill;
      State = BulletState.WaitLaunch;
    }

    /// <summary>
    /// 发射
    /// </summary>
    /// <param name="agrs"></param>
    public virtual void Launch(params object[] agrs)
    {
      State = BulletState.Launching;
    }

    /// <summary>
    /// 起效
    /// </summary>
    /// <param name="agrs"></param>
    public virtual void TakeEffect(params object[] agrs)
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="time"></param>
    public virtual void BulletUpdate(float time)
    {

    }

    public virtual void Destroy()
    {
      State = BulletState.Destroyed;
      Skill.RemoveBullet(this);
    }
  }


  public abstract class MonoBulletBase<S> : MonoBulletBase where S : SkillBase
  {
    protected new S Skill;
    public virtual void Load(S skill)
    {
      base.Load(skill);
      Skill = skill;
    }
  }
}
