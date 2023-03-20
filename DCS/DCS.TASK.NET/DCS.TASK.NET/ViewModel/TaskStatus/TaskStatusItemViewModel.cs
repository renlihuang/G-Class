using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASK.NET.ViewModel.TaskStatus
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public class TaskStatusItemViewModel : ViewModelBase
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        private string _taskName;
        public string TaskName
        {
            set
            {
                if (_taskName != value)
                {
                    _taskName = value;
                    RaisePropertyChanged();
                }

            }
            get { return _taskName; }
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        private bool _status;

        public bool Status
        { 
           set 
            {
                _status = value;
                //设置文本
                TaskStatus = _status == true ? "已启动" : "已停止";
                //设置填充颜色
                StatusColor = _status == true ? "Green" : "Red";
     
            }
           get { return _status; }
        }

        /// <summary>
        /// 状态颜色
        /// </summary>
        private string _statusColor; 

        public string StatusColor
        { 
           set 
            {
                if (_statusColor != value)
                {
                    _statusColor = value;
                    RaisePropertyChanged();
                }
            
            }
            get { return _statusColor; }
        }


        /// <summary>
        /// 任务状态
        /// </summary>
        private string _taskStatus;
        public string TaskStatus
        {
            private set
            {
                if (_taskStatus != value)
                {
                    _taskStatus = value;
                    RaisePropertyChanged();
                }

            }
            get { return _taskStatus; }
        }

        /// <summary>
        /// 任务运行周期
        /// </summary>
        private long _taskRuntime;
        public long TaskRuntime
        {
            set
            {
                _taskRuntime = value;
                if (_taskRuntime == 0)
                {
                    RunInterval = "<1ms";
                }
                else
                {
                    RunInterval = $"{_taskRuntime}ms";
                }
            }
            get { return _taskRuntime; }
        }


        /// <summary>
        /// 运行时间
        /// </summary>
        string _runInterval;

        public string RunInterval
        {
            set 
            {
                if (_runInterval != value)
                {
                    _runInterval = value;
                    RaisePropertyChanged();
                }
            }
            get { return _runInterval; }   
        }

        /// <summary>
        /// 任务运行周期
        /// </summary>
        private string _updateTime;
        public string UpdateTime
        {
            set
            {
                if (_updateTime != value)
                {
                    _updateTime = value;
                    RaisePropertyChanged();
                }

            }
            get { return _updateTime; }
        }

        /// <summary>
        /// 任务消息
        /// </summary>
        private string _taskMsg;
        public string TaskMsg
        {
            set
            {
                if (_taskMsg != value)
                {
                    _taskMsg = value;
                    RaisePropertyChanged();
                }

            }
            get { return _taskMsg; }
        }
    }
}
