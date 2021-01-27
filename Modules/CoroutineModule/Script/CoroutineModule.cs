using System;
using UnityEngine;
using System.Collections;

namespace lgu3d
{
    public class CoroutineModule : ManagerContorBase<CoroutineModule>
    {
        private CoroutineModuleDataComp DataComp;

        public override void Load(params object[] _Agr)
        {
            DataComp = AddComp<CoroutineModuleDataComp>();
            base.Load(_Agr);
        }

        public CoroutineTask StartCoroutineTask(IEnumerator coroutine)
        {
           return DataComp.StartCoroutine(coroutine);
        }

        public void StartTask(CoroutineTask Task)
        {
            Manager_ManagerModel.Instance.StartCoroutine(Task.CallWrapper());
        }
    }
}
