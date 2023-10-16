using UnityEngine;
using UnityEngine.Playables;

public class VolumeAsset : PlayableAsset
{
    public float lift = 1f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<VolumeBehaviour>.Create(graph);
        var volumeBehaviour = playable.GetBehaviour();



        volumeBehaviour.lift = lift;
        return playable;
    }
}
