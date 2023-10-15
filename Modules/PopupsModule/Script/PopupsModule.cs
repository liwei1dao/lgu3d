using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 弹窗管理模块
    /// </summary>
    public class PopupsModule : ManagerContorBase<PopupsModule>
    {
        private PopupsRootViewComp viewComp;
        public override void Load(params object[] _Agr)
        {
            viewComp = AddComp<PopupsRootViewComp>();
            base.Load(_Agr);
        }

        public GameObject CreatePopups(GameObject uiasset)
        {
            return viewComp.CreatePopups(uiasset);
        }

        public void ShowNextQueuePopups()
        {
            viewComp.ShowNextQueuePopups();
        }

        public void PushPopupsView(IPopupsView view, object data)
        {
            viewComp.PushPopupsView(view, data);
        }
    }
}

