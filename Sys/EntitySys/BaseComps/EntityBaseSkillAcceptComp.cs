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
    public abstract class EntityBaseSkillAcceptComp<E> : EntityBaseSkillAcceptComp where E : EntityBase
    {
        public new E Entity { get; set; }

        public virtual void Load(E entity, params object[] agrs)
        {
            Entity = entity;
            base.Load(entity);
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

        public override void Load(IEntityBase entity, params object[] agrs)
        {
            Entity = entity as E;
            base.Load(entity);
        }
    }
}
