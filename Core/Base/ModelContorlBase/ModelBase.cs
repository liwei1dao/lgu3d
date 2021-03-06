﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/*
 作者 : liwei1dao
 文件 : ModelContorlBase.cs
 描述 : 模块基类控制器
 修改 : 抽象类使用，各类模块控制器的基类设计 框架基类尽量简洁
*/

namespace lgu3d
{
    //模块初始化回调委托
    public delegate void ModelLoadBackCall<C>(C ModelContorl)where C: ModelBase;
    //模块基础状态
    public enum ModelBaseState
    {
        Close = -1,          //关闭状态
        Loading = 1,        //加载中状态
        LoadEnd = 2,        //加载完成状态
        Start = 3,          //启动状态
    }


    public interface IModule
    {
        void Load(params object[] agr);                                                    //模块初始化
        bool LoadEnd();                                                                    //加载完毕
        void Start(params object[] agr);                                                   //模块启动
        void Close();                                                                      //模块关闭
        void ShowInspector();                                                              //UI               
    }

    public interface IUpdataMode: IModule
    {
        void Update(float time);
        
    }

    public interface IFixedUpdateMode : IModule
    {
        void FixedUpdate();
    }

    public interface ISceneMode : IModule
    {
        IEnumerator LoadScene();
    }

    #region 普通模块基类
    public abstract class ModelBase : IModule
    {
        public string ModuleName;                                                   //模块名称
        public ModelBaseState State = ModelBaseState.Close;                         //模块状态
        protected List<ModelCompBase> MyComps = new List<ModelCompBase>();          //组件列表
        protected Module_TimerComp TimerComp;                                       //计时器组件 （需要则初始化）
        protected Module_CoroutineComp CoroutineComp;                               //协程组件（需要则初始化）
        protected Module_ResourceComp ResourceComp;                                 //资源管理组件（需要则初始化）

        public ModelBase()
        {
            State = ModelBaseState.Close;
        }

        public virtual void Load(params object[] agr)
        {
            State = ModelBaseState.Loading;
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Load(this, agr);
            }
            LoadEnd();
        }
        public virtual bool LoadEnd()
        {
            if (State >= ModelBaseState.LoadEnd) //模块已经加载成功了
                return false;
            for (int i = 0; i < MyComps.Count; i++)
            {
                if (MyComps[i].State != ModelCompBaseState.LoadEnd)
                {
                    return false;
                }
            }
            if (State < ModelBaseState.LoadEnd)
            {
                State = ModelBaseState.LoadEnd;
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual void Start(params object[] agr)
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Start(agr);
            }
            State = ModelBaseState.Start;
        }
        public virtual void Close()
        {
            for (int i = 0; i < MyComps.Count; i++)
            {
                MyComps[i].Close();
            }
            MyComps.Clear();
            State = ModelBaseState.Close;
        }

        //添加组件
        protected virtual CP AddComp<CP>(params object[] agr) where CP : ModelCompBase, new()
        {
            CP Comp = new CP();
            MyComps.Add(Comp);
            if (State > ModelBaseState.Close)
                Comp.Load(this, agr);
            if (State == ModelBaseState.Start)
                Comp.Start(this, agr);
            return Comp;
        }

        protected virtual ModelCompBase AddComp(ModelCompBase Comp, params object[] agr)
        {
            MyComps.Add(Comp);
            if (State > ModelBaseState.Close)
                Comp.Load(this, agr);
            if (State == ModelBaseState.Start)
                Comp.Start(this, agr);
            return Comp;
        }

        //移除组件
        protected virtual void RemoveComp(ModelCompBase Comp)
        {
            MyComps.Remove(Comp);
            Comp.Close();
        }

        #region 扩展接口
        /// <summary>
        /// 获取模块组件列表
        /// </summary>
        /// <returns></returns>
        public List<ModelCompBase> GetMyComps()
        {
            return MyComps;
        }

        #region 协程组件扩展
        /// <summary>
        /// 启动协程
        /// </summary>
        public CoroutineTask StartCoroutine(IEnumerator coroutine)
        {
            if (CoroutineComp == null)
            {
                Debug.LogError(ModuleName + " No Load CoroutineComp");
                return null;
            }
            return CoroutineComp.StartCoroutine(coroutine);
        }
        #endregion

        #region 计时器组件扩展
        /// <summary>
        /// 启动计时器
        /// </summary>
        public uint VP(float start, Action handler)
        {
            if (TimerComp == null)
            {
                Debug.LogError(ModuleName + " No Load TimerComp");
                return 0;
            }
            return TimerComp.VP(start, handler);
        }
        #endregion

        #region 资源管理组件扩展

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="ObjectPath"></param>
        /// <param name="Parnt"></param>
        /// <returns></returns>
        public GameObject CreateObj(string BundleOrPath, string ObjectPath, GameObject Parnt)
        {
            GameObject obj = LoadAsset<GameObject>(BundleOrPath, ObjectPath);
            if (Parnt == null)
                return obj.CreateToParnt(null);
            else
                return obj.CreateToParnt(Parnt);
        }

        public UnityEngine.Object LoadAsset(string BundleOrPath, string AssetName)
        {
            if (ResourceComp == null) {
                Debug.LogError("模块未加载 ResourceComp");
            }
            return ResourceComp.LoadAsset<UnityEngine.Object>(BundleOrPath, AssetName);
        }

        public void LoadBundle(string BundleOrPath)
        {
            AssetBundle bundle = ResourceComp.LoadAssetBundle(BundleOrPath);
        }

        public T LoadAsset<T>(string BundleOrPath, string AssetName) where T : UnityEngine.Object
        {
            return ResourceComp.LoadAsset<T>(BundleOrPath, AssetName);
        }
        #endregion
            #endregion

            /// <summary>
            /// Inspector 属性界面
            /// </summary>
        public virtual void ShowInspector(){}
    }
    #endregion
}

