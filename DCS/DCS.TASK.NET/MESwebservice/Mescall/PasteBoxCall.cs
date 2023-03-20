using CS.Base.AppSet;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using MESwebservice.AssAndCollData;
using MESwebservice.CheckBomInventory;
using MESwebservice.GetBarCodeInfo;
using MESwebservice.ModuleEntry;
using System;
using System.Collections.Generic;
using modeProcessSFC = MESwebservice.ModuleEntry.modeProcessSFC;

/// <summary>
/// M020-2贴箱体标
/// 2022-04-08
/// </summary>
namespace MESwebservice.Mescall
{
    public class PasteBoxCall
    {
        //csvdata
        private static Dictionary<string, object> _dic = new Dictionary<string, object>();
        //csvlog
        private static Dictionary<string, object> _dic_log = new Dictionary<string, object>();

        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();

        //日志帮助类
        static ILogOperator _log;
        static CollectTaskContext _collectTaskContext;

        public PasteBoxCall(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            _log = _collectTaskContext.LogOperator;
        }

        /// <summary>
        /// 贴模组标进站--进站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miFindCustomAndSfcDataResponse ShimEntry(string inipath, string sitename, string packno, string pn)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiFindCustomAndSfcDataServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            var req = new miFindCustomAndSfcData();
            var req_arg = new findCustomAndSfcDataRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.modeProcessSFC = modeProcessSFC.MODE_START_SFC;
            string sfc=String.Empty;
            if (packno.Length>=24)
            {
                sfc = packno.Substring(0, 24);
            }
            else
            {
                sfc=packno;
            }
           

