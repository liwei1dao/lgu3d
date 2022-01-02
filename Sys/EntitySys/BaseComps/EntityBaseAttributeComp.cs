
using System;
using System.Collections.Generic;

namespace lgu3d
{

  public class EntityBaseAttributeComp<E, A> : EntityCompBase<E> where E : EntityBase where A : Enum
  {
    protected Dictionary<A, float> Attributes;

    public override void Load(E entity, params object[] agrs)
    {
      base.Load(entity, agrs);
      Attributes = new Dictionary<A, float>();
      foreach (A item in Enum.GetValues(typeof(A)))
      {
        Attributes[item] = 0;
      }
    }
    public virtual void AddAttribute(A aType, float value)
    {
      Attributes[aType] = value;
    }

    public virtual float GetAttribute(A aType)
    {
      return Attributes[aType];
    }
  }
}
