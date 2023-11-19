using System;
using UnityEngine;


namespace lgu3d
{
    /// <summary>
    /// 对象池模块
    /// </summary>
    public class ObjectPoolModule : ManagerContorBase<ObjectPoolModule>
    {
        public GameObject ObjectPools { get; private set; }
        public ObjectPoolsComp objectPoolsComp;
        public override void Load(params object[] _Agr)
        {
            ObjectPools = new GameObject("ObjectPools");
            UnityEngine.Object.DontDestroyOnLoad(ObjectPools);
            objectPoolsComp = AddComp<ObjectPoolsComp>();
            base.Load(_Agr);
        }

        public void NewObjectPool<T>(string poolname, Func<GameObject, T> cf) where T : UnityEngine.Object
        {
            objectPoolsComp.RegisterQueuePool(poolname, cf);
        }

        public void NewObjectPool<T>(string poolname, Func<string, GameObject, T> cf) where T : UnityEngine.Object
        {
            objectPoolsComp.RegisterDictionaryPool(poolname, cf);
        }

        public T Get<T>(string poolname) where T : UnityEngine.Object
        {
            return objectPoolsComp.GetByQueuePool<T>(poolname);
        }

        public T Get<T>(string poolname, string key) where T : UnityEngine.Object
        {
            return objectPoolsComp.GetByDictionaryPool<T>(poolname, key);
        }

        public void Push<T>(string poolname, T obj) where T : UnityEngine.Object
        {
            objectPoolsComp.PushByQueuePool<T>(poolname, obj);
        }
        public void Push<T>(string poolname, string key, T obj) where T : UnityEngine.Object
        {
            objectPoolsComp.PushByDictionaryPool<T>(poolname, key, obj);
        }
    }
}