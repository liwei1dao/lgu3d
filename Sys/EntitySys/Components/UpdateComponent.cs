using System;

namespace LG.EntitySys
{
    [EnableUpdate]
    public class UpdateComponent : LGEntityComp
    {
        public override bool DefaultEnable { get; set; } = true;

        public override void LGUpdate(float time)
        {
            Entity.LGUpdate(time);
        }
    }
}