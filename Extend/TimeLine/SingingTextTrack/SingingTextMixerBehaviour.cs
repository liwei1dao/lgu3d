using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/*
主要处理clip 过度逻辑 以及 轨道空白处显示逻辑
*/
public class SingingTextMixerBehaviour : PlayableBehaviour
{
  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    LineController controller = playerData as LineController;
    int inputCount = playable.GetInputCount();
    bool isEmptyClip = true;
    for (int i = 0; i < inputCount; i++)
    {
      float inputWeight = playable.GetInputWeight(i);
      if (inputWeight > 0)
      {
        isEmptyClip = false;
      }
    }
    if (isEmptyClip)
    {
      controller.TextLineOnUpDate("", 0);
    }
  }
}
