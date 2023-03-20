using DCS.CORE.Interface;

namespace DCS.CORE
{
    /// <summary>
    /// 定时任务上下文
    /// </summary>
    public class TimedTaskContext
    {
        /// <summary>
        /// 事件循环
        /// </summary>
        public IEventLoop EventLoop { set; get; }

        /// <summary>
        /// 数据获取已经配置的参数
        /// </summary>
        public IDataMap DataMap { set; get; }

        /// <summary>
        /// 提供LOG操作
        /// </summary>
        public ILogOperator LogOperator { set; get; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { set; get; }

        /// <summary>
        /// 消息操作
        /// </summary>
        public ISetTaskMsg TaskMsgOperator { set; get; }
        //清空消息
        public IResetTaskMsg RestMsgOperator { set; get; }
    }

    /// <summary>
    /// 采集任务上下文
    /// </summary>
    public class CollectTaskContext : TimedTaskContext
    {
        /// <summary>
        /// OPC操作相关
        /// </summary>
        public IOpcOperator OpcOperator;
    }
}