using System;

namespace lgu3d
{
    public abstract class ActorCompBase
    {
        protected ActorBase Actor;

        public virtual void Load(ActorBase _Actor, params object[] _Agr)
        {
            Actor = _Actor;
        }

        public virtual void Init()
        {

        }

        public virtual void Updata(float time)
        {

        }

        public virtual void Destroy()
        {

        }
    }

    public abstract class ActorCompBase<A>: ActorCompBase where A: ActorBase
    {
        protected new A Actor;

        public override void Load(ActorBase _Actor, params object[] _Agr)
        {
            Actor = _Actor as A;
            base.Load(Actor, _Agr);
        }
    }

}
