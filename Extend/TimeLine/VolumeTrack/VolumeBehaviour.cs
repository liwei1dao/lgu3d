using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeBehaviour : PlayableBehaviour
{
    public float lift = 1f;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Volume volume = playerData as Volume;
        if (volume != null)
        {
            foreach (var item in volume.profile.components)
            {
                Debug.Log(item.ToString());
                if (item is LiftGammaGain)
                {
                    LiftGammaGain liftGammaGain = item as LiftGammaGain;
                    liftGammaGain.lift.SetValue(new Vector4Parameter(new Vector4(1, 1, 1, lift)));
                }
            }
        }
    }
}
