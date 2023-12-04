using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 技能对象
    /// </summary>
    public abstract class LGSkillBase : LGEntityCompBase, ILGSkill
    {
        public LGSkillState State { get; set; }
        public List<ILGSkillTargetFinder> Finders;
        public override void LGInit(ILGEntity entity, params object[] agrs)
        {
            base.LGInit(entity);
            State = LGSkillState.NoRelease;
        }
    }
}