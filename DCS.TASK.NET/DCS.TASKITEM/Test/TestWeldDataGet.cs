using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASKITEM.Test
{
    internal class TestWeldDataGet : IPeriodicTask
    {
        public TestWeldDataGet(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        RequestToHttpHelper _requestToHttpHelper;
        static CollectTaskContext _collectTaskContext;
        string _Apihost;
        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            _Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
        }

        public void DoTask()
        {

            
            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();
            string tagName = "\"WeldDataTest\".";

            DateTime starttime= DateTime.Now;
            HttpResponseResultModel<List<WeldDataEntity>> weldData = GetDataAync(new WeldDataEntity { MoudleCode = "20220415" });
            List<WeldDataEntity> weldDatas = weldData.BackResult;
            for (int i = 0; i < weldDatas.Count; i++)
            {
                writeTags[tagName + $"\"Welddatatype\"[{i}].X"] = weldDatas[i].X;
                writeTags[tagName + $"\"Welddatatype\"[{i}].Y"] = weldDatas[i].Y;
                writeTags[tagName + $"\"Welddatatype\"[{i}].Z"] = weldDatas[i].Z;
            }
            
            //写入PLC变量
            if (!opc.WriteNodes(writeTags))
            {
                _collectTaskContext.TaskMsgOperator.SetPairText("写入失败", "111");
            }
            DateTime endtime= DateTime.Now;
            string diff = DateDiff(starttime, endtime);
            _collectTaskContext.TaskMsgOperator.SetPairText("测试耗时", diff);
        }

        public void DoUnInit()
        {
            throw new NotImplementedException();
        }
        HttpResponseResultModel<List<WeldDataEntity>> GetDataAync(WeldDataEntity weldDataEntity)
        {
            return _requestToHttpHelper.GetAsync<List<WeldDataEntity>>(new HttpRequestModel
            {
                Host = _Apihost,
                Path = $"/WeldData/GetList?&MoudleCode=" + weldDataEntity.MoudleCode,
                Data = weldDataEntity
            }).Result;
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
