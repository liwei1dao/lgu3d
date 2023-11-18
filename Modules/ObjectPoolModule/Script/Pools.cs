using UnityEngine;
using System.Collections.Generic;
using System;

namespace lgu3d
{

    /// <summary>
    /// 队列对象池
    /// </summary>
    public class GameObjectPoolByQueue
    {
        public GameObject Root;
        public ModuleBase Module;
        public Func<GameObject, UnityEngine.Object> CreateFun;
        public Queue<UnityEngine.Object> Pool;
        public GameObjectPoolByQueue(string pname, Func<GameObject, UnityEngine.Object> cf)
        {
            Root = ObjectPoolModule.Instance.ObjectPools.CreateChild(pname);
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
                return CreateFun(Root);
            }
        }

        public void Push(UnityEngine.Object obj)
        {
            if (obj is GameObject)
            {
                GameObject gameObject = obj as GameObject;
                gameObject.SetParent(Root);
                gameObject.SetActive(false);
            }
            else if (obj is Component)
            {
                Component component = obj as Component;
                component.gameObject.SetParent(Root);
                component.gameObject.SetActive(false);
            }
            Pool.Enqueue(obj);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            Root.ClearChilds();
            Pool.Clear();
        }

    }

    /// <summary>
    /// 字典对象池
    /// </summary>
    public class GameObjectPoolByDictionary
    {
        public GameObject Root;
        public Func<string, GameObject, UnityEngine.Object> CreateFun;
        public Dictionary<string, Queue<UnityEngine.Object>> Pool;
        public GameObjectPoolByDictionary(string pname, Func<string, GameObject, UnityEngine.Object> cf)
        {
            Root = ObjectPoolModule.Instance.ObjectPools.CreateChild(pname);
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
                UnityEngine.Object obj = CreateFun(key, Root);
                return obj;
            }
        }

        public void Push(string key, UnityEngine.Object obj)
        {
            if (!Pool.ContainsKey(key))
            {
                Pool[key] = new Queue<UnityEngine.Object>();
            }
            if (obj is GameObject)
            {
                GameObject gameObject = obj as GameObject;
                gameObject.SetParent(Root);
                gameObject.SetActive(false);
            }
            else if (obj is Component)
            {
                Component component = obj as Component;
                component.gameObject.SetParent(Root);
                component.gameObject.SetActive(false);
            }
            Pool[key].Enqueue(obj);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            Root.ClearChilds();
            Pool.Clear();
        }
    }

}