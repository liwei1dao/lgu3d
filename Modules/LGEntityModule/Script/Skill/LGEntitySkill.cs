using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 技能对象
    /// </summary>
    public abstract class LGEntitySkill<C> : LGEntityCompBase, ILGSkill where C : SkillBaseConfig<C>
    {
        protected C Config;
        public LGSkillState State { get; set; }
        protected LGSkillCD CD;
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
            State = LGSkillState.NoRelease;
            CD = new LGSkillCD(this);
        }

        //技能释放
        public abstract void Release();

        /// <summary>
        /// 技能释放条件检测
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkillReleaseConditionCheck()
        {
            return true;
        }

        /// <summary>
        /// 选择目标对象
        /// </summary>
        /// <returns></returns>
        protected abstract List<ILGEntity> SelectTarget();

        /// <summary>
        /// 执行技能效果
        /// </summary>
        /// <param name="targets"></param>
        protected abstract void ExecutionEffect(List<ILGEntity> targets);

        //cd结束
        public virtual void CdEnd()
        {
            State = LGSkillState.NoRelease;
        }

        public override void LGUpdate(float time)
        {
            if (State == LGSkillState.InCd)
            {
                CD.Update(time);
            }
        }
    }
}