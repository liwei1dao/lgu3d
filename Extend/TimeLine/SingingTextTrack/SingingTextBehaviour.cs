using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/*
Data Clip里面的数据
*/
public class SingingTextBehaviour : PlayableBehaviour
{
  public string line;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {

    LineController controller = playerData as LineController;
    float progress = (float)(playable.GetTime() / playable.GetDuration());
    controller.TextLineOnUpDate(line, progress);
  }

}
