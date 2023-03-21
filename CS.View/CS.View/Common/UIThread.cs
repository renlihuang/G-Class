using System.Windows.Threading;

namespace CS.View.Common
{
    /// <summary>
    ///在UI线程上执行对象
    /// </summary>
    internal class UIThread
    {
        /// <summary>
        ///
        /// </summary>
        public static Dispatcher Dispatcher { get; private set; }

        public UIThread(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
    }
}