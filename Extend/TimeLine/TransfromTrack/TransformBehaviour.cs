using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TransformBehaviour : PlayableBehaviour
{
  public Transform endLocation;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    Transform actor = playerData as Transform;
    actor.position = Vector3.Lerp(actor.position, endLocation.position, info.deltaTime * 0.5f);
  }

}
