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

        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity);
            BT = gameObject.AddComponent<BehaviorTree>();
        }
    }
}