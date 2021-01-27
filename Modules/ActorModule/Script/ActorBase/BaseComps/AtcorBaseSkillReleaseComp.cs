using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lgu3d
{

    /// <summary>
    /// 技能承受接口
    /// </summary>
    public interface ISkillRelease
    {
        bool Release(params object[] _Agr);
        void ReleaseEnd(params object[] _Agr);
    }

    public class AtcorBaseSkillReleaseComp : ActorCompBase, ISkillRelease
    {
        public enum CompState
        {
            Idle,
            InRelease,
        }
        public CompState State;
        protected SkillBase[] Skills;

        public override void Load(ActorBase _Actor, params object[] _Agr)
        {
            State = CompState.Idle;
            base.Load(_Actor, _Agr);
        }


        public virtual bool Release(params object[] _Agr)
        {
            State = CompState.InRelease;
            return true;
        }

        public override void Updata(float time)
        {
            if (Skills == null) return;
            for (int i = 0; i < Skills.Length; i++)
            {
                Skills[i].Update(time);
            }
        } 

        public virtual void ReleaseEnd(params object[] _Agr)
        {
            State = CompState.Idle;
        }
    }


    /// <summary>
    /// 技能承受组件基类
    /// </summary>
    public class AtcorBaseSkillReleaseComp<A> : AtcorBaseSkillReleaseComp where A : ActorBase
    {
        protected new A Actor;
        public override void Load(ActorBase _Actor, params object[] _Agr)
        {
            base.Load(_Actor, _Agr);
            Actor = _Actor as A;
        }
        public override bool Release(params object[] _Agr)
        {
           return base.Release(_Agr);
        }
        public override void ReleaseEnd(params object[] _Agr)
        {
            base.ReleaseEnd(_Agr);
        }
    }
}
