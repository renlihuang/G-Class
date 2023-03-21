using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Model.Business.Entiry
{
    public class ParamValueEntiry
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        ///  对应参数名ID
        /// </summary>
        public long ParentID { get; set; }

        /// <summary>
        ///  参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  参数值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
