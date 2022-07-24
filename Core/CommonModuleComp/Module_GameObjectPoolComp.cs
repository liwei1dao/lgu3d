using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace lgu3d
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class Module_GameObjectPoolComp : ModelCompBase
    {
        /// <summary>
        /// 队列对象池
        /// </summary>
        public class GameObjectPoolByQueue
        {
            public Func<UnityEngine.Object> CreateFun;
            public Queue<UnityEngine.Object> Pool;
            public GameObjectPoolByQueue(Func<UnityEngine.Object> cf)
            {
                CreateFun = cf;
                Pool = new Queue<UnityEngine.Object>();
            }

            public UnityEngine.Object Get()
            {
                if (Pool.Count > 0)
                {
                    return Pool.Dequeue();
                }
                else
                {
                    return CreateFun();
                }
            }

            public void Push(UnityEngine.Object obj)
            {
                Pool.Enqueue(obj);
            }

        }

        /// <summary>
        /// 字典对象池
        /// </summary>
        public class GameObjectPoolByDictionary
        {
            public Func<string, UnityEngine.Object> CreateFun;
            public Dictionary<string, Queue<UnityEngine.Object>> Pool;
            public GameObjectPoolByDictionary(Func<string, UnityEngine.Object> cf)
            {
                CreateFun = cf;
                Pool = new Dictionary<string, Queue<UnityEngine.Object>>();
            }

            public UnityEngine.Object Get(string key)
            {
                if (Pool.ContainsKey(key) && Pool[key].Count > 0)
                {
                    return Pool[key].Dequeue();
                }
                else
                {
                    UnityEngine.Object obj = CreateFun(key);
                    return obj;
                }
            }

            public void Push(string key, UnityEngine.Object obj)
            {
                if (!Pool.ContainsKey(key))
                {
                    Pool[key] = new Queue<UnityEngine.Object>();
                }
                Pool[key].Enqueue(obj);
            }
        }

        protected Dictionary<string, GameObjectPoolByQueue> qpools = new Dictionary<string, GameObjectPoolByQueue>();
        protected Dictionary<string, GameObjectPoolByDictionary> dpools = new Dictionary<string, GameObjectPoolByDictionary>();
        public override void Load(ModelBase module, params object[] agr)
        {
            base.Load(module, agr);
            LoadEnd();
        }

        public override void Close()
        {
            base.Close();
            qpools.Clear();
        }

        #region QueueObjectPool
        public virtual void RegisterQueuePool<T>(string poolname, Func<T> cf) where T : UnityEngine.Object
        {
            if (qpools.ContainsKey(poolname))
            {
                Debug.LogError("重复注册对象池:" + poolname);
            }
            else
            {
                GameObjectPoolByQueue pool = new GameObjectPoolByQueue(cf);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="cf"></param>
        public virtual void RegisterDictionaryPool<T>(string poolname, Func<string, T> cf) where T : UnityEngine.Object
        {
            if (dpools.ContainsKey(poolname))
            {
                Debug.LogError("重复注册对象池:" + poolname);
            }
            else
            {
                GameObjectPoolByDictionary pool = new GameObjectPoolByDictionary(cf);
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