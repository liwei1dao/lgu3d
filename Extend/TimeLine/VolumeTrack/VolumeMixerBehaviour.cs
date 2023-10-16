using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeMixerBehaviour : PlayableBehaviour
{
    public float lift = 1f;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Volume volume = playerData as Volume;
        int inputCount = playable.GetInputCount();
        lift = 0;
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<VolumeBehaviour> inputPlayable = (ScriptPlayable<VolumeBehaviour>)playable.GetInput(i);
            VolumeBehaviour input = inputPlayable.GetBehaviour();
            lift += inputWeight * input.lift;
        }

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
