using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.Common;
using CS.View.Common.Enum;
using CS.View.ViewModel.Base;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CS.View.ViewModel.BusinessViewModel
{
    class ModuleTypeViewModel: DataProcess<ModuleTypeEntity>
    {
        readonly IModuleTypeService _moduleTypeService;
        public ModuleTypeViewModel(IModuleTypeService moduleTypeService)
        {
            _moduleTypeService = moduleTypeService;
            //创建命令
            UploadImageComaand = new RelayCommand(UploadImage);
            //初始化
            this.Init();
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        DateTime _startTime = DateTime.Now.AddDays(-1);
        public DateTime StartTime
        {
            set
            {
                _startTime = value;
            }

            get
            {
                return _startTime;
            }
        }


        /// <summary>
        /// 结束时间
        /// </summary>
        DateTime _endTime = DateTime.Now;
        public DateTime EndTime
        {
            set
            {
                _endTime = value;
            }

            get
            {
                return _endTime;
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pageInex"></param>
        protected async override void GetPageData(int pageInex)
        {
          //查询数据  
           var queryResult =  await _moduleTypeService.GetModuleTypeListAsync(pageInex, PageSize, new ModuleTypeCondition() {  ModuleCode = ModuleNo});

            if (queryResult.Data != null)
            {
                DataGridDatas.Clear();

                foreach (var item in queryResult.Data)
                {
                    DataGridDatas.Add(item);
                }
            }
        }

        protected override void Add(ModuleTypeEntity model)
        {
            BatteryCount = string.Empty;
            ModuleImage = null;
            ImageName = string.Empty;
            base.Add(model);
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="model"></param>
        protected override void Edit(ModuleTypeEntity model)
        {
            ModuleImage = ImageHelp.CreateBitmapImage(model.ImageBytes);
            ImageName = model.ModuleImage;
            //
            BatteryCount = model.BatteryCount.ToString();
            base.Edit(model);
        }



        /// <summary>
        /// s's
        /// </summary>
        protected async override void Save()
        {

            bool result = false;

            if (string.IsNullOrEmpty(Model.ModuleName))
            {
                HnitText = "请输入模组名称";
                return;
            }

            if (string.IsNullOrEmpty(Model.ModuleCode))
            {
                HnitText = "请输入模组编码";
                return;
            }

            if (string.IsNullOrEmpty(ImageName))
            {
                HnitText = "请选择模组图片";
                return;
            }

            if (string.IsNullOrEmpty(BatteryCount))
            {
                HnitText = "请输入电芯数量";
                return;
            }

            Model.BatteryCount = int.Parse(BatteryCount);

            if (Mode == ActionMode.Add)
            {
                result = await _moduleTypeService.AddModuleTypeAsync(Model);
            }
            else if (Mode == ActionMode.Edit)
            {
                result = await _moduleTypeService.UpdateModuleTypeAsync(Model.Id,Model);
            }

            if (result)
            {
                base.Save();
            }
        }


        protected override void Cancel()
        {
            base.Cancel();
            //
    
        }


        protected async override void Delete(ModuleTypeEntity model)
        {
            bool result = await SkinMessageBox.Question("确认删除?");

            if (result)
            {
                if (await _moduleTypeService.DeleteModuleTypeAsync(model.Id))
                {
                    base.Delete(model);
                }
            }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        private void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "配置文件|*.jpg*";
            openFileDialog.Title = "请选择图片";
            //显示对话框
            openFileDialog.ShowDialog();

            var filePath = openFileDialog.FileName;

            if (!string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    ImageName = Path.GetFileName(filePath);
                    //读取文件
                    var bytes = File.ReadAllBytes(filePath);
                    Model.ImageBytes = bytes;
                    Model.ModuleImage = ImageName;
                    //设置图片
                    ModuleImage = ImageHelp.CreateBitmapImage(bytes);
                }
            }
        }

        /// <summary>
        /// 模组编号
        /// </summary>
        string _moduleNo;
        public string ModuleNo
        {
            set { _moduleNo = value;RaisePropertyChanged();}
            get { return _moduleNo; }
        }

        /// <summary>
        /// 当前绑定图片
        /// </summary>
        BitmapImage _moduleImage;
        public BitmapImage ModuleImage
        {
            set 
            {
                if (_moduleImage != value)
                {
                    //这样设置防止占用太多内存而翻车
                    if (_moduleImage != null)
                    {
                        _moduleImage.StreamSource =  null;
                    }
                    _moduleImage = value;
                    RaisePropertyChanged();
                }
        
            }
            get { return _moduleImage; }
        }


        string _batteryCount;

        public string BatteryCount
        {
            set 
            {
                if (_batteryCount != value)
                {
                    _batteryCount = value;
                    RaisePropertyChanged();
                }

            }
            get { return _batteryCount; }
        }

        /// <summary>
        /// 图片名称
        /// </summary>
        string _imageName;
        public string ImageName
        {
            set { _imageName = value;RaisePropertyChanged(); }
            get { return _imageName; }
        }

        /// <summary>
        /// 上传图片命令
        /// </summary>
        public RelayCommand UploadImageComaand { private set; get; }
    }
}
