using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lgu3d
{
    public interface ISkillFactory
    {
        void Load(SkillModule Model);
        SkillBase Create(ActorBase Actor, int Id);
    }
}
