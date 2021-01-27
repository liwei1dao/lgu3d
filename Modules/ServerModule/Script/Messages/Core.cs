using UnityEngine;
using UnityEditor;
namespace lgu3d
{
    public enum MessageType
    {
        DefMessage,
        CayxMessage,
    }

    public interface IMessage
    {
        ushort GetMainId();
        ushort GetSubId();
        byte[] GetMsg();
        void WriteMessage(ushort _mainId, ushort _subId, byte[] _msg);
        bool ReadBuffer(byte[] buffer, out int length);
        byte[] ToBytes();
    }
}