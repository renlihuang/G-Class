using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{

    public class MessageQueue
    {
        private ConcurrentQueue<MessageItemBase> _baseMessageItems;
        public MessageQueue()
        {
            _baseMessageItems = new ConcurrentQueue<MessageItemBase>();
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="itemBase"></param>
        public void PostMessage(MessageItemBase itemBase)
        {
            _baseMessageItems.Enqueue(itemBase);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        public MessageItemBase GetMessage()
        {
            MessageItemBase messageItemBase = null;

            if (!_baseMessageItems.IsEmpty)
            {
                _baseMessageItems.TryDequeue(out messageItemBase);
            }

            return messageItemBase;
        }
    }
}
