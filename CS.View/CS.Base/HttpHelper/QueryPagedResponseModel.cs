using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Base.HttpHelper
{
    public class QueryPagedResponseModel<T>
    {
       //总行数 
       public int Total { set; get;}
        
       //查询结果LIST
       public List<T> Data;
    }
}
