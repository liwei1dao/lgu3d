using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitySys
{

    /// <summary>
    /// 实体车辆属性管理组件
    /// </summary>
    public class EntityCarAttributeComp : MonoEntityCompBase<EntityCar>
    {
        public float hopHeight { get; }
    }
}
