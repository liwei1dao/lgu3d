namespace lgu3d
{

    [System.Serializable]
    public class SkillDataBase : ConfigDataBase<int>
    {
        public string SkillName;                            //技能名称
        public float SkillReleaseTime;                      //技能释放时间
        public float CdTime;                                //Cd时间(技能释放完毕才开始Cd)
    }

    public class SkillTableDataBase<A> : ConfigTableDataBase<int, A> where A : SkillDataBase
    {

    }
}
