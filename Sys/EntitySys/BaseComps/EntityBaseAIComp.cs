using BehaviorDesigner.Runtime;

namespace lgu3d
{
    public class MonoEntityBaseAIComp : MonoEntityCompBase
    {
        protected BehaviorTree BT { get; set; }
        public override void Load(IEntityBase entity, params object[] agrs)
        {
            BT = gameObject.AddComponent<BehaviorTree>();
            base.Load(entity, agrs);
        }
    }
    public class MonoEntityBaseAIComp<E> : MonoEntityCompBase where E : MonoEntityBase
    {
        public new E Entity { get; set; }

        public virtual void Load(E entity, params object[] agrs)
        {
            Entity = entity;
            base.Load(entity);
        }
    }
}