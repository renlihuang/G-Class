using DCS.BASE.IniFile;
using MESwebservice.AutoWeight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 自动称重
/// </summary>
namespace MESwebservice.Mescall
{
    public static class WeightCall
    {
     

        /// <summary>
        /// 模组称重收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static dataCollectForModuleTestResponse CheckWeight(string inipath, string sitename)
        {
            // 获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MachineIntegrationServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;

            var req = new dataCollectForModuleTest();
            var req_arg = new dcForModuleTestRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.user = IniFileAPI.INIGetStringValue(inipath, sitename, "user", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activityId = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.dcGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroup", "");
            req_arg.dcGroupRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "dcGroupRevision", "");
            string modeProcessSfc= IniFileAPI.INIGetStringValue(inipath, sitename, "modeProcessSfc", "");
            switch (modeProcessSfc)
            {
                case "MODE_START_SFC_PRE_DC_SFC_COMPLETE":
                    req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_START_SFC_PRE_DC_SFC_COMPLETE;
                    break;
                case "MODE_START_SFC_PRE_DC":
                    req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_START_SFC_PRE_DC;
                    break;
                case "MODE_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_COMPLETE_SFC_POST_DC;
                    break;
                case "MODE_PASS_SFC_POST_DC":
                    req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_PASS_SFC_POST_DC;
                    break;
                case "MODE_REMOVE_PROCESSLOT_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_REMOVE_PROCESSLOT_COMPLETE_SFC_POST_DC;
                    break;
                case "MODE_START_AND_COMPLETE_SFC_POST_DC":
                    req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_START_AND_COMPLETE_SFC_POST_DC;
                    break;

                default:
                    req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_NONE;
                    break;
            }
            
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            List<machineIntegrationParametricData> datas = new List<machineIntegrationParametricData>();
            machineIntegrationParametricData machineIntegrationParametricData = new machineIntegrationParametricData();
            machineIntegrationParametricData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "");
            machineIntegrationParametricData.value = IniFileAPI.INIGetStringValue(inipath, sitename, "dcvalue", "");
            datas.Clear();
            datas.Add(machineIntegrationParametricData);
            req_arg.parametricDataArray = datas.ToArray();
            List<nonConfirmCodeArray> nclist = new List<nonConfirmCodeArray>();
            nonConfirmCodeArray ncen = new nonConfirmCodeArray();
            ncen.ncCode = "";
            ncen.hasNc = false;
            nclist.Clear();
            nclist.Add(ncen);
            req_arg.ncCodeArray = nclist.ToArray();
            req.DcForModuleTestRequest = req_arg;
            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            dataCollectForModuleTestResponse miResponse = null;
            try
            {
                miResponse = wsService.dataCollectForModuleTest(req as dataCollectForModuleTest);
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

    }
}
