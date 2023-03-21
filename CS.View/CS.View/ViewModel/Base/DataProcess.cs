using CS.View.Common.Enum;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace CS.View.ViewModel.Base
{
    /// <summary>
    ///处理单页增删改查基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    partial class DataProcess<T> : ViewModelBase
        where T : new()
    {
        /// <summary>
        /// 工具栏按钮
        /// </summary>
        private ObservableCollection<BaseToolBar<T>> _toolBarButtons = new ObservableCollection<BaseToolBar<T>>();

        /// <summary>
        /// 工具栏按钮
        /// </summary>
        public ObservableCollection<BaseToolBar<T>> ToolBarButtons
        {
            get { return _toolBarButtons; }
        }

        private ObservableCollection<BaseToolBar<T>> _operatorBarButtons = new ObservableCollection<BaseToolBar<T>>();

        /// <summary>
        /// 工具栏按钮
        /// </summary>
        public ObservableCollection<BaseToolBar<T>> OperatorBarButtons
        {
            get { return _operatorBarButtons; }
        }

        private ObservableCollection<BaseToolBar<T>> _detailButtons = new ObservableCollection<BaseToolBar<T>>();

        /// <summary>
        /// 明细按钮
        /// </summary>
        public ObservableCollection<BaseToolBar<T>> DetailButtons
        {
            get { return _detailButtons; }
        }

        private ObservableCollection<T> _dataGridDatas = new ObservableCollection<T>();

        /// <summary>
        /// 列表数据
        /// </summary>
        public ObservableCollection<T> DataGridDatas
        {
            get { return _dataGridDatas; }
        }

        /// <summary>
        /// 添加命令
        /// </summary>
        public RelayCommand<T> AddCommand { set; get; }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public RelayCommand<T> EditCommand { set; get; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public RelayCommand<T> DeleteCommand { set; get; }

        private T _model;

        /// <summary>
        /// 当前操作的实体
        /// </summary>
        public T Model
        {
            get { return _model; }
            set { _model = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 操作模式
        /// </summary>
        public ActionMode Mode { get; set; }

        private int _tabPageIndex;

        /// <summary>
        /// tab页面索引
        /// </summary>
        public int TabPageIndex
        {
            get { return _tabPageIndex; }
            set { _tabPageIndex = value; RaisePropertyChanged(); }
        }
          
        /// <summary>
        /// 提示文本
        /// </summary>
        string _hnitText;
        public string HnitText
        {
            set { _hnitText = value;RaisePropertyChanged(); }
            get { return _hnitText; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {
            InitCommand();
            //初始化工具栏按钮
            SetDefaultToolBarButtons();
            //跳转默认叶
            GetPageData(PageIndex);
        }

        /// <summary>
        /// 初始化命令
        /// </summary>
        public void InitCommand()
        {
            EditCommand = new RelayCommand<T>(Edit);
            DeleteCommand = new RelayCommand<T>(Delete);
        }

        /// <summary>
        /// 设置默认按钮
        /// </summary>
        protected virtual void SetDefaultToolBarButtons()
        {
            //默认工具栏
            _toolBarButtons.Add(new BaseToolBar<T>() { Text = "添加", Icon = "Plus", Command = new RelayCommand<T>(Add) });
            //查询按钮
            _operatorBarButtons.Add(new BaseToolBar<T>() { Text = "查询", Icon = "Magnify", Command = new RelayCommand<T>((t) => Query()) });
            _operatorBarButtons.Add(new BaseToolBar<T>() { Text = "重置", Icon = "Refresh", Command = new RelayCommand<T>((t) => Reset()) });
            //添加和编辑明细按钮
            _detailButtons.Add(new BaseToolBar<T>() { Text = "保存", Icon = "Check", Command = new RelayCommand<T>((t) => Save()) });
            _detailButtons.Add(new BaseToolBar<T>() { Text = "取消", Icon = "Close", Command = new RelayCommand<T>((t) => Cancel()) });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        protected virtual void Add(T model)
        {
            Model = new T();
            TabPageIndex = 1;
            //添加模式
            Mode = ActionMode.Add;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        protected virtual void Edit(T model)
        {
            Model = model;

            Mode = ActionMode.Edit;
            //设置编辑模式
            TabPageIndex = 1;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        protected virtual void Delete(T model)
        {
            //删除表
            if (_dataGridDatas.Contains(model))
            {
                _dataGridDatas.Remove(model);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        protected virtual void Query()
        {
            //从第一页开始查询
            PageIndex = 1;
            //查询数据
            GetPageData(PageIndex);
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        protected virtual void Reset()
        {
        }

        /// <summary>
        /// 保存
        /// </summary>
        protected virtual void Save()
        {
            //删除获取增加了以后需要重新加载数据
            GetPageData(PageIndex);
            TabPageIndex = 0;
        }

        /// <summary>
        /// 取消
        /// </summary>
        protected virtual void Cancel()
        {
            TabPageIndex = 0;
        }
    }

    partial class DataProcess<T>
    {
        private int _totailCount;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotailCount
        {
            get 
            {
                return _totailCount; 
            }

            set
            {
                //设置页数
                _totailCount = value;
                //设置页数
                SetPageCount();
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// 每页大小，默认显示20页
        /// </summary>
        private int _pageSize = 20;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; RaisePropertyChanged(); }
        }

        private int _pageCount;

        /// <summary>
        ///
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; RaisePropertyChanged(); }
        }

        private int _pageIndex = 1;

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; RaisePropertyChanged(); }
        }

        private RelayCommand _homPageCommand;

        /// <summary>
        /// 跳转到主页
        /// </summary>
        public RelayCommand GoHomPageCommand
        {
            get
            {
                if (_homPageCommand == null)
                {
                    _homPageCommand = new RelayCommand(GoHomPage);
                }
                return _homPageCommand;
            }
        }

        private RelayCommand _endPageCommand;

        /// <summary>
        /// 跳转到主页
        /// </summary>
        public RelayCommand GoEndPageCommand
        {
            get
            {
                if (_endPageCommand == null)
                {
                    _endPageCommand = new RelayCommand(GoEndPage);
                }
                return _endPageCommand;
            }
        }

        private RelayCommand _prevPageCommand;

        /// <summary>
        /// 跳转到上一叶
        /// </summary>
        public RelayCommand GoPrevPageCommand
        {
            get
            {
                if (_prevPageCommand == null)
                {
                    _prevPageCommand = new RelayCommand(GoPrevPage);
                }
                return _prevPageCommand;
            }
        }

        private RelayCommand _nextPageCommand;

        /// <summary>
        /// 跳转到主页
        /// </summary>
        public RelayCommand GoNextPageCommand
        {
            get
            {
                if (_nextPageCommand == null)
                {
                    _nextPageCommand = new RelayCommand(GoNextPage);
                }
                return _nextPageCommand;
            }
        }

        /// <summary>
        /// 设置页数
        /// </summary>
        protected virtual void SetPageCount()
        {
            PageCount = TotailCount / PageSize;
            //是否有尾页
            PageCount = (TotailCount % PageSize) > 0 ? PageCount + 1 : PageCount;
        }

        /// <summary>
        /// 跳转到首页
        /// </summary>
        protected virtual void GoHomPage()
        {
            if (PageIndex > 1)
            {
                PageIndex = 1;
                GetPageData(PageIndex);
            }
        }

        /// <summary>
        /// 跳转到尾页
        /// </summary>
        protected virtual void GoEndPage()
        {
            if (PageCount > 0)
            {
                PageIndex = PageCount;
                GetPageData(PageCount);
            }
        }

        /// <summary>
        /// 跳转到上一页
        /// </summary>
        protected virtual void GoPrevPage()
        {
            if (PageIndex > 1)
            {
                PageIndex--;
                GetPageData(PageIndex);
            }
        }

        /// <summary>
        /// 跳转到下一页
        /// </summary>
        protected virtual void GoNextPage()
        {
            if (PageIndex < PageCount)
            {
                PageIndex++;
                GetPageData(PageIndex);
            }
        }

        /// <summary>
        ///按页数获取数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected virtual void GetPageData(int pageInex) { }
   
    }
}