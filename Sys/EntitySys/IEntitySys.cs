using System;
using System.Collections;
using UnityEngine;

namespace lgu3d
{
    public interface IEntityBase
    {
        IEntityBase Entity { get; set; }
        EntityState State { get; set; }
        void LGInit(IEntityBase entity);
        void LGStart();
        CP LGAddComp<CP>(CP comp, params object[] agrs) where CP : class, IEntityCompBase;

        CP LGGetComp<CP>() where CP : class, IEntityCompBase;

        void LGRemoveComp(IEntityCompBase comp);
        CoroutineTask LGStartCoroutine(IEnumerator routine);
        void LGDestroy();
    }


    public interface IEntityCompBase
    {
        IEntityBase Entity { get; set; }
        void LGInit(IEntityBase entity, params object[] agrs);
        void LGStart();
        void LGDestroy();
    }
}