using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.OCVCheckData;
using MESwebservice.OCVgetData;
using System;
using System.Collections.Generic;

namespace DCS.TASKITEM.DataGather
{
   /// <summary>
   /// ocv测试
   /// </summary>
    internal class GCOCVTestGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        private static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        private static bool _outStaionResult = false;

        //csvlist
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();

        public GCOCVTestGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        private RequestToHttpHelper _requestToHttpHelper;

        private static CollectTaskContext _collectTaskContext;
        private string plcDataStatus;
        private string mesDataStatus;
        private string needleCount;
        private string Apihost;
        private static ILogOperator _log;
        private List<GCOcvTestEntity> _batterylist = new List<GCOcvTestEntity>();

        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;

            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            var opc = _collectTaskContext.OpcOperator;

            //读取flag字典
            Dictionary<string, object> dataflag = new Dictionary<string, object>();
            //读取数据字典
            Dictionary<string, object> data = new Dictionary<string, object>();

            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();
            //拼接结构体读取data
            string tagName = "\"GCOCVTESTDB\".";

            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            dataflag[plcDataStatus] = null;
            dataflag[mesDataStatus] = null;
            for (int i = 0; i < 48; i++)
            {
                data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"] = null;
                data[tagName + $"\"OCVDATA\".LocationCode[{i}]"] = null;
                data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"] = null;
                data[tagName + $"\"OCVDATA\".Temperature [{i}]"] = null;
                data[tagName + $"\"OCVDATA\".Result[{i}]"] = null;
            }

            //判断是否读取成功
            if (opc.ReadNodes(dataflag))
            {
                _collectTaskContext.TaskMsgOperator.SetPairText("PLC标志位", data[tagName + "dataStatus_PLC"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("MES标志位", data[tagName + "dataStatus_MES"].ToString());

                int PlcFalg = Convert.ToInt16(dataflag[plcDataStatus].ToString());
                int MesFalg = Convert.ToInt16(dataflag[mesDataStatus].ToString());
                _outStaionResult = true;
                int code = -1;
                string msg = "";

                bool isFinish = MesFalg == 4 || MesFalg == 0;
                if (PlcFalg == 1 && isFinish)
                {
                    _collectTaskContext.TaskMsgOperator.SetText("OVC测试开始处理PLC数据");
                    _log.AddUserLog("读取数据", "读取PLC", "PLC标志位等于" + PlcFalg);
                    if (opc.ReadNodes(data))
                    {
                        for (int i = 0; i < 48; i++)
                        {
                            string tempcode = data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"].ToString();
                            if (string.IsNullOrWhiteSpace(tempcode) || tempcode.Length < 22)
                            {
                                writeTags[tagName + $"\"OCVDATA\".Result[{i}]"] = Convert.ToInt16(3);
                                _log.AddUserLog("电芯OCV测试", "电芯OCV测试", string.Format("电芯条码为空或者位数不合格,条码为:" + tempcode));
                            }
                            //mes调用校验库存
                        }
                    }
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("电芯OCV测试", "电芯OCV测试", string.Format("电芯OCV测试resul写入失败"));
                    }

                    _batterylist.Clear();
                    if (opc.ReadNodes(data))
                    {
                        for (int i = 0; i < 48; i++)
                        {
                            GCOcvTestEntity battery = new GCOcvTestEntity();
                            battery.Temperature = Convert.ToSingle(data[tagName + "Temperature"].ToString());

                            battery.BatteryCode = data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"].ToString() == null ? "" : data[tagName + $"\"OCVDATA\".BatteryCoreCode[{i}]"].ToString();
                            if (data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"].ToString().Length <= 5)
                            {
                                int blenth = data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"].ToString().Length;
                                string bvolatge = data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"].ToString();
                                for (int j = 0; j < 8 - blenth; j++)
                                {
                                    bvolatge += "0";
                                }
                                battery.OcvVoltage = Convert.ToSingle(bvolatge);
                            }
                            else
                            {
                                battery.OcvVoltage = Convert.ToSingle(data[tagName + $"\"OCVDATA\".OcvVoltage[{i}]"].ToString());
                            }

                            battery.LocationCode = data[tagName + $"\"OCVDATA\".LocationCode[{i}]"].ToString();
                            battery.OcvResult = data[tagName + $"\"OCVDATA\".Result[{i}]"].ToString() == "1" ? "ok" : "ng";
                            _batterylist.Add(battery);
                            _log.AddUserLog("电芯OCV测试", "电芯OCV测试", "电芯测试数据：" + battery.ToJson());
                        }
                    }
                    short writeResult = -1;

                    #region 批量插入数据

                    AddEntityAync(_batterylist);

                    #endregion 批量插入数据

                    dicLst.Clear();
                    data.Add("Date", DateTime.Now.ToString());
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst, "", "OCV测试");
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
        private HttpResponseResultModel<GCOcvTestEntity> AddEntityAync(List<GCOcvTestEntity> batteryCoreOcvTestEntity)
        {
            HttpResponseResultModel<GCOcvTestEntity> result = _requestToHttpHelper.PostAsync<GCOcvTestEntity>(new HttpRequestModel
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