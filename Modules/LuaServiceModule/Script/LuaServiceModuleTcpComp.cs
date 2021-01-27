using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using Random = System.Random;
using System.Linq;

namespace lgu3d
{
    public class LuaServiceModuleTcpComp : ModelCompBase<LuaServiceModule>
    {
        public class StateObject : IDisposable
        {
            public Socket socket = null;
            public bool IsUse = true;
            public Action<StateObject> cBack;                   //链接回调
            public Action<StateObject> dBack;                   //断开回调

            public StateObject(int timeout, Action<StateObject> connectBack, Action<StateObject> disconnectBack)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = timeout;
                cBack = connectBack;
                dBack = disconnectBack;
                IsUse = true;
            }

            public void Dispose()
            {
                cBack = null;
                dBack = null;
                IsUse = false;
            }
        }

        private const int timeout = 500;                    //超时
        private const int recvBufferSize = 1024 * 128;        //接收缓冲区空间大小
        private const int maxPackage = 1024 * 64;             //数据包的最大长度
        [lgu3d_SerializeName("Tcp 连接状态")]
        private bool IsConnect = false;
        private StateObject state;
        private byte[] recvBuffer = null;           //接收缓冲区
        private byte[] backupBuffer = null;         //备份缓冲区，用来缓存半包




        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl);
            base.LoadEnd();
        }

        public override void Close()
        {
            Debug.LogError("连接服务器 Close");
            Disconnect(true);
            base.Close();
        }



        #region 链接
        /// <summary>
        /// 连接主机
        /// </summary>
        /// <param name="aRemoteIPAddress">主机地址</param>
        /// <param name="aRemotePort">主机端口</param>
        /// <returns>返回操作是否成功</returns>
        public bool Connect(string ip, int port)
        {
            if (IsConnect)
            {
                MyModule.NoteSocketConnect();
                return true;
            }

            try
            {
                StateObject client = new StateObject(timeout, (obj) => {
                    state = obj;
                    IsConnect = true;
                    recvBuffer = new byte[recvBufferSize];
                    state.socket.BeginReceive(recvBuffer, 0, recvBufferSize, SocketFlags.None, new AsyncCallback(OnRecv), obj);
                    MyModule.NoteSocketConnect();
                }, (obj) => {
                    if (state == null || state == obj)
                    {
                        state = null;
                        IsConnect = false;
                        if (obj.IsUse)
                        {
                            MyModule.NoteSocketDisconnect();
                        }
                        obj.Dispose();
                    }
                });
                client.socket.BeginConnect(ip, port, new AsyncCallback(AsyncConnect), client);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }



        /// <summary>
        /// 异步连接完成回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncConnect(IAsyncResult ar)
        {
            StateObject sock = (StateObject)ar.AsyncState;
            try
            {
                sock.socket.EndConnect(ar);
                if (sock.socket.Connected)
                {
                    sock.cBack?.Invoke(sock);
                }
                else
                {
                    new Exception("Unable to connect to remote machine, Connect Failed!");
                    sock.dBack?.Invoke(sock);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                sock.dBack?.Invoke(sock);
            }
        }

        #endregion

        #region 关闭
        /// <summary>
        /// 断开与主机的连接
        /// </summary>
        /// <returns></returns>
        public void Disconnect(bool IsNotice)
        {
            //判断是否已连接
            if (!IsConnect || state == null)
                throw new SocketException(10057);
            lock (this)
            {
                if (!IsNotice)
                {
                    state.IsUse = false;
                }
                //Socket异步断开并等待完成
                state.socket.BeginDisconnect(false, EndDisconnect, state).AsyncWaitHandle.WaitOne();
            }
        }


        private void EndDisconnect(IAsyncResult ar)
        {
            StateObject sock = (StateObject)ar.AsyncState;
            try
            {
                sock.socket.EndDisconnect(ar);
                sock.dBack?.Invoke(sock);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        #endregion

        #region 发送

        //异步发送数据
        public void SocketSend(byte[] data, int offset, int size)
        {
            if (IsConnect)
            {
                try
                {
                    state.socket.BeginSend(data, offset, size, SocketFlags.None, new AsyncCallback(OnSend), state);
                }
                catch
                {
                    Close();
                }
            }
            else
            {
                MyModule.NoteSocketDisconnect();
            }
        }

        //结束异步发送
        private void OnSend(IAsyncResult ar)
        {
            StateObject sock = (StateObject)ar.AsyncState;
            sock.socket.EndSend(ar);

        }

        #endregion

        #region 接受
        private void OnRecv(IAsyncResult ar)
        {
            StateObject sock = (StateObject)ar.AsyncState;
            if (sock.IsUse)
            {
                try
                {
                    int recvLength = sock.socket.EndReceive(ar);
                    int pkgLength = 0;
                    byte[] data;
                    //如果半包数据不为空
                    if (null != backupBuffer)
                    {
                        //将半包和后面接收到的数据报拼接起来
                        data = backupBuffer.Concat(recvBuffer.Take(recvLength).ToArray()).ToArray();
                    }
                    else
                    {
                        data = recvBuffer.Take(recvLength).ToArray();
                    }
                    //解决连包问题
                    while (true)
                    {
                        // 1. recvEvent返回一个完整的数据包长度
                        pkgLength = MyModule.RecvMessage(data);
                        if (pkgLength == -1 || pkgLength == 0 || pkgLength > maxPackage)
                        {
                            Debug.LogError("接收到异常消息包!" + BitConverter.ToString(data));
                            if (sock.IsUse) {
                                Close();
                            }
                            break;
                        }

                        // 2. 如果正常处理完,则直接启动下次接收
                        if (pkgLength == data.Length)
                        {
                            ////TraceUtil.Log("正常");
                            backupBuffer = null;
                            Array.Clear(recvBuffer, 0, recvBuffer.Length);
                            pkgLength = 0;
                            break;
                        }
                        // 3. 如果还未接收完,半包则在偏移处等待
                        else if (pkgLength > data.Length)
                        {
                            //将半包数据拷贝到backupBuffer
                            ////TraceUtil.Log("半包");
                            backupBuffer = data.Take(data.Length).ToArray();
                            break;
                        }
                        // 4. 如果是连包，则循环处理
                        else if (pkgLength < data.Length)
                        {
                            //自增偏移量并跳过已经处理的包
                            ////TraceUtil.Log("连包");
                            data = data.Skip(pkgLength).ToArray();
                            backupBuffer = null;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (IsConnect)
                    {
                        //递归实现连续接收
                        sock.socket.BeginReceive(recvBuffer, 0, recvBufferSize, SocketFlags.None, new AsyncCallback(OnRecv), sock);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    if (sock.IsUse)
                    {
                        Close();
                    }
                }
            }
        }
        #endregion
    }
}

