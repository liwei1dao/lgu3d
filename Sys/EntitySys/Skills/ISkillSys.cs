using System;
using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{
    public enum SkillState
    {
        NoRelease,          //没有释放
        InRelease,          //释放中
        InCd,               //CD冷却中
        Reloading,          //装弹中
    }
    public enum SkillCDState
    {
        CdEnd,
        CdIn,
    }

    public enum BulletState
    {
        Inmotion = 1,   //运动中
        Release = 2,    //释放
        Destroy = 3,    //销毁
    }
    //子弹
    public interface IBullet
    {
        Dictionary<string, object> GetMeta();
    }
    public interface IBulletBase : IBullet
    {
        void Launch(IEntityBase target, Dictionary<string, object> meta);
    }
    public interface IMonoBulletBase : IBullet
    {
        void Launch(MonoEntityBase target, Dictionary<string, object> meta);
    }

    public interface ISkillMonitor
    {
        void OnReleaseEnd(ISkillBase skil);
        void OnCdEnd(ISkillBase skil);
    }
    public interface ISkillBase : IEntityCompBase
    {
        public SkillState GetState();
        IEntityBase GetHostEntity();
        ///技能释放接口
        void Release(IEntityBase target, params object[] agrs);
        void Release(Vector3 direction, params object[] agrs);
        void CdEnd();
        void ReclaimIBullet(IBullet bullet);
    }
}