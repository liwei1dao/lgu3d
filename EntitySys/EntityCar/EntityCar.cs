using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySys {
    public interface IEntityCarInput {
        /// <summary>
        /// 用于确定卡丁车是否应该提高前进速度。
        /// </summary>
        float Acceleration { get; }

        /// <summary>
        /// 用来决定卡丁车什么时候该跳。也用于引发漂移。
        /// </summary>
        bool HopPressed { get; }
    }

    public interface IEntityCarAttributeComp
    {
        /// <summary>
        /// 跳车时给卡丁车的速度。
        /// </summary>
        float hopHeight { get; }

    }

    public class EntityCar : MonoEntityBase<EntityCarConifg>
    {
        public IEntityCarInput InputComp;
        public IEntityCarAttributeComp AttributeComp;

        public override void AddComp(IEntityComp comp)
        {
            base.AddComp(comp);
            if (comp is IEntityCarInput)
            {
                InputComp = comp as IEntityCarInput;
            }
            else if (comp is IEntityCarAttributeComp) {
                AttributeComp = comp as IEntityCarAttributeComp;
            }
        }

    }
}
