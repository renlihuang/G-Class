using CS.View.Common;
using CS.View.View.Dlg;
using CS.View.ViewModel.Base;
using CS.View.ViewModel.BusinessViewModel.ParamEdit;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.BusinessViewModel
{
    class ParamEditViewModel: ViewModelBase
    {
        public ParamEditViewModel()
        {
            //打开配置文件
            OpenFileCommand = new RelayCommand(OpenFile);
            //下拉框选中命令
            SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
            //编辑参数命令
            EditParamCommand = new RelayCommand<GridItem>(EditParam);

            //界面与显示的关闭
            _dicGrids = new Dictionary<string, ObservableCollection<GridItem>>();
            //执行初始化
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            string filePath = GetFilePath();

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                OpenFilePath = filePath;
                //解析配置文件内容
                LoadConfigFile();
            }
        }


        /// <summary>
        ///打开文件
        /// </summary>
        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //
            openFileDialog.Filter = "配置文件|*.ini*";
            openFileDialog.ShowDialog();

            string filePath = openFileDialog.FileName;

            if (!string.IsNullOrEmpty(filePath))
            {
                if (OpenFilePath != filePath)
                {
                    OpenFilePath = filePath;
                }
                //保存一下路径
                SaveFilePath();
                //解析配置文件内容
                LoadConfigFile();
            }

        }

        /// <summary>
        /// 保存文件路径
        /// </summary>
        private void SaveFilePath()
        {
            string iniFileName = System.Environment.CurrentDirectory + "\\config.ini";

            if (!File.Exists(iniFileName))
            {
                File.Create(iniFileName);
            }

            IniFileAPI.INIWriteValue(iniFileName, "config", "filePath", OpenFilePath);
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <returns></returns>
        private string GetFilePath()
        {
            string result = string.Empty;

            string iniFileName = System.Environment.CurrentDirectory + "\\config.ini";

            if (File.Exists(iniFileName))
            {
                result = IniFileAPI.INIGetStringValue(iniFileName, "config", "filePath", OpenFilePath);
            }

            return result;
        }


        /// <summary>
        /// 加载配置
        /// </summary>
        private void LoadConfigFile()
        {
            _dicGrids.Clear();
            //显示的配置项
            var sectionItems = new ObservableCollection<SectionItem>();
            //获取所有参数名
            var sectionNames = IniFileAPI.INIGetAllSectionNames(OpenFilePath);
            //遍历
            foreach (string name in sectionNames)
            {
                if (!_dicGrids.ContainsKey(name))
                {
                    _dicGrids[name] = new ObservableCollection<GridItem>();
                    //添加到列表
                    sectionItems.Add(new SectionItem { Name = name });
                }
                //获取明细
                var gridItems = _dicGrids[name];
                //获取参数值和values
                var allItems = IniFileAPI.INIGetAllItems(OpenFilePath, name);

                foreach (string item in allItems)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] items = item.Split('=');

                        if (items != null && items.Length == 2)
                        {
                            gridItems.Add(new GridItem
                            {
                                Key = items[0],
                                Value = items[1]
                            });
                        }
                    }
                }
            }

            SectionNames = sectionItems;

            if (SectionNames.Count > 0)
            {
                SelectionChanged(SectionNames[0]);
            }
        }

        /// <summary>
        /// listBox选中事件
        /// </summary>
        /// <param name="item"></param>
        private void SelectionChanged(object item)
        {
            if (item != null)
            {
                string name = (item as SectionItem).Name;

                if (_dicGrids.ContainsKey(name))
                {
                    _selectedSectionName = name;
                    GridItems = _dicGrids[name];
                }
            } 
        }

        /// <summary>
        /// 编辑参数
        /// </summary>
        /// <param name="gridItem"></param>
        private async void EditParam(GridItem gridItem)
        {
            //创建viewModel
            ParamModifyViewModel ViewModel = new ParamModifyViewModel()
            {
                ParamKey = gridItem.Key,
                ParamValue = gridItem.Value
            };

            BaseMsgDialog baseMsgDialog = new BaseMsgDialog();
            //绑定viewModel
            baseMsgDialog.BindDataContex<ParamModifyDialog, ParamModifyViewModel>(new ParamModifyDialog(), ViewModel);

            bool isSave = await baseMsgDialog.ShowDialog();

            //是否保存成功
            if (isSave)
            {
                if (ViewModel.ParamValue != gridItem.Value)
                {
                    gridItem.Value = ViewModel.ParamValue;
                    //写入配置文件
                    SaveToIniFile(gridItem);
                }
            }
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="gridItem"></param>
        private void SaveToIniFile(GridItem gridItem)
        {
            //写入配置文件
            IniFileAPI.INIWriteValue(OpenFilePath, _selectedSectionName, gridItem.Key, gridItem.Value);
        }

     


        /// <summary>
        /// 当前选中的名称
        /// </summary>
        private string _selectedSectionName;

        /// <summary>
        /// 当前选中的索引
        /// </summary>
        private int _selectedItemIndex;
        public int SelectedItemIndex
        {
            set { _selectedItemIndex = value; RaisePropertyChanged(); }
            get { return _selectedItemIndex; }
        }



        /// <summary>
        /// 当前要显示的参数
        /// </summary>
        private ObservableCollection<GridItem> _gridItems;

        public ObservableCollection<GridItem> GridItems
        {
            set { _gridItems = value; RaisePropertyChanged(); }
            get { return _gridItems; }
        }

        /// <summary>
        /// 当前文件路径
        /// </summary>
        private string _openFilePath;
        public string OpenFilePath
        {
            set { _openFilePath = value; RaisePropertyChanged(); }
            get { return _openFilePath; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ObservableCollection<GridItem>> _dicGrids;

        /// <summary>
        /// 
        /// </summary>
        /// 
        ObservableCollection<SectionItem> _sectionNames;
        public ObservableCollection<SectionItem> SectionNames 
        {
            private set { _sectionNames = value;RaisePropertyChanged();} 
            get { return _sectionNames; } 
        }

        /// <summary>
        /// 打开文件命令
        /// </summary>
        public RelayCommand OpenFileCommand { private set; get; }

        /// <summary>
        /// 最小化
        /// </summary>
        public RelayCommand MinimizeWindowCommand { private set; get; }

        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        public RelayCommand CloseWindowCommand { private set; get; }

        /// <summary>
        /// 最大化命令
        /// </summary>
        public RelayCommand MaxWindowCommand { private set; get; }

        /// <summary>
        /// 最大化命令
        /// </summary>
        public RelayCommand<object> SelectionChangedCommand { private set; get; }

        /// <summary>
        /// 编辑参数命令
        /// </summary>
        public RelayCommand<GridItem> EditParamCommand { private set; get; }
    }
}
