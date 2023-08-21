namespace lgu3d
{

    public interface IEntityBaseSkillAcceptComp : IEntityCompBase
    {
        void Accept(IBulletBase Bullet);
    }

    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class EntityBaseSkillAcceptComp<E> : EntityCompBase<E> where E : EntityBase<E>
    {
        public virtual void Accept(IBulletBase Bullet)
        {

        }
    }

    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class MonoEntityBaseSkillAcceptComp<E> : MonoEntityCompBase<E> where E : MonoEntityBase
    {
        public virtual void Accept(IBulletBase Bullet)
        {

        }
    }
}
