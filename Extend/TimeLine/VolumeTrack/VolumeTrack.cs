using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

[TrackClipType(typeof(VolumeAsset))]
[TrackBindingType(typeof(Volume))]
public class VolumeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<VolumeMixerBehaviour>.Create(graph, inputCount);
    }
}
