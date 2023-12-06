using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using System.Collections.Generic;


namespace lgu3d
{
    /// <summary>
    /// 游戏实体技能 模块
    /// </summary>
    public abstract class LGEntityModule<C> : ManagerContorBase<C>, ILGEntityModule where C : ManagerContorBase<C>, new()
    {

        public override void Load(params object[] agrs)
        {
            base.Load(agrs);
        }

    }
}