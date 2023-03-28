using DCS.CORE;
using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.TASKITEM.Test
{
    public class ReadTask : IPeriodicTask
    {
        TimedTaskContext _timedTaskContext;

        int index = 0;

        Dictionary<string,int> TagName = new Dictionary<string,int>();

        List<string> Tags ;

        public void DoInit(TimedTaskContext taskContext)
        {
            _timedTaskContext = taskContext;
            index = 0;

            for (int i = 0; i < 10; i++)
            {
                TagName["Tag" + i] = i;
            }

            Tags = TagName.Keys.ToList();
        }

        public void DoTask()
        {
            index++;
            _timedTaskContext.TaskMsgOperator.SetText($"任务已运行:{index}次");

            foreach (var item in Tags)
            {
                TagName[item]++;
            }

            foreach (var item in TagName)
            {
                _timedTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
            }
        }

        public void DoUnInit()
        {

        }
    }
}
