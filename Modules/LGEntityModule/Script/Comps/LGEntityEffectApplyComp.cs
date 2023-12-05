

using System;

namespace lgu3d
{
    /// <summary>
    /// 实体技能效果接受组件
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class LGEntityEffectApplyComp<E, Ef> : LGEntityCompBase<E> where E : class, ILGEntity where Ef : Enum
    {
        public override void LGInit(ILGEntity entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
        }

    }
}
