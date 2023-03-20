using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.CORE.Interface
{
    /// <summary>
    /// 保存参数相关
    /// </summary>
    public interface IDataMap
    {

        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetDataByKey(string key);
    }
}
