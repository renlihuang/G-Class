using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.Mescall;
using MESwebservice.OCVCheckData;
using MESwebservice.OCVgetData;
using System;
using System.Collections.Generic;

namespace DCS.TASKITEM.DataGather
{
    internal class BatteryCoreOcvTestGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        static bool _outStaionResult = false;
        //csvlist
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();
        public BatteryCoreOcvTestGather(RequestToHttpHelper requestToHttpHelper)
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
        List<BatteryCoreOcvTestEntity> _batterylist = new List<BatteryCoreOcvTestEntity>();


        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            //cellCustomDCCheckResponse ocvcheck = OCVCall.cellCustomDCCheck(@AppConfig.WebserviceiniPath, "A030电芯判定数据", _batterylist);
            //int code = ocvcheck.@return.code;
            //string msg = ocvcheck.@return.message;

           

            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();

            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();
            //拼接结构体读取data
            string tagName = "\"TMCOCVTESTDB\".";



            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            //data[tagName + "OCVDATA"] = null;

            if (opc.ReadNodes(data))
            {
                var ocvdata = tagName + "OCVDATA";
            }
            needleCount = tagName + "needlenumber";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[needleCount] = null; 
            for (int i = 0; i < 36; i++)
            {
                data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"] = null;
                data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"] = null;
                data[tagName + $"\"OCVDATA\".BatteryState[{i}]"] = null;
                data[tagName + $"\"OCVDATA\".Result[{i}]"] = null;
            }
            for (int i = 0; i < 2; i++)
            {
                data[tagName + $"\"OcvTEMP\"[{i}]"] = null;
            }

