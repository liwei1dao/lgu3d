using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 机关触发器
  /// </summary>
  /// <typeparam name="O"></typeparam>
  public abstract class OrganTrigger<O> : IOrganTrigger<O> where O : IOrgan
  {
    public O Organ { get; set; }

    public OrganTrigger(O organ)
    {
      Load(organ);
    }
    public void Load(O organ)
    {
      Organ = organ;
    }

    public virtual void Trigger(params object[] agrs)
    {
      Organ.OrganTrigger(agrs);
    }

    public virtual void TriggerStart(params object[] agrs)
    {

    }

    public virtual void TriggerClose()
    {

    }
  }

  /// <summary>
  /// 机关触发器
  /// </summary>
  /// <typeparam name="O"></typeparam>
  public abstract class MonoOrganTrigger<O> : MonoBehaviour, IOrganTrigger<O> where O : IOrgan
  {
    public O Organ { get; set; }
    public virtual void Load(O organ)
    {
      Organ = organ;
    }
    public virtual void Trigger(params object[] agrs)
    {
      Organ.OrganTrigger(agrs);
    }

    public virtual void TriggerClose()
    {

    }

    public virtual void TriggerStart(params object[] agrs)
    {

    }
  }
}