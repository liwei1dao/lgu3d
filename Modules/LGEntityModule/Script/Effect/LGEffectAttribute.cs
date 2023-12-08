using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace lgu3d
{
    //技能效果属性
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class LGEffectAttribute : Attribute
    {
        readonly string effectType;
        readonly int order;

        public LGEffectAttribute(string effectType, int order)
        {
            this.effectType = effectType;
            this.order = order;
        }

        public string EffectType
        {
            get { return effectType; }
        }

        public int Order
        {
            get { return order; }
        }
    }


    [Serializable]
    public abstract class LGEffect
    {

        [HideInInspector]
        public bool IsSkillEffect;

        [HideInInspector]
        private bool IsHaveEntityTarget
        {
            get
            {
                return SkillTargetInputType == SkillTargetInputType.Auto || SkillTargetInputType == SkillTargetInputType.Entity;
            }
        }

        [HideInInspector]
        private SkillTargetInputType SkillTargetInputType;
        [HideInInspector]
        public virtual string Label => "Effect";

        [ToggleGroup("Enabled", "$Label")]
        public bool Enabled;
        [ToggleGroup("Enabled"), ValueDropdown("GetLGSkillEffetTargetType")]
        public LGSkillEffetTargetType LGSkillEffetTargetType;

        [ToggleGroup("Enabled"), ValueDropdown("GetEffectTriggerType")]
        public EffectTriggerType EffectTriggerType;

        [ToggleGroup("Enabled"), LabelText("触发概率")]
        public string TriggerProbability = "100%";

        //初始化
        public abstract void LGInit(ILGSkill skill);

        /// <summary>
        /// 技能输入类型变化 重置效果作用目标
        /// </summary>
        public void SetSkillTargetInputType(SkillTargetInputType skillTargetInputType)
        {
            SkillTargetInputType = skillTargetInputType;
            if (!IsHaveEntityTarget)
            {
                if (IsSkillEffect && LGSkillEffetTargetType == LGSkillEffetTargetType.SkillTarget)
                {
                    LGSkillEffetTargetType = LGSkillEffetTargetType.Self;
                }
            }
        }

        /// <summary>
        /// 获取效果目标类型
        /// </summary>
        /// <returns></returns>
        private IEnumerable GetLGSkillEffetTargetType()
        {
            ValueDropdownList<LGSkillEffetTargetType> valeus = new() { { "自身", LGSkillEffetTargetType.Self } };

            if (IsHaveEntityTarget)
            {
                valeus.Add("技能目标", LGSkillEffetTargetType.SkillTarget);
            }
            return valeus;
        }


        private IEnumerable GetEffectTriggerType()
        {
            ValueDropdownList<EffectTriggerType> valeus = new() { { "（空）", EffectTriggerType.None } };

            if (IsHaveEntityTarget)
            {

            }
            return valeus;
        }
    }

}