namespace DCS.CORE.Interface
{
    /// <summary>
    /// 周期任务接口
    /// </summary>
    public interface IPeriodicTask
    {
        /// <summary>
        /// 任务初始化
        /// </summary>
        void DoInit(TimedTaskContext taskContext);

        /// <summary>
        /// 运行任务
        /// </summary>
        void DoTask();

        /// <summary>
        /// 任务停止运行
        /// </summary>
        void DoUnInit();
    }
}