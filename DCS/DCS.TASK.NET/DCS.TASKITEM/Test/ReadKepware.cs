using DCS.CORE;
using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASKITEM.Test
{
    public class ReadKepware : IPeriodicTask
    {
        CollectTaskContext _collectTaskContext = null;

        Dictionary<string, object> _data = new Dictionary<string, object>();

        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;


            for (int i = 1; i <= 8; i++)
            {
                _data[$"模拟器示例.函数.Random{i}"] = null;
            }
        }


        public void DoTask()
        {
            if (_collectTaskContext.OpcOperator.ReadNodes(_data))
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
