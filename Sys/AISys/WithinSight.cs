using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace lgu3d
{
  public class WithinSight : Conditional
  {
    public float fieldViewAnagle;
    public string targerTag;
    public SharedTransform target;
    private Transform[] possibleTargets;

    public override void OnAwake()
    {
      base.OnAwake();
      var targets = GameObject.FindGameObjectsWithTag(targerTag);
      possibleTargets = new Transform[targets.Length];
      for (var i = 0; i < targets.Length; i++)
      {
        possibleTargets[i] = targets[i].transform;
      }
    }

    public override TaskStatus OnUpdate()
    {
      for (var i = 0; i < possibleTargets.Length - 1; i++)
      {
        if (withinSight(possibleTargets[i], fieldViewAnagle))
        {
          target.Value = possibleTargets[i];
          return TaskStatus.Success;
        }
      }
      return TaskStatus.Failure;
    }

    public bool withinSight(Transform target, float fieldViewAnagle)
    {
      Vector3 vect = target.position - transform.position;
      return Vector3.Angle(vect, transform.forward) < fieldViewAnagle;
    }
  }

}