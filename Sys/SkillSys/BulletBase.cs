using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{

  public abstract class BulletBase : IBulletBase
  {

    public SkillBase Skill { get; set; }
    public BulletBase(SkillBase skill)
    {
      Skill = skill;
    }

    /// <summary>
    /// 发射
    /// </summary>
    /// <param name="agrs"></param>
    public virtual void Launch(params object[] agrs)
    {

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
    public virtual void Update(float time)
    {

    }

    public virtual void Destroy()
    {
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

}
