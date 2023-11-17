using UnityEngine;
using System.Collections.Generic;
using System;

namespace lgu3d
{
    /// <summary>
    /// 对象池管理组件
    /// </summary>
    public class ObjectPoolsComp : ModelCompBase<ObjectPoolModule>
    {

        protected Dictionary<string, GameObjectPoolByQueue> qpools;
        protected Dictionary<string, GameObjectPoolByDictionary> dpools;
        public override void Load(ModuleBase module, params object[] agr)
        {
            base.Load(module, agr);
            qpools = new Dictionary<string, GameObjectPoolByQueue>();
            dpools = new Dictionary<string, GameObjectPoolByDictionary>();
            LoadEnd();
        }


        /// <summary>
        /// 卸载对象池
        /// </summary>
        public void UnloadPools()
        {
            foreach (var pool in qpools)
            {
                pool.Value.Release();
            }
            foreach (var pool in dpools)
            {
                pool.Value.Release();
            }
        }
        /// <summary>
        /// 卸载对象池
        /// </summary>
        public void UnloadPools(string pool)
        {
            if (qpools.ContainsKey(pool))
            {
                qpools[pool].Release();
            }
            if (dpools.ContainsKey(pool))
            {
                dpools[pool].Release();
            }
        }
        #region QueueObjectPool
        public virtual void RegisterQueuePool(string poolname, Func<GameObject, UnityEngine.Object> cf)
        {
            if (qpools.ContainsKey(poolname))
            {
                Debug.LogError("重复注册对象池:" + poolname);
            }
            else
            {
                GameObjectPoolByQueue pool = new(poolname, cf);
                qpools.Add(poolname, pool);
            }
        }

        public virtual T GetByQueuePool<T>(string poolname) where T : UnityEngine.Object
        {
            if (qpools.ContainsKey(poolname))
            {
                return qpools[poolname].Get() as T;
            }
            else
            {
                Debug.LogError("对象池没有注册:" + poolname);
                return null;
            }
        }

        public virtual void PushByQueuePool<T>(string poolname, T obj) where T : UnityEngine.Object
        {
            if (qpools.ContainsKey(poolname))
            {
                qpools[poolname].Push(obj);
            }
            else
            {
                Debug.LogError("对象池没有注册:" + poolname);
            }
        }
        #endregion

        #region DictionaryObjectPool
        /// <summary>
        /// 注册队列对象池
        /// </summary>
        /// <param name="cf"></param>
        public virtual void RegisterDictionaryPool<T>(string poolname, Func<string, GameObject, T> cf) where T : UnityEngine.Object
        {
            if (dpools.ContainsKey(poolname))
            {
                Debug.LogError("重复注册对象池:" + poolname);
            }
            else
            {
                GameObjectPoolByDictionary pool = new(poolname, cf);
                dpools.Add(poolname, pool);
            }
        }

        public virtual T GetByDictionaryPool<T>(string poolname, string key) where T : UnityEngine.Object
        {
            if (dpools.ContainsKey(poolname))
            {
                return dpools[poolname].Get(key) as T;
            }
            else
            {
                Debug.LogError("对象池没有注册:" + poolname);
                return null;
            }
        }

        public virtual void PushByDictionaryPool<T>(string poolname, string key, T obj) where T : UnityEngine.Object
        {
            if (dpools.ContainsKey(poolname))
            {
                dpools[poolname].Push(key, obj);
            }
            else
            {
                Debug.LogError("对象池没有注册:" + poolname);
            }
        }
        #endregion
    }

}