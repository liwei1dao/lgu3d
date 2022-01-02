using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{

  public abstract class BulletBase : IBulletBase
  {

    public SkillBase Skill { get; set; }
    public BulletBase(SkillBase skill, params object[] agrs)
    {
      Skill = skill;
    }


    public virtual void Launch(params object[] agrs)
    {

    }

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
    public BulletBase(S skill, params object[] agrs)
        : base(skill, agrs)
    {
      Skill = skill;
    }
  }

}
