using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.CORE.Interface
{
    /// <summary>
    /// 消息
    /// </summary>
    public interface ISetTaskMsg
    {
        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="text"></param>
        void SetText(string text);

        /// <summary>
        /// 设置键值文本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetPairText(string key, string value);

    }

    public class MsgItem
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string CreateTime { set; get; }
    }

    /// <summary>
    /// 消息
    /// </summary>
    public interface IGetTaskMsg
    {
        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="text"></param>
        MsgItem GetText();

        /// <summary>
        /// 设置键值文本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        IDictionary<string, MsgItem> GetPairs();

    }

    /// <summary>
    /// 消息
    /// </summary>
    public interface IResetTaskMsg
    { 
        void Reset();
    }

}
