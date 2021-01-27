using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 消息管理组件
    /// </summary>
    public class ServiceModuleMessageComp : ModelCompBase<ServiceModule>
    {
        private Queue<IMessage> ReceiveMessageQueue;           //消息接收队列
        private bool IsDealMesageing;                          //消息处理中
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            ReceiveMessageQueue = new Queue<IMessage>();
            base.Load(_ModelContorl, _Agr);
            IsDealMesageing = false;
            LoadEnd();
        }

        public void ReceiveMessage(IMessage Msg)
        {
            lock (ReceiveMessageQueue)
            {
                ReceiveMessageQueue.Enqueue(Msg);
                if (!IsDealMesageing)
                {
                    IsDealMesageing = true;
                    MyModule.VP(0, () =>
                    {
                        MyModule.StartCoroutine(DealMesageCoroutine());
                    });
                }
            }
        }

        /// <summary>
        /// 一帧处理一条消息
        /// </summary>
        /// <returns></returns>
        private IEnumerator DealMesageCoroutine()
        {
            while (ReceiveMessageQueue.Count > 0)
            {
                yield return null;
                lock (ReceiveMessageQueue)
                {
                    MyModule.DealMessage(ReceiveMessageQueue.Dequeue());
                }
            }
            IsDealMesageing = false;
        }
    }
}
