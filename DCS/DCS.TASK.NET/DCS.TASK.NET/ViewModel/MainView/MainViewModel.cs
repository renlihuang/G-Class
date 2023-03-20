using CS.View.Common;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.TASK.NET.Common;
using DCS.TASK.NET.CommonControl;
using DCS.TASK.NET.Config;
using DCS.TASK.NET.ViewDlg;
using DCS.TASK.NET.ViewModel.Base;
using DCS.TASK.NET.ViewModel.MesShow;
using DCS.TASK.NET.ViewModel.TaskStatus;
using DCS.TASK.NET.ViewModel.TaskTags;
using DCS.TASK.NET.ViewModel.ViewDlgViewModel;
using DCS.TaskManage.FrameWork;
using DCS.TaskManage.Log;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DCS.TASK.NET.ViewModel.MainView
{
    internal partial class MainViewModel
    {
        /// <summary>
        /// 窗体加载完成
        /// </summary>
        protected async override void OnStartup()
        {
            //创建log操作
            LogHelp  log = new LogHelp();
            //关联回调
            log.SystemLogCallBack = OnSystemLogEvent;
            //关联LOG
            Global.GlobalAssemblyManage.SetLogOperator(log);
            //关联LOG
            _log = log;
            //获取是否是调试模式
            _isManage = IniFileConfig.Current.GetIntValue("UserConfig", "IsManage") == 1;
            //不是管理管理员将隐藏菜单
            TreeNodeFactory.IsManage = _isManage;
            //创建根节点
            CreateRoootNode();
            //加载配置模型
            var configModel = await SystemConfig.LoadConfigModelAsync();

            //判断配置模型是否加载成功
            if (configModel != null)
            {
                Global.GlobalDataMap = configModel.GlobalDataMap;
                //设置模型
                _rootNode.NodeModel = configModel;
                //加载配置树
                LoadTaskTree();
                //展开节点
                ExpandThree();
                //添加LOG
                _log.AddSystemLog("系统运行", "系统加载", "加载配置成功");
            }
            else
            {
                //添加LOG
                _log.AddSystemLog("系统运行", "系统加载", "加载配置失败");
                //设置全局的dataMap
                Global.GlobalDataMap = (_rootNode.NodeModel as TaskRootConfig).GlobalDataMap;
            }

            //创建默认页
            CreateDefaultTabPage();
            //启动定时器
            SetTimer(1.0);
        }

        /// <summary>
        /// UI线程定时器
        /// </summary>
        protected  override void OnTimer()
        {
            //保存LOG
            _log.SaveLogAsync();
            //处理消息
            ProcessMessage();
            //处理状态现象
            ProcessStatus();
        }


        /// <summary>
        /// 处理状态监控
        /// </summary>
        private void ProcessStatus()
        {
            //更新状态
            if (_currentUpdateStatus != null)
            {
                _currentUpdateStatus.UpdateData();
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        private void ProcessMessage()
        {
            for (int i = 0; i < 10; i++)
            {
                var message = Global.GlobalMessageQueue.GetMessage();
                //处理消息
                if (message == null)
                {
                    break;
                }

                switch (message.MessageType)
                {
                    //处理LOG消息
                    case MessageType.AddLog:
                        {
                            var logItem = message as logMessageItem;
                            //显示消息列表
                            LogItemViewModels.Insert(0, logItem.logItem);
                            //保留前两百条数据
                            if (LogItemViewModels.Count > 200)
                            {
                                LogItemViewModels.RemoveAt(LogItemViewModels.Count - 1);
                            }
                        }
                        break;
                    //连接状态改变
                    case MessageType.OpcUaStatus:
                        {
                            var opcStatusItem = message as OpcStatusItem;
                            //获取状态
                            var taskGroupStatus = opcStatusItem.taskGroupStatus;
                            //得到任务组
                            var taskGroup = taskGroupStatus.collectTask;
                            //查找对应数据
                            var taskGroupNode = _rootNode.FindTaskTreeNodeByItemData(taskGroup);
                            //查找对应节点
                            if (taskGroupNode != null)
                            {
                                string text = taskGroupStatus.nowStatus ? "已连接" : "已断开";
                                //设置名称
                                _log.AddSystemLog("系统运行", "OPC UA连接", $"OPC UA ：{taskGroup.GetTaskGroupInfo().GroupName} {text}");
                                //设置连接状态
                                taskGroupNode.Status = taskGroupStatus.nowStatus;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 加载任务树
        /// </summary>
        private void LoadTaskTree()
        {
            List<BaseTreeViewModel> treeList = new List<BaseTreeViewModel>();
            //添加根节点
            treeList.Add(_rootNode);
            //加载配置树
            while (treeList.Count > 0)
            {
                var frontNode = treeList[0];
                //移除元素
                treeList.RemoveAt(0);
                //加载子节点
                LoadChild(treeList, frontNode);
            }
        }

        /// <summary>
        /// 加载子节点
        /// </summary>
        /// <param name="treeList"></param>
        /// <param name="treeNode"></param>
        private void LoadChild(List<BaseTreeViewModel> treeList, BaseTreeViewModel treeNode)
        {
            #region 加载子节点
            BaseTreeViewModel newNode = null;

            if (treeNode.NodeType == TaskTreeNodeType.TaskRoot)
            {
                foreach (var item in (treeNode.NodeModel as TaskRootConfig).TaskDicrectories)
                {
                    //创建节点
                    newNode = TreeNodeFactory.Create(item.NodeType, item.Name, treeNode);
                    //关联参数
                    newNode.NodeModel = item;
                    //关联命令
                    newNode.ToolButtons = CreateMenus(item.NodeType, newNode);
                    //添加到节点
                    treeNode.Children.Add(newNode);
                    //节点大于0需要
                    if (item.TaskGroups.Count > 0)
                    {
                        treeList.Add(newNode);
                    }
                }
            }//任务目录
            else if (treeNode.NodeType == TaskTreeNodeType.TaskDicrectory)
            {
                foreach (var item in (treeNode.NodeModel as TaskDicrectory).TaskGroups)
                {
                    //创建节点
                    newNode = TreeNodeFactory.Create(item.NodeType, item.GroupName, treeNode);
                    //关联参数
                    newNode.NodeModel = item;
                    //创建任务组
                    var taskGroup = new CollectTaskGroup(item);
                    //启动任务组
                    taskGroup.Start();
                    //设置任务目录名称
                    taskGroup.TaskDicrectoryName = treeNode.Name;
                    //关联节点参数
                    newNode.NodeParam = taskGroup;
                    //关联命令
                    newNode.ToolButtons = CreateMenus(item.NodeType, newNode);
                    //添加到节点
                    treeNode.Children.Add(newNode);
                    //节点大于0需要
                    if (item.CollectTasks.Count > 0)
                    {
                        treeList.Add(newNode);
                    }
                }
            }//任务组
            else if (treeNode.NodeType == TaskTreeNodeType.TaskCollectGroup ||
                     treeNode.NodeType == TaskTreeNodeType.TaskTimerGroup)
            {
                foreach (var item in (treeNode.NodeModel as TaskGroupInfo).CollectTasks)
                {
                    //创建节点
                    newNode = TreeNodeFactory.Create(item.NodeType, item.TaskName, treeNode);
                    //关联参数
                    newNode.NodeModel = item;
                    //关联命令
                    newNode.ToolButtons = CreateMenus(item.NodeType, newNode);

                    //获取父节点
                    var taskGroup = treeNode.NodeParam as CollectTaskGroup;
                    //创建任务
                    var taskItem = new CollectTaskItem(item);
                    //设置LOG操作类
                    taskItem.SetLogOperator(taskGroup.GetLogOperator());
                    //设置LOG操作类
                    taskItem.SetTaskEvenLoop(taskGroup.GetEvenLoop());
                    //设置LOG操作类
                    taskItem.SetOpcUaOperator(taskGroup.GetOpcOperator());
                    //关联节点参数
                    newNode.NodeParam = taskItem;

                    //启动任务
                    bool isStart = false;

                    if (item.IsRunming)
                    {
                        isStart = taskItem.Start();

                        if (!isStart)
                        {
                            _log.AddSystemLog("系统运行", "用户操作", $"任务: {item.TaskName} 启动失败，请检查对应实现类");
                        }
                    }

                    //设置状态
                    newNode.Status = isStart;

                    //设置菜单状态
                    for (int i = 0; i < newNode.ToolButtons.Count; i++)
                    {
                        if (i == 1)
                        {
                            newNode.ToolButtons[i].IsEnable = isStart;
                        }
                        else
                        {
                            newNode.ToolButtons[i].IsEnable = !isStart;
                        }
                    }

                    //添加到节点
                    treeNode.Children.Add(newNode);
                }
            }
            #endregion
        }

        /// <summary>
        /// 展开任务树
        /// </summary>
        private void ExpandThree()
        {
            List<BaseTreeViewModel> treeList = new List<BaseTreeViewModel>();
            //添加根节点
            treeList.Add(_rootNode);
            //加载配置树
            while (treeList.Count > 0)
            {
                var frontNode = treeList[0];
                //移除元素
                treeList.RemoveAt(0);
                //加载子节点
                if (frontNode.Children.Count > 0)
                {
                    frontNode.IsExpand = true;
                }
                //循环遍历节点
                foreach (var item in frontNode.Children)
                {
                    //如果是有父节点
                    if (item.Children.Count > 0)
                    {
                        treeList.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        protected override async void OnClose()
        {
            //调试模式才提醒保存配置
            if (_isManage)
            {
                if (SkinMessageBox.Question("退出前保存一下配置?"))
                {
                    await SaveConfigAync();
                }
            }
        

            if (SkinMessageBox.Question("确认退出系统?"))
            {
                _log.AddSystemLog("系统运行", "用户操作", "用户退出了采集系统");
                //关闭窗口
                base.OnClose();
            }
        }

        /// <summary>
        /// 创建默认页面
        /// </summary>
        private void CreateDefaultTabPage()
        {
            //创建viewModel
            var viewModel =  new AllTaskStatusViewModel(_rootNode);
            //绑定viewModel
            UserControl taskInfo = new TaskInfoControl()
            {
                DataContext = viewModel
            };
            //添加默认页
            TabPages.Add(new TabPageViewModel
            {
                PageIcon = "Monitor",
                PageText = "全局任务状态",
                PageItem = taskInfo,
                ItemParam = viewModel
            });

            //任务状态viewModel
            _allTaskStatusTabPage = viewModel;

            //添加变量明细显示页
            UserControl taskTags = new TaskTagsControl()
            {
                DataContext = new TaskTagsViewModel()
            };

            //创建监控明细页
            _taskTagsTabPage = new TabPageViewModel()
            {
                PageIcon = "CheckboxMarkedOutline",
                PageText = "任务明细变量",
                PageItem = taskTags,
                ItemParam = taskTags.DataContext
            };


            UserControl taskTagsmew = new TaskTagsControl()
            {
                DataContext = new MesShoewViewModel()
            };
            //默认查看第一个
            _currentUpdateStatus = viewModel;
            //加载初始化加载数据
            viewModel.ReloadData();
        }




        /// <summary>
        /// 创建根节点
        /// </summary>
        private void CreateRoootNode()
        {
            //创建根节点
            _rootNode = TreeNodeFactory.Create(TaskTreeNodeType.TaskRoot, "采集任务管理");
            //关联参数
            _rootNode.NodeModel = new TaskRootConfig() { NodeType = TaskTreeNodeType.TaskRoot };
            //关联菜单
            _rootNode.ToolButtons = CreateMenus(TaskTreeNodeType.TaskRoot, _rootNode);
            //添加根节点
            TaskTreeNodes.Add(_rootNode);
        }

        /// <summary>
        /// log消息
        /// </summary>
        /// <param name="logInfo"></param>
        private void OnSystemLogEvent(LogInfo logInfo)
        {
            LogItemViewModel viewModel = new LogItemViewModel()
            {
                LogTime = logInfo.CreateTime,
                LogType = logInfo.LogFileName,
                LogText = logInfo.Text
            };
            //投递消息
            Global.GlobalMessageQueue.PostMessage(new logMessageItem() { logItem = viewModel });
        }

        /// <summary>
        /// 任务状态ViewModel
        /// </summary>
        AllTaskStatusViewModel  _allTaskStatusTabPage;

        /// <summary>
        /// 任务状态
        /// </summary>
        private TabPageViewModel _taskTagsTabPage;

        /// <summary>
        /// 当前LOG操作
        /// </summary>
        private ILogOperator _log;

        /// <summary>
        /// 当前需要更新数据接口
        /// </summary>
        private IUpdateStatus _currentUpdateStatus;

        /// <summary>
        /// 当前tab页面索引
        /// </summary>
        private int _tabPageSelectedIndex = 0;
        public int TabPageSelectedIndex
        {
            set 
            {
                if (_tabPageSelectedIndex != value)
                {
                    _tabPageSelectedIndex = value;
                    //设置当前要更新变量
                    _currentUpdateStatus = TabPages[value].ItemParam as IUpdateStatus;
                    //更新状态
                    RaisePropertyChanged();
                }
               
            }
            get { return _tabPageSelectedIndex; }
        }


        /// <summary>
        /// 当前所有的tab页集合
        /// </summary>
        public ObservableCollection<TabPageViewModel> TabPages { private set; get; } = new ObservableCollection<TabPageViewModel>();

        /// <summary>
        /// LOG显示
        /// </summary>
        public ObservableCollection<LogItemViewModel> LogItemViewModels { private set; get; } = new ObservableCollection<LogItemViewModel>();
    }

    internal partial class MainViewModel : BaseWindowViewModel
    {
        public MainViewModel(Window ownerWindow) : base(ownerWindow)
        {
            //任务目录命令
            AddDirectoryCommand = new RelayCommand<object>(AddDirectory);
            EditDirectoryCommand = new RelayCommand<object>(EditDirectory);
            DeleteDirectoryCommand = new RelayCommand<object>(DeleteDirectory);
            //设置全局参数命令
            SetGlobalParamCommand = new RelayCommand<object>(SetGlobalParam);
            //保存配置
            SaveConfigCommand = new RelayCommand<object>(SaveConfig);
            //任务组命令
            AddGroupCommand = new RelayCommand<object>(AddGroup);
            EditGroupCommand = new RelayCommand<object>(EditGroup);
            DeleteGroupCommand = new RelayCommand<object>(DeleteGroup);
            //采集任务组
            AddCollectionGroupCommand = new RelayCommand<object>(AddColletionGroup);
            //任务操作命令
            AddTaskCommand = new RelayCommand<object>(AddTask);
            SetTaskParamCommand = new RelayCommand<object>(SetTaskParam);
            DeleteTaskCommand = new RelayCommand<object>(DeleteTask);
            EditTaskCommand = new RelayCommand<object>(EditTask);
            StopTaskCommand = new RelayCommand<object>(StopTask);
            SatrtTaskCommand = new RelayCommand<object>(SatrtTask);
            //选择树节点
            TreeSelectedCommand = new RelayCommand<object>(TreeSelected);
        }


        /// <summary>
        /// 树节选中事件
        /// </summary>
        /// <param name="item"></param>
        private void TreeSelected(object item)
        { 
            var treeItem = item as BaseTreeViewModel;
            //是否是任务节点目录
            if (treeItem.NodeType == TaskTreeNodeType.TaskTimerItem ||
                treeItem.NodeType == TaskTreeNodeType.TaskCollectItem)
            {
                var taskItem = treeItem.NodeParam as CollectTaskItem;
                //只有任务运行时候才允许配置参数
                if (taskItem.IsRunming)
                {
                    //添加默认页
                    if (!_isOpenTagsPage)
                    {
                        TabPages.Add(_taskTagsTabPage);
                        _isOpenTagsPage = true;
                    }
                    //设置名称
                    _taskTagsTabPage.PageText = taskItem.GetCollectTaskInfo().TaskName + "(变量监控)";
                    //获取标签对应的viewModel
                    var viewModel = _taskTagsTabPage.ItemParam as TaskTagsViewModel;
                    //设置
                    viewModel.SetTaskData(taskItem.TaskMsg.GetPairs());
                    //设置选中页
                    TabPageSelectedIndex = 1;
                }
            
            }
        }


        /// <summary>
        /// 添加目录
        /// </summary>
        /// <param name="item"></param>
        private void AddDirectory(object item)
        {
            AddDirectoryDlg addDirectoryDlg = new AddDirectoryDlg();
            var viewModel = new AddDirectoryViewModel(addDirectoryDlg);
            addDirectoryDlg.DataContext = viewModel;
            //显示对话框
            addDirectoryDlg.ShowDialog();
            //判断是否添加节点
            if (viewModel.DialogResult)
            {
                //获取当前菜单
                var menuItem = item as PopupBoxViewModel;
                //获取树节点
                var treeNode = menuItem.ItemParam as BaseTreeViewModel;
                //创建节点
                var newNode = TreeNodeFactory.Create(TaskTreeNodeType.TaskDicrectory, viewModel.DirectoryName, treeNode);
                //关联参数
                newNode.NodeModel = new TaskDicrectory()
                {
                    Name = viewModel.DirectoryName,
                    NodeType = TaskTreeNodeType.TaskDicrectory
                };
                //关联命令
                newNode.ToolButtons = CreateMenus(TaskTreeNodeType.TaskDicrectory, newNode);
                //添加到节点
                treeNode.Children.Add(newNode);
                //展开节点
                if (!treeNode.IsExpand)
                {
                    treeNode.IsExpand = true;
                }
            }
        }


        /// <summary>
        /// 编辑目录
        /// </summary>
        /// <param name="item"></param>
        private void EditDirectory(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //对话框
            AddDirectoryDlg addDirectoryDlg = new AddDirectoryDlg();
            //创建viewModel
            var viewModel = new AddDirectoryViewModel(addDirectoryDlg) { DirectoryName = treeNode.Name };
            addDirectoryDlg.DataContext = viewModel;
            //显示对话框
            addDirectoryDlg.ShowDialog();
            //判断是否添加节点
            if (viewModel.DialogResult)
            {
                if (viewModel.DirectoryName != treeNode.Name)
                {
                    _log.AddSystemLog("系统运行", "用户操作", $"目录名称由:{treeNode.Name}修改为:{viewModel.DirectoryName}");
                    //修改
                    treeNode.Name = viewModel.DirectoryName;

                    foreach(var node in treeNode.Children)
                    {
                       var taskGroup = node.NodeParam as CollectTaskGroup;
                        //更新命名空间名
                        taskGroup.TaskDicrectoryName = viewModel.DirectoryName;
                    }
                }
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="item"></param>
        private void DeleteDirectory(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //删除
            DeleteTreeNode(treeNode);
        }

        /// <summary>
        /// 添加任务组
        /// </summary>
        /// <param name="item"></param>
        private void AddGroup(object item)
        {
            //创建ViewModel
            AddGroupDlg addGroupDlg = new AddGroupDlg();
            //创建viewModel
            AddGroupNameDlgViewModel viewModel = new AddGroupNameDlgViewModel(addGroupDlg);
            //绑定ViewModel
            addGroupDlg.DataContext = viewModel;
            //显示对话框
            addGroupDlg.ShowDialog();
            //是否点了确定
            if (viewModel.DialogResult)
            {
                //获取当前菜单
                var menuItem = item as PopupBoxViewModel;
                //获取树节点
                var treeNode = menuItem.ItemParam as BaseTreeViewModel;
                //创建节点
                var newNode = TreeNodeFactory.Create(TaskTreeNodeType.TaskTimerGroup, viewModel.GroupName, treeNode);
                //得到任务信息
                var groupInfo = new TaskGroupInfo()
                {
                    GroupName = viewModel.GroupName,
                    NodeType = TaskTreeNodeType.TaskTimerGroup
                };
                //关联参数
                newNode.NodeModel = groupInfo;
                //创建任务组
                var taskGroup = new  CollectTaskGroup(groupInfo);
                //启动任务组
                taskGroup.Start();
                //设置任务目录名称
                taskGroup.TaskDicrectoryName = treeNode.Name;
                //关联节点参数
                newNode.NodeParam = taskGroup;
                //关联命令
                newNode.ToolButtons = CreateMenus(TaskTreeNodeType.TaskTimerGroup, newNode);
                //添加到节点
                treeNode.Children.Add(newNode);

                //展开节点
                if (!treeNode.IsExpand)
                {
                    treeNode.IsExpand = true;
                }
            }
        }

       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void EditGroup(object item)
        {
            //创建ViewModel
            AddGroupDlg addGroupDlg = new AddGroupDlg();
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;

            //获取任务组
            var groupInfo = treeNode.NodeParam as CollectTaskGroup;

            if (treeNode.NodeType == TaskTreeNodeType.TaskTimerGroup)
            {
                //创建viewModel
                AddGroupNameDlgViewModel viewModel = new AddGroupNameDlgViewModel(addGroupDlg) { GroupName = treeNode.Name };
                //绑定ViewModel
                addGroupDlg.DataContext = viewModel;
                //显示对话框
                addGroupDlg.ShowDialog();

                if (viewModel.DialogResult)
                {
                    treeNode.Name = viewModel.GroupName;
                    //更新任务组名称
                    groupInfo.UpdateGroupName();
                    //记录LOG
                    _log.AddSystemLog("系统运行", "用户操作", $"任务组名称由: {treeNode.Name} 修改为: {viewModel.GroupName}" );
                }
            }
            else
            {
                TaskGroupInfo taskGroupInfo = treeNode.NodeModel as TaskGroupInfo;
                //创建对话框
                AddCollectionGroupDlg addCollectionGroupDlg = new AddCollectionGroupDlg();
                //创建viewModel
                AddCollectionGroupViewModel viewModel = new AddCollectionGroupViewModel(addCollectionGroupDlg)
                {
                    GroupName = taskGroupInfo.GroupName,
                    OpcUrl = taskGroupInfo.OpcUaUrl,
                    NodeNS = taskGroupInfo.NodeNS
                };
                //关联viewModel
                addCollectionGroupDlg.DataContext = viewModel;
                //显示对话框
                addCollectionGroupDlg.ShowDialog();
                //是否确定
                if (viewModel.DialogResult)
                {
                    //名称更新
                    if (taskGroupInfo.GroupName != viewModel.GroupName)
                    {
                        taskGroupInfo.GroupName = viewModel.GroupName;
                        //更新任务组名称
                        groupInfo.UpdateGroupName();
                        //记录LOG
                        _log.AddSystemLog("系统运行", "用户操作", $"任务组名称由: {treeNode.Name} 修改为: {viewModel.GroupName}");
                    }
                    //URL更新
                    if (taskGroupInfo.OpcUaUrl != viewModel.OpcUrl)
                    {
                        taskGroupInfo.OpcUaUrl = viewModel.OpcUrl;
                        //重新连接PLC
                        groupInfo.PostReconnect();
                        //记录LOG
                        _log.AddSystemLog("系统运行", "用户操作", $"OPC URL 名称由: {taskGroupInfo.OpcUaUrl} 修改为: {viewModel.OpcUrl} 正在重新连接PLC");
                    }
                    //NS更新
                    if (taskGroupInfo.NodeNS != viewModel.NodeNS)
                    {
                        taskGroupInfo.NodeNS = viewModel.NodeNS;
                        //更新命名空间名
                        groupInfo.UpdateNameSpaceIndex();
                    }
                }
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void DeleteGroup(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //删除
            DeleteTreeNode(treeNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private  void DeleteTreeNode(BaseTreeViewModel baseTree)
        { 
            string msgText = string.Empty;
            //格式消息
            switch (baseTree.NodeType)
            {
                case TaskTreeNodeType.TaskDicrectory:
                    msgText = "该操作会删除任务目录下所有任务组和任务";
                    break;
                case TaskTreeNodeType.TaskTimerGroup:
                case TaskTreeNodeType.TaskCollectGroup:
                    msgText = "该操作会删除任务组和任务组下所有的任务";
                    break;
                case TaskTreeNodeType.TaskCollectItem:
                case TaskTreeNodeType.TaskTimerItem:
                    msgText = "该操作会删除该任务";
                    break;
            }

            if (!SkinMessageBox.Question(msgText))
            {
                return;
            }

            //删除节点
            DeleteTaskNode(baseTree);
            //停止服务
            TryStopServer(baseTree);
            //获取父节点
            var parentNode = baseTree.ParentNode;
            //删除当前节点
            parentNode.Children.Remove(baseTree);
            //重新加载任务列表
            _allTaskStatusTabPage.ReloadData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void DeleteTaskNode(BaseTreeViewModel baseTree)
        {
            foreach (var item in baseTree.Children)
            {
                if (item.Children.Count > 0)
                {
                    DeleteTaskNode(item);
                }

                TryStopServer(item);
            }
            //删除节点
            baseTree.Children.Clear();
        }


        /// <summary>
        /// 尝试停止服务
        /// </summary>
        /// <param name="item"></param>
        private void TryStopServer(BaseTreeViewModel item)
        {
            //删除任务类型
            switch (item.NodeType)
            {
                case TaskTreeNodeType.TaskCollectGroup:
                case TaskTreeNodeType.TaskTimerGroup:
                    var taskGroup = item.NodeParam as CollectTaskGroup;
                    _log.AddSystemLog("系统运行", "用户操作", $"已经停止任务组: {item.Name}");
                    //停止任务组
                    taskGroup.Stop();
                    break;
                case TaskTreeNodeType.TaskCollectItem:
                case TaskTreeNodeType.TaskTimerItem:
                    var taskItem = item.NodeParam as CollectTaskItem;
                    _log.AddSystemLog("系统运行", "用户操作", $"已经停止任务: {item.Name}");
                    //停止任务
                    taskItem.Stop();
                    break;
            }
        }


        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="item"></param>
        private void AddTask(object item)
        {
            AddTaskDlg addTaskDlg = new AddTaskDlg();
            //创建viewModel
            AddTaskViewModel viewModel = new AddTaskViewModel(addTaskDlg);
            //绑定viewModel
            addTaskDlg.DataContext = viewModel;
            //显示对话框
            addTaskDlg.ShowDialog();
            //判断是否要添加任务
            if (viewModel.DialogResult)
            {
                //获取当前菜单
                var menuItem = item as PopupBoxViewModel;
                //获取树节点
                var treeNode = menuItem.ItemParam as BaseTreeViewModel;
                //节点
                TaskTreeNodeType nodeType = TaskTreeNodeType.TaskCollectGroup == treeNode.NodeType ? TaskTreeNodeType.TaskTimerItem : TaskTreeNodeType.TaskCollectItem;
                //创建节点
                var newNode = TreeNodeFactory.Create(nodeType, viewModel.TaskName, treeNode);
                //关联命令
                newNode.ToolButtons = CreateMenus(nodeType, newNode);
                //关联参数
                var taskInfo = new CollectTaskInfo()
                {
                    TaskName = viewModel.TaskName,
                    Interval = viewModel.Interval,
                    NodeType = nodeType,
                    ClassName = viewModel.ClassName,
                };

                newNode.NodeModel = taskInfo;
                //获取父节点
                var taskGroup = treeNode.NodeParam as CollectTaskGroup;
                //创建任务
                var taskItem = new  CollectTaskItem(taskInfo);
                //设置LOG操作类
                taskItem.SetLogOperator(taskGroup.GetLogOperator());
                //设置LOG操作类
                taskItem.SetTaskEvenLoop(taskGroup.GetEvenLoop());
                //设置LOG操作类
                taskItem.SetOpcUaOperator(taskGroup.GetOpcOperator());
                //关联节点参数
                newNode.NodeParam = taskItem;
                //添加到节点
                treeNode.Children.Add(newNode);
                //是否展开
                if (!treeNode.IsExpand)
                {
                    treeNode.IsExpand = true;
                }

                //重新加载任务列表
                _allTaskStatusTabPage.ReloadData();
            }
        }


        /// <summary>
        /// 设置任务参数
        /// </summary>
        /// <param name="item"></param>
        private void SetTaskParam(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //获取节点关联数据
            var nodeModel = treeNode.NodeModel as CollectTaskInfo;
            //创建对话框
            AddParamDialog addParamDialog = new AddParamDialog();
            //创建viewModel
            AddParamViewModel viewModel = new AddParamViewModel(addParamDialog);
            //设置参数
            viewModel.SetParams(nodeModel.DicDataMap);
            //关联数据上下文
            addParamDialog.DataContext = viewModel;
            //显示对话框
            addParamDialog.ShowDialog();
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="item"></param>
        private void SatrtTask(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //得到关联的任务栏
            var toolBar = treeNode.ToolButtons;
            //启动任务
            CollectTaskInfo taskInfo = treeNode.NodeModel as CollectTaskInfo;
            //获取当前任务
            var taskItem = treeNode.NodeParam as CollectTaskItem;
            //启动任务
            taskInfo.IsRunming = taskItem.Start(); ;
            //是否启动任务
            if (taskInfo.IsRunming)
            {
                for (int i = 0; i < toolBar.Count; i++)
                {
                    if (i == 1)
                    {
                        toolBar[i].IsEnable = true;
                    }
                    else
                    {
                        toolBar[i].IsEnable = false;
                    }
                }

                _log.AddSystemLog("系统运行", "用户操作", $"任务: {taskInfo.TaskName} 已启动");
                //
                treeNode.Status = true;
            }
            else
            {
                _log.AddSystemLog("系统运行", "用户操作", $"任务: {taskInfo.TaskName} 启动失败，请检查对应实现类");
            }
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="item"></param>
        private void StopTask(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //得到关联的任务栏
            var toolBar = treeNode.ToolButtons;
            //启动任务
            CollectTaskInfo taskInfo = treeNode.NodeModel as CollectTaskInfo;
            //获取当前任务
            var taskItem = treeNode.NodeParam as CollectTaskItem;

            //是否启动任务
            if (taskInfo.IsRunming)
            {
                for (int i = 0; i < toolBar.Count; i++)
                {
                    if (i == 1)
                    {
                        toolBar[i].IsEnable = false;
                    }
                    else
                    {
                        toolBar[i].IsEnable = true;
                    }
                }

                //停止任务
                taskItem.Stop();

                _log.AddSystemLog("系统运行", "用户操作", $"任务: {taskInfo.TaskName} 已停止");

                //是否运行
                taskInfo.IsRunming = false;
                //设置状态
                treeNode.Status = false;
            }
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="item"></param>
        private void EditTask(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //获取节点数据
            var taskInfo = treeNode.NodeModel as CollectTaskInfo;
            //创建对话框
            AddTaskDlg addTaskDlg = new AddTaskDlg();
            //创建viewModel
            AddTaskViewModel viewModel = new AddTaskViewModel(addTaskDlg)
            {
                TaskName = taskInfo.TaskName,
                TaskCycle = taskInfo.Interval.ToString(),
                ClassName = taskInfo.ClassName,
            };
            //绑定viewModel
            addTaskDlg.DataContext = viewModel;
            //显示对话框
            addTaskDlg.ShowDialog();
            //是否选中
            if (viewModel.DialogResult)
            {
                if (taskInfo.TaskName != viewModel.TaskName)
                {
                    _log.AddSystemLog("系统运行", "用户操作", $"任务名称由 : {taskInfo.TaskName} 修改为: {viewModel.TaskName}");
                    taskInfo.TaskName = viewModel.TaskName;
                }

                if (taskInfo.Interval != viewModel.Interval)
                {
                    _log.AddSystemLog("系统运行", "用户操作", $"任务运行周期由 : {taskInfo.Interval}ms 修改为: {viewModel.Interval}ms");
                    taskInfo.Interval = viewModel.Interval;
                }

                if (taskInfo.ClassName != viewModel.ClassName)
                {
                    _log.AddSystemLog("系统运行", "用户操作", $"任务实现类由 : {taskInfo.ClassName} 修改为: {viewModel.ClassName}");
                    taskInfo.ClassName = viewModel.ClassName;
                }
        
            }
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="item"></param>
        private void DeleteTask(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //删除
            DeleteTreeNode(treeNode);
        }

        /// <summary>
        /// 添加采集任务
        /// </summary>
        /// <param name="item"></param>
        private void AddColletionGroup(object item)
        {
            //创建对话框
            AddCollectionGroupDlg addCollectionGroupDlg = new AddCollectionGroupDlg();
            //创建viewModel
            AddCollectionGroupViewModel viewModel = new AddCollectionGroupViewModel(addCollectionGroupDlg);
            //关联viewModel
            addCollectionGroupDlg.DataContext = viewModel;
            //显示对话框
            addCollectionGroupDlg.ShowDialog();
            //判断是否点击了确定
            if (viewModel.DialogResult)
            {
                //获取当前菜单
                var menuItem = item as PopupBoxViewModel;
                //获取树节点
                var treeNode = menuItem.ItemParam as BaseTreeViewModel;
                //创建树节点
                var newNode = TreeNodeFactory.Create(TaskTreeNodeType.TaskCollectGroup, viewModel.GroupName, treeNode);
                //关联菜单
                newNode.ToolButtons = CreateMenus(TaskTreeNodeType.TaskCollectGroup, newNode);
                //获取任务信息
                var groupInfo = new TaskGroupInfo()
                {
                    GroupName = viewModel.GroupName,
                    OpcUaUrl = viewModel.OpcUrl,
                    NodeNS = viewModel.NodeNS,
                    NodeType = TaskTreeNodeType.TaskCollectGroup
                };

                //关联数据
                newNode.NodeModel = groupInfo;
                //创建任务组
                var taskGroup = new CollectTaskGroup(groupInfo);
                //启动任务组
                taskGroup.Start();
                //设置任务目录名称
                taskGroup.TaskDicrectoryName = treeNode.Name;
                //关联节点参数
                newNode.NodeParam = taskGroup;

                //添加节点
                treeNode.Children.Add(newNode);
                //节点是否展开
                if (!treeNode.IsExpand)
                {
                    treeNode.IsExpand = true;
                }
            }
        }

        /// <summary>
        /// 设置全局参数
        /// </summary>
        /// <param name="item"></param>
        private void SetGlobalParam(object item)
        {
            //获取当前菜单
            var menuItem = item as PopupBoxViewModel;
            //获取树节点
            var treeNode = menuItem.ItemParam as BaseTreeViewModel;
            //获取节点关联数据
            var nodeModel = treeNode.NodeModel as TaskRootConfig;
            //创建对话框
            AddParamDialog addParamDialog = new AddParamDialog();
            //创建viewModel
            AddParamViewModel viewModel = new AddParamViewModel(addParamDialog);
            //设置参数
            viewModel.SetParams(nodeModel.GlobalDataMap);
            //关联数据上下文
            addParamDialog.DataContext = viewModel;
            //显示对话框
            addParamDialog.ShowDialog();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="item"></param>
        private  async void SaveConfig(object item)
        {
            await SaveConfigAync();
        }

        /// <summary>
        /// 创建配置
        /// </summary>
        private async Task SaveConfigAync()
        {
            //创建配置model
            var configModel = SystemConfig.CreateConfigModel(_rootNode);
            //保存配置
            bool result = await SystemConfig.SaveConfigAsync(configModel);
            //是否保存
            if (result)
            {
                _log.AddSystemLog("系统运行", "用户操作", "保存配置成功");
            }
            else
            {
                _log.AddSystemLog("系统运行", "用户操作", "保存配置失败");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        private ObservableCollection<PopupBoxViewModel> CreateMenus(TaskTreeNodeType nodeType, object owner)
        {
            ObservableCollection<PopupBoxViewModel> popupBoxViewModels = new ObservableCollection<PopupBoxViewModel>();
            //拥有管理员权限才加载菜单
            if (_isManage)
            {
                switch (nodeType)
                {
                    case TaskTreeNodeType.TaskRoot:
                        {
                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "添加任务目录",
                                IconName = "Add",
                                ExcuteCommand = AddDirectoryCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "设置全局参数",
                                IconName = "Settings",
                                ExcuteCommand = SetGlobalParamCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "保存当前配置",
                                IconName = "ContentSave",
                                ExcuteCommand = SaveConfigCommand
                            });
                        }
                        break;
                    case TaskTreeNodeType.TaskDicrectory:
                        {
                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "添加周期任务组",
                                IconName = "Add",
                                ExcuteCommand = AddGroupCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "添加采集任务组",
                                IconName = "Add",
                                ExcuteCommand = AddCollectionGroupCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "编辑任务目录",
                                IconName = "Edit",
                                ExcuteCommand = EditDirectoryCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "删除任务目录",
                                IconName = "Delete",
                                ExcuteCommand = DeleteDirectoryCommand
                            });
                        }
                        break;
                    case TaskTreeNodeType.TaskTimerGroup:
                    case TaskTreeNodeType.TaskCollectGroup:
                        {
                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "添加任务",
                                IconName = "Add",
                                ExcuteCommand = AddTaskCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "编辑任务组",
                                IconName = "Edit",
                                ExcuteCommand = EditGroupCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "删除任务组",
                                IconName = "Delete",
                                ExcuteCommand = DeleteGroupCommand
                            });

                        }
                        break;
                    case TaskTreeNodeType.TaskTimerItem:
                    case TaskTreeNodeType.TaskCollectItem:
                        {
                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "启动任务",
                                IconName = "Play",
                                ExcuteCommand = SatrtTaskCommand
                            });

                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "停止任务",
                                IconName = "Stop",
                                IsEnable = false,
                                ExcuteCommand = StopTaskCommand
                            });


                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "编辑任务",
                                IconName = "Edit",
                                ExcuteCommand = EditTaskCommand
                            });


                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "设置任务参数",
                                IconName = "Settings",
                                ExcuteCommand = SetTaskParamCommand
                            });



                            popupBoxViewModels.Add(new PopupBoxViewModel(owner)
                            {
                                Text = "删除任务",
                                IconName = "Delete",
                                ExcuteCommand = DeleteTaskCommand
                            });
                        }
                        break;
                }
            }


            return popupBoxViewModels;
        }


        /// <summary>
        /// 任务信息选择事件
        /// </summary>
        public RelayCommand<object> TreeSelectedCommand { private set; get; }

        /// <summary>
        /// 设置任务参数
        /// </summary>
        public RelayCommand<object> SetTaskParamCommand { private set; get; }

        /// <summary>
        /// 删除任务
        /// </summary>
        public RelayCommand<object> DeleteTaskCommand { private set; get; }

        /// <summary>
        /// 编辑任务
        /// </summary>
        public RelayCommand<object> EditTaskCommand { private set; get; }

        /// <summary>
        /// 停止任务
        /// </summary>
        public RelayCommand<object> StopTaskCommand { private set; get; }

        /// <summary>
        /// 启动任务命令
        /// </summary>
        public RelayCommand<object> SatrtTaskCommand { private set; get; }

        /// <summary>
        /// 添加任务命令
        /// </summary>
        public RelayCommand<object> EditGroupCommand { private set; get; }

        /// <summary>
        /// 删除任务组
        /// </summary>
        public RelayCommand<object> DeleteGroupCommand { private set; get; }

        /// <summary>
        /// 添加任务命令
        /// </summary>
        public RelayCommand<object> AddTaskCommand { private set; get; }

        /// <summary>
        /// 添加任务组
        /// </summary>
        public RelayCommand<object> AddGroupCommand { private set; get; }

        /// <summary>
        /// 添加采集任务组
        /// </summary>
        public RelayCommand<object> AddCollectionGroupCommand { private set; get; }


        /// <summary>
        /// 添加目录命令
        /// </summary>
        public RelayCommand<object> AddDirectoryCommand { private set; get; }

        /// <summary>
        /// 编辑目录命令
        /// </summary>
        public RelayCommand<object> EditDirectoryCommand { private set; get; }

        /// <summary>
        /// 删除目录命令
        /// </summary>
        public RelayCommand<object> DeleteDirectoryCommand { private set; get; }

        /// <summary>
        /// 设置参数命令
        /// </summary>
        public RelayCommand<object> SetGlobalParamCommand { private set; get; }

        /// <summary>
        /// 设置参数命令
        /// </summary>
        public RelayCommand<object> SaveConfigCommand { private set; get; }


        /// <summary>
        /// 根节点
        /// </summary>
        private BaseTreeViewModel _rootNode;

        /// <summary>
        /// 是否打开了变量监控页面
        /// </summary>
        private bool _isOpenTagsPage = false;

        /// <summary>
        /// 是否是管理员
        /// </summary>
        private bool _isManage = false;

        /// <summary>
        /// 任务树跟节点
        /// </summary>
        public ObservableCollection<BaseTreeViewModel> TaskTreeNodes { private set; get; } = new ObservableCollection<BaseTreeViewModel>();
    }
}
