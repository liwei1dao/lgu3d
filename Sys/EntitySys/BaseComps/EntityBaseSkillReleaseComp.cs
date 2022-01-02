using UnityEngine;
namespace lgu3d
{

  /// <summary>
  /// 技能释放接口
  /// </summary>
  public interface IEntityBaseSkillReleaseComp
  {
    bool Release(params object[] _Agr);
    void ReleaseEnd(params object[] _Agr);
  }

  /// <summary>
  /// 实体技能释放组件
  /// </summary>
  /// <typeparam name="E"></typeparam>
  public class EntityBaseSkillReleaseComp<E> : EntityCompBase<E>, IEntityBaseSkillReleaseComp where E : EntityBase
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

    private void Updata()
    {
      if (Skills == null) return;
      for (int i = 0; i < Skills.Length; i++)
      {
        Skills[i].Update(Time.deltaTime);
      }
    }

    public virtual void ReleaseEnd(params object[] _Agr)
    {
      State = CompState.Idle;
    }
  }
}
