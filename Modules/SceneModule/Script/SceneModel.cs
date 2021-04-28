using UnityEngine;
using System;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 场景模块控制器
    /// </summary>
    public class SceneModule : ManagerContorBase<SceneModule>
    {
        private SceneChedulerComp  ChangeSceneComp;
        private SceneLoadingViewComp LoadingViewComp;
        public int LoadingSteps = 100;                                            //加载进度平率
        public override void Load(params object[] _Agr)
        {
            IScenesChedulerBase Cheduler;
            if (_Agr.Length == 1 && _Agr[0] is IScenesChedulerBase)
            {
                Cheduler = _Agr[0] as IScenesChedulerBase;
            }
            else
            {
                Cheduler = new ScenesDefaultCheduler();
                _Agr =new object[] {Cheduler};
            }
            CoroutineComp = AddComp<Module_CoroutineComp>();
            ResourceComp = AddComp<Module_ResourceComp>();

            ChangeSceneComp = AddComp<SceneChedulerComp>(Cheduler);
            base.Load(_Agr);
        }
        public override void Start(params object[] _Agr)
        {
            base.Start(_Agr);
        }

        /// <summary>
        /// 获取通用加载组件
        /// </summary>
        /// <returns></returns>
        public SceneLoadingViewComp GetLoadingViewComp()
        {
            if (LoadingViewComp == null)
            {
                LoadingViewComp = AddComp<SceneLoadingViewComp>();
            }
            return LoadingViewComp;
        }

        /// <summary>
        /// 跳转场景
        /// </summary>
        /// <param name="SceneId"></param>
        /// <param name="CallBack"></param>
        public void ChangeScene (ISceneLoadCompBase SceneLoadComp)
        {
            ChangeSceneComp.ChangeScene(SceneLoadComp);
        }
    }
}
