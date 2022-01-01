namespace lgu3d
{
  [System.Serializable]
  public abstract class EntityDataBase : ConfigDataBase<int>
  {
    public string EntityName;//角色名称
  }

  public class EntityTableDataBase<A> : ConfigTableDataBase<int, A> where A : EntityDataBase
  {

  }
}