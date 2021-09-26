namespace lgu3d
{
  public interface ILoadViewComp : ViewComp, IModelCompBase
  {
    void UpdateProgress(float progress, string describe);
  }
}