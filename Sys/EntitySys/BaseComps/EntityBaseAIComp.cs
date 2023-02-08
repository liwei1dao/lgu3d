using BehaviorDesigner.Runtime;

namespace lgu3d
{
    /// <summary>
    /// MonoAI组件基类
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public class MonoEntityBaseAIComp<E> : MonoEntityCompBase<E> where E : MonoEntityBase
    {
        protected BehaviorTree BT { get; set; }

        public override void Init(E entity, params object[] agrs)
        {
            base.Init(entity);
            Entity = entity;
            BT = gameObject.AddComponent<BehaviorTree>();
        }
    }
}