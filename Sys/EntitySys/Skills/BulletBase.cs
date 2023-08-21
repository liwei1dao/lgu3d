using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{

    public enum BulletState
    {
        Inmotion = 1,   //运动中
        Release = 2,    //释放
        Destroy = 3,    //销毁
    }

    //子弹
    public interface IBulletBase
    {
        BulletState State { get;  set; }
        void LGInit(ISkillBase skill, params object[] agrs);
    }

    public abstract class BulletBase<S> : IBulletBase where S : class, ISkillBase
    {
        public S Skill;
        public BulletState State { get;  set; }
        public BulletBase(S skill, params object[] agrs)
        {
            LGInit(skill, agrs);
        }

        public virtual void LGInit(ISkillBase skill, params object[] agrs)
        {
            Skill = skill as S;
        }
    }

    public abstract class MonoBulletBase<S> : MonoBehaviour,IBulletBase where S : class, ISkillBase
    {
        public S Skill;
        public BulletState State { get;  set; }
        public virtual void LGInit(ISkillBase skill, params object[] agrs)
        {
            Skill = skill as S;
        }

        /// <summary>
        /// 释放子弹效果
        /// </summary>
        protected virtual void LGRelease(params IEntityBaseSkillAcceptComp[] targets) {
            foreach (var target in targets)
            {
                target.Accept(this);
            }
        }

        /// <summary>
        /// 自我销毁
        /// </summary>
        protected virtual void LGDestroy() {
            GameObject.Destroy(this);
        }
    }
}
