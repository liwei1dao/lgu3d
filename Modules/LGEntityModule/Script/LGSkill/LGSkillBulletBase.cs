using System;

namespace lgu3d
{

    /// <summary>
    /// 子弹实体对象
    /// </summary>
    public abstract class LGSkillBulletBase : LGEntityBase, ILGSkillBullet
    {
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
        }

        public abstract void Launch(ILGSkill skill, params object[] agrs);
    }
}