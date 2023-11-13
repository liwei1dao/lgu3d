using System;
using BestHTTP;
using BestHTTP.WebSocket;

namespace lgu3d
{
    /// <summary>
    /// 事件模块数据管理组件
    /// </summary>
    public class WebSocketComp : ModelCompBase
    {
        private IAppServiceModule module;
        protected WebSocket webSocket;
        public override void Load(ModuleBase module, params object[] agrs)
        {
            base.Load(module, agrs);
            if (AppConfig.ServiceAddr == string.Empty)
            {
                throw new Exception("AppConfig.ServiceAddr is Empty!");
            }
            this.module = module as IAppServiceModule;
            webSocket = new WebSocket(new Uri(AppConfig.ServiceAddr));
#if !UNITY_WEBGL
            webSocket.StartPingThread = true;
#if !BESTHTTP_DISABLE_PROXY
            if (HTTPManager.Proxy != null)
                webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
#endif
#endif
            webSocket.OnOpen += OnOpen;
            webSocket.OnBinary += OnMessageReceived;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnClosed += OnClosed;
            webSocket.OnError += OnError;
            webSocket.Open();
            base.LoadEnd();
        }
        public override void Close()
        {
            webSocket.OnOpen -= OnOpen;
            webSocket.OnBinary -= OnMessageReceived;
            webSocket.OnMessage -= OnMessageReceived;
            webSocket.OnClosed -= OnClosed;
            webSocket.OnError -= OnError;
            webSocket.Close();
            base.Close();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="buff"></param>
        public void Send(byte[] buff)
        {
            webSocket.Send(buff);
        }

        #region WebSocket 事件函数
        /// <summary>
        /// 链接成功
        /// </summary>
        /// <param name="ws"></param>
        public virtual void OnOpen(WebSocket ws)
        {
            module.OnOpen(ws);
        }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="message"></param>
        public virtual void OnMessageReceived(WebSocket ws, byte[] data)
        {
            module.OnMessageReceived(ws, data);
        }
        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="message"></param>
        public virtual void OnMessageReceived(WebSocket ws, string message)
        {
            module.OnMessageReceived(ws, message);
        }


        /// <summary>
        /// 链接关闭
        /// </summary>
        public virtual void OnClosed(WebSocket ws, UInt16 code, string message)
        {
            module.OnClosed(ws, code, message);
        }

        /// <summary>
        /// 错误通知
        /// </summary>
        public virtual void OnError(WebSocket ws, Exception ex)
        {
            string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
            if (ws.InternalRequest.Response != null)
            {
                errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
            }
#endif
            webSocket = null;
            module.OnError(ws, ex);
        }

        #endregion
    }
}