using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.CORE.Interface
{
    /// <summary>
    /// 用于异步执行委托
    /// </summary>
    public interface IEventLoop
    {
        /// <summary>
        /// 添加任务到队列
        /// </summary>
        /// <param name="action"></param>
        void PostTaskEvent(Action action);

        /// <summary>
        /// 添加任务到队列
        /// </summary>
        /// <param name="action"></param>
        void PostTaskEvent(Action<object> action, object param);
    }
}
