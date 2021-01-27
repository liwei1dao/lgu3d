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
            Modules = new Dictionary<string, ManagerContorBase>();
            base.Init();
        }
        public override void StartModule<C>(ModelLoadBackCall<C> BackCall = null, params object[] _Agr)
        {
            string ModelName = typeof(C).Name;
            if (!Modules.ContainsKey(ModelName))
            {
                Modules[ModelName] = new C();
                Modules[ModelName].ModuleName = ModelName;
                base.StartModule(ModelName, Modules[ModelName]);
                Modules[ModelName].Load<C>((model)=> {
                    StartCoroutine(ModuleStart<C>(model, BackCall, _Agr));
                }, _Agr);
            }
            else
            {
                Debug.LogError("This Model Already Load:" + typeof(C).Name);
            }
        }
        public override void StartModuleObj(string moduleName,ManagerContorBase Mdule, ModelLoadBackCall<ManagerContorBase> BackCall = null, params object[] _Agr)
        {
            if (!Modules.ContainsKey(moduleName))
            {
                Modules[moduleName] = Mdule;
                Modules[moduleName].ModuleName = moduleName;
                base.StartModule(moduleName, Modules[moduleName]);
                Modules[moduleName].Load<ManagerContorBase>((model) => {
                    StartCoroutine(ModuleStart<ManagerContorBase>(model, BackCall, _Agr));
                }, _Agr);
            }
            else
            {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }

        public override void StartModuleForName(string nameSpace, string moduleName, ModelLoadBackCall<ManagerContorBase> BackCall = null, params object[] _Agr)
        {
            if (!Modules.ContainsKey(moduleName))
            {
                Modules[moduleName] = Assembly.GetExecutingAssembly().CreateInstance(nameSpace == "" ? moduleName : nameSpace + "." + moduleName, true, System.Reflection.BindingFlags.Default, null, null, null, null) as ManagerContorBase;
                if (Modules[moduleName] == null)
                {
                    Debug.LogError("StartModelForName  Error 反射为空  " + nameSpace + "." + moduleName);
                    return;
                }
                Modules[moduleName].ModuleName = moduleName;
                base.StartModule(moduleName, Modules[moduleName]);
                Modules[moduleName].Load<ManagerContorBase>((model) => {
                    StartCoroutine(ModuleStart<ManagerContorBase>(model, BackCall, _Agr));
                }, _Agr);
            }
            else
            {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }
        protected override IEnumerator ModuleStart<C>(C model, ModelLoadBackCall<C> BackCall, params object[] _Agr)
        {
            yield return new WaitForEndOfFrame();
            model.Start(_Agr);
            if (BackCall != null)
                BackCall(model);
        }

        public override void CloseModule<C>()
        {
            string ModelName = typeof(C).Name;
            if (Modules.ContainsKey(ModelName))
            {
                Modules[ModelName].Close();
                Modules.Remove(ModelName);
                base.CloseModule(ModelName);
            }
            else
            {
                Debug.LogWarning("This Model Already Close:" + typeof(C).Name);
            }
        }
        public override void CloseModuleForName(string moduleName)
        {
            if (Modules.ContainsKey(moduleName))
            {
                Modules[moduleName].Close();
                Modules.Remove(moduleName);
                base.CloseModule(moduleName);
            }
            else
            {
                Debug.LogWarning("This Model Already Close:" + moduleName);
            }
        }
    }
}
