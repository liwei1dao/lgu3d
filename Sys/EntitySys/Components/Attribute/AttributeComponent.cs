using System.Collections.Generic;

namespace LG.EntitySys
{
    public class AttributeUpdateEvent { public FloatNumeric Numeric; }

    /// <summary>
    /// 战斗属性数值组件，在这里管理角色所有战斗属性数值的存储、变更、刷新等
    /// </summary>
    public class AttributeComponent : LGEntityComp
    {
        private Dictionary<string, float> initAttribute;
        private readonly Dictionary<string, FloatNumeric> attributeNameNumerics = new();
        private readonly AttributeUpdateEvent attributeUpdateEvent = new();

        public override void LGInit(LGEntity entity, params object[] agrs)
        {
            base.LGInit(entity, agrs);
            initAttribute = agrs[0] as Dictionary<string, float>;
            Initialize();
        }
        public void Initialize()
        {
            foreach (var item in initAttribute)
            {
                this.AddNumeric(item.Key, item.Value);
            }
        }

        public FloatNumeric AddNumeric(string attributeType, float baseValue)
        {
            var numeric = Entity.AddChild<FloatNumeric>();
            numeric.Name = attributeType;
            numeric.SetBase(baseValue);
            attributeNameNumerics.Add(attributeType, numeric);
            return numeric;
        }

        public FloatNumeric GetNumeric(string attributeName)
        {
            return attributeNameNumerics[attributeName];
        }

        public void OnNumericUpdate(FloatNumeric numeric)
        {
            attributeUpdateEvent.Numeric = numeric;
            Entity.Publish(attributeUpdateEvent);
        }
    }
}