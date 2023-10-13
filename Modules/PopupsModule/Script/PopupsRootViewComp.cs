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
    public class PopupsRootViewComp : Model_BaseViewComp<CommonModule>
    {
        /// <summary>
        /// 弹窗队列
        /// </summary>
        public Queue<IPopupsView> queue;

        public override void Load(ModuleBase module, params object[] agrs)
        {
            base.Load(module, "PopupsViewCpmp", UILevel.NormalUI, UIOption.Empty);
            queue = new Queue<IPopupsView>();
            Hide();
            LoadEnd();
        }

        public GameObject CreatePopups(GameObject uiasset)
        {
            GameObject view = uiasset.CreateToParnt(UIGameobject);
            RectTransform rectTrans = view.GetComponent<RectTransform>();
            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTrans.anchorMin = Vector2.zero;
            rectTrans.anchorMax = Vector2.one;
            Canvas canvas = view.AddMissingComponent<Canvas>();
            view.AddMissingComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            return view;
        }

        //添加弹窗显示
        public void PushPopupsView(IPopupsView view)
        {
            queue.Enqueue(view);
            if (!ishow)
            {
                Show();
                ShowNextQueuePopups();
            }
        }

        public void ShowNextQueuePopups()
        {
            if (queue.Count > 0)
            {
                IPopupsView view = queue.Dequeue();
                view.Show();
            }
            else
            {
                Hide();
            }
        }
    }
}