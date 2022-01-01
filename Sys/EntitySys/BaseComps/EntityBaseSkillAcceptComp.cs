namespace lgu3d
{
  /// <summary>
  /// 技能承受接口
  /// </summary>
  public interface ISkillAccept
  {
    void Accept(BulletBase Bullet);
  }

  public class EntityBaseSkillAcceptComp<E> : EntityCompBase<E>, ISkillAccept where E : EntityBase
  {
    public virtual void Accept(BulletBase Bullet)
    {

    }
  }
}
