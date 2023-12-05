

using System;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 实体属性管理组件
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class LGEntityAttributeComp<E, A> : LGEntityCompBase<E> where E : class, ILGEntity where A : Enum
    {
        private readonly Dictionary<A, FloatNumeric> attributeNameNumerics = new();

        public FloatNumeric AddNumeric(A attr, float baseValue)
        {
            var numeric = new FloatNumeric();
            numeric.SetBase(baseValue);
            attributeNameNumerics.Add(attr, numeric);
            return numeric;
        }

        public FloatNumeric GetNumeric(A attributeName)
        {
            return attributeNameNumerics[attributeName];
        }
    }
}