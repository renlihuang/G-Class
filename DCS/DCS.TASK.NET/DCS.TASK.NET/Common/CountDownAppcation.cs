using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


namespace CS.View.Common
{

    public class Win32Api
    {
        /// <summary>
        /// 坐标点
        /// </summary>
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override bool Equals(object obj)
            {
                POINT pt = (POINT)obj;
                return this.X == pt.X && this.Y == pt.Y;
            }


            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        /// <summary>   
        /// 获取鼠标的坐标   
        /// </summary>   
        /// <param name="lpPoint">传址参数，坐标point类型</param>   
        /// <returns>获取成功返回真</returns>   
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
    }


    //倒计时关闭应用程序
    public class CountDownAppcation
    {

        /// <summary>
        /// 是否激活
        /// </summary>
        bool _isActive { set; get; }

        /// <summary>
        /// 光标最后的位置
        /// </summary>
        Win32Api.POINT _lastCursorPoint;

        /// <summary>
        /// 停留次数
        /// </summary>
        int _curRemainCount;

        //最大停留次数
        int _maxRemainCount;

        /// <summary>
        /// 是否启用
        /// </summary>
        bool _isStart;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minute"></param>
        public void SetMinute(int minute)
        {
            _maxRemainCount = minute * 60 / 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            if (active)
            {
                _curRemainCount = 0;
            }

            _isActive = active;
        }

        /// <summary>
        /// 重启当前进程
        /// </summary>
        private void RestartApplication()
        {
            //获取当前应用程序路径和名称
            string friendlyName = System.AppDomain.CurrentDomain.FriendlyName + ".exe";
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            //创建进程
            Process process = new Process();
            process.StartInfo.FileName = baseDirectory + friendlyName;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            //停止当前进程
            Application.Current.Shutdown();
        }

        public void Start()
        {
            if (!_isStart)
            {
                DispatcherTimer timer = new DispatcherTimer();
                //十秒检测一一次
                timer.Interval = TimeSpan.FromSeconds(10);
                //关联事件
                timer.Tick += Timer_Tick;
                timer.Start();

                _isStart = true;
            }
        }

        /// <summary>
        /// 定时器回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            Win32Api.POINT pt;

            if (_isActive)
            {
                Win32Api.GetCursorPos(out pt);

                if (_lastCursorPoint.Equals(pt))
                {
                    _curRemainCount++;
                }
                else
                {
                    _lastCursorPoint = pt;
                    _curRemainCount = 0;
                }
            }
            else
            {
                _curRemainCount++;
            }

            //停留时间超过时间
            if (_curRemainCount >= _maxRemainCount)
            {
                //重新启动进程
                RestartApplication();
            }
        }
    }
}
