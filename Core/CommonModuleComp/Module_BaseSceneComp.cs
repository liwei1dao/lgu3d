using System.Collections;

namespace lgu3d
{
  public abstract class Module_BaseSceneComp<C> : ModelCompBase<C>, ISceneLoadCompBase where C : ModelBase, new()
  {
    protected float Process;

    public float GetProcess()
    {
      return Process;
    }

    public abstract string GetSceneName();

    public abstract IEnumerator LoadScene();

    public abstract IEnumerator UnloadScene();
  }
}
