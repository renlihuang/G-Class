using CS.Base.AppSet;
using DCS.BASE.IniFile;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace MESwebservice.Mesini
{
    /// <summary>
    /// mes配置文档帮助类
    /// </summary>
    public class MesiniHelper
    {
        #region MES配置文档相关
        /// <summary>
        /// 初始化所有mes配置文档
        /// </summary>
        public static void InitAllmesset()
        {
            if (!File.Exists(string.Format(@AppConfig.MesFilePath + "MESCFG.ini")))
            {
                if (!Directory.Exists(string.Format(@AppConfig.MesFilePath)))
                {
                    //新建文件夹
                    Directory.CreateDirectory(string.Format(@AppConfig.MesFilePath));
                }
                //#region ocv电芯测试初始化
                //Ocvgetdata(@AppConfig.WebserviceiniPath, "A030电芯获取数据");
                //Ocvcheckdata(@AppConfig.WebserviceiniPath, "A030电芯判定数据");
                //#endregion
            }
        } 
        #endregion
        #region A030OVC相关
        /// <summary>
        /// ocv判定数据
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void Ocvcheckdata(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2002");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Multispec", "true");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventoryId", "SFC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcSequence", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "marking", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");
        }
        /// <summary>
        /// ocv获取数据数据
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void Ocvgetdata(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcSequence", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Multispec", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "OPERATION01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventoryList", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventoryId", "14T7175LA194");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventoryDatalist", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "name", "OCVM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "value", "2.169328");
        }
        #endregion
        #region BLOCK相关
        /// <summary>
        /// block释放模组号
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BlockreleaseMoudle(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcQty", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "processlot", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", "MODE_START_SFC_PRE_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "isCarrierType", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "ColumnOrRowFirst", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "location", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");

        }
        /// <summary>
        /// Block电芯校验
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BlockcheckInventory(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeCheckInventory", "MODE_NONE");
            //该模组校验的第一个电芯为true，其它电芯为false
            IniFileAPI.INIWriteValue(Mespath, Sitename, "firstInventory", "true");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "requiredQuantity", "1");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventoryArray", "");


        }
        /// <summary>
        /// Block模组装配电芯
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BlockcheckassembleInventory(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_NONE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "partialAssembly", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "ncCode", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "hasNc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventoryArray", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");

        }
        /// <summary>
        /// Block收数
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BlockcollectForSFC(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_NONE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");
        }
        /// <summary>
        /// Block出站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BlockComplete(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcArray", "");
        }
        /// <summary>
        /// Block入packing进站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BlockinPackGetin(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcOrder", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "targetOrder", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "findSfcByInventory", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "category", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dataField", "XPXS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "masterDataArray", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_START_SFC_PRE_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventory", "EAP_WS");

        }
        /// <summary>
        /// Block入packing收数出站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        /// 
        private static void BlockinPackDatacollect(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_COMPLETE_SFC_POST_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");
        }
        #endregion
        #region Packing垫片相关
        /// <summary>
        /// Packing垫片进站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void PackingshimGetin(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcOrder", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "targetOrder", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "findSfcByInventory", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "category", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dataField", "XPXS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "masterDataArray", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_START_SFC_PRE_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventory", "EAP_WS");
        }
        /// <summary>
        /// Packing垫片收数
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void PackingDatacollect(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_COMPLETE_SFC_POST_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");
        }
        #endregion
        #region 贴模组标相关
        /// <summary>
        /// 贴模组标进站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void MoudleTagGetin(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcOrder", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "targetOrder", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "findSfcByInventory", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "category", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dataField", "XPXS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "masterDataArray", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_START_SFC_PRE_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventory", "EAP_WS");
        }
        /// <summary>
        /// 贴模组标收数
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void MoudleTagDatacollect(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_COMPLETE_SFC_POST_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");
        }
        /// <summary>
        /// 电芯装配校验
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void MoudleTagDatarelease(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
        //为空时，默认为true True: 为SFC模式，即在SFC字段中输入的是SFC；False: 为组件模式，即在SFC字段中输入的是SFC的组件
            IniFileAPI.INIWriteValue(Mespath, Sitename, "SFCMode", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "SFC", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "ITEMGROUP", "FC-FL");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Attributes", "M_CELL_SEQUENCE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Value", " ");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "isCheckSequence ", "FALSE");
        }
        #endregion
        #region 焊前寻址相关
        /// <summary>
        /// 焊前寻址进站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void SeekSiteGetin(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcOrder", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "targetOrder", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "findSfcByInventory", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "category", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dataField", "XPXS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "masterDataArray", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_START_SFC_PRE_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventory", "EAP_WS");

        }
        /// <summary>
        /// 焊前寻址收数出站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void SeekSiteDatacollect(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_COMPLETE_SFC_POST_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");

        }
        #endregion
        #region 贴箱体标相关
        /// <summary>
        /// 贴箱体标进站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BoxLabelGetin(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcOrder", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "targetOrder", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "findSfcByInventory", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "category", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dataField", "XPXS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "masterDataArray", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_START_SFC_PRE_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventory", "EAP_WS");

        }
        /// <summary>
        /// 贴箱体标获取铭牌
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BoxLabelGetPrint(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");

            IniFileAPI.INIWriteValue(Mespath, Sitename, "SFC", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "ITEM ", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TEMPLATE", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "templateVersion", "1");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "field", "TEXT1");

        }
        /// <summary>
        /// 贴箱体标校验贴纸pn及库存
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BoxLabelCheckBom(string Mespath, string Sitename)
        {

            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_COMPLETE_SFC_POST_DC");
        }
        /// <summary>
        /// 贴箱体标组装物料
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BoxLabelMiAssemble(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_NONE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "partialAssembly", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "ncCode", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "hasNc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventoryArray", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");


        }
        /// <summary>
        /// 贴箱体标收数出站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void BoxLabelDatacollect(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_COMPLETE_SFC_POST_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");
        }
        #endregion
        #region 模组称重相关
        /// <summary>
        /// 模组称重进站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void ModeleWeightGetin(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfcOrder", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "targetOrder", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "findSfcByInventory", "TRUE");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "category", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dataField", "XPXS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "masterDataArray", "ITEM");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_START_SFC_PRE_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "inventory", "EAP_WS");

        }
        /// <summary>
        /// 模组称重收数出站
        /// </summary>
        /// <param name="Mespath"></param>
        /// <param name="Sitename"></param>
        private static void ModeleWeightDatacollect(string Mespath, string Sitename)
        {
            IniFileAPI.INIWriteValue(Mespath, Sitename, "MESWSDL", @"http://fdmes2.catlbattery.com:8103/atlmeswebservice/MiFindCustomAndSfcDataServiceService?wsdl");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "TimeOut", "50000");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Username", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Password", "test12345");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "site", "2001");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "user", "SUP_TEST01");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operation", "BARSTK");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "operationRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "activityId", "EAP_WS");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "Resource", "MMAW0010");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroup", "*");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcGroupRevision", "#");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "modeProcessSfc", " MODE_COMPLETE_SFC_POST_DC");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "sfc", "");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcname", "ocv");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dcvalue", "1.2");
            IniFileAPI.INIWriteValue(Mespath, Sitename, "dctype", "NUMBER");

        }
        #endregion
    }
}   
