using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.BlockCheckinvent;
//using MESwebservice.BlockdataCollect;
using MESwebservice.BlockMiAssembleAndCollectData;
using MESwebservice.BlockReleaseSfc;
using MESwebservice.BlocksfcComplete;
using MESwebservice.FindCusAndSfc;
using MESwebservice.GetBarCodeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modeProcessSFC = MESwebservice.FindCusAndSfc.modeProcessSFC;

namespace MESwebservice.Mescall
{
    /// <summary>
    /// BlockMes调用
    /// </summary>
    public class BlockCall
    {
        //csvdata
        private static Dictionary<string, object> _dic = new Dictionary<string, object>();
        //csvlog
        private static Dictionary<string, object> _dic_log = new Dictionary<string, object>();

        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();

        //日志帮助类
        static ILogOperator _log;
        static CollectTaskContext _collectTaskContext;


        public BlockCall(TimedTaskContext taskContext, RequestToHttpHelper requestToHttpHelper)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            _log = _collectTaskContext.LogOperator;
            _requestToHttpHelper = requestToHttpHelper;
        }
        static RequestToHttpHelper _requestToHttpHelper;

        /// <summary>
        /// Block校验电芯
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="inventlist"></param>
        /// <returns></returns>
        public static miCheckInventoryAttributesResponse BlockCheckinvent(string inipath, string sitename, List<BlockEntity> blockEntities, string sfc)
        {
            //MiCheckInventoryAttributesServiceService
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiCheckInventoryAttributesServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new miCheckInventoryAttributes();
            var req_arg = new ModuleCellMarkingOrTimeCheckRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.sfc = sfc;
            string mode = IniFileAPI.INIGetStringValue(inipath, sitename, "modeCheckInventory", "");
            switch (mode)
            {
                case "MODE_MARK_ONLY":
                    req_arg.modeCheckInventory = modeCheckInventory.MODE_MARK_ONLY;
                    break;
                case "MODE_TIME_ONLY":
                    req_arg.modeCheckInventory = modeCheckInventory.MODE_TIME_ONLY;
                    break;
                case "MODE_MARK_AND_TIME":
                    req_arg.modeCheckInventory = modeCheckInventory.MODE_MARK_AND_TIME;
                    break;
                default:
                    req_arg.modeCheckInventory = modeCheckInventory.MODE_NONE;
                    break;
            }
            req_arg.requiredQuantity = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "requiredQuantity", ""));
            List<string> inventlist = new List<string>();
            inventlist.Clear();
            for (int i = 0; i < blockEntities.Count; i++)
            {
                inventlist.Add(blockEntities[i].BatteryCoreCode);
            }
            req_arg.inventoryArray = inventlist.ToArray();
            DateTime endtime = DateTime.Now;
            miCheckInventoryAttributesResponse miCheck = null;
            req.CheckInventoryAttributesRequest = req_arg;
            try
            {
                miCheck = wsService.miCheckInventoryAttributes(req as miCheckInventoryAttributes);
                endtime = DateTime.Now;
            }
            catch (Exception)
            {

                throw;
            }
            return miCheck;
        }
        /// <summary>
        /// 释放模组号
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static miReleaseSfcWithActivityResponse BlockReleaseSfc(string inipath, string sitename)
        {

            DateTime startime = DateTime.Now;
            var wsService = new MiReleaseSfcWithActivityServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new miReleaseSfcWithActivity();
            var req_arg = new releaseSfcWithActivityRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            #region 生产模式相关
            string mesmode = IniFileAPI.INIGetStringValue(inipath, "生产模式", "Mode", "");
            string mestype = IniFileAPI.INIGetStringValue(inipath, "生产模式", "Type", "");
            string LastType = IniFileAPI.INIGetStringValue(inipath, "生产模式", "LastType", "");
            if (mesmode == "AB")
            {
                if (LastType == "B")
                {
                    req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
                    IniFileAPI.INIWriteValue(inipath, "生产模式", "Type", "B");
                }
                else
                {
                    req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
                    IniFileAPI.INIWriteValue(inipath, "生产模式", "Type", "A");
                }
            }
            else if(mesmode == "A")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
            }
            else if(mesmode == "B")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
            }
            #endregion
            //req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.sfcQty = Convert.ToDecimal(IniFileAPI.INIGetStringValue(inipath, sitename, "sfcQty", ""));
            req_arg.processlot = IniFileAPI.INIGetStringValue(inipath, sitename, "processlot", "");
            req_arg.location = new string[1] { IniFileAPI.INIGetStringValue(inipath, sitename, "location", "") };
            string mode = IniFileAPI.INIGetStringValue(inipath, sitename, "modeProcessSfc", "");
            switch (mode)
            {
                case "MODE_PASS_SFC":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_PASS_SFC;
                    break;
                case "MODE_COMPLETE_SFC":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_COMPLETE_SFC;
                    break;
                case "MODE_START_COMPLETE_SFC":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_START_COMPLETE_SFC;
                    break;
                case "MODE_NONE":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_NONE;
                    break;
                default:
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_START_SFC;
                    break;
            }
            req_arg.isCarrierType = true;
            string columorrow = IniFileAPI.INIGetStringValue(inipath, sitename, "ColumnOrRowFirst", "");
            switch (columorrow)
            {
                case "COLUMN_FIRST":
                    req_arg.ColumnOrRowFirst = columnOrRow.COLUMN_FIRST;
                    break;
                default:
                    req_arg.ColumnOrRowFirst = columnOrRow.ROW_FIRST;
                    break;
            }
            req_arg.ColumnOrRowFirstSpecified = true;

            DateTime endtime = DateTime.Now;
            miReleaseSfcWithActivityResponse miReleaseSfcWith = null;
            req.ReleaseSfcWithActivityRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                miReleaseSfcWith = wsService.miReleaseSfcWithActivity(req as miReleaseSfcWithActivity);
                endtime = DateTime.Now;

                // 保存模组号和资源号数据
                ModuleAndPnEntity moduleAndPn = new ModuleAndPnEntity();
                moduleAndPn.ModuleCode = miReleaseSfcWith.@return.sfcArray[0].sfc.ToString(); // 模号 001MEAVN000002C7M0500003
                moduleAndPn.SourceNo = req_arg.resource;  // 资源号 // ITEQA6U7
                moduleAndPn.PnNo = req_arg.resource == "MCGMA01W" ? "830100-02436" : "830100-02437"; // 830100-02436
                HttpResponseResultModel<ModuleAndPnEntity> pnResult = AddModuleAndPnAync(moduleAndPn);
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("sfcQty", req_arg.sfcQty);
                _dic.Add("processlot", req_arg.processlot);
                _dic.Add("location", req_arg.location);
                _dic.Add("modeProcessSFC", req_arg.modeProcessSFC);
                _dic.Add("isCarrierType", req_arg.isCarrierType);
                _dic.Add("ColumnOrRowFirst", req_arg.ColumnOrRowFirst);
                _dic.Add("ColumnOrRowFirstSpecified", req_arg.ColumnOrRowFirstSpecified);
                _dic.Add("code", miReleaseSfcWith.@return.code);
                _dic.Add("msg", miReleaseSfcWith.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

                string sfc = string.Empty;
                if (miReleaseSfcWith.@return.sfcArray == null)
                {
                    sfc = "";
                }
                _log.ToCSVData(dicLst, sfc, sitename);

                if (isSaveCsvlogFile)
                {
                    _dic_log.Clear();
                    _dic_log.Add("SFC", sfc);
                    _dic_log.Add("接口调用时间", startime);
                    string s = "{";
                    foreach (var item in _dic)
                    {
                        s += item.Key.ToString() + ":" + item.Value.ToString() + ",";
                    }
                    s += "}";
                    _dic_log.Add("接口传参数", s);
                    _dic_log.Add("接口调用返回时间", endtime);
                    string difftime = DateDiff(startime, endtime);
                    _dic_log.Add("耗时", difftime);
                    _dic_log.Add("返回代码", miReleaseSfcWith.@return.code);
                    _dic_log.Add("返回信息", miReleaseSfcWith.@return.message);
                    dicLst.Clear();
                    dicLst.Add(_dic_log);
                    _log.ToCSVLOG(dicLst, sitename, sfc, "释放模组号");
                }
            }
            return miReleaseSfcWith;
        }
        /// <summary>
        /// Busbar释放pack号
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static miReleaseSfcWithActivityResponse BusbarReleaseSfc(string inipath, string sitename)
        {

            DateTime startime = DateTime.Now;
            var wsService = new MiReleaseSfcWithActivityServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new miReleaseSfcWithActivity();
            var req_arg = new releaseSfcWithActivityRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.sfcQty = Convert.ToDecimal(IniFileAPI.INIGetStringValue(inipath, sitename, "sfcQty", ""));
            req_arg.processlot = IniFileAPI.INIGetStringValue(inipath, sitename, "processlot", "");
            req_arg.location = new string[1] { IniFileAPI.INIGetStringValue(inipath, sitename, "location", "") };
            string mode = IniFileAPI.INIGetStringValue(inipath, sitename, "modeProcessSfc", "");
            switch (mode)
            {
                case "MODE_PASS_SFC":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_PASS_SFC;
                    break;
                case "MODE_COMPLETE_SFC":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_COMPLETE_SFC;
                    break;
                case "MODE_START_COMPLETE_SFC":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_START_COMPLETE_SFC;
                    break;
                case "MODE_NONE":
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_NONE;
                    break;
                default:
                    req_arg.modeProcessSFC = MESwebservice.BlockReleaseSfc.modeProcessSFC.MODE_START_SFC;
                    break;
            }
            req_arg.isCarrierType = true;
            string columorrow = IniFileAPI.INIGetStringValue(inipath, sitename, "ColumnOrRowFirst", "");
            switch (columorrow)
            {
                case "COLUMN_FIRST":
                    req_arg.ColumnOrRowFirst = columnOrRow.COLUMN_FIRST;
                    break;
                default:
                    req_arg.ColumnOrRowFirst = columnOrRow.ROW_FIRST;
                    break;
            }
            req_arg.ColumnOrRowFirstSpecified = true;

            DateTime endtime = DateTime.Now;
            miReleaseSfcWithActivityResponse miReleaseSfcWith = null;
            req.ReleaseSfcWithActivityRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                miReleaseSfcWith = wsService.miReleaseSfcWithActivity(req as miReleaseSfcWithActivity);
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("sfcQty", req_arg.sfcQty);
                _dic.Add("processlot", req_arg.processlot);
                _dic.Add("location", req_arg.location);
                _dic.Add("modeProcessSFC", req_arg.modeProcessSFC);
                _dic.Add("isCarrierType", req_arg.isCarrierType);
                _dic.Add("ColumnOrRowFirst", req_arg.ColumnOrRowFirst);
                _dic.Add("ColumnOrRowFirstSpecified", req_arg.ColumnOrRowFirstSpecified);
                _dic.Add("code", miReleaseSfcWith.@return.code);
                _dic.Add("msg", miReleaseSfcWith.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

                string sfc = string.Empty;
                if (miReleaseSfcWith.@return.sfcArray == null)
                {
                    sfc = "";
                }
                _log.ToCSVData(dicLst, sfc, sitename);

                if (isSaveCsvlogFile)
                {
                    _dic_log.Clear();
                    _dic_log.Add("SFC", sfc);
                    _dic_log.Add("接口调用时间", startime);
                    string s = "{";
                    foreach (var item in _dic)
                    {
                        s += item.Key.ToString() + ":" + item.Value.ToString() + ",";
                    }
                    s += "}";
                    _dic_log.Add("接口传参数", s);
                    _dic_log.Add("接口调用返回时间", endtime);
                    string difftime = DateDiff(startime, endtime);
                    _dic_log.Add("耗时", difftime);
                    _dic_log.Add("返回代码", miReleaseSfcWith.@return.code);
                    _dic_log.Add("返回信息", miReleaseSfcWith.@return.message);
                    dicLst.Clear();
                    dicLst.Add(_dic_log);
                    _log.ToCSVLOG(dicLst, sitename, sfc, "Busbar释放pack号");
                }
            }
            return miReleaseSfcWith;
        }
        /// <summary>
        /// 模组装配电芯
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static miAssmebleAndCollectDataForSfcResponse BlockMiAssemble(string inipath, string sitename, List<BlockEntity> blockEntities, string sfc, string pnType)
        {

            DateTime startime = DateTime.Now;
            var wsService = new MiAssembleAndCollectDataForSfcServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new miAssmebleAndCollectDataForSfc();
            var req_arg = new assembleAndCollectDataForSfcRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            #region 生产模式相关
            if (pnType == "830100-02436")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
            }
            else
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
            }

            #endregion
            //req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            string sfcmode = IniFileAPI.INIGetStringValue(inipath, sitename, "mode", "");
            switch (sfcmode)
            {
                case "MODE_NONE":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_NONE;
                    break;
                case "MODE_START_SFC_PRE_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_START_SFC_PRE_DC;
                    break;
                case "MODE_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_COMPLETE_SFC_POST_DC;
                    break;
                case "MODE_PASS_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_PASS_SFC_POST_DC;
                    break;
                case "MODE_REMOVE_PROCESSLOT_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_REMOVE_PROCESSLOT_COMPLETE_SFC_POST_DC;
                    break;
                case "MODE_START_AND_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_START_AND_COMPLETE_SFC_POST_DC;
                    break;
                default:
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_START_SFC_PRE_DC_SFC_COMPLETE;
                    break;
            }
            req_arg.partialAssembly = true;
            req_arg.sfc = sfc;
            //List<BlockMiAssembleAndCollectData.nonConfirmCodeArray> nccodeArrays = new List<BlockMiAssembleAndCollectData.nonConfirmCodeArray>();
            //BlockMiAssembleAndCollectData.nonConfirmCodeArray nonConfirmCodeArray = new BlockMiAssembleAndCollectData.nonConfirmCodeArray();
            //nonConfirmCodeArray.ncCode = "";
            //nonConfirmCodeArray.hasNc = true;
            //nccodeArrays.Clear();
            //nccodeArrays.Add(nonConfirmCodeArray);
            //req_arg.ncCodeArray=nccodeArrays.ToArray();


            List<miInventoryData> midataList = new List<miInventoryData>();
            midataList.Clear();
            List<AssemblyDataField> assemblylist = new List<AssemblyDataField>();
            assemblylist.Clear();
            for (int i = 0; i < blockEntities.Count; i++)
            {

                miInventoryData midata = new miInventoryData();
                midata.inventory = blockEntities[i].BatteryCoreCode.Substring(0,24);
                midata.qty = "1";

                //AssemblyDataField assemblyDataField = new AssemblyDataField();
                //assemblyDataField.sequence = Convert.ToDecimal(2);
                //assemblyDataField.sequenceSpecified = true;
                //assemblylist.Add(assemblyDataField);
                //midata.assemblyDataFields = assemblylist.ToArray();
                midataList.Add(midata);

            }

            req_arg.inventoryArray = midataList.ToArray();
            List<BlockMiAssembleAndCollectData.machineIntegrationParametricData> machineIntegrationParametricDatas = new List<BlockMiAssembleAndCollectData.machineIntegrationParametricData>();
            machineIntegrationParametricDatas.Clear();

            //BlockMiAssembleAndCollectData.machineIntegrationParametricData machineIntegrationParametricData = new BlockMiAssembleAndCollectData.machineIntegrationParametricData();
            //for (int i = 1; i < 31; i++)
            //{
            //    machineIntegrationParametricData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "")+i.ToString();
            //    machineIntegrationParametricData.value = blockEntities[i].BatteryCoreCode;
            //    string dctype = IniFileAPI.INIGetStringValue(inipath, sitename, "dctype", "");
            //    machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.TEXT;
            //    //switch (dctype)
            //    //{
            //    //    case "NUMBER":
            //    //        machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.NUMBER;
            //    //        break;
            //    //    case "TEXT":
                       
            //    //        break;
            //    //    case "FORMULA":
            //    //        machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.FORMULA;
            //    //        break;
            //    //    default:
            //    //        machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.BOOLEAN;
            //    //        break;
            //    //}
            //    machineIntegrationParametricDatas.Add(machineIntegrationParametricData);
            //}
            //req_arg.parametricDataArray = machineIntegrationParametricDatas.ToArray();
            //req_arg.remark = "";


            miAssmebleAndCollectDataForSfcResponse miAssmebleAndCollectDataForSfcResponse = null;
            req.AssembleAndCollectDataForSfcRequest = req_arg;

            bool isSaveCsvFile =  true;
            bool isSaveCsvlogFile = true;
            try
            {
                miAssmebleAndCollectDataForSfcResponse = wsService.miAssmebleAndCollectDataForSfc(req as miAssmebleAndCollectDataForSfc);

            }
            catch (Exception ex)
            {
                isSaveCsvFile = false;
                isSaveCsvlogFile = false;
                throw ex;
            }
            DateTime endtime = DateTime.Now;
            if (isSaveCsvFile)
            {
                //csv文件数据保存
                _dic.Clear();
                _dic.Add("url", wsService.Url);
                _dic.Add("Timeout", wsService.Timeout);
                _dic.Add("Username", IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""));
                _dic.Add("Password", IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""));
                //req_arg.pr

                _dic.Add("site", req_arg.site);
                _dic.Add("user", req_arg.user);
                _dic.Add("operation", req_arg.operation);
                _dic.Add("operationRevision", req_arg.operationRevision);
                _dic.Add("activityId", req_arg.activityId);
                _dic.Add("resource", req_arg.resource);
                _dic.Add("dcGroup", req_arg.dcGroup);
                _dic.Add("dcGroupRevision", req_arg.dcGroupRevision);
                _dic.Add("modeProcessSFC", req_arg.modeProcessSFC);
                _dic.Add("partialAssembly", req_arg.partialAssembly);
                _dic.Add("sfc", req_arg.sfc);
                _dic.Add("inventoryArray", req_arg.inventoryArray);

                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

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
                foreach (var item in req_arg.inventoryArray)
                {
                    s += item.inventory.ToString() + ":" + item.qty.ToString() + "，";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", miAssmebleAndCollectDataForSfcResponse.@return.code);
                _dic_log.Add("返回信息", miAssmebleAndCollectDataForSfcResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "模组装配电芯");
            }
            return miAssmebleAndCollectDataForSfcResponse;
        }
        /// <summary>
        /// Busbar装配模组号
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="blockEntities"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public static miAssmebleAndCollectDataForSfcResponse BusbarMiAssemble(string inipath, string sitename, string sfc, string sfc2, string packNo)
        {

            DateTime startime = DateTime.Now;
            var wsService = new MiAssembleAndCollectDataForSfcServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new miAssmebleAndCollectDataForSfc();
            var req_arg = new assembleAndCollectDataForSfcRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            string sfcmode = IniFileAPI.INIGetStringValue(inipath, sitename, "mode", "");
            switch (sfcmode)
            {
                case "MODE_NONE":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_NONE;
                    break;
                case "MODE_START_SFC_PRE_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_START_SFC_PRE_DC;
                    break;
                case "MODE_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_COMPLETE_SFC_POST_DC;
                    break;
                case "MODE_PASS_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_PASS_SFC_POST_DC;
                    break;
                case "MODE_REMOVE_PROCESSLOT_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_REMOVE_PROCESSLOT_COMPLETE_SFC_POST_DC;
                    break;
                case "MODE_START_AND_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_START_AND_COMPLETE_SFC_POST_DC;
                    break;
                default:
                    req_arg.modeProcessSFC = BlockMiAssembleAndCollectData.dataCollectForSfcModeProcessSfc.MODE_START_SFC_PRE_DC_SFC_COMPLETE;
                    break;
            }
            req_arg.partialAssembly = true;

            req_arg.sfc = packNo;

            List<miInventoryData> midataList = new List<miInventoryData>();
            midataList.Clear();
            List<AssemblyDataField> assemblylist = new List<AssemblyDataField>();
            assemblylist.Clear();


            miInventoryData midata1 = new miInventoryData();
            midata1.inventory = sfc;
            midata1.qty = "1";

            miInventoryData midata2 = new miInventoryData();
            midata2.inventory = sfc2;
            midata2.qty = "1";

            midataList.Add(midata1);
            midataList.Add(midata2);


            //AssemblyDataField assemblyDataField = new AssemblyDataField();
            //assemblyDataField.sequence = Convert.ToDecimal(2);
            //assemblyDataField.sequenceSpecified = true;
            //assemblylist.Add(assemblyDataField);
            //midata1.assemblyDataFields = assemblylist.ToArray();
            //midata2.assemblyDataFields = assemblylist.ToArray();


            req_arg.inventoryArray = midataList.ToArray();

            miAssmebleAndCollectDataForSfcResponse miAssmebleAndCollectDataForSfcResponse = null;
            req.AssembleAndCollectDataForSfcRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                miAssmebleAndCollectDataForSfcResponse = wsService.miAssmebleAndCollectDataForSfc(req as miAssmebleAndCollectDataForSfc);

            }
            catch (Exception ex)
            {
                isSaveCsvFile = false;
                isSaveCsvlogFile = false;
                throw ex;
            }
            DateTime endtime = DateTime.Now;
            if (isSaveCsvFile)
            {
                //csv文件数据保存
                _dic.Clear();
                _dic.Add("url", wsService.Url);
                _dic.Add("Timeout", wsService.Timeout);
                _dic.Add("Username", IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""));
                _dic.Add("Password", IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""));
                _dic.Add("PreAuthenticate", wsService.PreAuthenticate);
                //req_arg.pr

                _dic.Add("site", req_arg.site);
                _dic.Add("user", req_arg.user);
                _dic.Add("operation", req_arg.operation);
                _dic.Add("operationRevision", req_arg.operationRevision);
                _dic.Add("activityId", req_arg.activityId);
                _dic.Add("resource", req_arg.resource);
                _dic.Add("dcGroup", req_arg.dcGroup);
                _dic.Add("dcGroupRevision", req_arg.dcGroupRevision);
                _dic.Add("modeProcessSFC", req_arg.modeProcessSFC);
                _dic.Add("partialAssembly", req_arg.partialAssembly);
                _dic.Add("sfc", req_arg.sfc);
                _dic.Add("inventoryArray", req_arg.inventoryArray);

                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

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
                    s += item.Key.ToString() + ":" + item.Value.ToString() + ",";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", miAssmebleAndCollectDataForSfcResponse.@return.code);
                _dic_log.Add("返回信息", miAssmebleAndCollectDataForSfcResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "模组装配电芯");
            }
            return miAssmebleAndCollectDataForSfcResponse;
        }
        /// <summary>
        /// Block收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static AutoWeight.dataCollectForSfcExResponse BlockDatacoll(string inipath, string sitename, string messfc, PasteBoxShimEntity pasteBoxShimEntity, string pnType)
        {
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.dataCollectForSfcEx();
            var req_arg = new AutoWeight.sfcDcExRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            //调用获取铭牌接口，根据pn判断调用a资源号还是b
            string sfc = messfc.Substring(0, 24);
            if (pnType == "830100-02436")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
            }
            else
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
            }

            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            req_arg.sfc = sfc;
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            // req_arg.dcMode = IniFileAPI.INIGetStringValue(inipath, sitename, "", "");
            List<AutoWeight.machineIntegrationParametricData> DClists = new List<AutoWeight.machineIntegrationParametricData>();
            DClists.Clear();
            // 模组长度
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric1 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric1.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname1", "");
            machineIntegrationParametric1.value = pasteBoxShimEntity.BlockL1.ToString();
            machineIntegrationParametric1.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric1);
            // 模组压紧力
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric2 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric2.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname2", "");
            machineIntegrationParametric2.value = pasteBoxShimEntity.BlockMPA_10.ToString();
            machineIntegrationParametric2.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric2);
            // 垫片数量
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric3 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric3.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname3", "");
            machineIntegrationParametric3.value = pasteBoxShimEntity.ShimNum.ToString();
            machineIntegrationParametric3.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric3);
            // 压紧保压时间
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric4 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric4.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname4", "");
            machineIntegrationParametric4.value = pasteBoxShimEntity.BlockTime_10.ToString();
            machineIntegrationParametric4.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric4);
            // 稳定压力
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric5 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric5.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname5", "");
            machineIntegrationParametric5.value = pasteBoxShimEntity.BlockMPA_4.ToString();
            machineIntegrationParametric5.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric5);
            // 稳压保压时间
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric6 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric6.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname6", "");
            machineIntegrationParametric6.value = pasteBoxShimEntity.BlockTime_4.ToString();
            machineIntegrationParametric6.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric6);


            req_arg.parametricDataArray = DClists.ToArray();

            DateTime endtime = DateTime.Now;
            AutoWeight.dataCollectForSfcExResponse dataCollectForSfcExResponse = null;
            req.SfcDcExRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                dataCollectForSfcExResponse = wsService.dataCollectForSfcEx(req as AutoWeight.dataCollectForSfcEx);
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("dcGroup", req_arg.dcGroup);
                _dic.Add("dcGroupRevision", req_arg.dcGroupRevision);
                _dic.Add("sfc", req_arg.sfc);
                _dic.Add("parametricDataArray", req_arg.parametricDataArray);

                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

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
                foreach (var item in req_arg.parametricDataArray)
                {
                    s += item.name.ToString() + ":" + item.value.ToString() + "，";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", dataCollectForSfcExResponse.@return.code);
                _dic_log.Add("返回信息", dataCollectForSfcExResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "收数");
            }

            return dataCollectForSfcExResponse;

        }
        /// <summary>
        /// 申请虚拟模组号收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static AutoWeight.dataCollectForSfcExResponse VirtualCodeDatacoll(string inipath, string sitename, string messfc, List<BatteryCoreOcvTestEntity> list,string mestype, string pnType)
        {
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.dataCollectForSfcEx();
            var req_arg = new AutoWeight.sfcDcExRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            //调用获取铭牌接口，根据pn判断调用a资源号还是b
            string sfc = messfc.Substring(0, 24);

            if (pnType == "830100-02436")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
            }
            else
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
            }

            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            req_arg.sfc = sfc;
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            // req_arg.dcMode = IniFileAPI.INIGetStringValue(inipath, sitename, "", "");
            List<AutoWeight.machineIntegrationParametricData> DClists = new List<AutoWeight.machineIntegrationParametricData>();
            DClists.Clear();
            // 电芯条码
            for (int i = 1; i < 31; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric1 = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric1.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname1" , "") + i.ToString();
                machineIntegrationParametric1.value = list[i-1].BatteryCoreCode;
                machineIntegrationParametric1.dataType = AutoWeight.ParameterDataType.TEXT;
                DClists.Add(machineIntegrationParametric1);
            }
            // 电芯电压
            for (int i = 1; i < 31; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric2 = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric2.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname2", "") + i.ToString();
                machineIntegrationParametric2.value = list[i-1].OcvVoltage;
                machineIntegrationParametric2.dataType = AutoWeight.ParameterDataType.NUMBER;
                DClists.Add(machineIntegrationParametric2);
            }

            req_arg.parametricDataArray = DClists.ToArray();

            DateTime endtime = DateTime.Now;
            AutoWeight.dataCollectForSfcExResponse dataCollectForSfcExResponse = null;
            req.SfcDcExRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                dataCollectForSfcExResponse = wsService.dataCollectForSfcEx(req as AutoWeight.dataCollectForSfcEx);
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("dcGroup", req_arg.dcGroup);
                _dic.Add("dcGroupRevision", req_arg.dcGroupRevision);
                _dic.Add("sfc", req_arg.sfc);
                _dic.Add("parametricDataArray", req_arg.parametricDataArray);

                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

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
                foreach (var item in req_arg.parametricDataArray)
                {
                    s += item.name.ToString() + ":" + item.value.ToString() + "，";
                }
                s += "}";

                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", dataCollectForSfcExResponse.@return.code);
                _dic_log.Add("返回信息", dataCollectForSfcExResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "收数");
            }

            return dataCollectForSfcExResponse;

        }
        public static AutoWeight.dataCollectForSfcExResponse BlockDatacoll1(string inipath, string sitename, string messfc, PasteBoxShimEntity pasteBoxShimEntity,string pn)
        {
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.dataCollectForSfcEx();
            var req_arg = new AutoWeight.sfcDcExRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            //调用获取铭牌接口，根据pn判断调用a资源号还是b
            string sfc = messfc.Substring(0, 24);
            if (pn == "830100-02436")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
            }
            else
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
            }

            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            req_arg.sfc = sfc;
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            // req_arg.dcMode = IniFileAPI.INIGetStringValue(inipath, sitename, "", "");
            List<AutoWeight.machineIntegrationParametricData> DClists = new List<AutoWeight.machineIntegrationParametricData>();
            DClists.Clear();
            // 模组长度
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric1 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric1.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname1", "");
            machineIntegrationParametric1.value = pasteBoxShimEntity.BlockL1.ToString();
            machineIntegrationParametric1.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric1);
            // 模组压紧力
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric2 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric2.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname2", "");
            machineIntegrationParametric2.value = pasteBoxShimEntity.BlockMPA_10.ToString();
            machineIntegrationParametric2.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric2);
            // 垫片数量
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric3 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric3.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname3", "");
            machineIntegrationParametric3.value = pasteBoxShimEntity.ShimNum.ToString();
            machineIntegrationParametric3.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric3);
            // 压紧保压时间
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric4 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric4.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname4", "");
            machineIntegrationParametric4.value = pasteBoxShimEntity.BlockTime_10.ToString();
            machineIntegrationParametric4.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric4);
            // 稳定压力
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric5 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric5.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname5", "");
            machineIntegrationParametric5.value = pasteBoxShimEntity.BlockMPA_4.ToString();
            machineIntegrationParametric5.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric5);
            // 稳压保压时间
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric6 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric6.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname6", "");
            machineIntegrationParametric6.value = pasteBoxShimEntity.BlockTime_4.ToString();
            machineIntegrationParametric6.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric6);


            req_arg.parametricDataArray = DClists.ToArray();

            DateTime endtime = DateTime.Now;
            AutoWeight.dataCollectForSfcExResponse dataCollectForSfcExResponse = null;
            req.SfcDcExRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                dataCollectForSfcExResponse = wsService.dataCollectForSfcEx(req as AutoWeight.dataCollectForSfcEx);
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("dcGroup", req_arg.dcGroup);
                _dic.Add("dcGroupRevision", req_arg.dcGroupRevision);
                _dic.Add("sfc", req_arg.sfc);
                _dic.Add("parametricDataArray", req_arg.parametricDataArray);

                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

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
                foreach (var item in req_arg.parametricDataArray)
                {
                    s += item.name.ToString() + ":" + item.value.ToString() + "，";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", dataCollectForSfcExResponse.@return.code);
                _dic_log.Add("返回信息", dataCollectForSfcExResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "收数");
            }

            return dataCollectForSfcExResponse;

        }
        /// <summary>
        /// 焊前寻址收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public static AutoWeight.dataCollectForSfcExResponse SeekSiteDatacoll(string inipath, string sitename, string sfc)
        {
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.dataCollectForSfcEx();
            var req_arg = new AutoWeight.sfcDcExRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            if (sitename == "贴箱体标获取铭牌")
            {
                miGetSlotDataResponse slotDataResponse = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌", sfc);
                if (slotDataResponse.@return.labelListArray != null)
                {
                    string pn = slotDataResponse.@return.labelListArray[0].value;
                    if (pn == "830100-02437")
                    {
                        req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
                    }
                    else
                    {
                        req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
                    }
                }
            }
            else
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            }
            //调用获取铭牌接口，根据pn判断调用a资源号还是b

            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            req_arg.sfc = sfc;
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            // req_arg.dcMode = IniFileAPI.INIGetStringValue(inipath, sitename, "", "");
            List<AutoWeight.machineIntegrationParametricData> DClists = new List<AutoWeight.machineIntegrationParametricData>();
            DClists.Clear();
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric1 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric1.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "");
            machineIntegrationParametric1.value = "1";
            machineIntegrationParametric1.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric1);

            req_arg.parametricDataArray = DClists.ToArray();

            DateTime endtime = DateTime.Now;
            AutoWeight.dataCollectForSfcExResponse dataCollectForSfcExResponse = null;
            req.SfcDcExRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                dataCollectForSfcExResponse = wsService.dataCollectForSfcEx(req as AutoWeight.dataCollectForSfcEx);
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("dcGroup", req_arg.dcGroup);
                _dic.Add("dcGroupRevision", req_arg.dcGroupRevision);
                _dic.Add("sfc", req_arg.sfc);
                _dic.Add("activityId", req_arg.activityId);
                _dic.Add("parametricDataArray", req_arg.parametricDataArray);
                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有
                _log.ToCSVData(dicLst, sfc, sitename);
            }
            if (isSaveCsvlogFile)
            {
                _dic_log.Clear();
                //_dic_log.Add("SFC", sfc);
                _dic_log.Add("接口调用时间", startime);
                string s = "{";
                foreach (var item in _dic)
                {
                    s += item.Key.ToString() + ":" + item.Value.ToString() + ",";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", dataCollectForSfcExResponse.@return.code);
                _dic_log.Add("返回信息", dataCollectForSfcExResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "出站");
            }
            return dataCollectForSfcExResponse;

        }
        /// <summary>
        /// Busbar收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="packNo"></param>
        /// <returns></returns>
        public static AutoWeight.dataCollectForSfcExResponse BusbarDatacoll(string inipath, string sitename, string packNo, BusbarWeldEntity busbarWeldEntity)
        {
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.dataCollectForSfcEx();
            var req_arg = new AutoWeight.sfcDcExRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            req_arg.sfc = packNo;
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            // req_arg.dcMode = IniFileAPI.INIGetStringValue(inipath, sitename, "", "");
            List<AutoWeight.machineIntegrationParametricData> DClists = new List<AutoWeight.machineIntegrationParametricData>();
            DClists.Clear();

            #region 收数数据处理
            // 外环最大输出功率
            string outMaxScgl = busbarWeldEntity.RingPower_MAX1 + busbarWeldEntity.RingPower_MAX2
                + busbarWeldEntity.RingPower_MAX3 + busbarWeldEntity.RingPower_MAX4 + busbarWeldEntity.RingPower_MAX5;
            // 外环输出最小功率
            string outMinScgl = busbarWeldEntity.RingPower_MIN1 + busbarWeldEntity.RingPower_MIN2
                + busbarWeldEntity.RingPower_MIN3 + busbarWeldEntity.RingPower_MIN4 + busbarWeldEntity.RingPower_MIN5;
            // 外环输出平均功率
            string outAvgScgl = busbarWeldEntity.RingPower_group1 + busbarWeldEntity.RingPower_group2
                + busbarWeldEntity.RingPower_group3 + busbarWeldEntity.RingPower_group4 + busbarWeldEntity.RingPower_group5;
            string outAllScgl = outMaxScgl + outMinScgl + outAvgScgl;
            string[] outAllScglMode = outAllScgl.Substring(0, outAllScgl.Length - 1).Split(';'); // 360
            for (int i = 1; i < 361; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname1", "") + i.ToString();
                machineIntegrationParametric.value = outAllScglMode[i - 1];
                machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
                DClists.Add(machineIntegrationParametric);
            }

            // 中心输出最大功率+中心输出最小功率+中心输出平均功率 120
            string centerMaxScgl = busbarWeldEntity.CenterPower_MAX1 + busbarWeldEntity.CenterPower_MAX2
            + busbarWeldEntity.CenterPower_MAX3 + busbarWeldEntity.CenterPower_MAX4 + busbarWeldEntity.CenterPower_MAX5;
            // 外环输出最小功率
            string centerMinScgl = busbarWeldEntity.CenterPower_MIN1 + busbarWeldEntity.CenterPower_MIN2
                + busbarWeldEntity.CenterPower_MIN3 + busbarWeldEntity.CenterPower_MIN4 + busbarWeldEntity.CenterPower_MIN5;
            // 外环输出平均功率
            string centerAvgScgl = busbarWeldEntity.CenterPower_group1 + busbarWeldEntity.CenterPower_group2
                + busbarWeldEntity.CenterPower_group3 + busbarWeldEntity.CenterPower_group4 + busbarWeldEntity.CenterPower_group5;
            string centerAllScgl = centerMaxScgl + centerMinScgl + centerAvgScgl;
            string[] centerAllScglMode = centerAllScgl.Substring(0, centerAllScgl.Length - 1).Split(';'); // 360
            for (int i = 1; i < 361; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname2", "") + i.ToString();
                machineIntegrationParametric.value = centerAllScglMode[i - 1];
                machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
                DClists.Add(machineIntegrationParametric);
            }

            // 测距值 120 ModuleA
            //string[] offsetModeA = busbarWeldEntity.Module_A.Substring(0, busbarWeldEntity.Module_A.Length - 1).Split(';'); // 120
            //for (int i = 1; i <121; i++)
            //{
            //    AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
            //    machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname3", "") + i.ToString();
            //    machineIntegrationParametric.value = offsetModeA[i-1];
            //    machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
            //    DClists.Add(machineIntegrationParametric);
            //}
            // 测距值A
            for (int i = 1; i <= 8; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname3", "") + i.ToString();
                machineIntegrationParametric.value = "1.1";
                machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
                DClists.Add(machineIntegrationParametric);
            }
            // 测距值B
            for (int i = 1; i <= 8; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname31", "") + i.ToString();
                machineIntegrationParametric.value = "1.2";
                machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
                DClists.Add(machineIntegrationParametric);
            }

            // 补偿后测距值 120 ModuleB
            //string[] offsetModeB = busbarWeldEntity.Module_B.Substring(0, busbarWeldEntity.Module_B.Length - 1).Split(';'); // 120
            //for (int i = 1; i < 121; i++)
            //{
            //    AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
            //    machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname4", "") + i.ToString();
            //    machineIntegrationParametric.value = offsetModeB[i-1];
            //    machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
            //    DClists.Add(machineIntegrationParametric);
            //}
            // 补偿后测距值A
            for (int i = 1; i <= 8; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname4", "") + i.ToString();
                machineIntegrationParametric.value = "1.3";
                machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
                DClists.Add(machineIntegrationParametric);
            }
            // 补偿后测距值B
            for (int i = 1; i <= 8; i++)
            {
                AutoWeight.machineIntegrationParametricData machineIntegrationParametric = new AutoWeight.machineIntegrationParametricData();
                machineIntegrationParametric.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname41", "") + i.ToString();
                machineIntegrationParametric.value = "1.4";
                machineIntegrationParametric.dataType = AutoWeight.ParameterDataType.NUMBER;
                DClists.Add(machineIntegrationParametric);
            }

            // 理论补偿值A
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric5 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric5.name = "LLBCZA";
            machineIntegrationParametric5.value = "1.5";
            machineIntegrationParametric5.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric5);
            // 理论补偿值B
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric51 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric51.name = "LLBCZB";
            machineIntegrationParametric51.value = "1.6";
            machineIntegrationParametric51.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric51);
            // 实际补偿值A
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric6 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric6.name = "SJBCZA";
            machineIntegrationParametric6.value = "1";
            machineIntegrationParametric6.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric6);
            // 实际补偿值B
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric61 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric61.name = "SJBCZB";
            machineIntegrationParametric61.value = "1";
            machineIntegrationParametric61.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric61);
            // 基准值
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric8 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric8.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname8", "");
            machineIntegrationParametric8.value = "1";
            machineIntegrationParametric8.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric8);
            // 中值A（测距值最大值 + 测距值最小值）/ 2
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric9 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric9.name = "ZZA";
            machineIntegrationParametric9.value = "1";
            machineIntegrationParametric9.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric9);
            // 中值B（测距值最大值 + 测距值最小值）/ 2
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric91 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric91.name = "ZZB";
            machineIntegrationParametric91.value = "1";
            machineIntegrationParametric91.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric91);
            // 极差值A（测距值最大值 - 最小值）
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric10 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric10.name = "JCZA";
            machineIntegrationParametric10.value = "0.9";
            machineIntegrationParametric10.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric10);
            // 极差值B（测距值最大值 - 最小值）
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric10b = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric10b.name = "JCZB";
            machineIntegrationParametric10b.value = "1.1";
            machineIntegrationParametric10b.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric10b);
            // 补偿值差值A(理论补偿值与实际补偿值差值)
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric11 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric11.name = "BCZCZA";
            machineIntegrationParametric11.value = "1";
            machineIntegrationParametric11.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric11);
            // 补偿值差值B(理论补偿值与实际补偿值差值)
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric11b = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric11b.name = "BCZCZB";
            machineIntegrationParametric11b.value = "1";
            machineIntegrationParametric11b.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric11b);
            //离焦量
            AutoWeight.machineIntegrationParametricData machineIntegrationParametric12 = new AutoWeight.machineIntegrationParametricData();
            machineIntegrationParametric12.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname12", "");
            machineIntegrationParametric12.value = "1.3";
            machineIntegrationParametric12.dataType = AutoWeight.ParameterDataType.NUMBER;
            DClists.Add(machineIntegrationParametric12);
            #endregion

            req_arg.parametricDataArray = DClists.ToArray();

            DateTime endtime = DateTime.Now;
            AutoWeight.dataCollectForSfcExResponse dataCollectForSfcExResponse = null;
            req.SfcDcExRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                dataCollectForSfcExResponse = wsService.dataCollectForSfcEx(req as AutoWeight.dataCollectForSfcEx);
            }
            catch (Exception ex)
            {
                isSaveCsvFile = false;
                isSaveCsvlogFile = false;
                throw ex;
            }
            return dataCollectForSfcExResponse;

        }

        /// <summary>
        /// block出站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static AutoWeight.sfcCompleteResponse BlocksfcComplete(string inipath, string sitename, string messfc, string pn)
        {
            //sfcComplete
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.sfcComplete();
            var req_arg = new AutoWeight.completeSfcRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            if (string.IsNullOrEmpty(pn))
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            }
            else
            {
                if (pn == "830100-02437")
                {
                    req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
                }
                else
                {
                    req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourcB", "");
                }
            }
            string[] sfcs = { messfc };
            req_arg.sfcArray = sfcs;
            DateTime endtime = DateTime.Now;
            AutoWeight.sfcCompleteResponse sfcCompleteResponse = null;
            req.CompleteSfcRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                sfcCompleteResponse = wsService.sfcComplete(req as AutoWeight.sfcComplete);
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("sfcArray", req_arg.sfcArray);
                dicLst.Clear();
                dicLst.Add(_dic);
                // 判断模组号是否有

                // _log.ToCSVData(dicLst, sfc, sitename);
            }
            if (isSaveCsvlogFile)
            {
                _dic_log.Clear();
                //_dic_log.Add("SFC", sfc);
                _dic_log.Add("接口调用时间", startime);
                string s = "{";
                foreach (var item in _dic)
                {
                    s += item.Key.ToString() + ":" + item.Value.ToString() + ",";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", sfcCompleteResponse.@return.code);
                _dic_log.Add("返回信息", sfcCompleteResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                //_log.ToCSVLOG(dicLst, sitename, sfc, "出站");
            }
            return sfcCompleteResponse;
        }

        /// <summary>
        /// 焊前寻址出站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="messfc">模组号</param>
        /// <returns></returns>
        public static AutoWeight.sfcCompleteResponse SeekAddrComplete(string inipath, string sitename, string messfc)
        {
            //sfcComplete
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.sfcComplete();
            var req_arg = new AutoWeight.completeSfcRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resourc", "");
            string[] sfcs = { messfc };
            req_arg.sfcArray = sfcs;
            DateTime endtime = DateTime.Now;
            AutoWeight.sfcCompleteResponse sfcCompleteResponse = null;
            req.CompleteSfcRequest = req_arg;
            try
            {
                sfcCompleteResponse = wsService.sfcComplete(req as AutoWeight.sfcComplete);
            }
            catch (Exception)
            {

                throw;
            }
            return sfcCompleteResponse;
        }

        /// <summary>
        /// Busbar出站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="messfc">模组号</param>
        /// <returns></returns>
        public static AutoWeight.sfcCompleteResponse BusbarComplete(string inipath, string sitename, string messfc)
        {
            //sfcComplete
            DateTime startime = DateTime.Now;
            var wsService = new AutoWeight.MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            //设置传输参数
            var req = new AutoWeight.sfcComplete();
            var req_arg = new AutoWeight.completeSfcRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resourc", "");
            string[] sfcs = { messfc };
            req_arg.sfcArray = sfcs;
            DateTime endtime = DateTime.Now;
            AutoWeight.sfcCompleteResponse sfcCompleteResponse = null;
            req.CompleteSfcRequest = req_arg;

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            try
            {
                sfcCompleteResponse = wsService.sfcComplete(req as AutoWeight.sfcComplete);
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
                _dic.Add("user", req_arg.user);
                _dic.Add("operation", req_arg.operation);
                _dic.Add("operationRevision", req_arg.operationRevision);
                _dic.Add("resource", req_arg.resource);
                _dic.Add("sfcArray", req_arg.sfcArray);
                dicLst.Clear();
                dicLst.Add(_dic);
                _log.ToCSVData(dicLst, messfc, sitename);
            }
            if (isSaveCsvlogFile)
            {
                _dic_log.Clear();
                _dic_log.Add("SFC", messfc);
                _dic_log.Add("接口调用时间", startime);
                string s = "{";
                foreach (var item in _dic)
                {
                    s += item.Key.ToString() + ":" + item.Value.ToString() + ",";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", sfcCompleteResponse.@return.code);
                _dic_log.Add("返回信息", sfcCompleteResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, messfc, "Busbar出站");
            }

            return sfcCompleteResponse;
        }

        /// <summary>
        /// block进站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miFindCustomAndSfcDataResponse ShimEntry(string inipath, string sitename, string messfc, string pnType)
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
            string sfc = messfc.Substring(0, 24);

            if (pnType == "830100-02436")
            {
                req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
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
                _dic.Add("resource", req_arg.resource);
                _dic.Add("modeProcessSFC", req_arg.modeProcessSFC);
                _dic.Add("sfc", sfc);
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
                    s += item.Key.ToString() + ":" + item.Value.ToString() + ",";
                }
                s += "}";
                _dic_log.Add("接口传参数", s);
                _dic_log.Add("接口调用返回时间", endtime);
                string difftime = DateDiff(startime, endtime);
                _dic_log.Add("耗时", difftime);
                _dic_log.Add("返回代码", miResponse.@return.code);
                _dic_log.Add("返回信息", miResponse.@return.message);
                dicLst.Clear();
                dicLst.Add(_dic_log);
                _log.ToCSVLOG(dicLst, sitename, sfc, "进站");
            }
            return miResponse;
        }

        /// 计算两个日期的时间间隔,返回的是时间间隔的日期差的绝对值.
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
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

        /// <summary>
        /// 保存模组号 资源号数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        static HttpResponseResultModel<ModuleAndPnEntity> AddModuleAndPnAync(ModuleAndPnEntity moduleAndPnEntity)
        {
            return _requestToHttpHelper.PostAsync<ModuleAndPnEntity>(new HttpRequestModel
            {
                Host = "http://localhost:8081/",
                Path = "/ModuleAndPn/insert",
                Data = moduleAndPnEntity
            }).Result;
        }

    }
}
