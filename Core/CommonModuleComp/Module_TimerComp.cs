using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public class Module_TimerComp : ModelCompBase
    {
        #region 框架构造
        public override void Load(ModuleBase _ModelContorl, params object[] _Agr)
        {
            if (TimerModule.Instance == null)
            {
                Debug.LogError("TimerModel User but No Load");
                return;
            }
            Model_VP = new List<uint>();
            base.Load(_ModelContorl);
            base.LoadEnd();
        }
        #endregion

        /// <summary>
        /// 模块内计时器id列表
        /// </summary>
        protected List<uint> Model_VP;           //计时器

        #region 模块内计时器
        protected void ClearVP()
        {
            for (int i = 0; i < Model_VP.Count; i++)
            {
                TimerModule.Instance.DelTimer(Model_VP[i]);
            }
            Model_VP.Clear();
        }

        public uint VP(float start, Action handler)
        {
            uint _start = (uint)(start * 1000);
            uint vp_id = TimerModule.Instance.AddTimer(_start, 0, handler);
            Model_VP.Add(vp_id);
            return vp_id;
        }
        public uint VP(float start, int interval, Action handler)
        {
            uint _start = (uint)(start * 1000);
            uint vp_id = TimerModule.Instance.AddTimer(_start, interval, handler);
            Model_VP.Add(vp_id);
            return vp_id;
        }
        public uint VP<T>(float start, int interval, Action<T> handler, T arg1)
        {
            uint _start = (uint)(start * 1000);
            uint vp_id = TimerModule.Instance.AddTimer<T>(_start, interval, handler, arg1);
            Model_VP.Add(vp_id);
            return vp_id;
        }
        #endregion

        /// <summary>
        /// 模块内计时器执行完成通知接口
        /// </summary>
        /// <param name="CompleteId"></param>
        public void ModelTimerExecutionComplete(uint CompleteId)
        {
            if (Model_VP.Contains(CompleteId))
            {
                Model_VP.Remove(CompleteId);
            }
            else
            {
                Debug.LogError("ModelTimer Complete a No Keep Timer -- ModelName:" + this.GetType().FullName);
            }
        }
    }
}
