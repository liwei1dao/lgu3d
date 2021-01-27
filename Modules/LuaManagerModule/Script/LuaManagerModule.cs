using System.IO;
using LuaInterface;
using UnityEngine;
using System;

namespace lgu3d
{
    public class LuaManagerModule : ManagerContorBase<LuaManagerModule>
    {
        protected LuaState lua;
        private LuaManagerFileComp FileComp;

        
        
        #region 框架接口
        public LuaManagerModule()
        {
            lua = new LuaState();
            FileComp = AddComp<LuaManagerFileComp>();
        }

        public override void Load(params object[] _Agr)
        {
            lua.LogGC = false;
            OpenLibs();
            lua.LuaSetTop(0);
            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, Manager_ManagerModel.Instance);
            base.Load(_Agr);
        }
        #region lua第三方插件

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        public void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb);
           
            lua.OpenLibs(LuaDLL.luaopen_pb_io);
            lua.OpenLibs(LuaDLL.luaopen_pb_conv);
            lua.OpenLibs(LuaDLL.luaopen_pb_buffer);
            lua.OpenLibs(LuaDLL.luaopen_pb_slice);

            lua.OpenLibs(LuaDLL.luaopen_struct);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            lua.OpenLibs(LuaDLL.luaopen_bit);
#endif
            if (LuaConst.openLuaSocket)
            {
                OpenLuaSocket();
            }

            if (LuaConst.openLuaDebugger)
            {
                OpenZbsDebugger();
            }
            OpenCJson();

        }



        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;

            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.RegFunction("mime.core", LuaOpen_Mime_Core);
            lua.EndPreLoad();
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }


        public void OpenZbsDebugger(string ip = "localhost")
        {
            if (!Directory.Exists(LuaConst.zbsDir))
            {
                Debugger.LogWarning("ZeroBraneStudio not install or LuaConst.zbsDir not right");
                return;
            }

            if (!LuaConst.openLuaSocket)
            {
                OpenLuaSocket();
            }

            if (!string.IsNullOrEmpty(LuaConst.zbsDir))
            {
                lua.AddSearchPath(LuaConst.zbsDir);
            }

            lua.LuaDoString(string.Format("DebugServerIp = '{0}'", ip), "@LuaClient.cs");
        }

        #endregion

        public override void Start(params object[] _Agr)
        {
            DoFile("ToLua/tolua");
            lua.Start();
            Manager_ManagerModel.Instance.GetOrAddComponent<LuaLooper>().luaState = lua;
            base.Start(_Agr);
            DoFile("Main");
        }
        public override void Close()
        {
            lua.LuaClose();
            base.Close();
        }
        #endregion

        public void StartCsModule(string nameSpace, string moduleName, LuaFunction BackCall = null, params object[] _Agr)
        {
            Manager_ManagerModel.Instance.StartModuleForName(nameSpace, moduleName,(module) => {
                if (BackCall != null)
                {
                    BackCall.Call(module);
                }
            }, _Agr);
        }

        public void StartLuaModule(string moduleName, LuaFunction BackCall = null, params object[] _Agr)
        {
            if (!Manager_ManagerModel.Instance.IsKeepModule(moduleName))
            {
                LuaModelControlBase Model = new LuaModelControlBase(moduleName);
                Manager_ManagerModel.Instance.StartModuleObj(moduleName, Model, (module) =>
                {
                    if (BackCall != null)
                    {
                        BackCall.Call(module);
                    }
                }, _Agr);
            }
            else {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }

        public void StartLuaUpDataModule(string moduleName, LuaFunction BackCall = null, params object[] _Agr)
        {
            if (!Manager_ManagerModel.Instance.IsKeepModule(moduleName))
            {
                LuaUpdataModelControl Model = new LuaUpdataModelControl(moduleName);
                Manager_ManagerModel.Instance.StartModuleObj(moduleName, Model, (module) =>
                {
                    if (BackCall != null)
                    {
                        BackCall.Call(module);
                    }
                }, _Agr);
            }
            else {
                Debug.LogWarning("This Model Already Load:" + moduleName);
            }
        }

        public void CloseModule(string modelName)
        {
            Manager_ManagerModel.Instance.CloseModuleForName(modelName);
        }


        public string FindFile(string modelFileName)
        {
            return FileComp.FindFile(modelFileName);
        }

        public byte[] ReadFile(string modelFileName)
        {
            return FileComp.ReadFile(modelFileName);
        }

        public byte[] ReadProtoFile(string modelFileName)
        {
            return FileComp.ReadProtoFile(modelFileName);
        }

        public void DoFile(string modelName,string FileName)
        {
            if (State == ModelBaseState.Start)
            {
                byte[] buffer = FileComp.ReadFile(modelName, FileName);
                if (buffer != null)
                {
                    try
                    {
                        lua.LuaLoadBuffer(buffer, Path.Combine(modelName, FileName));
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                        throw;
                    }
                }
            }
            else
            {
                Debug.LogError("LuaManagerModelControl No Start:" + Path.Combine(modelName, FileName));
            }
        }

        private void DoFile(string fileName)
        {
            byte[] buffer = FileComp.ReadFile("LuaManagerModule", fileName);
            if (buffer != null)
            {
                lua.LuaLoadBuffer(buffer, Path.Combine("LuaManagerModule", fileName));
            }
            else {
                Debug.LogError("DoFile lua :"+ fileName+" 错误");
            }
        }

        public LuaTable GetTable(string fullPath)
        {
            return lua.GetTable(fullPath);
        }
        public LuaFunction GetFunction(string fullPath)
        {
            return lua.GetFunction(fullPath);
        }

    }
}
