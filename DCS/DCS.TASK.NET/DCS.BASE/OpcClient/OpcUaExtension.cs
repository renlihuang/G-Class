using DCS.CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCS.CORE
{
    /// <summary>
    /// OPC扩展方法
    /// </summary>
    public static class ExtensionOpcUaFunction
    {
        /// <summary>
        /// 写入数据并检查一致性
        /// </summary>
        /// <param name="opcOperator"></param>
        /// <param name="Writes"></param>
        /// <returns></returns>
        public static bool ConsistencyWrite(this IOpcOperator opcOperator, Dictionary<string, object> Writes)
        {
            bool blResult = false;
            //读取的长度
            Dictionary<string, object> reads = new Dictionary<string, object>();
            //构造一个副本
            Dictionary<string, object> writes = new Dictionary<string, object>(Writes);

            List<string> tagNames = new List<string>();

            //最多检查10次，10次也不行的话.....
            for (int i = 0; i < 10; i++)
            {
                if (writes.Count == 0)
                {
                    blResult = true;
                    break;
                }
                //先写入
                if (opcOperator.WriteNodes(writes))
                {
                    //写入成功再读取
                    reads.Clear();
                    //获取变量名
                    var keys = writes.Keys.ToList();
                    //填充变量名
                    foreach (string key in keys)
                    {
                        reads[key] = null;
                    }
                    //读取变量
                    if (opcOperator.ReadNodes(reads))
                    {
                        tagNames.Clear();
                        //循环比较变量差异
                        foreach (string key in keys)
                        {
                            object a = reads[key];
                            object b = Writes[key];
                            //
                            if (a != null && b != null)
                            {
                                //找出有差异的变量
                                if (!a.Equals(b))
                                {
                                    tagNames.Add(key);
                                }
                            }
                            else
                            {
                                //读到是null,写的值不为null
                                //说明没写成功
                                if (a == null && b != null)
                                {
                                    tagNames.Add(key);
                                }
                            }
                        }

                        writes.Clear();
                        //把检查不一致的变量名重新写入
                        foreach (string tagName in tagNames)
                        {
                            writes[tagName] = Writes[tagName];
                        }
                    }
                }
            }

            return blResult;
        }
    }
}
