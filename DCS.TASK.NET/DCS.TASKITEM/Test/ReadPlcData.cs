using DCS.CORE;
using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASKITEM.Test
{
    internal class ReadPlcData : IPeriodicTask
    {
        CollectTaskContext _collectTaskContext = null;

        Dictionary<string, object> _data = new Dictionary<string, object>();

        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;

            string tagName = "\"TestDB\".";

            for (int i = 0; i < 10; i++)
            {
                _data[tagName + $"\"a\"[{i}]"] = null;
            }
        }

        public void DoTask()
        {
           var opc =  _collectTaskContext.OpcOperator;

            if (opc.ReadNodes(_data))
            {
                foreach (var item in _data)
                {
                    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                }
            }
        }

        public void DoUnInit()
        {

        }
    }
}
