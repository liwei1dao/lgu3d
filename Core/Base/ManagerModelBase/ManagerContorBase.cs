using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
管理模块 设计基类
*/
namespace lgu3d
{
    public class ManagerContorBase : ModuleBase
    {
        protected ModelLoadBackCall<ManagerContorBase> LoadBackCall;

        public ManagerContorBase()
            : base()
        {

        }

        public virtual void Load<Model>(ModelLoadBackCall<Model> _LoadBackCall, params object[] agrs) where Model : ManagerContorBase
        {
            LoadBackCall = _LoadBackCall as ModelLoadBackCall<ManagerContorBase>;
            Load(agrs);
        }
        public override bool LoadEnd()
        {
            if (base.LoadEnd())
            {
                if (LoadBackCall != null)
                {
                    LoadBackCall(this as ManagerContorBase);
                    LoadBackCall = null;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public class ManagerContorBase<C> : ManagerContorBase where C : ManagerContorBase<C>, new()
    {
        #region 单例接口
        protected static C _instance = null;
        public static C Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("This Model No LoadEnd:" + typeof(C).Name);
                }
                return _instance;
            }
            protected set
            {
                _instance = value;
            }
        }
        #endregion

        protected new ModelLoadBackCall<C> LoadBackCall;

        public ManagerContorBase()
            : base()
        {
            _instance = this as C;
        }

        public override void Load<Model>(ModelLoadBackCall<Model> _LoadBackCall, params object[] agrs)
        {
            LoadBackCall = _LoadBackCall as ModelLoadBackCall<C>;
            base.Load<Model>(_LoadBackCall, agrs);
        }

        public override bool LoadEnd()
        {
            if (base.LoadEnd())
            {
                if (LoadBackCall != null)
                {
                    LoadBackCall(this as C);
                    LoadBackCall = null;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
