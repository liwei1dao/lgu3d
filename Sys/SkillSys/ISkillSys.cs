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
    EntityBase Entity { get; set; }
    SkillDataBase Config { get; set; }
    void Load(EntityBase entity, SkillDataBase config);          //加载
    void Init(params object[] agrs);
    void Release(params object[] agrs);                         //技能释放
    void Update(float time);                                    //技能更新
    void ReleaseEnd();                                          //技能释放结束
  }
  public interface ISkillBase<E, D> : ISkillBase where E : EntityBase where D : SkillDataBase
  {
    new E Entity { get; set; }
    new D Config { get; set; }
    void Load(E entity, D config);
  }

  public interface IBulletBase
  {
    SkillBase Skill { get; set; }
    void Destroy();
    void Launch(params object[] _Agr);
    void Update(float time);
  }
  public interface IBulletBase<S> : IBulletBase where S : ISkillBase
  {
    new S Skill { get; set; }
  }
}