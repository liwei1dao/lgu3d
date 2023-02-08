namespace lgu3d
{


    public interface IEntityBaseSkillAcceptComp
    {
        void Accept(IBulletBase Bullet);
    }
    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class EntityBaseSkillAcceptComp : EntityCompBase, IEntityBaseSkillAcceptComp
    {
        public virtual void Accept(IBulletBase Bullet)
        {

        }
    }
    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class EntityBaseSkillAcceptComp<E> : EntityCompBase<E> where E : EntityBase<E>
    {
        public new E Entity { get; set; }

        public virtual void Load(E entity, params object[] agrs)
        {
            Entity = entity;
            base.Init(entity);
        }
        public virtual void Accept(IBulletBase Bullet)
        {

        }
    }

    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class MonoEntityBaseSkillAcceptComp : MonoEntityCompBase, IEntityBaseSkillAcceptComp
    {
        public virtual void Accept(IBulletBase Bullet)
        {

        }
    }
    /// <summary>
    /// 技能接收
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class MonoEntityBaseSkillAcceptComp<E> : MonoEntityBaseSkillAcceptComp where E : MonoEntityBase
    {
        public new E Entity { get; set; }

        public override void Init(IEntityBase entity, params object[] agrs)
        {
            Entity = entity as E;
            base.Init(entity);
        }
    }
}
