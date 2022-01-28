using System.Collections;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 机关
  /// </summary>
  public abstract class OrganBase : EntityBase, IOrgan
  {
    public OrganState State { get; set; }
    public virtual void OrganStart(params object[] agrs)
    {
      State = OrganState.Runing;
    }
    public virtual void OrganTrigger(params object[] agrs) { }
    public virtual void OrganExecute(params object[] agrs) { }
    public virtual void OrganClose()
    {
      State = OrganState.NoStart;
    }
    public virtual void OrganDestroy()
    {
      State = OrganState.Destroyed;
    }
    public virtual void OrganReset() { }
  }
  /// <summary>
  /// 带触发器的机关
  /// </summary>
  /// <typeparam name="O"></typeparam>
  /// <typeparam name="T"></typeparam>
  public abstract class OrganBaseByTrigger<O, T> : OrganBase, IOrganByTrigger<O, T> where O : OrganBaseByTrigger<O, T> where T : IOrganTrigger<O>
  {
    public T Trigger { get; set; }
    public OrganBaseByTrigger(O organ, T trigger)
    {
      Load(organ, trigger);
    }

    public virtual void Load(O organ, T trigger)
    {
      Trigger = trigger;
      Trigger.Load(organ);
    }
  }

  /// <summary>
  /// 带触发器的机关
  /// </summary>
  /// <typeparam name="O"></typeparam>
  /// <typeparam name="T"></typeparam>
  public abstract class OrganBaseByActuator<O, A> : OrganBase, IOrganByActuator<O, A> where O : OrganBaseByActuator<O, A> where A : IOrganActuator<O>
  {
    public A Actuator { get; set; }
    public OrganBaseByActuator(O organ, A actuator)
    {
      Load(organ, actuator);
    }

    public virtual void Load(O organ, A actuator)
    {
      Actuator = actuator;
      Actuator.Load(organ);
    }
  }

  /// <summary>
  /// 带触发器与执行器的机关
  /// </summary>
  /// <typeparam name="O"></typeparam>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="A"></typeparam>
  public abstract class OrganBaseByTriggerAndActuator<O, T, A> : OrganBase, IOrganByTriggerAndActuator<O, T, A> where O : IOrganByTriggerAndActuator<O, T, A> where T : IOrganTrigger<O> where A : IOrganActuator<O>
  {
    public T Trigger { get; set; }
    public A Actuator { get; set; }
    public OrganBaseByTriggerAndActuator(O organ, T trigger, A actuator)
    {
      Load(organ, trigger, actuator);
    }

    public virtual void Load(O organ, T trigger, A actuator)
    {
      Trigger = trigger;
      Actuator = actuator;
      Trigger.Load(organ);
      Actuator.Load(organ);
    }
  }

  public abstract class MonoOrganBase : MonoEntityBase, IOrgan
  {
    public OrganState State { get; set; }
    public virtual void OrganStart(params object[] agrs)
    {
      State = OrganState.Runing;
    }
    public virtual void OrganTrigger(params object[] agrs) { }
    public virtual void OrganExecute(params object[] agrs) { }
    public virtual void OrganClose()
    {
      State = OrganState.NoStart;
    }
    public virtual void OrganDestroy()
    {
      State = OrganState.Destroyed;
    }
    public virtual void OrganReset()
    {
      State = OrganState.NoStart;
    }
  }

  public abstract class MonoOrganBaseByTrigger<O, T> : MonoOrganBase, IOrganByTrigger<O, T> where O : MonoOrganBaseByTrigger<O, T> where T : IOrganTrigger<O>
  {
    public T Trigger { get; set; }

    public virtual void Load(O organ, T trigger)
    {
      Trigger = trigger;
      Trigger.Load(organ);
    }
    public override void OrganStart(params object[] agrs)
    {
      Trigger.TriggerStart();
    }
    public override void OrganClose()
    {
      Trigger.TriggerClose();
    }
    public override void OrganDestroy()
    {
      Trigger.TriggerDestroy();
    }
  }

  public abstract class MonoOrganBaseByActuator<O, A> : MonoOrganBase, IOrganByActuator<O, A> where O : MonoOrganBaseByActuator<O, A> where A : IOrganActuator<O>
  {
    public A Actuator { get; set; }

    public void Load(O organ, A actuator)
    {
      Actuator = actuator;
      Actuator.Load(organ);
    }
  }

  /// <summary>
  /// 带触发器的机关
  /// </summary>
  /// <typeparam name="O"></typeparam>
  /// <typeparam name="T"></typeparam>
  public abstract class MonoOrganBaseByTriggerAndActuator<O, T, A> : MonoOrganBase, IOrganByTriggerAndActuator<O, T, A> where O : OrganBaseByTriggerAndActuator<O, T, A> where T : IOrganTrigger<O> where A : IOrganActuator<O>
  {
    public T Trigger { get; set; }
    public A Actuator { get; set; }
    public virtual void Load(O organ, T trigger, A actuator)
    {
      Trigger = trigger;
      Actuator = actuator;
      Trigger.Load(organ);
      Actuator.Load(organ);
    }
  }
}