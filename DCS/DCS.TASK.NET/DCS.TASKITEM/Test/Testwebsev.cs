using DCS.CORE;
using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESwebservice.QQwebsev;
using MESwebservice.Mesini;

namespace DCS.TASKITEM.Test
{
    public class Testwebsev : IPeriodicTask
    {
        CollectTaskContext _collectTaskContext;
        //string plcDataStatusNmae;

        //string mesDataStatusNmae;

        //string ocvUName;
        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;

            //plcDataStatusNmae = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            //mesDataStatusNmae = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            //ocvUName = _collectTaskContext.DataMap.GetDataByKey("Ocv_U");
        }

        public void DoTask()
        {
            string qq = "634401095";
            var wsService = new qqOnlineWebService();
            string result = wsService.qqCheckOnline(qq);
            string showtext;
            switch (result)
            {
                case "Y":
                    showtext = "在线";
                    break;
                case "N":
                    showtext = "离线";
                    break;
                case "V":
                    showtext = "免费次数用尽";
                    break;
                default:
                    showtext = "QQ号错误";
                    break;
            }

            _collectTaskContext.TaskMsgOperator.SetText("返回值："+ result);
            _collectTaskContext.TaskMsgOperator.SetPairText(qq, showtext);

        }

        public void DoUnInit()
        {
            throw new NotImplementedException();
        }
    }
}
