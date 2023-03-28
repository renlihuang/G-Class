using DCS.CORE.Interface;
using DCS.TASK.NET.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.TaskManage.FrameWork
{
    public class DataMap : IDataMap
    {
        Dictionary<string, string> _dicParams;



        public DataMap(Dictionary<string, string> dataMap)
        {
            _dicParams = dataMap;
        }

     

        /// <summary>
        /// 获取参数数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDataByKey(string key)
        {
            string value = "";
            //查找对应的参数
            if (_dicParams.ContainsKey(key))
            {
                value = _dicParams[key];
            }
            else
            {
                //从全局字典中获取
                var dataMap = Global.GlobalDataMap;

                if (dataMap.ContainsKey(key))
                {
                    value = dataMap[key];
                }
            }

            return value;
        }
    }
}
