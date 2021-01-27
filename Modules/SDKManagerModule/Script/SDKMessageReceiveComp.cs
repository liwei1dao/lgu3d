using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace lgu3d
{
    /// <summary>
    /// SDK消息接收组件
    /// </summary>
    public class SDKMessageReceiveComp : ModelCompBase<SDKManagerModule>
    {
        private GameObject sdkobject;
        private Dictionary<string, SDKMessageAgent> agents;

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            sdkobject = new GameObject("SdkObject");
            sdkobject.AddComponent<SdkObject>().Init(MyModule.MessageReceive);
            GameObject.DontDestroyOnLoad(sdkobject);
            agents = new Dictionary<string, SDKMessageAgent>();
            base.LoadEnd();
        }

        public void MessageReceive(string msgId, string data) {
            if (agents.ContainsKey(msgId)){
                if (agents[msgId] != null){
                    agents[msgId](data);
                }
                else {
                    Debug.LogError("SDK 消息处理函数为空 msgId:" + msgId);
                }
            }
            else {
                Debug.LogWarning("SDK 没有注册对应消息的处理函数 msgId:" + msgId);
            }
        }

        public void RegisterMessageDeal(string msgId, SDKMessageAgent agent) {
            if (agents.ContainsKey(msgId)){
                Debug.LogWarning("SDK 重复注册SDK消息处理接口 msgId:" + msgId);
            }
            agents[msgId] = agent;
        }
    }
}