            //判断是否读取成功
            if (opc.ReadNodes(data))
            {
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}
                _collectTaskContext.TaskMsgOperator.SetPairText("PLC标志位", data[tagName + "dataStatus_PLC"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("MES标志位", data[tagName + "dataStatus_MES"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("探针使用次数", data[tagName + "needlenumber"].ToString());
               // _collectTaskContext.TaskMsgOperator.SetPairText("错误代码", "11111");

                int PlcFalg = Convert.ToInt16(data[plcDataStatus].ToString());
                int MesFalg = Convert.ToInt16(data[mesDataStatus].ToString());
                string Needlenum = data[needleCount].ToString();
                _outStaionResult = true;
                int code = -1;
                string msg = "";
                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;
                if (PlcFalg == 1 && isFinish)
                {
              
                    _collectTaskContext.TaskMsgOperator.SetText("OVC测试开始处理PLC数据");
                    _log.AddUserLog("读取数据", "读取PLC", "PLC标志位等于" + PlcFalg);

                    //记录数据
                    _log.AddUserLog("读取数据", "读取PLC", "记录数据：" + JsonHelper.SerializeObject(_batterylist));

                    #region ocv mes交互
                    //获取数据
                    try
                    {
                        miBatchStartAndGetParameterResponse ocvget = OCVCall.GetOCVdata(@AppConfig.WebserviceiniPath, "A030电芯获取数据", _batterylist);
                        code = ocvget.@return.code;
                        msg = ocvget.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }          
                    if (code==0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC获取数据接口调用成功","");
                    }
                    else if (code>0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC获取数据接口调用失败","");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: "+code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC获取数据接口返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC获取数据接口返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code==-1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    }
                    // 判断数据
                    try
                    {
                        cellCustomDCCheckResponse ocvcheck = OCVCall.cellCustomDCCheck(@AppConfig.WebserviceiniPath, "A030电芯判定数据", _batterylist);
                        code = ocvcheck.@return.code;
                        msg = ocvcheck.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC判定数据接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC判定数据接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC判定数据接口返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("OVC判定数据接口返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code == -1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    }
                    #endregion


                    //采集完成，写入MES标志位
                    for (int i = 0; i <36; i++)
                    {
                        if (Convert.ToInt32( data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"])>5|| Convert.ToInt32(data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"])<3 ||string.IsNullOrEmpty(data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"].ToString()))
                        {
                            writeTags[tagName + $"\"OCVDATA\".Result[{i}]"] = Convert.ToInt16(2);
                        }
                        else
                        {
                            writeTags[tagName + $"\"OCVDATA\".Result[{i}]"] = Convert.ToInt16(1);
                        } 
                    }
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("电芯OCV测试", "电芯OCV测试", string.Format("电芯OCV测试resul写入失败"));
                    }
                    if (opc.ReadNodes(data))
                    {
                        _batterylist.Clear();
                        for (int i = 0; i < 36; i++)
                        {
                            BatteryCoreOcvTestEntity battery = new BatteryCoreOcvTestEntity();
                            battery.needlenumber = data[tagName + "needlenumber"].ToString();

                            battery.BatteryCoreCode = data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"].ToString() == null ? "" : data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"].ToString();
                            battery.OcvVoltage = data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"].ToString();
                            battery.BatteryState = data[tagName + $"\"OCVDATA\".BatteryState[{i}]"].ToString();
                            battery.Result = data[tagName + $"\"OCVDATA\".Result[{i}]"].ToString() == "1" ? "ok" : "ng";
                            for (int j = 0; j < 2; j++)
                            {
                                battery.OcvTEMP += data[tagName + $"\"OcvTEMP\"[{j}]"].ToString() + ";";
                            }
                            _batterylist.Add(battery);
                        }
                    }
                   
                    short writeResult = -1;
                    #region 批量插入数据
                        AddEntityAync(_batterylist);
                    #endregion
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst, "","OCV测试");
                    //根据是否出站成功来回写标准位
                    if (_outStaionResult)
                    {
                        writeResult = 1;
                    }
                    else
                    {
                        writeResult = 2;
                    }
                    writeTags[mesDataStatus] = Convert.ToInt16(writeResult);
                    //写入PLC变量
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("电芯OCV测试", "电芯OCV测试", string.Format("电芯OCV测试resul写入失败"));
                    }
                
                            //标记结束出站
                            _isOutStation = false;
                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("电芯OCV测试", "电芯OCV测试", string.Format("电芯OCV测试标志位写0失败"));
                    }
                    _isOutStation = false;
                }
                if (PlcFalg == 3)
                {

                    _isOutStation = false;

                    writeTags[mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("电芯OCV测试", "电芯OCV测试", string.Format("电芯OCV测试标志位写4失败"));
                        }
                    }

                }
            }

        }
        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<BatteryCoreOcvTestEntity> AddEntityAync(List< BatteryCoreOcvTestEntity> batteryCoreOcvTestEntity)
        {
            HttpResponseResultModel<BatteryCoreOcvTestEntity> result = _requestToHttpHelper.PostAsync<BatteryCoreOcvTestEntity>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/BatteryCoreOcvTest/InsertBatch",
                Data = batteryCoreOcvTestEntity
            }).Result;

            return result;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="Bentities"></param>
        /// <returns></returns>
        private static bool GetOCVdata(string inipath, string sitename, List<BatteryCoreOcvTestEntity> Bentities)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiBatchStartAndGetParameterServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            var req = new miBatchStartAndGetParameter();
            var req_arg = new batchStartAndGetParamRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.startMode = true;
            req_arg.processLot = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            string[] sfcarry = new string[36];
            string[] paramArrayField = new string[36];
            req_arg.sfcArray = sfcarry;
            req_arg.paramArray = paramArrayField;
            req_arg.isGetFirstValue = IniFileAPI.INIGetStringValue(inipath, sitename, "isGetFirstValue", "");

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            try
            {

                miBatchStartAndGetParameterResponse miResponse = wsService.miBatchStartAndGetParameter(req as miBatchStartAndGetParameter);
                code = miResponse.@return.code;
                msg = miResponse.@return.message;
                endtime = DateTime.Now;

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        /// <summary>
        /// 判定数据
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="Bentities"></param>
        /// <returns></returns>
        private static bool CheckOCVdata(string inipath, string sitename, List<BatteryCoreOcvTestEntity> Bentities)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiCustomDCForCellServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new cellCustomDCCheck();
            var req_arg = new miCustomDCForCellRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.dcSequence = IniFileAPI.INIGetStringValue(inipath, sitename, "dcSequence", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.Multispec = IniFileAPI.INIGetStringValue(inipath, sitename, "Multispec", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            miCustomDCForCellInventory inventory = new miCustomDCForCellInventory();
            inventory.inventoryId = IniFileAPI.INIGetStringValue(inipath, sitename, "inventoryId", "");
            inventory.marking = IniFileAPI.INIGetStringValue(inipath, sitename, "marking", "");
            miCustomDCForCellInventoryData inventoryData = new miCustomDCForCellInventoryData();
            inventoryData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "");
            inventoryData.value = IniFileAPI.INIGetStringValue(inipath, sitename, "dcvalue", "");
            List<miCustomDCForCellInventoryData> miCustomDCForCellInventoryDatas = new List<miCustomDCForCellInventoryData>();
            miCustomDCForCellInventoryDatas.Clear();
            miCustomDCForCellInventoryDatas.Add(inventoryData);
            inventory.inventoryDatalist = miCustomDCForCellInventoryDatas.ToArray();
            List<miCustomDCForCellInventory> miCustomDCForCellInventories = new List<miCustomDCForCellInventory>();
            miCustomDCForCellInventories.Clear();
            miCustomDCForCellInventories.Add(inventory);
            req_arg.inventoryList = miCustomDCForCellInventories.ToArray();
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
                _log.AddUserLog("OCV测试", "OCV测试", string.Format("接口调用失败,CODE：" + code + "，错误信息:" + msg));
            }

            return code == 0;

        }

        public void DoUnInit()
        {

        }
    }
}
