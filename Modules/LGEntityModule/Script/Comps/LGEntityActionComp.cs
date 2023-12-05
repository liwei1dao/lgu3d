

using System;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 实体行为组件
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class LGEntityActionComp<E, A> : LGEntityCompBase<E> where E : class, ILGEntity where A : Enum
    {
        private readonly Dictionary<A, ILGAction> action = new();
        public override void LGInit(ILGEntity entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
        }

    }
}
