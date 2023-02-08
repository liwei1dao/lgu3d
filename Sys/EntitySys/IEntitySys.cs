using System;
using System.Collections;
using UnityEngine;

namespace lgu3d
{
    public interface IEntityBase
    {
        IEntityBase Entity { get; set; }
        void Init(IEntityBase entity);
        void Start();
        CP AddComp<CP>(CP comp, params object[] agrs) where CP : class, IEntityCompBase;

        CP GetComp<CP>() where CP : class, IEntityCompBase;

        void RemoveComp(IEntityCompBase comp);
        CoroutineTask StartCoroutine(IEnumerator routine);
        void Destroy();
    }


    public interface IEntityCompBase
    {
        IEntityBase Entity { get; set; }
        void Init(IEntityBase entity, params object[] agrs);
        void Start();
        void Destroy();
    }
}