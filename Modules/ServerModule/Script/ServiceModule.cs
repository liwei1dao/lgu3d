using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public class ServiceModule : ManagerContorBase<ServiceModule>
    {
        #region 模块项目配置
        public delegate void ThreadDelegate();
        #endregion
        [lgu3d_SerializeName("远程服务器")]
        public string ip;
        [lgu3d_SerializeName("远程TCP端口")]
        public int prot;
        [lgu3d_SerializeName("远程UDP端口")]
        public int udpprot;
        public MessageType msgType;
        public ProtoType protoType;

        private ServiceModuleTcpComp TcpComp;
        private ServiceModuleUdpComp UdpComp;
        private ServiceModuleMessageComp MessageComp;

        public override void Load(params object[] _Agr)
        {
            TimerComp = AddComp<Module_TimerComp>();
            CoroutineComp = AddComp<Module_CoroutineComp>();
            MessageComp = AddComp<ServiceModuleMessageComp>();
            base.Load(_Agr);
        }


        //接收消息
        public int RecvMessage(byte[] buffer)
        {
            if (buffer.Length == 0)
            {
                return -1;
            }
            IMessage msg;
            int length;
            switch (msgType)
            {
                case MessageType.DefMessage:
                    msg = (IMessage)new DefMessage();
                    break;
                default:
                    msg = (IMessage)new DefMessage();
                    break;
            }
            bool ok = msg.ReadBuffer(buffer, out length);
            if (ok)
            {
                MessageComp.ReceiveMessage(msg);
            }
            return length;
        }

        public void DealMessage(IMessage msg)
        {
            if (MessageComp != null)
            {
                if (protoType == ProtoType.Json)
                {
                    string str = System.Text.Encoding.UTF8.GetString(msg.GetMsg());
                    JSONNode json = JSON.Parse(str);

                }
                else
                {
                   
                }
            }
        }

        #region TCP
        public void Disconnect()
        {
            TcpComp.Disconnect(false);
        }
        public void Connect(string Addr, int Prot)
        {
            ip = Addr;
            prot = Prot;
            TcpComp.Connect(ip, prot);
        }
        /// <summary>
        /// 通告Socket 连接
        /// </summary>
        public void NoteSocketConnect()
        {
        }
        /// <summary>
        /// 通告Socket 断开连接
        /// </summary>
        public void NoteSocketDisconnect()
        {
        }

        public void SendMessage(ushort mainId, ushort subId, byte[] buffer)
        {
            IMessage msg;
            switch (msgType)
            {
                case MessageType.DefMessage:
                    msg = new DefMessage();
                    break;
                default:
                    msg = new DefMessage();
                    break;
            }
            msg.WriteMessage(mainId, subId, buffer);
            byte[] buf = msg.ToBytes();
            Debug.Log("发送消息 TCP ComId =" + msg.GetMainId() + " MsgId = " + msg.GetSubId() + " MsgLength" + buf.Length);
            TcpComp.SocketSend(buf, 0, buf.Length);
        }
        public void SendMessage(ushort mainId, ushort subId, string _buffer)
        {
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(_buffer);
            IMessage msg;
            switch (msgType)
            {
                case MessageType.DefMessage:
                    msg = new DefMessage();
                    break;
                default:
                    msg = new DefMessage();
                    break;
            }
            msg.WriteMessage(mainId, subId, buffer);
            byte[] buf = msg.ToBytes();
            Debug.Log("发送消息 TCP ComId =" + msg.GetMainId() + " MsgId = " + msg.GetSubId() + " MsgLength" + buf.Length);
            TcpComp.SocketSend(buf, 0, buf.Length);
        }
        #endregion

        #region UDP
        public void InitUdp(int prot) {
            udpprot = prot;
            if (UdpComp == null)
                UdpComp = AddComp<ServiceModuleUdpComp>();
            else
                Debug.LogWarning("ServiceModule UdpComp 已经初始化过了");
        }

        public void SendMesageUdp(ushort mainId, ushort subId, byte[] buffer)
        {
            IMessage msg;
            switch (msgType)
            {
                case MessageType.DefMessage:
                    msg = (IMessage)new DefMessage();
                    break;
                default:
                    msg = (IMessage)new DefMessage();
                    break;
            }
            msg.WriteMessage(mainId, subId, buffer);
            byte[] buf = msg.ToBytes();
            //LoggerHelper.Debug("发送消息 UDP ComId =" + Msg.Head.ComId + " MsgId = " + Msg.Head.MsgId);
            UdpComp.SendMsg(buf);
        }
        #endregion
    }
}
