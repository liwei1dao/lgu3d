using System;

namespace lgu3d
{

  public enum BuffState
  {
    NoStart,
    TakeIng,
    End,
  }

  /// <summary>
  /// Buff接口定义
  /// </summary>
  public interface IBuffBase<A> where A : Enum
  {
    A BuffType { get; set; }
    BuffDataBase Config { get; set; }
    BuffState BuffState { get; set; }
    IEntityBaseBuffContainerComp<A> Host { get; set; }
    float Progress { get; set; }
    float CurrCd { get; set; }
    /// <summary>
    /// Buff校验
    /// </summary>
    /// <param name="agrs"></param>
    /// <returns></returns>
    bool OnCheck(params object[] agrs);

    /// <summary>
    /// Buff开始
    /// </summary>
    /// <param name="agrs"></param>
    void OnStart(params object[] agrs);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="time"></param>
    void Update(float time);
    /// <summary>
    /// Buff销毁
    /// </summary>
    /// <param name="agrs"></param>
    void OnDestroy();
  }


  public interface IBuffBase<A, D> : IBuffBase<A> where A : Enum where D : BuffDataBase
  {
    new D Config { get; set; }
  }

}