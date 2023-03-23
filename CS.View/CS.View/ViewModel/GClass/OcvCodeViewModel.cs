using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using CS.View.ViewModel.Base;
using DCS.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.ViewModel.GClass
{
    internal class OcvCodeViewModel: DataProcess<OCVcodeEntity>
    {

        private string  _code;

        public string  code
        {
            get { return _code; }
            set { _code = value;RaisePropertyChanged(); }
        }

        /// <summary>
        /// 参数管理名管理接口
        /// </summary>
        private readonly IGCblockService _petService;

        public OcvCodeViewModel(GCblockEntity  gCblockEntity,IGCblockService gCblockService)
        {

            this.Init();

            _petService = gCblockService;

            _ = GetPageDataAsync(gCblockEntity);

        }


        protected async Task GetPageDataAsync(GCblockEntity gCblockEntity)
        {
            ////查询数据
            var result = await _petService.GetGCblockAsync(1, PageSize, new GCblockCondition()
            {
                VirtualCode = gCblockEntity.VirtualCode,
            });
            DataGridDatas.Clear();
            TotailCount = result.Total;
            //填充数据
            if (result.Data != null)
            {
                if (result.Data.Count>0)
                {
                    code = result.Data.First().VirtualCode;
                }
                List<OCVcodeEntity> listocv= new List<OCVcodeEntity>();
                for (int i = 0; i < 49; i++)
                {
                    listocv.Add(new OCVcodeEntity { BatteryCode = i.ToString() });
                }
                var aa = listocv.ToJson();
                List<OCVcodeEntity> list =JsonHelper.DeserializeObject<List<OCVcodeEntity>>(aa);
                if (list!=null)
                {
                    foreach (var item in list)
                    {
                        DataGridDatas.Add(item);
                    }
                }
                else
                {
                    DataGridDatas.Add(new OCVcodeEntity { BatteryCode = "空" });
                }
                
            }
        }
    }
}
