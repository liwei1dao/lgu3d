using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public class SkillModuleFactoryComp: ModelCompBase<SkillModule>
    {
        private Dictionary<int, ISkillFactory> SkillModelFactorys = new Dictionary<int, ISkillFactory>();

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl);
            foreach (var item in SkillModelFactorys)
            {
                 item.Value.Load(MyModule);
            }
            LoadEnd();
        }

        public void RegisterActor(int SkillType, ISkillFactory SkillFactory)
        {
            SkillModelFactorys[SkillType] = SkillFactory;
            if (State > ModelCompBaseState.Close)
            {
                SkillModelFactorys[SkillType].Load(MyModule);
            }
        }


        public S CreateActor<S>(ActorBase Actor, int SkillId) where S : SkillBase
        {
            int SkillType = SkillId / 10000;
            if (SkillModelFactorys.ContainsKey(SkillType))
            {
                return SkillModelFactorys[SkillType].Create(Actor,SkillId) as S;
            }
            else
            {
                Debug.LogError("技能工厂没有注册 = " + SkillId);
                return null;
            }
        }
    }
}
