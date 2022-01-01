
namespace lgu3d
{
  public delegate void EntityAttributeChanage(byte AttributeType);
  public class EntityBaseAttributeComp<E> : EntityCompBase<E> where E : EntityBase
  {
    protected EntityAttributeChanage AttributeChanage;

    public void RegisterChanageEvent(EntityAttributeChanage _AttributeChanage)
    {
      AttributeChanage += _AttributeChanage;
    }

    public void UnRegisterChanageEvent(EntityAttributeChanage _AttributeChanage)
    {
      AttributeChanage -= _AttributeChanage;
    }

    public virtual void SetAttribute(byte _AttributeType, object _Value)
    {
      if (AttributeChanage != null)
      {
        AttributeChanage(_AttributeType);
      }
    }

    public virtual object GetAttribute(byte _AttributeType)
    {
      return null;
    }
  }
}
