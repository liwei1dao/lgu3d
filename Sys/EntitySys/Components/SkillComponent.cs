

using System.Collections.Generic;
using UnityEngine;

namespace LG.EntitySys
{
    /// <summary>
    /// 技能施法组件
    /// </summary>
    public abstract class SkillComponent : LGEntityComp
    {
        protected Dictionary<int, ILGSkill> skill = new();

        public override void LGInit(LGEntity entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
        }


        public void SpellWithTarget(ILGSkill skill, LGEntity targetEntity)
        {

        }

        public void SpellWithPoint(ILGSkill skill, Vector3 point)
        {

        }

        public void SpellWithDirect(ILGSkill skill, Vector3 direction)
        {

        }
    }
}