using DCS.BASE.IniFile;
using MESwebservice.AttrDataEntry;
using MESwebservice.FindCusAndSfc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESwebservice.Mescall
{
    public static class PasteModuleCall
    {

        /// <summary>
        /// 获取模组号
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miFindCustomAndSfcDataResponse FindCustomAndSfcData(string inipath, string sitename)
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
            // dcGroup
            // dcGroupRevision
            req_arg.modeProcessSFC = modeProcessSFC.MODE_START_SFC;
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
        /// 更改生产
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static ChangePro.miFindCustomAndSfcDataResponse ChangePro(string inipath, string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new ChangePro.MiFindCustomAndSfcDataServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;
            var req = new ChangePro.miFindCustomAndSfcData();
            var req_arg = new ChangePro.findCustomAndSfcDataRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.operation = IniFileAPI.INIGetStringValue(inipath, sitename, "operation", "");
            req_arg.operationRevision = IniFileAPI.INIGetStringValue(inipath, sitename, "operationRevision", "");
            req_arg.activity = IniFileAPI.INIGetStringValue(inipath, sitename, "activityId", "");
            req_arg.resource = IniFileAPI.INIGetStringValue(inipath, sitename, "resource", "");
            req_arg.modeProcessSFC = MESwebservice.ChangePro.modeProcessSFC.MODE_START_SFC;
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            // dcGroup
            // dcGroupRevision
            // dcname
            // dcvalue
            // dctype

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            ChangePro.miFindCustomAndSfcDataResponse miResponse = null;
            try
            {
                miResponse = wsService.miFindCustomAndSfcData(req as ChangePro.miFindCustomAndSfcData);
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
        /// 电芯装配校验
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static miSfcAttriDataEntryResponse AttrDataEntry(string inipath, string sitename)
        {
            //获取登录参数
            DateTime startime = DateTime.Now;
            var wsService = new MiSFCAttriDataEntryServiceService();
            wsService.Url = IniFileAPI.INIGetStringValue(inipath, sitename, "MESWSDL", "");
            wsService.Timeout = Convert.ToInt32(IniFileAPI.INIGetStringValue(inipath, sitename, "TimeOut", ""));
            wsService.Credentials = new System.Net.NetworkCredential(IniFileAPI.INIGetStringValue(inipath, sitename, "Username", ""), IniFileAPI.INIGetStringValue(inipath, sitename, "Password", ""), null);
            wsService.PreAuthenticate = true;

            //设置传输参数
            var req = new miSfcAttriDataEntry();
            var req_arg = new miSFCAttriDataEntryRequest();
            req_arg.site = IniFileAPI.INIGetStringValue(inipath, sitename, "site", "");
            req_arg.userId = IniFileAPI.INIGetStringValue(inipath, sitename, "userId", "");
            // req_arg.op
            req_arg.sfcMode = IniFileAPI.INIGetStringValue(inipath, sitename, "SFCMode", "");
            req_arg.sfc = IniFileAPI.INIGetStringValue(inipath, sitename, "sfc", "");
            req_arg.itemGroup = IniFileAPI.INIGetStringValue(inipath, sitename, "itemGroup", "");
            List<sfcData> sfcDatas = new List<sfcData>();
            sfcData data = new sfcData();
            data.attributes = IniFileAPI.INIGetStringValue(inipath, sitename, "Attributes", "");
            data.value = IniFileAPI.INIGetStringValue(inipath, sitename, "value", "");
            data.sequence = IniFileAPI.INIGetStringValue(inipath, sitename, "sequence", "");
            sfcDatas.Clear();
            sfcDatas.Add(data);
            req_arg.sfcDatalist = sfcDatas.ToArray();
            req_arg.isCheckSequence = IniFileAPI.INIGetStringValue(inipath, sitename, "isCheckSequence", "");

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            miSfcAttriDataEntryResponse miResponse = null;
            req.MiSFCAttriDataEntryRequest = req_arg;
            try
            {
                miResponse = wsService.miSfcAttriDataEntry(req as miSfcAttriDataEntry);
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


    }
}
