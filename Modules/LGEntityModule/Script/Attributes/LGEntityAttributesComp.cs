using System;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 属性管理
    /// </summary>
    public abstract class LGEntityAttributesComp<A> : LGEntityCompBase, ILGEntityAttributesComp<A>
    {
        protected readonly Dictionary<A, FloatNumeric> attributeNameNumerics = new Dictionary<A, FloatNumeric>();
        public override void LGInit(ILGEntity entity)
        {
            base.LGInit(entity);
        }

        public FloatNumeric AddNumeric(A attr, float baseValue)
        {
            var numeric = new FloatNumeric(attr.ToString(), baseValue);
            attributeNameNumerics.Add(attr, numeric);
            return numeric;
        }

        public FloatNumeric GetNumeric(A attr)
        {
            return attributeNameNumerics[attr];
        }

        public void OnNumericUpdate(FloatNumeric numeric)
        {

        }
    }
}