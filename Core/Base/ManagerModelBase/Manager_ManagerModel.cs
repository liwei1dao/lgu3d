using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/*
管理器:管理管理器模块
Init 管理器装配初始化借口
LoadModel 加载对应管理器模块接口
CloseModel 卸载对应管理器模块接口
*/
namespace lgu3d
{
    public class Manager_ManagerModel : ManagerBase<Manager_ManagerModel, ManagerContorBase>
    {
        protected override void Init()
        {
            DontDestroyOnLoad(gameObject);
            modules = new Dictionary<string, ManagerContorBase>();
            base.Init();
        }
        public override void StartModule<C>(ModelLoadBackCall<C> BackCall = null, params object[] agrs)
        {
            string ModelName = typeof(C).Name;
            if (!modules.ContainsKey(ModelName))
            {
                modules[ModelName] = new C
                {
                    ModuleName = ModelName
                };
                base.StartModule(ModelName, modules[ModelName]);
                modules[ModelName].Load<C>((model) =>
                {
                    StartCoroutine(ModuleStart<C>(model, BackCall, agrs));
                }, agrs);
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(C).Name);
            }
        }
        public override void StartModule<C>(string ModelName, ModelLoadBackCall<C> BackCall = null, params object[] agrs)
        {
            if (!modules.ContainsKey(ModelName))
            {
                modules[ModelName] = new C
                {
                    ModuleName = ModelName
                };
                base.StartModule(ModelName, modules[ModelName]);
                modules[ModelName].Load<C>((model) =>
                {
                    StartCoroutine(ModuleStart<C>(model, BackCall, agrs));
                }, agrs);
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(C).Name);
            }
        }
        public override void StartModuleByTag<C>(string tag, ModelLoadBackCall<C> BackCall = null, params object[] agrs)
        {
            string ModelName = typeof(C).Name;
            if (!modules.ContainsKey(ModelName))
            {
                modules[ModelName] = new C
                {
                    ModuleName = ModelName,
                    ModuleTag = tag,
                };
                base.StartModule(ModelName, modules[ModelName]);
                modules[ModelName].Load<C>((model) =>
                {
                    StartCoroutine(ModuleStart<C>(model, BackCall, agrs));
                }, agrs);
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(C).Name);
            }
        }
        public override void StartModuleObj(string moduleName, ManagerContorBase Mdule, ModelLoadBackCall<ManagerContorBase> BackCall = null, params object[] agrs)
        {
            if (!modules.ContainsKey(moduleName))
            {
                modules[moduleName] = Mdule;
                modules[moduleName].ModuleName = moduleName;
                base.StartModule(moduleName, modules[moduleName]);
                modules[moduleName].Load<ManagerContorBase>((model) =>
                {
                    StartCoroutine(ModuleStart<ManagerContorBase>(model, BackCall, agrs));
                }, agrs);
            }
            else
            {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }

        public override void StartModuleForName(string nameSpace, string moduleName, ModelLoadBackCall<ManagerContorBase> BackCall = null, params object[] agrs)
        {
            if (!modules.ContainsKey(moduleName))
            {
                modules[moduleName] = Assembly.GetExecutingAssembly().CreateInstance(nameSpace == "" ? moduleName : nameSpace + "." + moduleName, true, System.Reflection.BindingFlags.Default, null, null, null, null) as ManagerContorBase;
                if (modules[moduleName] == null)
                {
                    Debug.LogError("StartModelForName  Error 反射为空  " + nameSpace + "." + moduleName);
                    return;
                }
                modules[moduleName].ModuleName = moduleName;
                base.StartModule(moduleName, modules[moduleName]);
                modules[moduleName].Load<ManagerContorBase>((model) =>
                {
                    StartCoroutine(ModuleStart<ManagerContorBase>(model, BackCall, agrs));
                }, agrs);
            }
            else
            {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }
        protected override IEnumerator ModuleStart<C>(C model, ModelLoadBackCall<C> BackCall, params object[] agrs)
        {
            yield return new WaitForEndOfFrame();
            model.Start(agrs);
            BackCall?.Invoke(model);
        }

        public override C GetModuleByTag<C>(string mtag)
        {
            foreach (var module in modules)
            {
                if (module.Value.ModuleTag == mtag)
                {
                    return module.Value as C;
                }
            }
            return null;
        }

        public override void CloseModule<C>()
        {
            string ModelName = typeof(C).Name;
            if (modules.ContainsKey(ModelName))
            {
                modules[ModelName].Close();
                modules.Remove(ModelName);
                base.CloseModule(ModelName);
            }
            else
            {
                Debug.LogWarning("This Model Already Close:" + typeof(C).Name);
            }
        }
        public override void CloseModuleForName(string moduleName)
        {
            if (modules.ContainsKey(moduleName))
            {
                modules[moduleName].Close();
                modules.Remove(moduleName);
                base.CloseModule(moduleName);
            }
            else
            {
                Debug.LogWarning("This Model Already Close:" + moduleName);
            }
        }
    }
}
