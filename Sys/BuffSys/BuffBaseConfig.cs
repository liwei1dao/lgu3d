namespace lgu3d
{

  [System.Serializable]
  public class BuffDataBase : ConfigDataBase<int>
  {
    public string BuffName;                             //Buff名称
    public float BuffTime;                              //Buff时长
  }

  public class BuffTableDataBase<A> : ConfigTableDataBase<int, A> where A : BuffDataBase
  {

  }
}