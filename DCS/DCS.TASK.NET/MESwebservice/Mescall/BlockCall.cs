using CS.Base.AppSet;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
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

        public BlockCall(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            _log = _collectTaskContext.LogOperator;
        }

        /// <summary>
        /// Block校验电芯
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="inventlist"></param>
        /// <returns></returns>
        public static miCheckInventoryAttributesResponse BlockCheckinvent(string inipath, string sitename) 
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
            req_arg.site= IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
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
            req_arg.requiredQuantity =Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "requiredQuantity", ""));
            // req_arg.inventoryArray=inventlist.ToArray();
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
            req_arg.site= IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            req_arg.sfcQty =Convert.ToDecimal( IniFileAPI.INIGetStringValue(inipath, sitename, "sfcQty", ""));
            req_arg.processlot = IniFileAPI.INIGetStringValue(inipath, sitename, "processlot", "");
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
                    req_arg.ColumnOrRowFirst= columnOrRow.COLUMN_FIRST;
                    break;
                default:
                    req_arg.ColumnOrRowFirst = columnOrRow.ROW_FIRST;
                    break;
            }
            // req_arg.location= location;
            req_arg.shopOrder= IniFileAPI.INIGetStringValue(inipath, sitename, "shopOrder", "");
            DateTime endtime = DateTime.Now;
            miReleaseSfcWithActivityResponse miReleaseSfcWith = null;
            req.ReleaseSfcWithActivityRequest = req_arg;
            try
            {
                miReleaseSfcWith = wsService.miReleaseSfcWithActivity(req as miReleaseSfcWithActivity);

                endtime = DateTime.Now;
            }
            catch (Exception)
            {

                throw;
            }
            return miReleaseSfcWith;

        }
        /// <summary>
        /// 模组装配电芯
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static miAssmebleAndCollectDataForSfcResponse BlockMiAssemble(string inipath, string sitename) 
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
            string sfcmode= IniFileAPI.INIGetStringValue(inipath, sitename, "mode","");
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
            req_arg.sfc= IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            List<BlockMiAssembleAndCollectData.nonConfirmCodeArray> nccodeArrays = new List<BlockMiAssembleAndCollectData.nonConfirmCodeArray>();
            BlockMiAssembleAndCollectData.nonConfirmCodeArray nonConfirmCodeArray = new BlockMiAssembleAndCollectData.nonConfirmCodeArray();
            nonConfirmCodeArray.ncCode = "";
            nonConfirmCodeArray.hasNc = true;
            nccodeArrays.Clear();
            nccodeArrays.Add(nonConfirmCodeArray);
            req_arg.ncCodeArray=nccodeArrays.ToArray();

            miInventoryData midata= new miInventoryData();
            List<miInventoryData> midataList= new List<miInventoryData>();
            midata.inventory = "";
            midata.qty = "";
            AssemblyDataField assemblyDataField = new AssemblyDataField();
            assemblyDataField.sequence =Convert.ToDecimal(2) ;
            assemblyDataField.sequenceSpecified = true;
            List<AssemblyDataField> assemblylist= new List<AssemblyDataField>();
            assemblylist.Clear(); 
            assemblylist.Add(assemblyDataField);
            midata.assemblyDataFields=assemblylist.ToArray();
            midataList.Clear();
            midataList.Add(midata);
            req_arg.inventoryArray=midataList.ToArray();

            BlockMiAssembleAndCollectData.machineIntegrationParametricData machineIntegrationParametricData = new BlockMiAssembleAndCollectData.machineIntegrationParametricData();
            machineIntegrationParametricData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "");
            machineIntegrationParametricData.value = "";
            string dctype= IniFileAPI.INIGetStringValue(inipath, sitename, "dctype", "");
            switch (dctype)
            {
                case "NUMBER":
                    machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.NUMBER;
                    break;
                case "TEXT":
                    machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.TEXT;
                    break;
                case "FORMULA":
                    machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.FORMULA;
                    break;
                default:
                    machineIntegrationParametricData.dataType = BlockMiAssembleAndCollectData.ParameterDataType.BOOLEAN;
                    break;
            }
            List<BlockMiAssembleAndCollectData.machineIntegrationParametricData> machineIntegrationParametricDatas = new List<BlockMiAssembleAndCollectData.machineIntegrationParametricData>();
            machineIntegrationParametricDatas.Clear();
            machineIntegrationParametricDatas.Add(machineIntegrationParametricData);
            req_arg.parametricDataArray= machineIntegrationParametricDatas.ToArray();
            req_arg.remark = "";
            DateTime endtime= DateTime.Now;
            miAssmebleAndCollectDataForSfcResponse miAssmebleAndCollectDataForSfcResponse = null;
            req.AssembleAndCollectDataForSfcRequest = req_arg;
            try
            {
                miAssmebleAndCollectDataForSfcResponse = wsService.miAssmebleAndCollectDataForSfc(req as miAssmebleAndCollectDataForSfc);
            }
            catch (Exception)
            {

                throw;
            }

            return miAssmebleAndCollectDataForSfcResponse;
        }
        /// <summary>
        /// Block收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static AutoWeight.dataCollectForSfcExResponse BlockDatacoll(string inipath, string sitename,string sfc)
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
                        req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
                    }
                    else
                    {
                        req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
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
                    if (pn == "830100-02436")
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
            int code = -1;
            string msg = string.Empty;
            try
            {
                dataCollectForSfcExResponse = wsService.dataCollectForSfcEx(req as AutoWeight.dataCollectForSfcEx);
                code = dataCollectForSfcExResponse.@return.code;
                msg = dataCollectForSfcExResponse.@return.message;
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
                _dic.Add("activityId", req_arg.activityId);
                _dic.Add("parametricDataArray", req_arg.parametricDataArray);
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
                foreach (var item in req_arg.parametricDataArray)
                {
                    s += item.name.ToString() + ":" + item.value.ToString() + "，";
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
                _log.ToCSVLOG(dicLst, sitename, sfc, "焊前寻址收数");
            }
            return dataCollectForSfcExResponse;

        }
        /// <summary>
        /// Busbar收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public static AutoWeight.dataCollectForSfcExResponse BusbarDatacoll(string inipath, string sitename, string sfc)
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
            return dataCollectForSfcExResponse;

        }
        /// <summary>
        /// block出站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static AutoWeight.sfcCompleteResponse BlocksfcComplete(string inipath, string sitename,string messfc,string pn)
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
                if (pn == "830100-02436")
                {
                    req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceA", "");
                }
                else
                {
                    req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "ResourceB", "");
                }
            }
          
            string[] sfcs = { messfc };
            req_arg.sfcArray = sfcs;
            DateTime endtime = DateTime.Now;
            AutoWeight.sfcCompleteResponse sfcCompleteResponse = null;
            req.CompleteSfcRequest = req_arg;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            int code = -1;
            string msg = string.Empty;
            try
            {
                sfcCompleteResponse = wsService.sfcComplete(req as AutoWeight.sfcComplete);
                code = sfcCompleteResponse.@return.code;
                msg = sfcCompleteResponse.@return.message;
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
                _log.ToCSVLOG(dicLst, "贴模组标出站", messfc, "贴模组标出站");
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
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            string[] sfcs = { messfc };
            req_arg.sfcArray = sfcs;
            DateTime endtime = DateTime.Now;
            AutoWeight.sfcCompleteResponse sfcCompleteResponse = null;
            req.CompleteSfcRequest = req_arg;

            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            int code = -1;
            string msg = string.Empty;
            try
            {
                sfcCompleteResponse = wsService.sfcComplete(req as AutoWeight.sfcComplete);
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
                _log.ToCSVLOG(dicLst, sitename, messfc, "焊前寻址出站");
            }
            return sfcCompleteResponse;
        }

        /// <summary>
        /// block进站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miFindCustomAndSfcDataResponse ShimEntry(string inipath, string sitename)
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
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.sfcOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "sfcOrder", "");
            req_arg.targetOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "targetOrder", "");
            req_arg.findSfcByInventory = true;
            //List<customDataInParametricData> parametricDataList = new List<customDataInParametricData>();
            //customDataInParametricData parametricData = new customDataInParametricData();
            //parametricData.category = ObjectAliasEnum.ITEM;
            //parametricData.dataField = IniFileAPI.INIGetStringValue(inipath, sitename, "dataField", "");
            //parametricDataList.Clear();
            //parametricDataList.Add(parametricData);
            //req_arg.customDataArray = parametricDataList.ToArray();

            List<ObjectAliasEnum> objectAliasEnumsList = new List<ObjectAliasEnum>();
            objectAliasEnumsList.Clear();
            objectAliasEnumsList.Add(ObjectAliasEnum.ITEM);
            req_arg.masterDataArray = objectAliasEnumsList.ToArray();

            req_arg.modeProcessSFC = MESwebservice.FindCusAndSfc.modeProcessSFC.MODE_START_SFC;

            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            req_arg.inventory = IniFileAPI.INIGetStringValue(inipath, sitename, "inventory", "");

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            miFindCustomAndSfcDataResponse miResponse = null;
            req.FindCustomAndSfcDataRequest = req_arg;
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
