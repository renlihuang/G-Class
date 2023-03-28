using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using MESwebservice.OCVgetData;
using MESwebservice.OCVCheckData;
using System;
using System.Collections.Generic;
using CS.Base.AppSet;
using DCS.TASK.ITEM;
using DCS.BASE;

namespace DCS.TASKITEM.Test
{
    internal class OCVtest : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;
        public OCVtest(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        RequestToHttpHelper _requestToHttpHelper;


        static CollectTaskContext _collectTaskContext;
        string plcDataStatus;
        string mesDataStatus;
        string needleCount;
        string Apihost;
        static ILogOperator _log;


        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            plcDataStatus = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            mesDataStatus = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            needleCount = _collectTaskContext.DataMap.GetDataByKey("NeedleCount");
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();

            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[needleCount] = null;
            //判断是否读取成功
            if (opc.ReadNodes(data))
            {
                foreach (var item in data)
                {
                    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                }
                int PlcFalg = Convert.ToInt16(data[plcDataStatus]);
                int MesFalg = Convert.ToInt16(data[mesDataStatus]);
                string Needlenum = data[needleCount].ToString();
                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;
                if (PlcFalg == 1 && isFinish)
                {
                    _collectTaskContext.TaskMsgOperator.SetText("OVC测试开始处理PLC数据");
                    _log.AddUserLog("读取数据", "读取PLC", "PLC标志位等于" + PlcFalg);
                    //ocv mes交互
                    CheckOCVdata(@AppConfig.WebserviceiniPath, "A030电芯判定数据");
                    Dictionary<string, object> writeTags = new Dictionary<string, object>();
                    //采集完成，写入MES标志位
                    short writeResult = -1;
                    if (true)
                    {

                    }
                    writeTags[mesDataStatus] = Convert.ToInt16(2);
                    opc.WriteNodes(writeTags);

                    #region 初始化实体对象
                    BatteryCoreOcvTestEntity Bi = new BatteryCoreOcvTestEntity();
                    Bi.BatteryCoreCode = "1214";
                    Bi.LocationCode = "1";
                    Bi.OcvVoltage = "1";
                    Bi.Temperature = "1";
                    Bi.OcvOfflineTime = DateTime.Now;
                    Bi.OcvOnlineTime = DateTime.Now;
                    Bi.BatteryCoreDownVoltage2 = "1";
                    Bi.OcvOnmVoltage = "1";
                    Bi.SingleOcvVoltageSpec = "1";
                    Bi.OcvOnt = "1";
                    Bi.OcvOffsett = "1";
                    Bi.OcvDiff = "1";
                    Bi.Ocvresult = "1"; 
                    #endregion
                    AddWarningAync(Bi);
                }
            }

        }
        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<string> AddWarningAync(BatteryCoreOcvTestEntity batteryCoreOcvTestEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/BatteryCoreOcvTest/insert",
                Data = batteryCoreOcvTestEntity
            }).Result;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private static bool GetOCVdata(string v1, string v2)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            Dictionary<string, string> pairs = IniFileAPI.INIGetAllItemsDict(v1, v2);
            var wsService = new MiBatchStartAndGetParameterServiceService();
            wsService.Url = pairs["MESWSDL"];
            wsService.Timeout=Convert.ToInt32( pairs["TimeOut"]);
            wsService.Credentials = new System.Net.NetworkCredential(pairs["Username"], pairs["Password"], null);
            wsService.PreAuthenticate = true;
            var req = new miBatchStartAndGetParameter();
            var req_arg = new batchStartAndGetParamRequest();
            req_arg.site = pairs["site"];
            req_arg.user = pairs["user"];
            req_arg.operation = pairs["operation"];
            req_arg.operationRevision = pairs["operationRevision"];            
            req_arg.resource = pairs["Resource"];
            req_arg.processLot = pairs["Resource"];

            return true;
        }

        /// <summary>
        /// 判定ocv数据
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        private static bool CheckOCVdata(string inipath,string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            Dictionary<string, string> pairs = IniFileAPI.INIGetAllItemsDict(inipath,sitename);
            var wsService = new MiCustomDCForCellServiceService();
            wsService.Url = pairs["MESWSDL"];
            wsService.Timeout =Convert.ToInt32( pairs["TimeOut"]);
            wsService.Credentials = new System.Net.NetworkCredential(pairs["Username"], pairs["Password"], null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new cellCustomDCCheck();
            var req_arg = new miCustomDCForCellRequest();
            req_arg.site= pairs["site"];
            req_arg.dcSequence= pairs["dcSequence"];
            req_arg.user= pairs["user"];
            req_arg.Multispec= pairs["Multispec"];
            req_arg.operation= pairs["operation"];
            req_arg.resource= pairs["Resource"];
            miCustomDCForCellInventory inventory = new miCustomDCForCellInventory();
            inventory.inventoryId = pairs["inventoryId"];
            inventory.marking = pairs["marking"];
            miCustomDCForCellInventoryData inventoryData= new miCustomDCForCellInventoryData();
            inventoryData.name = pairs["dcname"];
            inventoryData.value = pairs["dcvalue"];
            List<miCustomDCForCellInventoryData> miCustomDCForCellInventoryDatas = new List<miCustomDCForCellInventoryData>();
            miCustomDCForCellInventoryDatas.Clear();
            miCustomDCForCellInventoryDatas.Add(inventoryData);
            inventory.inventoryDatalist = miCustomDCForCellInventoryDatas.ToArray();    
            List<miCustomDCForCellInventory> miCustomDCForCellInventories=  new List<miCustomDCForCellInventory>();
            miCustomDCForCellInventories.Clear();
            miCustomDCForCellInventories.Add(inventory);
            req_arg.inventoryList= miCustomDCForCellInventories.ToArray();
            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            try
            {
                cellCustomDCCheckResponse cellCustomDCCheck = wsService.cellCustomDCCheck(req as cellCustomDCCheck);
                code = cellCustomDCCheck.@return.code;
                msg = cellCustomDCCheck.@return.message;
                endtime = DateTime.Now;
            }
            catch (Exception ex)
            {
                isSaveCsvFile = false;
                isSaveCsvlogFile = false;
                _log.AddUserLog("OCV测试", "OCV测试", string.Format("调用接口异常，异常代码为{0}", ex.Message));
            }
            if (code == 0)
            {
                _log.AddUserLog("OCV测试", "OCV测试", string.Format("接口调用成功"));
            }
            else if (code == -1) 
            {
                _collectTaskContext.TaskMsgOperator.SetText("接口调用失败，请检查网络状况");
            }
            else
            {
                _log.AddUserLog("OCV测试", "OCV测试", string.Format("进站接口调用失败,CODE：" + code + "，错误信息:" + msg));
            }

            return code == 0;

        }

        public void DoUnInit()
        {

        }
    }
}
