using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace lgu3d
{

    /// <summary>
    /// 技能效果基础类型
    /// </summary>
    public abstract class LGEffectApplyBase : ILGEffect
    {
        public abstract bool Apply(ILGEntity target);
    }

    /// <summary>
    /// 技能效果基础类型
    /// </summary>
    public abstract class LGEffectApplyBase<E> : LGEffectApplyBase
    {

    }


    //技能效果属性
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class LGEffectAttribute : Attribute
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


    }
}