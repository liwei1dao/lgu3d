using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace lgu3d {
    public class DefMessage: IMessage
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Head
        {
            public ushort mainId;
            public ushort subId;
            public byte checkId;
            public ushort mmsglength;
        }

        public static int headsize = Marshal.SizeOf(typeof(Head));
        public Head head;                               //数据包头
        public byte[] msg;                              //数据缓冲,SOCKET_TCP_PACKET


        public const byte DK_CHECK_NEW = 0x24;          //校验类型
        public ushort GetMainId() {
            return head.mainId;
        }
        public ushort GetSubId()
        {
            return head.subId;
        }
        public byte[] GetMsg()
        {
            return msg;
        }

        public void WriteMessage(ushort _mainId, ushort _subId, byte[] _msg) {
            head = new Head()
            {
                mainId = _mainId,
                subId = _subId,
                checkId = DK_CHECK_NEW,
                mmsglength = (ushort)(headsize + _msg.Length),
            };
            msg = _msg;
        }

        public bool ReadBuffer(byte[] buffer,out int length)
        {
            length = 0;
            if (buffer.Length >= headsize)
            {
                byte[] headdata = buffer.Skip(0).Take(headsize).ToArray();
                head = DataChangeTools.ByteaToStruct<Head>(headdata);                           //结构体序列化 暂时弃用
                if ((head.checkId & DK_CHECK_NEW) != DK_CHECK_NEW) {
                    length = -1;
                    throw new Exception("消息头协议异常 校验字段错误 checkId:" + string.Format("0:X000", head.checkId));
                }
                length = head.mmsglength;
                if (buffer.Length >= head.mmsglength)
                {
                    msg = buffer.Skip(headsize).Take(buffer.Length - headsize).ToArray();
                }
                else {
                    return false;
                }
            }
            return true;
        }

        public byte[] ToBytes()
        {
            byte[] buffer = new byte[head.mmsglength];
            var Headbuffer = DataChangeTools.StructToBytes(head);
            Array.Copy(Headbuffer, buffer, headsize);
            Array.Copy(msg, 0, buffer, headsize, msg.Length);
            return buffer;
        }
    }
}
