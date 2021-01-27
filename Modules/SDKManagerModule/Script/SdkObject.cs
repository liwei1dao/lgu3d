using UnityEngine;
using System;

namespace lgu3d {
    public class SdkObject : MonoBehaviour
    {
        private Action<string,string> agents;

        public void Init(Action<string, string> _agents){
            agents = _agents;
        }

        /// <summary>
        /// SDK消息接收函数
        /// </summary>
        /// <param name="data">消息数据</param>
        void ReceiveMessage(string data){
            string[] _data = data.Split('|');
            if (_data != null && _data.Length == 2)
            {
                Debug.Log("收到 sdk 消息 msgid:" + _data[0] + " data:" + _data[1]);
                if (agents != null)
                {
                    agents(_data[0], _data[1]);
                }
                else
                {
                    Debug.LogError("SDK消息总代为空 请检查代码");
                }
            }
            else {
                Debug.LogError("SDK消息回调异:"+data);
            }
        }
    }
}
