using System;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 技能Id的规范 (0010001) 高3位技能类型 低四位技能编号
    /// 技能 子弹的发射器
    /// 子弹 运输技能效果的对象
    /// </summary>
    public class SkillModule : ManagerContorBase<SkillModule>
    {
        private SkillModuleFactoryComp SkillFactoryComp;
        private SkillModuleEffectManagerComp EffectManagerComp;

        public override void Load(params object[] _Agr)
        {
            ResourceComp = AddComp<Module_ResourceComp>();
            SkillFactoryComp = AddComp<SkillModuleFactoryComp>();
            EffectManagerComp = AddComp<SkillModuleEffectManagerComp>();
            base.Load(_Agr);
        }

        public void RegisterSkill(int SkillType, ISkillFactory SkillFactory)
        {
            SkillFactoryComp.RegisterActor(SkillType, SkillFactory);
        }

        public S CreateSkill<S>(ActorBase Actor, int ActorId) where S : SkillBase
        {
            return SkillFactoryComp.CreateActor<S>(Actor, ActorId);
        }

        public GameObject CreateEffect(int EffectId,GameObject Parent)
        {
            return GameObjectExtend.CreateToParnt(EffectManagerComp.LoadEffect(EffectId), Parent);;
        }
    }
}
