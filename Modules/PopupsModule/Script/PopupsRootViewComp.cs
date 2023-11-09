using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using lgu3d;
using UnityEngine;
using UnityEngine.UI;

namespace lgu3d
{
    public class PopupsViewQueueData
    {
        public IPopupsView view;
        public object data;
    }
    /// <summary>
    /// 弹窗界面
    /// </summary>
    public class PopupsRootViewComp : Model_BaseViewComp<PopupsModule>
    {
        /// <summary>
        /// 弹窗队列
        /// </summary>
        public Queue<PopupsViewQueueData> queue;

        public override void Load(ModuleBase module, params object[] agrs)
        {
            base.Load(module, "PopupsViewCpmp", UILevel.NormalUI, UIOption.Empty);
            queue = new Queue<PopupsViewQueueData>();
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
        public void PushPopupsView(IPopupsView view, object data)
        {
            queue.Enqueue(new() { view = view, data = data });
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
                PopupsViewQueueData data = queue.Dequeue();
                data.view.Show(data.data);
            }
            else
            {
                Hide();
            }
        }
    }
}