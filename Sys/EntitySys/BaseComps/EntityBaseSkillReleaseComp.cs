using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lgu3d
{

  /// <summary>
  /// 技能承受接口
  /// </summary>
  public interface ISkillRelease
  {
    bool Release(params object[] _Agr);
    void ReleaseEnd(params object[] _Agr);
  }

  public class EntityBaseSkillReleaseComp<E> : EntityCompBase<E>, ISkillRelease where E : EntityBase
  {
    public enum CompState
    {
      Idle,
      InRelease,
    }
    public CompState State;
    protected SkillBase[] Skills;

    public override void Load(E entity, params object[] agrs)
    {
      State = CompState.Idle;
      base.Load(entity, agrs);
    }


    public virtual bool Release(params object[] _Agr)
    {
      State = CompState.InRelease;
      return true;
    }

    public override void Updata(float time)
    {
      if (Skills == null) return;
      for (int i = 0; i < Skills.Length; i++)
      {
        Skills[i].Update(time);
      }
    }

    public virtual void ReleaseEnd(params object[] _Agr)
    {
      State = CompState.Idle;
    }
  }
}
