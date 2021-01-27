namespace lgu3d
{
    /// <summary>
    /// 技能承受接口
    /// </summary>
    public interface ISkillAccept
    {
        void Accept(BulletBase Bullet);
    }

    public class AtcorBaseSkillAcceptComp : ActorCompBase, ISkillAccept
    {
        public virtual void Accept(BulletBase Bullet)
        {
            
        }
    }

    /// <summary>
    /// 技能承受组件基类
    /// </summary>
    public class AtcorBaseSkillAcceptComp<A>: AtcorBaseSkillAcceptComp where A : ActorBase
    {
        protected new A Actor;
        public override void Load(ActorBase _Actor, params object[] _Agr)
        {
            base.Load(_Actor, _Agr);
            Actor = _Actor as A;
        }

        public override void Accept(BulletBase Bullet)
        {
            base.Accept(Bullet);
        }
    }
}
