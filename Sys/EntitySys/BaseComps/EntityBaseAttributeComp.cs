
using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public class EntityBaseAttributeComp<E, A> : EntityCompBase<E> where E : class, IEntityBase where A : Enum
    {
        protected Dictionary<A, float> Attributes;

        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            Attributes = new Dictionary<A, float>();
            foreach (A item in Enum.GetValues(typeof(A)))
            {
                Attributes[item] = 0;
            }
            base.LGInit(entity, agrs);
        }
        public virtual void AddAttribute(A aType, float value)
        {
            Attributes[aType] += value;
        }

        public virtual float GetAttribute(A aType)
        {
            return Attributes[aType];
        }
    }

    public class MonoEntityBaseAttributeComp<E, A> : MonoEntityCompBase<E> where E : MonoEntityBase where A : Enum
    {
        protected Dictionary<A, float> Attributes;

        public override void LGInit(IEntityBase entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            Attributes = new Dictionary<A, float>();
            foreach (A item in Enum.GetValues(typeof(A)))
            {
                Attributes[item] = 0;
            }
        }
        public virtual void AddAttribute(A aType, float value)
        {
            Attributes[aType] += value;
        }

        public virtual float GetAttribute(A aType)
        {
            return Attributes[aType];
        }
    }
}
