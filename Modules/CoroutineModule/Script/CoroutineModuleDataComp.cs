using System;
using System.Collections;
using System.Collections.Generic;

namespace lgu3d
{
    public class CoroutineModuleDataComp : ModelCompBase<CoroutineModule>
    {
        #region 框架构造
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            AllTask = new List<CoroutineTask>();
            base.Load(_ModelContorl);
            base.LoadEnd();
        }
        #endregion
        private List<CoroutineTask> AllTask;

        public CoroutineTask StartCoroutine(IEnumerator coroutine)
        {
            CoroutineTask task = new CoroutineTask(coroutine);
            task.Finished += TaskFinished;
            AllTask.Add(task);
            MyModule.StartTask(task);
            return task;
        }

        /// <summary>
        ///  任务完成通知
        /// </summary>
        public void TaskFinished(CoroutineTask Task,bool IsFinish)
        {
            AllTask.Remove(Task);
        }

    }
}