            if (pn == "830100-02436")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
            }
            else if (pn == "1")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            }
            else
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
            }


            req_arg.sfc = sfc;

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            req.FindCustomAndSfcDataRequest = req_arg;
            miFindCustomAndSfcDataResponse miResponse = null;
            try
            {
                miResponse = wsService.miFindCustomAndSfcData(req as miFindCustomAndSfcData);
                code = miResponse.@return.code;
                msg = miResponse.@return.message;
                endtime = DateTime.Now;
            }
            catch (Exception ex)
            {
                isSaveCsvFile = false;
                isSaveCsvlogFile = false;
                throw ex;
            }
            if (isSaveCsvFile)
            {
                //csv文件数据保存
                _dic.Clear();
                _dic.Add("url", wsService.Url);
                _dic.Add("Timeout", wsService.Timeout);
                _dic.Add("Username", IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""));
                _dic.Add("Password", IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""));
                _dic.Add("PreAuthenticate", wsService.PreAuthenticate);
                _dic.Add("site", req_arg.site);
                _dic.Add("user", req_arg.user);
                _dic.Add("operation", req_arg.operation);
                _dic.Add("operationRevision", req_arg.operationRevision);
                _dic.Add("activity", req_arg.activity);
                _dic.Add("modeProcessSFC", req_arg.modeProcessSFC);
                _dic.Add("resource", req_arg.resource);
                _dic.Add("sfc", req_arg.sfc);
                dicLst.Clear();
                dicLst.Add(_dic);
                _log.ToCSVData(dicLst, sfc, sitename);
            }
            if (isSaveCsvlogFile)
            {
                _dic_log.Clear();
                _dic_log.Add("SFC", sfc);
                _dic_log.Add("接口调用时间", startime);
                string s = "{";
                foreach (var item in _dic)
                {
                    s += item.Key.ToString() + ":" + item.Value.ToString() + "，";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", code);
                _dic_log.Add("返回信息", msg);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "贴模组标进站");
            }

            return miResponse;
        }

        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miGetSlotDataResponse GetBarCodeInfo(string inipath, string sitename, string sfc)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiGetPrintContentServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            // 设置传输参数
            var req = new miGetSlotData();
            var req_arg = new getPrintContentRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.sfc = sfc;
            req_arg.item = IniFileAPI.INIGetStringValue(inipath, sitename, "item", "");
            req_arg.template = IniFileAPI.INIGetStringValue(inipath, sitename, "TEMPLATE", "");
            req_arg.templateVersion = IniFileAPI.INIGetStringValue(inipath, sitename, "templateVersion", "");
            string[] field = new string[] { IniFileAPI.INIGetStringValue(inipath, sitename, "field1", "") };
            req_arg.field = field;

            int code = -1;
            string msg = string.Empty;
            string pn = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            miGetSlotDataResponse miResponse = null;
            req.MiGetBarcodeRequest = req_arg;
            try
            {
                miResponse = wsService.miGetSlotData(req as miGetSlotData);
                code = miResponse.@return.code;
                msg = miResponse.@return.message;
                if (miResponse.@return.labelListArray != null)
                {
                    pn = miResponse.@return.labelListArray[0].value;
                }
                endtime = DateTime.Now;
            }
            catch (Exception)
            {
                isSaveCsvFile = false;
                isSaveCsvlogFile = false;
                throw;
            }

            if (isSaveCsvFile)
            {
                //csv文件数据保存
                _dic.Clear();
                _dic.Add("url", wsService.Url);
                _dic.Add("Timeout", wsService.Timeout);
                _dic.Add("Username", IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""));
                _dic.Add("Password", IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""));
                _dic.Add("PreAuthenticate", wsService.PreAuthenticate);
                _dic.Add("site", req_arg.site);
                _dic.Add("sfc", req_arg.sfc);
                _dic.Add("item", req_arg.item);
                _dic.Add("template", req_arg.template);
                _dic.Add("templateVersion", req_arg.templateVersion);
                _dic.Add("field", req_arg.field);

                dicLst.Clear();
                dicLst.Add(_dic);
                _log.ToCSVData(dicLst, sfc, sitename);
            }
            if (isSaveCsvlogFile)
            {
                _dic_log.Clear();
                _dic_log.Add("SFC", sfc);
                _dic_log.Add("接口调用时间", startime);
                string s = "{";
                foreach (var item in _dic)
                {
                    s += item.Key.ToString() + ":" + item.Value.ToString() + "，";
                }
                foreach (var item in field)
                {
                    s += item.ToString()+"，";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", code);
                _dic_log.Add("返回信息", msg);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "释放模组号");
            }

            return miResponse;
        }



        /// <summary>
        /// 模组称重进站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static miFindCustomAndSfcDataResponse ModuleEntry(string inipath, string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiFindCustomAndSfcDataServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            // 设置传输参数
            var req = new miFindCustomAndSfcData();
            var req_arg = new findCustomAndSfcDataRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.sfcOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "sfcOrder", "");
            req_arg.targetOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "targetOrder", "");
            req_arg.findSfcByInventory = true;
            customDataInParametricData parametricData = new customDataInParametricData();
            ModuleEntry.ObjectAliasEnum objectAliasEnum = new ModuleEntry.ObjectAliasEnum();
            parametricData.category = objectAliasEnum;
            parametricData.dataField = IniFileAPI.INIGetStringValue(inipath, sitename, "dataField", "");
            ModuleEntry.ObjectAliasEnum[] objectAliasEnums = new ModuleEntry.ObjectAliasEnum[35];
            req_arg.masterDataArray = objectAliasEnums;
            // dcGroup
            // dcGroupRevision
            ModuleEntry.modeProcessSFC modeProcessSFCField = new ModuleEntry.modeProcessSFC();
            req_arg.modeProcessSFC = modeProcessSFCField;
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            req_arg.inventory = IniFileAPI.INIGetStringValue(inipath, sitename, "inventory", "");

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            miFindCustomAndSfcDataResponse miResponse = null;
            try
            {
                miResponse = wsService.miFindCustomAndSfcData(req as miFindCustomAndSfcData);
                code = miResponse.@return.code;
                msg = miResponse.@return.message;
                endtime = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return miResponse;
        }

        /// <summary>
        /// 校验贴纸PN以及库存
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static miCheckBOMInventoryResponse ChenkBomInvent(string inipath, string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiCheckBOMInventoryServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            var req = new miCheckBOMInventory();
            var req_arg = new checkBOMInventoryRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activity", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            req_arg.modeCheckOperation = true;
            CheckBomInventory.modeProcessSFC modeProcessSFCField = new CheckBomInventory.modeProcessSFC();
            req_arg.modeProcessSFC = modeProcessSFCField;
            //checkBomInventoryData[] inventoryDataArrayField = new checkBomInventoryData[50];
            //req_arg.inventoryDataArray = inventoryDataArrayField;

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;

            miCheckBOMInventoryResponse miResponse = null;
            req.CheckBOMInventoryRequest = req_arg;
            try
            {
                miResponse = wsService.miCheckBOMInventory(req as miCheckBOMInventory);
                code = miResponse.@return.code;
                msg = miResponse.@return.message;
                endtime = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return miResponse;
        }

        /// <summary>
        /// 组装物料
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miAssmebleAndCollectDataForSfcResponse AssemblyMater(string inipath, string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiAssembleAndCollectDataForSfcServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            var req = new miAssmebleAndCollectDataForSfc();
            var req_arg = new assembleAndCollectDataForSfcRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            dataCollectForSfcModeProcessSfc modeProcessSFCField = new dataCollectForSfcModeProcessSfc();
            req_arg.modeProcessSFC = modeProcessSFCField;
            req_arg.partialAssembly = true;
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");

            List<nonConfirmCodeArray> nonConfirmCodeArrays = new List<nonConfirmCodeArray>();
            nonConfirmCodeArray ncCodeArrayField = new nonConfirmCodeArray();
            ncCodeArrayField.ncCode = IniFileAPI.INIGetStringValue(inipath, sitename, "ncCode", "");
            ncCodeArrayField.hasNc = true; // --
            nonConfirmCodeArrays.Clear();
            nonConfirmCodeArrays.Add(ncCodeArrayField);
            req_arg.ncCodeArray = nonConfirmCodeArrays.ToArray();

            List<miInventoryData> miInventoryDatas = new List<miInventoryData>();
            miInventoryData inventoryArrayField = new miInventoryData();
            inventoryArrayField.inventory = IniFileAPI.INIGetStringValue(inipath, sitename, "inventory", "");
            inventoryArrayField.qty = IniFileAPI.INIGetStringValue(inipath, sitename, "qty", "");
            miInventoryDatas.Clear();
            miInventoryDatas.Add(inventoryArrayField);
            req_arg.inventoryArray = miInventoryDatas.ToArray();

            //machineIntegrationParametricData parametricData = new machineIntegrationParametricData();
            //parametricData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "");
            //parametricData.value = IniFileAPI.INIGetStringValue(inipath, sitename, "dcvalue", "");

            miAssmebleAndCollectDataForSfcResponse miResponse = null;
            req.AssembleAndCollectDataForSfcRequest = req_arg;
            try
            {
                miResponse = wsService.miAssmebleAndCollectDataForSfc(req as miAssmebleAndCollectDataForSfc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return miResponse;
        }

        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        private static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Hours.ToString() + "小时"
                        + ts.Minutes.ToString() + "分钟"
                        + ts.Seconds.ToString() + "秒"
                        + ts.Milliseconds.ToString() + "毫秒";
            }
            catch
            {

            }
            return dateDiff;
        }
    }
}
