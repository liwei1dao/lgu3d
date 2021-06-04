using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/*
动画片段
*/
public class SingingTextClip : PlayableAsset, ITimelineClipAsset
{
  private SingingTextBehaviour template = new SingingTextBehaviour();
  public string line;
  public ClipCaps clipCaps => ClipCaps.None;
  public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
  {
    var playable = ScriptPlayable<SingingTextBehaviour>.Create(graph, template);
    SingingTextBehaviour clone = playable.GetBehaviour();
    clone.line = this.line;
    return playable;
  }
}
