namespace lgu3d
{
  public enum SkillState
  {
    NoRelease,
    InRelease,
    InCd,
  }


  public interface ISkillBase
  {
    void Load(EntityBase entity, SkillDataBase config);          //加载
    void Init(params object[] agrs);
    void Release(params object[] agrs);                         //技能释放
    void Update(float time);                                    //技能更新
    void ReleaseEnd();                                          //技能释放结束
  }
}