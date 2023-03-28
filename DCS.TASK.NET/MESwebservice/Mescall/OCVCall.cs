using DCS.BASE.IniFile;
using DCS.MODEL.Entiry;
using MESwebservice.OCVCheckData;
using MESwebservice.OCVgetData;
using System;
using System.Collections.Generic;

namespace MESwebservice.Mescall
{
    /// <summary>
    /// OCV测试web调用相关
    /// </summary>
    public static class OCVCall
    {
        /// <summary>
        /// ocv判定数据
        /// </summary>
        /// <param name = "inipath" ></ param >
        /// < param name="sitename"></param>
        /// <returns></returns>
        public static cellCustomDCCheckResponse cellCustomDCCheck(string inipath, string sitename, List<BatteryCoreOcvTestEntity> entities)
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
            List<miCustomDCForCellInventory> miCustomDCForCellInventories = new List<miCustomDCForCellInventory>();
            List<miCustomDCForCellInventoryData> miCustomDCForCellInventoryDatas = new List<miCustomDCForCellInventoryData>();
            miCustomDCForCellInventories.Clear();
            miCustomDCForCellInventoryDatas.Clear();

            for (int i = 0; i < entities.Count; i++)
            {
                miCustomDCForCellInventory inventory = new miCustomDCForCellInventory();
                miCustomDCForCellInventoryData inventoryData = new miCustomDCForCellInventoryData();
                inventory.inventoryId = entities[i].BatteryCoreCode;
                inventory.marking = IniFileAPI.INIGetStringValue(inipath, sitename, "marking", "");
                inventoryData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "")+$"{i}";
                inventoryData.value = entities[i].OcvVoltage;
                miCustomDCForCellInventories.Add(inventory);
                miCustomDCForCellInventoryDatas.Add(inventoryData);
                inventory.inventoryDatalist = miCustomDCForCellInventoryDatas.ToArray();
            }
            req_arg.inventoryList = miCustomDCForCellInventories.ToArray();
            DateTime endtime = DateTime.Now;
            cellCustomDCCheckResponse cellRespon = null;
            req.Request = req_arg;
            try
            {
                cellRespon = wsService.cellCustomDCCheck(req as cellCustomDCCheck);
                endtime = DateTime.Now;
            }
            catch (Exception ex)
            {

            }
            return cellRespon;
        }
        /// <summary>
        /// ocv获取数据
        /// </summary>
        /// <param name = "inipath" ></ param >
        /// < param name="sitename"></param>
        /// <returns></returns>
        public static miBatchStartAndGetParameterResponse GetOCVdata(string inipath, string sitename, List<BatteryCoreOcvTestEntity> Bentities)
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
           // req_arg.startMode = true;
            //req_arg.processLot = IniFileAPI.INIGetStringValue(inipath, sitename, "Resource", "");
            List<string> cods = new List<string>();
            cods.Clear();
            for (int i = 0; i < Bentities.Count; i++)
            {
                cods.Add(Bentities[i].BatteryCoreCode);
            }
            req_arg.sfcArray = cods.ToArray();
            req_arg.isGetFirstValue = IniFileAPI.INIGetStringValue(inipath, sitename, "isGetFirstValue", "");

            int code = -1;
            string msg = string.Empty;
            bool isSaveCsvFile = true;
            bool isSaveCsvlogFile = true;
            DateTime endtime = DateTime.Now;
            miBatchStartAndGetParameterResponse miResponse = null;
            req.BatchStartSfcRequest = req_arg;
            try
            {

                miResponse = wsService.miBatchStartAndGetParameter(req as miBatchStartAndGetParameter);
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
