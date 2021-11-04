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
  public class Module_GameObjectPool : ModelCompBase
  {
    public class GameObjectPool
    {
      public Func<UnityEngine.Object> CreateFun;
      public Queue<UnityEngine.Object> Pool;
      public GameObjectPool(Func<UnityEngine.Object> cf)
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

    protected Dictionary<string, GameObjectPool> pools;
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      pools = new Dictionary<string, GameObjectPool>();
      base.Load(_ModelContorl);
    }


    public virtual void RegisterPool(string poolname, Func<GameObject> cf)
    {
      if (pools.ContainsKey(poolname))
      {
        Debug.LogError("重复注册对象池:" + poolname);
      }
      else
      {
        GameObjectPool pool = new GameObjectPool(cf);
        pools.Add(poolname, pool);
      }
    }

    /// <summary>
    /// 注册对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cf"></param>
    public virtual void RegisterPool<T>(string poolname, Func<T> cf) where T : UnityEngine.Object
    {
      if (pools.ContainsKey(poolname))
      {
        Debug.LogError("重复注册对象池:" + poolname);
      }
      else
      {
        GameObjectPool pool = new GameObjectPool(cf);
        pools.Add(poolname, pool);
      }
    }


    public virtual GameObject Get(string poolname)
    {
      if (pools.ContainsKey(poolname))
      {
        return pools[poolname].Get() as GameObject;
      }
      else
      {
        Debug.LogError("对象池没有注册:" + poolname);
        return null;
      }
    }
    public virtual T Get<T>(string poolname) where T : MonoBehaviour
    {
      if (pools.ContainsKey(poolname))
      {
        return pools[poolname].Get() as T;
      }
      else
      {
        Debug.LogError("对象池没有注册:" + poolname);
        return null;
      }
    }

    public virtual void Push(string poolname, GameObject obj)
    {
      if (pools.ContainsKey(poolname))
      {
        pools[poolname].Push(obj);
      }
      else
      {
        Debug.LogError("对象池没有注册:" + poolname);
      }
    }

    public virtual void Push<T>(string poolname, T obj) where T : MonoBehaviour
    {
      if (pools.ContainsKey(poolname))
      {
        pools[poolname].Push(obj);
      }
      else
      {
        Debug.LogError("对象池没有注册:" + poolname);
      }
    }
  }
}