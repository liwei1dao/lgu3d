using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 技能对象
    /// </summary>
    public abstract class LGSkillBase : LGEntityCompBase, ILGSkill
    {
        public LGSkillState State { get; set; }
        protected LGSkillCD CD;
        public override void LGInit(ILGEntity entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            State = LGSkillState.NoRelease;
        }

        //技能释放
        public abstract void Release(params object[] agrs);
        //目标索敌
        public abstract List<ILGEntity> TargetFilters();
        //子弹创建
        public abstract List<ILGSkillBullet> CreateSkillBullet();
        //cd结束
        public abstract void CdEnd();
        public override void LGUpdate(float time)
        {
            CD.Update(time);
        }
    }
}