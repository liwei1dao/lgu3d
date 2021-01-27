using System;
using UnityEngine;
using LuaInterface;


namespace lgu3d
{
    public enum ProtoType
    {
        ProtoBuff,
        Json,
    }

    public class LuaServiceModule : LuaModelControlBase<LuaServiceModule>
    {
        #region 模块项目配置
        public delegate void ThreadDelegate();
        #endregion
        [lgu3d_SerializeName("远程服务器")]
        public string ip;
        [lgu3d_SerializeName("远程TCP端口")]
        public int prot;
        public MessageType msgType;
        public ProtoType protoType;
        private LuaServiceModuleTcpComp TcpComp;
        private LuaServiceModuleMessageComp MessageComp;
        private LuaFunction ScoketDisconnectNote;
        private LuaFunction ScoketConnectNote;
        private LuaFunction LuaDealMessage;
        public override void Load(params object[] _Agr)
        {
            ScoketDisconnectNote = LuaManagerModule.Instance.GetFunction(ModuleName + ".ScoketDisconnectNote");
            ScoketConnectNote = LuaManagerModule.Instance.GetFunction(ModuleName + ".ScoketConnectNote");
            LuaDealMessage = LuaManagerModule.Instance.GetFunction(ModuleName + ".DealMessage");
            TimerComp = AddComp<Module_TimerComp>();
            CoroutineComp = AddComp<Module_CoroutineComp>();
            MessageComp = AddComp<LuaServiceModuleMessageComp>();
            base.Load(_Agr);
        }

        public override void Start(params object[] _Agr){
            base.Start(_Agr);
            //Connect(Ip,Port);
        }

        public void SendMessage(ushort mainId, ushort subId, LuaByteBuffer buffer)
        {
            IMessage msg;
            switch (msgType)
            {
                case MessageType.DefMessage:
                    msg = new DefMessage();
                    break;
                case MessageType.CayxMessage:
                    msg = new CayxMessage();
                    break;
                default:
                    msg = new DefMessage();
                    break;
            }
            msg.WriteMessage(mainId, subId, buffer.buffer);
            byte[] buf = msg.ToBytes();
            Debug.Log("发送消息 TCP ComId =" + msg.GetMainId() + " MsgId = " + msg.GetSubId()+ " MsgLength" + buf.Length);
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
                case MessageType.CayxMessage:
                    msg = new CayxMessage();
                    break;
                default:
                    msg = new CayxMessage();
                    break;
            }
            msg.WriteMessage(mainId, subId, buffer);
            byte[] buf = msg.ToBytes();
            Debug.Log("发送消息 TCP ComId =" + msg.GetMainId() + " MsgId = " + msg.GetSubId() + " MsgLength" + buf.Length);
            TcpComp.SocketSend(buf, 0, buf.Length);
        }

        public int RecvMessage(byte[] buffer)
        {
            if (buffer.Length == 0)
            {
                return -1;
            }
            IMessage msg;
            int length;
            switch (msgType) {
                case MessageType.DefMessage:
                    msg = (IMessage)new DefMessage();
                break;
                case MessageType.CayxMessage:
                    msg = new CayxMessage();
                    break;
                default:
                    msg = (IMessage)new DefMessage();
                break;
            }
            bool ok = msg.ReadBuffer(buffer, out length);
            if (ok) {
                MessageComp.ReceiveMessage(msg);
            }
            return length;
        }

        public void DealMessage(IMessage msg)
        {
            if (LuaDealMessage != null)
            {
                if (protoType == ProtoType.Json)
                {
                    string str = System.Text.Encoding.UTF8.GetString(msg.GetMsg());
                    JSONNode json = JSON.Parse(str);
                    LuaDealMessage.Call<ushort, ushort, string>(msg.GetMainId(), msg.GetSubId(), str);
                }
                else
                {
                    LuaDealMessage.Call<ushort, ushort, LuaByteBuffer>(msg.GetMainId(), msg.GetSubId(), new LuaByteBuffer(msg.GetMsg()));
                }
            }
        }

        public void Disconnect() {
            TcpComp.Disconnect(false);
        }


        public void Connect(string Addr , int Prot) {
            ip = Addr;
            prot = Prot;
            TcpComp.Connect(ip, prot);
        }

        /// <summary>
        /// 通告Socket 连接
        /// </summary>
        public void NoteSocketConnect() {
            if (ScoketConnectNote != null) {
                VP(0, () => {
                    ScoketConnectNote.Call();
                });
            }
            else{
                Debug.LogError("链接成功！ 错误 没有");
            }
        }

        /// <summary>
        /// 通告Socket 断开连接
        /// </summary>
        public void NoteSocketDisconnect(){
            if (ScoketDisconnectNote != null)
            {
                VP(0, () => {
                    ScoketDisconnectNote.Call();
                });
            }
            else{
                Debug.LogError("断开链接！ 错误 没有");
            }
        }

    }
}
