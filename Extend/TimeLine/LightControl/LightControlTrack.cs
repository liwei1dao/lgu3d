using UnityEngine;
using UnityEngine.Timeline;

[TrackClipType(typeof(LightControlAsset))]
[TrackBindingType(typeof(Light))]
public class LightControlTrack : TrackAsset { }
