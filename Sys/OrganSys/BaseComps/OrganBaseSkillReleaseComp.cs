namespace lgu3d
{
  /// <summary>
  /// 机关 技能释放组件
  /// </summary>
  /// <typeparam name="E"></typeparam>
  public class OrganBaseSkillReleaseComp : EntityBaseSkillReleaseComp<OrganBase>
  {
    public override bool Release(string skillName, params object[] agrs)
    {
      Skills[skillName].Release(agrs);
      return true;
    }
  }

  /// <summary>
  /// 机关 技能释放组件
  /// </summary>
  /// <typeparam name="E"></typeparam>
  public class MonoOrganBaseSkillReleaseComp : MonoEntityBaseSkillReleaseComp<MonoOrganBase>
  {
    public override bool Release(string skillName, params object[] agrs)
    {
      Skills[skillName].Release(agrs);
      return true;
    }
  }

}