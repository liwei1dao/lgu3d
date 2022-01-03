using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 机关触发器
  /// </summary>
  /// <typeparam name="O"></typeparam>
  public abstract class OrganActuatorBase<O> : IOrganActuator<O> where O : IOrgan
  {
    public O Organ { get; set; }

    public OrganActuatorBase(O organ)
    {
      Load(organ);
    }
    public void Load(O organ)
    {
      Organ = organ;
    }

    public virtual void Execute(params object[] agrs)
    {
      Organ.OrganExecute(agrs);
    }
  }

  /// <summary>
  /// 机关触发器
  /// </summary>
  /// <typeparam name="O"></typeparam>
  public abstract class MonoOrganActuatorBase<O> : MonoBehaviour, IOrganActuator<O> where O : IOrgan
  {
    public O Organ { get; set; }
    public virtual void Load(O organ)
    {
      Organ = organ;
    }
    public virtual void Execute(params object[] agrs)
    {
      Organ.OrganExecute(agrs);
    }
  }
}