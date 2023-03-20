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
    internal class APIDemo : IPeriodicTask
    {
        RequestToHttpHelper _requestToHttpHelper;

        public APIDemo(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public void DoInit(TimedTaskContext taskContext)
        {
         
        }

        public void DoTask()
        {

        }

        public void DoUnInit()
        {
       
        }
    }
}
