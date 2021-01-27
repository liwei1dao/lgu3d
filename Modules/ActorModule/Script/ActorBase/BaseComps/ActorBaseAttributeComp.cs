
namespace lgu3d
{
    public delegate void ActorAttributeChanage(byte AttributeType);
    public class ActorBaseAttributeComp : ActorCompBase
    {
        protected ActorAttributeChanage AttributeChanage;

        public void RegisterChanageEvent(ActorAttributeChanage _AttributeChanage)
        {
            AttributeChanage += _AttributeChanage;
        }

        public void UnRegisterChanageEvent(ActorAttributeChanage _AttributeChanage)
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

    public class ActorBaseAttributeComp<A> : ActorBaseAttributeComp where A : ActorBase
    {
        protected new A Actor;
        public override void Load(ActorBase _Actor, params object[] _Agr)
        {
            Actor = _Actor as A;
            base.Load(Actor, _Agr);
        }
    }
}
