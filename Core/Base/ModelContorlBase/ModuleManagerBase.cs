using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 模块管理器基类
 */
namespace lgu3d
{
  public abstract class ModuleManagerBase : MonoBehaviour
  {
    protected Dictionary<string, ModuleBase> modules;
    private bool Lock = false;                              //models 操作锁
    private List<ModuleBase> AddTmpmodels; //防止models循环中操作队列导致报错
    private List<string> RemoveTmpmodels; //防止models循环中操作队列导致报错
    public Dictionary<string, ModuleBase> Models
    {
      get { return modules; }
    }

    protected virtual void Init()
    {
      modules = new Dictionary<string, ModuleBase>();
      AddTmpmodels = new List<ModuleBase>();
      RemoveTmpmodels = new List<string>();
    }

    public virtual bool IsKeepModule(string moduleName)
    {

      return modules.ContainsKey(moduleName);
    }

    protected void StartModule(string moduleName, ModuleBase _Model)
    {
      if (!Lock)
      {
        modules[moduleName] = _Model;
      }
      else
      {
        AddTmpmodels.Add(_Model);
      }
    }

    protected void CloseModule(string moduleName)
    {
      if (!Lock)
      {
        modules.Remove(moduleName);
      }
      else
      {
        RemoveTmpmodels.Add(moduleName);
      }
    }

    protected void Update()
    {
      if (RemoveTmpmodels.Count > 0)
      {
        foreach (var module in RemoveTmpmodels)
        {
          Models.Remove(module);
        }
        RemoveTmpmodels.Clear();
      }
      if (AddTmpmodels.Count > 0)
      {
        foreach (var module in AddTmpmodels)
        {
          Models.Add(module.ModuleName, module);
        }
        AddTmpmodels.Clear();
      }
      Lock = true;
      foreach (var module in Models)
      {
        if (module.Value is IUpdataMode && module.Value.State == ModelBaseState.Start)
        {
          ((IUpdataMode)module.Value).Update(Time.deltaTime);
        }
      }
      Lock = false;
    }

    protected void FixedUpdate()
    {
      if (RemoveTmpmodels.Count > 0)
      {
        foreach (var module in RemoveTmpmodels)
        {
          Models.Remove(module);
        }
        RemoveTmpmodels.Clear();
      }
      if (AddTmpmodels.Count > 0)
      {
        foreach (var module in AddTmpmodels)
        {
          Models.Add(module.ModuleName, module);
        }
        AddTmpmodels.Clear();
      }
      Lock = true;
      foreach (var module in Models)
      {
        if (module.Value is IFixedUpdateMode && module.Value.State == ModelBaseState.Start)
        {
          ((IFixedUpdateMode)module.Value).FixedUpdate();
        }
      }
      Lock = false;
    }

    protected void LateUpdate()
    {
      if (RemoveTmpmodels.Count > 0)
      {
        foreach (var module in RemoveTmpmodels)
        {
          Models.Remove(module);
        }
        RemoveTmpmodels.Clear();
      }
      if (AddTmpmodels.Count > 0)
      {
        foreach (var module in AddTmpmodels)
        {
          Models.Add(module.ModuleName, module);
        }
        AddTmpmodels.Clear();
      }
      Lock = true;
      foreach (var module in Models)
      {
        if (module.Value is ILateUpdateModule && module.Value.State == ModelBaseState.Start)
        {
          ((ILateUpdateModule)module.Value).LateUpdate();
        }
      }
      Lock = false;
    }

  }

  public abstract class ManagerBase<M, BaseC> : ModuleManagerBase where M : ManagerBase<M, BaseC> where BaseC : IModule, new()
  {
    protected new Dictionary<string, BaseC> modules;
    #region 单例接口
    private static M _instance = null;
    public static M Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = FindObjectOfType(typeof(M)) as M;
          if (_instance == null)
          {
            GameObject obj = new GameObject(typeof(M).Name, typeof(M));
            _instance = obj.GetComponent<M>() as M;
            _instance.Init();
          }
        }
        return _instance;
      }
    }
    #endregion

    public abstract void StartModule<C>(ModelLoadBackCall<C> BackCall = null, params object[] agrs) where C : BaseC, new();
    public abstract void StartModule<C>(string moduleName, ModelLoadBackCall<C> BackCall = null, params object[] agrs) where C : BaseC, new();
    public abstract void StartModuleByTag<C>(string tag, ModelLoadBackCall<C> BackCall = null, params object[] agrs) where C : BaseC, new();
    public abstract void StartModuleObj(string moduleName, ManagerContorBase Mdule, ModelLoadBackCall<BaseC> BackCall = null, params object[] agrs);
    public abstract void StartModuleForName(string nameSpace, string moduleName, ModelLoadBackCall<BaseC> BackCall = null, params object[] agrs);
    protected abstract IEnumerator ModuleStart<C>(C model, ModelLoadBackCall<C> BackCall, params object[] agrs) where C : BaseC, new();
    public abstract C GetModuleByTag<C>(string mtag) where C : class, IModule;
    public abstract void CloseModule<C>() where C : BaseC, new();
    public abstract void CloseModuleForName(string moduleName);
  }
}