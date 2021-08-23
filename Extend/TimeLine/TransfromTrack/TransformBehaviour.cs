using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TransformBehaviour : PlayableBehaviour
{
  public Transform endLocation;
  public Transform startLocation;
  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    Transform actor = playerData as Transform;
    float progress = (float)(playable.GetTime() / playable.GetDuration());
    actor.position = Vector3.Lerp(startLocation.position, endLocation.position, progress);
    actor.rotation = Quaternion.Lerp(startLocation.rotation, endLocation.rotation, progress);
  }
}
