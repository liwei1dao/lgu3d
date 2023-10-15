using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using lgu3d;
using UnityEngine;
using UnityEngine.UI;

namespace lgu3d
{
    /// <summary>
    /// 弹窗界面
    /// </summary>
    public class PopupsRootViewComp<C> : ModelCompBase<C>, IPopupsView where C : ModuleBase, new()
    {
        protected GameObject UIGameobject;
        public override void Load(ModuleBase module, params object[] agrs)
        {
            base.Load(module, agrs);
            string PrefabName = (string)agrs[0];
            GameObject uiasset = MyModule.LoadAsset<GameObject>("Prefab", PrefabName);
            UIGameobject = PopupsModule.Instance.CreatePopups(uiasset);
        }

        public override void Close()
        {
            GameObject.Destroy(UIGameobject);
            base.Close();
        }

        public virtual void Show(object data)
        {
            UIGameobject.SetActive(true);
        }

        public virtual void Hide()
        {
            UIGameobject.SetActive(false);
            PopupsModule.Instance.ShowNextQueuePopups();
        }
    }
}