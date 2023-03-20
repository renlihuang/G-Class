using DCS.BASE.IniFile;
using MESwebservice.CollectSfc;
using MESwebservice.PoleArrival;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectAliasEnum = MESwebservice.PoleArrival.ObjectAliasEnum;
/// <summary>
/// 极性寻址
/// </summary>
namespace MESwebservice.Mescall
{
    public static class PoleCall
    {

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miFindCustomAndSfcDataResponse PoleModuleEntry(string inipath, string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiFindCustomAndSfcDataServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
             //设置传输参数
            var req = new miFindCustomAndSfcData();
            var req_reg = new findCustomAndSfcDataRequest();
            req_reg.site  = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_reg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_reg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_reg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_reg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_reg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_reg.sfcOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "sfcOrder", "");
            req_reg.targetOrder = IniFileAPI.INIGetStringValue(inipath, sitename, "targetOrder", "");
            req_reg.findSfcByInventory = true;
            customDataInParametricData[] customDataArrayField = new customDataInParametricData[50];
            req_reg.customDataArray = customDataArrayField;
            customDataInParametricData parametricData = new customDataInParametricData();

            parametricData.category = ObjectAliasEnum.ITEM;
            parametricData.dataField = IniFileAPI.INIGetStringValue(inipath, sitename, "dataField", "");
            ObjectAliasEnum[] objectAliasEnums = new ObjectAliasEnum[50];
            req_reg.masterDataArray = objectAliasEnums;
            req_reg.modeProcessSFC = modeProcessSFC.MODE_START_SFC;
            req_reg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            req_reg.inventory = IniFileAPI.INIGetStringValue(inipath, sitename, "inventory", "");

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
                isSaveCsvFile = false;
                isSaveCsvlogFile = false;
            }
            return miResponse;
        }

        /// <summary>
        /// 收数出站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static dataCollectForResourceFAIResponse CollectForSfcEx(string inipath, string sitename)
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
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            // modeProcessSfc
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            machineIntegrationParametricData parametricData = new machineIntegrationParametricData();
            parametricData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "name", "");
            parametricData.value = IniFileAPI.INIGetStringValue(inipath, sitename, "value", "");
            parametricData.dataType = ParameterDataType.NUMBER;

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;

            dataCollectForResourceFAIResponse miResponse = null;
            try
            {
                miResponse = wsService.dataCollectForResourceFAI(req as dataCollectForResourceFAI);
                code = miResponse.@return.code;
                msg = miResponse.@return.message;
                endtime = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw;
            }
            return miResponse;
        }


    }
}
