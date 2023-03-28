using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using MESwebservice.AutoWeight;
using MESwebservice.ShimEntry;
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
    public class WeightCall
    {
        //csvdata
        private static Dictionary<string, object> _dic = new Dictionary<string, object>();
        //csvlog
        private static Dictionary<string, object> _dic_log = new Dictionary<string, object>();

        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();

        //日志帮助类
        static ILogOperator _log;
        static CollectTaskContext _collectTaskContext;
        public WeightCall(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            _log = _collectTaskContext.LogOperator;
        }
        /// <summary>
        ///进站
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        public static miFindCustomAndSfcDataResponse ShimEntry(string inipath, string sitename, string packno)
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
            req_arg.sfc = packno;

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
            return miResponse;
        }

        /// <summary>
        /// 模组称重收数
        /// </summary>
        /// <param name="inipath"></param>
        /// <param name="sitename"></param>
        /// <returns></returns>
        public static dataCollectForModuleTestResponse CheckWeight(string inipath, string sitename, string sfc, string weight)
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
            req_arg.modeProcessSfc = dataCollectForSfcModeProcessSfc.MODE_NONE;
            req_arg.sfc = sfc;
            List<machineIntegrationParametricData> datas = new List<machineIntegrationParametricData>();
            machineIntegrationParametricData machineIntegrationParametricData = new machineIntegrationParametricData();
            machineIntegrationParametricData.name = IniFileAPI.INIGetStringValue(inipath, sitename, "dcname", "");
            machineIntegrationParametricData.value = weight;
            datas.Clear();
            datas.Add(machineIntegrationParametricData);
            req_arg.parametricDataArray = datas.ToArray();

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
                _dic.Add("activityId", req_arg.activityId);
                _dic.Add("resource", req_arg.resource);
                _dic.Add("dcGroup", req_arg.dcGroup);
                _dic.Add("dcGroupRevision", req_arg.dcGroupRevision);
                _dic.Add("modeProcessSfc", req_arg.modeProcessSfc);
                _dic.Add("sfc", sfc);

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
                _log.ToCSVLOG(dicLst, sitename, sfc, "称重收数");
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

    }
}
