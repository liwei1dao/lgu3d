namespace lgu3d
{
    [System.Serializable]
    public class ActorDataBase : ConfigDataBase<int>
    {
        public string ActorName;//角色名称
    }

    public class ActorTableDataBase<A> : ConfigTableDataBase<int, A> where A : ActorDataBase
    {

    }
}