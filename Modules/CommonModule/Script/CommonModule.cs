using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d {
    public class CommonModule : ManagerContorBase<CommonModule>
    {
        private MessageBoxViewComp MessageBoxComp;
        public override void Load(params object[] _Agr)
        {
            ResourceComp = AddComp<Module_ResourceComp>();
            MessageBoxComp = AddComp<MessageBoxViewComp>();
            base.Load(_Agr);
        }
        public void ShowBox(string msg, Action confirmFun, Action cancelFun) {
            MessageBoxComp.ShowBox(msg, confirmFun, cancelFun);
        }
    }
}

