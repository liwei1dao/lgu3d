namespace lgu3d
{

    public interface IEntityBaseSkillAcceptComp : IEntityCompBase
    {
        void Accept(IBullet Bullet);
    }
    public interface IMonoEntityBaseSkillAcceptComp : IEntityCompBase
    {
        void Accept(IBullet Bullet);
    }
    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class EntityBaseSkillAcceptComp<E> : EntityCompBase<E>, IEntityBaseSkillAcceptComp where E : EntityBase<E>
    {
        public virtual void Accept(IBullet Bullet)
        {

        }
    }

    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class MonoEntityBaseSkillAcceptComp<E> : MonoEntityCompBase<E>, IMonoEntityBaseSkillAcceptComp where E : MonoEntityBase
    {
        public virtual void Accept(IBullet Bullet)
        {

        }
    }
}
