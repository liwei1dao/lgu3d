using lgu3d;

namespace lgu3d
{
  /// <summary>
  /// 机关系统
  /// </summary>
  public interface IOrgan : IEntityBase
  {
    /// <summary>
    /// 启动
    /// </summary>
    /// <param name="agrs"></param>
    void OrganStart(params object[] agrs);

    /// <summary>
    /// 触发
    /// </summary>
    /// <param name="agrs"></param>
    void OrganTrigger(params object[] agrs);
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="agrs"></param>
    void OrganExecute(params object[] agrs);

    /// <summary>
    /// 关闭
    /// </summary>
    void OrganClose();
    /// <summary>
    /// 重置
    /// </summary>
    void OrganReset();
    /// <summary>
    /// 销毁
    /// </summary>
    void OrganDestroy();
  }

  public interface IOrganByTrigger<O, T> : IOrgan where O : IOrganByTrigger<O, T> where T : IOrganTrigger<O>
  {
    T Trigger { get; set; }
    void Load(O organ, T trigger);
  }
  public interface IOrganByActuator<O, A> : IOrgan where O : IOrganByActuator<O, A> where A : IOrganActuator<O>
  {
    A Actuator { get; set; }
    void Load(O organ, A Actuator);
  }

  public interface IOrganByTriggerAndActuator<O, T, A> : IOrgan where O : IOrganByTriggerAndActuator<O, T, A> where T : IOrganTrigger<O> where A : IOrganActuator<O>
  {
    T Trigger { get; set; }
    A Actuator { get; set; }
    void Load(O organ, T trigger, A actuator);
  }

  /// <summary>
  /// 机关触发器
  /// </summary>
  public interface IOrganTrigger<O> where O : IOrgan
  {
    O Organ { get; set; }
    void Load(O organ);
    void TriggerStart(params object[] agrs);
    void TriggerClose();
    void Trigger(params object[] agrs);
    void TriggerDestroy();
  }
  /// <summary>
  /// 机关执行器
  /// </summary>
  public interface IOrganActuator<O> where O : IOrgan
  {
    O Organ { get; set; }
    void Load(O organ);
    void Execute(params object[] agrs);
  }

}