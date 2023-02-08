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
        CP AddComp<CP>(params object[] agrs) where CP : IEntityCompBase;

        CP GetComp<CP>() where CP : IEntityCompBase;

        void RemoveComp(IEntityCompBase comp);
        CoroutineTask StartCoroutine(IEnumerator routine);
        void Destroy();
    }
    public interface IEntityBase<E> : IEntityBase where E : IEntityBase<E>
    {
        new E Entity { get; set; }
        void Init(E entity);
    }
    public interface IEntityBase<E, D> : IEntityBase where E : IEntityBase<E, D> where D : EntityDataBase
    {
        D Config { get; set; }
        new E Entity { get; set; }
        void Init(E entity, D config);
    }


    public interface IEntityCompBase
    {
        IEntityBase Entity { get; set; }
        void Init(IEntityBase entity, params object[] agrs);
        void Start();
        void Destroy();
    }
    public interface IEntityCompBase<E> : IEntityCompBase where E : IEntityBase
    {
        new E Entity { get; set; }
        void Init(E entity, params object[] agrs);
    }
}