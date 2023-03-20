using DCS.CORE.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.Common
{
  

    internal class TaskMsg : ISetTaskMsg, IGetTaskMsg, IResetTaskMsg
    {
        /// <summary>
        /// 保存消息字典
        /// </summary>
        public ConcurrentDictionary<string, MsgItem> MsgItems { private set; get; } = new ConcurrentDictionary<string, MsgItem>();

        /// <summary>
        /// 消息
        /// </summary>
        private MsgItem _msgItem = new MsgItem();
        private MesItem _mesItem = new MesItem();

        public IDictionary<string, MsgItem> GetPairs()
        {
            return MsgItems;
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <returns></returns>
        public MsgItem GetText()
        {
            return _msgItem;
        }

        public void SetPairText(string key, string value)
        {
            if (!MsgItems.ContainsKey(key))
            {
                MsgItems[key] = new MsgItem()
                {
                    Text = value,
                    CreateTime = DateTime.Now.ToString()
                };
            }
            else
            {
                var item = MsgItems[key];
                item.Text = value;
                item.CreateTime = DateTime.Now.ToString();
            }
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            _msgItem.Text = text;
            _msgItem.CreateTime = DateTime.Now.ToString() ;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            MsgItems.Clear();
            _msgItem.Text = string.Empty;
        }

        public void SetMesText(string name, string code, string msg)
        {
            _mesItem.mesname= name;
            _mesItem.code= code;
            _mesItem.msg= msg;
            _mesItem.CreateTime= DateTime.Now.ToString();
        }
    }
}
