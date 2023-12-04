using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 技能对象
    /// </summary>
    public abstract class LGSkillBase : LGEntityCompBase, ILGSkill
    {
        public LGSkillState State { get; set; }
        public override void LGInit(ILGEntity entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            State = LGSkillState.NoRelease;
        }

        public abstract void Release(params object[] agrs);
        public abstract List<ILGEntity> TargetFilters();
        public abstract List<ILGSkillBullet> CreateSkillBullet();

    }
}