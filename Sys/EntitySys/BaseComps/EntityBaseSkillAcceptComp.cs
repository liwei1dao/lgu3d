namespace lgu3d
{


  public interface IEntityBaseSkillAcceptComp
  {
    void Accept(BulletBase Bullet);
  }

  /// <summary>
  /// 技能接收
  /// </summary>
  /// <typeparam name="E"></typeparam>
  public abstract class EntityBaseSkillAcceptComp<E> : EntityCompBase<E>, IEntityBaseSkillAcceptComp where E : EntityBase
  {
    public virtual void Accept(BulletBase Bullet)
    {

    }
  }
  /// <summary>
  /// 技能接收
  /// </summary>
  /// <typeparam name="E"></typeparam>
  public abstract class MonoEntityBaseSkillAcceptComp<E> : MonoEntityCompBase<E>, IEntityBaseSkillAcceptComp where E : MonoEntityBase
  {
    public virtual void Accept(BulletBase Bullet)
    {

    }
  }
}
