using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.QueryCondition
{
    public class ParamValueCondition
    {
        /// <summary>
        ///  对应参数名ID
        /// </summary>
        public long ParentID { get; set; }

        /// <summary>
        ///  参数名称
        /// </summary>
        public string Name { get; set; }
    }
}
