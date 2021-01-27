using LuaInterface;
using System.Reflection;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// Lua模块控制器
    /// </summary>
    public class LuaModelControlBase : ManagerContorBase
    {
        public LuaModelControlBase(string _ModelName)
            : base()
        {
            ModuleName = _ModelName;
            LuaManagerModule.Instance.DoFile(ModuleName, "Main");
            LuaFunction func = LuaManagerModule.Instance.GetFunction(ModuleName + ".New");
            if (func != null)
            {
                func.BeginPCall();
                func.PushObject(this);
                func.PCall();
                func.EndPCall();
            }
        }
        public override void Load<Model>(ModelLoadBackCall<Model> _LoadBackCall, params object[] _Agr)
        {
            base.Load<Model>(_LoadBackCall,_Agr);
            LuaFunction func = LuaManagerModule.Instance.GetFunction(ModuleName + ".Load");
            if (func != null)
            {
                func.BeginPCall();
                func.PushArgs(_Agr);
                func.PCall();
                func.EndPCall();
            }
        }

        /// <summary>
        /// 获取cs层组件加载情况
        /// </summary>
        /// <returns></returns>
        private bool GetLoadEnd()
        {
            if (State > ModelBaseState.LoadEnd) 
                return false;
            for (int i = 0; i < MyComps.Count; i++)
            {
                if (MyComps[i].State != ModelCompBaseState.LoadEnd)
                {
                    return false;
                }
            }
            if (State <= ModelBaseState.LoadEnd)
            {
                State = ModelBaseState.LoadEnd;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool LoadEnd()
        {
            //LoggerHelper.Debug("Cs LoadEnd   Name =" + ModelName+ "  State ="+ State.ToString());
            LuaFunction func = LuaManagerModule.Instance.GetFunction(ModuleName + ".LoadEnd");
            bool IsLuaLoadEnd = false;
            if (func != null)
            {
                func.BeginPCall();
                func.PCall();
                IsLuaLoadEnd = (bool)func.CheckBoolean();
                func.EndPCall();
            }
            //LoggerHelper.Debug("Cs -----------LoadEnd  IsLuaLoadEnd = " + IsLuaLoadEnd+ "  *************  IsCsLoadEnd" + GetLoadEnd());
            if (IsLuaLoadEnd && GetLoadEnd())
            {
                if (LoadBackCall != null)
                {
                    LoadBackCall(this);
                    LoadBackCall = null;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Start(params object[] _Agr)
        {
            Debug.Log("Lua Start:" + ModuleName);
            base.Start(_Agr);
            LuaFunction func = LuaManagerModule.Instance.GetFunction(ModuleName + ".Start");
            if (func != null)
            {
                func.BeginPCall();
                func.PushArgs(_Agr);
                func.PCall();
                func.EndPCall();
            }

        }
        public override void Close()
        {
            LuaFunction func = LuaManagerModule.Instance.GetFunction(ModuleName + ".Close");
            if (func != null)
            {
                func.BeginPCall();
                func.PCall();
                func.EndPCall();
            }
            base.Close();
        }

        public virtual ModelCompBase AddComp(string nameSpace, string CPName, params object[] _Agr)
        {
            ModelCompBase Comp = Assembly.GetExecutingAssembly().CreateInstance(nameSpace + "." + CPName, true, System.Reflection.BindingFlags.Default, null, null, null, null) as ModelCompBase;
            MyComps.Add(Comp);
            if (State > ModelBaseState.Close)
                Comp.Load(this, _Agr);
            if (State == ModelBaseState.Start)
                Comp.Start(this, _Agr);
            return Comp;
        }

        public void LoadResourceComp()
        {
            ResourceComp = AddComp<Module_ResourceComp>();
        }

        public Sprite LoadSprite(string BundleOrPath, string AssetName)
        {
            return ResourceComp.LoadAsset<Sprite>(BundleOrPath, AssetName);
        }

        public Texture LoadTexture(string BundleOrPath, string AssetName)
        {
            return ResourceComp.LoadAsset<Texture>(BundleOrPath, AssetName);
        }

        public AudioClip LoadAudioClip(string BundleName, string AssetName)
        {
            return LoadAsset<AudioClip>(BundleName, AssetName);
        }

        public RuntimeAnimatorController LoadAnimatorController(string BundleName, string AssetName)
        {
            return LoadAsset<RuntimeAnimatorController>(BundleName, AssetName);
        }

        public TextAsset LoadTextAsset(string BundleName, string AssetName) {
            return LoadAsset<TextAsset>(BundleName, AssetName);
        }
    }

    public class LuaUpdataModelControl : LuaModelControlBase, IUpdataMode
    {
        LuaFunction LuaUpdate;
        public LuaUpdataModelControl(string _ModelName)
            :base(_ModelName)
        {
            LuaUpdate = LuaManagerModule.Instance.GetFunction(ModuleName + ".Update");
        }

        public void Update(float time)
        {
            if (LuaUpdate != null)
            {
                LuaUpdate.BeginPCall();
                LuaUpdate.Push(time);
                LuaUpdate.PCall();
                LuaUpdate.EndPCall();
            }
        }
    }

    public class LuaFixedUpdateModelControl : LuaModelControlBase, IFixedUpdateMode
    {
        LuaFunction LuaFixedUpdate;
        public LuaFixedUpdateModelControl(string _ModelName)
            : base(_ModelName)
        {
            LuaFixedUpdate = LuaManagerModule.Instance.GetFunction(ModuleName + ".FixedUpdate");
        }

        public void FixedUpdate()
        {
            if (LuaFixedUpdate != null)
            {
                LuaFixedUpdate.BeginPCall();
                LuaFixedUpdate.PCall();
                LuaFixedUpdate.EndPCall();
            }
        }
    }

    public class LuaModelControlBase<C> : LuaModelControlBase
    {
        public LuaModelControlBase()
            : base(typeof(C).Name)
        {

        }
        public LuaModelControlBase(string _ModelName)
            : base(_ModelName)
        {

        }
    }
}
