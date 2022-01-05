using System;

namespace lgu3d
{
  public enum SkillState
  {
    NoRelease,
    InRelease,
    InCd,
  }
  public enum BulletState
  {
    WaitLaunch,
    Launching,
    Destroyed,
  }

  public interface ISkillBase
  {
    IEntityBase Entity { get; set; }
    SkillDataBase Config { get; set; }
    SkillState State { get; set; }
    void Init(params object[] agrs);
    void Release(params object[] agrs);                         //技能释放
    void Update(float time);                                    //技能更新
    void ReleaseEnd();                                          //技能释放结束
    IBulletBase AddBullet(IBulletBase bullet);                   //添加子弹
    void RemoveBullet(IBulletBase bullet);                      //移除子弹
  }
  public interface ISkillBase<E, D> : ISkillBase where E : IEntityBase where D : SkillDataBase
  {
    string SkillId { get; set; }
    new E Entity { get; set; }
    new D Config { get; set; }

  }

  public interface IBulletBase
  {
    BulletState State { get; set; }
    SkillBase Skill { get; set; }
    void Destroy();
    void Launch(params object[] agrs);
    void TakeEffect(params object[] agrs);
    void Update(float time);
  }
  public interface IBulletBase<S> : IBulletBase where S : ISkillBase
  {
    new S Skill { get; set; }
  }
}