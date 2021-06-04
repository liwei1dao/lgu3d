using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/*
轨道
*/
[TrackColor(255 / 255f, 255 / 255f, 255 / 255f)]
[TrackClipType(typeof(SingingTextClip))]
[TrackBindingType(typeof(LineController))]
public class SingingTextTrack : TrackAsset
{
  public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
  {
    return ScriptPlayable<SingingTextMixerBehaviour>.Create(graph, inputCount);
  }
}
