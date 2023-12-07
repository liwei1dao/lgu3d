using System;
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
        public virtual string Label => "Effect";

        [ToggleGroup("Enabled", "$Label")]
        public bool Enabled;
        [ToggleGroup("Enabled")]
        public LGSkillEffetTargetType LGSkillEffetTargetType;

        [ToggleGroup("Enabled"), LabelText("触发概率")]
        public string TriggerProbability = "100%";

        //初始化
        public abstract void LGInit(ILGSkill skill);
    }




}