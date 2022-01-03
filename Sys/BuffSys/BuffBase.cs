using System;

namespace lgu3d
{
  /// <summary>
  /// Buff基类
  /// </summary>
  public abstract class BuffBase<A> : IBuffBase<A> where A : Enum
  {
    public A BuffType { get; set; }
    public BuffState BuffState { get; set; }
    public BuffDataBase Config { get; set; }
    public IEntityBaseBuffContainerComp<A> Host { get; set; }
    public float Progress { get; set; }
    public float CurrCd { get; set; }

    public BuffBase(A buffType, IEntityBaseBuffContainerComp<A> host, BuffDataBase config)
    {
      BuffType = buffType;
      Config = config;
      Host = host;
      BuffState = BuffState.NoStart;
      Progress = 1;
    }

    public virtual bool OnCheck(params object[] agrs)
    {
      return true;
    }

    public virtual void OnStart(params object[] agrs)
    {
      CurrCd = Config.BuffTime;
      BuffState = BuffState.TakeIng;
    }
    public virtual void Update(float time)
    {
      if (BuffState == BuffState.TakeIng)
      {
        CurrCd -= time;
        Progress = CurrCd / Config.BuffTime;
        if (Progress <= 0)
        {
          CurrCd = 0;
          Progress = 0;
          OnDestroy();
        }
      }

    }
    public virtual void OnDestroy()
    {
      BuffState = BuffState.End;
      Host.RemoveBuff(this);
    }
  }
  /// <summary>
  /// Buff基类
  /// </summary>
  public abstract class BuffBase<A, D> : BuffBase<A>, IBuffBase<A, D> where A : Enum where D : BuffDataBase
  {
    public new D Config { get; set; }
    public BuffBase(A buffType, IEntityBaseBuffContainerComp<A> host, D config) : base(buffType, host, config)
    {
      Config = config;
    }
  }
}