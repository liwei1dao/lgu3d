using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;


namespace lgu3d
{
    /// <summary>
    /// AppServiceModule 模块
    /// 支持 http 请求 以及 websocket协议
    /// </summary>
    public abstract class AppServiceModule<C> : ManagerContorBase<C>, IAppServiceModule where C : ManagerContorBase<C>, new()
    {
        protected bool RemoteServer;
        protected WebSocketComp webSocketComp;
        public override void Load(params object[] agrs)
        {
            RemoteServer = (bool)agrs[0];
            if (RemoteServer)
                webSocketComp = AddComp<WebSocketComp>();
            base.Load(agrs);
        }

        public virtual void OnOpen(WebSocket ws)
        {

        }

        public virtual void OnMessageReceived(WebSocket ws, byte[] data)
        {

        }
        public virtual void OnMessageReceived(WebSocket ws, string message)
        {

        }
        public virtual void OnClosed(WebSocket ws, ushort code, string message)
        {

        }

        public virtual void OnError(WebSocket ws, Exception ex)
        {

        }

        public virtual void Send(byte[] buff)
        {
            this.webSocketComp.Send(buff);
        }
    }
}