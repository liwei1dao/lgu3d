using UnityEngine;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 技能模块特效管理组件
    /// </summary>
    public class SkillModuleEffectManagerComp : ModelCompBase<SkillModule>
    {
        private Dictionary<int, GameObject> Effects;
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            Effects = new Dictionary<int, GameObject>();
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        public GameObject LoadEffect(int EffectId)
        {
            if (!Effects.ContainsKey(EffectId))
            {
                Effects[EffectId] = MyModule.LoadAsset<GameObject>("Effects/" + EffectId.ToString(), EffectId.ToString());
            }
            return Effects[EffectId];
        }
        
    }
}
