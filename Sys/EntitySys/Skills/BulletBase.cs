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
        object GetMeta(string key);
        void SetMeta(string key,object value);
        void LGInit(ISkillBase skill, Dictionary<string,object> meta);
    }

    public abstract class BulletBase<S> : IBulletBase where S : class, ISkillBase
    {
        public S Skill;
        public BulletState State { get;  set; }
        /// <summary>
        /// 元数据
        /// </summary>
        protected Dictionary<string,object> Meta;
        public BulletBase(S skill, Dictionary<string,object> meta)
        {
            LGInit(skill,meta);
        }

        public virtual object GetMeta(string key) {
            return Meta[key];
        }
        public void SetMeta(string key,object value){
            Meta[key] = value;
        }
        public virtual void LGInit(ISkillBase skill, Dictionary<string,object> meta)
        {
            Skill = skill as S;
        }
    }

    public abstract class MonoBulletBase<S> : MonoBehaviour,IBulletBase where S : class, ISkillBase
    {
        public S Skill;
        public BulletState State { get;  set; }
        protected Dictionary<string,object> Meta;

        public virtual object GetMeta(string key) {
            return Meta[key];
        }
        public void SetMeta(string key,object value){
            Meta[key] = value;
        }

        public virtual void LGInit(ISkillBase skill, Dictionary<string,object> meta)
        {
            Skill = skill as S;
            Meta = meta;
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
            GameObject.Destroy(gameObject);
        }
    }
}
