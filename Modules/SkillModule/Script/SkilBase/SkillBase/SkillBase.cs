using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public enum SkillState
    {
        NoRelease,
        InRelease,
        InCd,
    }

    public interface ISkillBase
    {
        void Load(ActorBase Actor, SkillDataBase _Config);          //加载
        void Init(params object[] _Agr);
        void Release(params object[] _Agr);                         //技能释放
        void Update(float time);                                    //技能更新
        void ReleaseEnd();                                          //技能释放结束
    }

    public class SkillBase: ISkillBase
    {
        public ActorBase Actor;
        protected SkillDataBase Config;
        public SkillCDBase Cd;
        public SkillState State;
        protected List<BulletBase> Bullets;

        public virtual void Load(ActorBase _Actor, SkillDataBase _Config)
        {
            Actor = _Actor;
            Config = _Config;
            Cd = new SkillCDBase(this, Config.CdTime);
            State = SkillState.NoRelease;
            Bullets = new List<BulletBase>();
        }

        public virtual void Init(params object[] _Agr)
        {

        }

        public virtual void Release(params object[] _Agr)
        {
            State = SkillState.InRelease;
            Actor.StartCoroutine(ReleaseAnim());
        }

        protected virtual IEnumerator ReleaseAnim()
        {
            yield return 1;

        }

        public virtual void Update(float time)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Update(time);
            }
            if(State == SkillState.InCd)
                Cd.Update(time);
        }

        public virtual void ReleaseEnd()
        {
            State = SkillState.InCd;
            Cd.CdStart();
        }

        public virtual void CdEnd()
        {
            State = SkillState.NoRelease;
        }

        public virtual void RemoveBullet(BulletBase _Bullet)
        {
            Bullets.Remove(_Bullet);
        }
    }
}