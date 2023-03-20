using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASKITEM.Test
{
    public class ReadOcvData : IPeriodicTask
    {
        public ReadOcvData(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        RequestToHttpHelper _requestToHttpHelper;

        CollectTaskContext _collectTaskContext;

        string plcDataStatusNmae;

        string mesDataStatusNmae;

        string ocvUName;

        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;

            plcDataStatusNmae = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            mesDataStatusNmae = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            ocvUName = _collectTaskContext.DataMap.GetDataByKey("Ocv_U");
        }

        public void DoTask()
        {
            var opc = _collectTaskContext.OpcOperator;

            var log = _collectTaskContext.LogOperator;

             Dictionary<string, object> data = new Dictionary<string, object>();

            data[plcDataStatusNmae] = null;
            data[mesDataStatusNmae] = null;
            data[ocvUName] = null;

            if (opc.ReadNodes(data))
            {
                foreach (var item in data)
                {
                    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                }
                

                Int16 plcFalg = Convert.ToInt16(data[plcDataStatusNmae]);

                Int16 mesFlag = Convert.ToInt16(data[mesDataStatusNmae]);

                if (plcFalg == 1)
                {
                    _collectTaskContext.TaskMsgOperator.SetText("开始处理PLC数据");
                    log.AddUserLog("读取数据", "读取PLC", "PLC标志位等于1");
                    Dictionary<string, object> writeTags = new Dictionary<string, object>();
                    writeTags[mesDataStatusNmae] = Convert.ToInt16(2);
                    opc.WriteNodes(writeTags);
                }

            }

        }

        public void DoUnInit()
        {

        }
    }
}
