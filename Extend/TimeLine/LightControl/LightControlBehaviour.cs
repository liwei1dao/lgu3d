using UnityEngine;
using UnityEngine.Playables;

public class LightControlBehaviour : PlayableBehaviour
{
  //public Light light = null; 不再需要它了
  public Color color = Color.white;
  public float intensity = 1f;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    Light light = playerData as Light; // 这个地方有变化

    if (light != null)
    {
      light.color = color;
      light.intensity = intensity;
    }
  }
}
