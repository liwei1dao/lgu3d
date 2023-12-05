

using System;

namespace lgu3d
{
    /// <summary>
    /// 实体技能释放组件
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class LGEntitySkillReleaseComp<E, C> : LGSkillBase<E, C> where E : class, ILGEntity where C : SkillBaseConfig<C>
    {
        public override void LGInit(ILGEntity entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
        }
    }
}
