using DCS.BASE.IniFile;
using MESwebservice.ShimCollForSfc;
using MESwebservice.ShimEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// A120箱体贴垫片
/// </summary>
namespace MESwebservice.Mescall
{
    public static class PasteBoxShimCall
    {
        /// <summary>
        /// A120箱体贴垫片--进站
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
            req_arg.site  = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.sfcOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "sfcOrder", "");
            req_arg.targetOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "targetOrder", "");
            req_arg.findSfcByInventory = true;
            customDataInParametricData parametricData = new customDataInParametricData();
            parametricData.category = MESwebservice.ShimEntry.ObjectAliasEnum.ITEM;
            parametricData.dataField = IniFileAPI.INIGetStringValue(inipath, sitename, "dataField", "");
            MESwebservice.ShimEntry.ObjectAliasEnum[] masterDataArrayField = new MESwebservice.ShimEntry.ObjectAliasEnum[50];
            req_arg.masterDataArray = masterDataArrayField;
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
        /// A120箱体贴垫片--收数出战
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static dataCollectForResourceFAIResponse ShimCollForSfc(string inipath, string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new DataCollectForResourceFAIServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            var req = new dataCollectForResourceFAI();
            var req_arg = new dataCollectForResourceFAIRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            // activityId
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            // modeProcessSfc
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            machineIntegrationParametricData parametricData = new machineIntegrationParametricData();
            parametricData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "name", "");
            parametricData.value = IniFileAPI.INIGetStringValue(inipath, sitename, "value", "");
            parametricData.dataType = ParameterDataType.NUMBER;
            List<machineIntegrationParametricData> machineIntegrationParametricDatas = new List<machineIntegrationParametricData>();
            machineIntegrationParametricDatas.Clear();
            machineIntegrationParametricDatas.Add(parametricData);
            req_arg.parametricDataArray = machineIntegrationParametricDatas.ToArray();

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            dataCollectForResourceFAIResponse response = null;
            try
            {
                response = wsService.dataCollectForResourceFAI(req as dataCollectForResourceFAI);
                code = response.@return.code;
                msg = response.@return.message;
                endtime = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }


    }
}
