using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/*
动画片段
*/
public class TransformClip : PlayableAsset, ITimelineClipAsset
{
  private TransformBehaviour template = new TransformBehaviour();
  public ExposedReference<Transform> startLocation;
  public ExposedReference<Transform> endLocation;

  public ClipCaps clipCaps => ClipCaps.None;
  public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
  {
    var playable = ScriptPlayable<TransformBehaviour>.Create(graph, template);
    TransformBehaviour clone = playable.GetBehaviour();
    clone.startLocation = startLocation.Resolve(graph.GetResolver());
    clone.endLocation = endLocation.Resolve(graph.GetResolver());
    return playable;
  }
}
