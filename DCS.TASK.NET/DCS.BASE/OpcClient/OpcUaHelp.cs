using DCS.CORE.Interface;
using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DCS.OpcClient
{
    public class OpcUaHelp : IOpcOperator
    {
        /// <summary>
        /// OPC 客户端操作
        /// </summary>
        OpcUaClient _opcUaClient;

        /// <summary>
        /// 变量NS值
        /// </summary>
        ushort _nodeNS;

        /// <summary>
        /// LOG操作
        /// </summary>
        ILogOperator _logOperator;

        /// <summary>
        /// 节点ID
        /// </summary>
        NodeId _stautsNodeId;

        /// <summary>
        /// 是否已经连接
        /// </summary>
        bool _isConnnection;

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        public static IOpcOperator CreateInstance()
        {
            return new OpcUaHelp();
        }


        private OpcUaHelp()
        {
            //创建节点
            _stautsNodeId = new NodeId(2256, 0);
            //创建客户端
            _opcUaClient = new OpcUaClient();
        }




        /// <summary>
        /// 设置LOG操作接口
        /// </summary>
        /// <param name="logOperator"></param>
        public void SetLogOperator(ILogOperator logOperator)
        {
            _logOperator = logOperator;
        }


        /// <summary>
        /// 设置ns值
        /// </summary>
        /// <param name="ns"></param>
        public void SetNodeNS(int ns)
        {
            _nodeNS = (ushort)ns;
        }

        /// <summary>
        /// 获取连接状态
        /// </summary>
        /// <returns></returns>
        public bool GetConnectStatus()
        {
            if (_isConnnection)
            {
                try
                {
                    var dataValue = _opcUaClient.ReadNode(_stautsNodeId);
                    _isConnnection =!ServiceResult.IsBad(dataValue.StatusCode);
                }
                catch
                {
                    _isConnnection = false;
                }
            }

            return _isConnnection;
        }

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool Open(string url)
        {
            if (!_isConnnection)
            {
                try
                {
                    _opcUaClient.Disconnect();
                    //连接OPC
                    Task task = _opcUaClient.ConnectServer(url);
                    //等待连接完成
                    task.Wait();
                    //是否已经连接
                    _isConnnection = true;
                    //记录LOG
                    _logOperator.AddLog(LogType.userLog, "任务组OPC UA连接", "OPC UA 连接", "OPC连接成功");
                }
                catch (Exception ex)
                {
                    //记录连接失败原因
                    _logOperator.AddLog(LogType.userLog, "任务组OPC UA连接", "OPC UA 连接", string.Format("OPC连接失败!原因为:{0}", ex.Message));
                }
            }

            return _isConnnection;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            if (_opcUaClient.Connected)
            {
                //断开连接
                _opcUaClient.Disconnect();
            }
        }

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool ReadNodes(Dictionary<string, object> values)
        {
            bool result = false;

            if (_opcUaClient.Connected && values != null && values.Count > 0)
            {
                //变量值
                NodeId [] nodeIds = new NodeId[values.Count];
                //变量名
                string[] tagNames = new string[values.Count];
                //复制字典Key值
                values.Keys.CopyTo(tagNames, 0);
                //变量索引
                int i;
                //生成NodeID
                for (i = 0; i < values.Count; i++)
                {
                    nodeIds[i] = new NodeId(tagNames[i], _nodeNS);
                }
                
                try
                {
                    //读取变量
                    List<DataValue> dataValues = _opcUaClient.ReadNodes(nodeIds);
                    //是否读取成功
                    if (dataValues.Count > 0)
                    {
                        //是否读取成功
                        bool isBad = false;

                        i = 0;
                        //判断值是否都是GOOD
                        foreach (DataValue dataValue in dataValues)
                        {
                            if (ServiceResult.IsBad(dataValue.StatusCode))
                            {
                                isBad = true;
                                _logOperator.AddLog(LogType.userLog, "任务组OPC变量读取", "读取变量失败", string.Format("读取失败变量名:{0} 状态码:{1} ", tagNames[i], dataValue.StatusCode));
                                break;
                            }
                            //增加计数
                            i++;
                        }

                        if (!isBad)
                        {
                            i = 0;
                            foreach (DataValue dataValue in dataValues)
                            {
                                values[tagNames[i++]] = dataValue.Value;
                            }
                            //设置返回结果
                            result = true;
                        }
                    }
                
                }
                catch (Exception ex)
                {
                    //记录读取失败原因
                    _logOperator.AddLog(LogType.userLog, "任务组OPC变量读取", "读取变量失败", string.Format("读取变量异常!原因为{0}", ex.Message));
                    //记录失败的变量名
                    foreach (var nodeItem in values)
                    {
                        _logOperator.AddLog(LogType.userLog, "任务组OPC变量读取", "读取变量失败", string.Format("读取失败变量名:{0}", nodeItem.Key));
                    }
                }
            }



            return result;
        }

        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool WriteNodes(Dictionary<string, object> values)
        {
            bool result = false;

            if (_opcUaClient.Connected && values != null && values.Count > 0)
            {
                try
                {
                    //变量名
                    string[] keys = new string[values.Count];
                    //变量值
                    object[] valus = new object[values.Count];
                    //复制变量值
                    values.Keys.CopyTo(keys,0);
                    //复制变量值
                    values.Values.CopyTo(valus, 0);
                    //写入变量
                    result = _opcUaClient.WriteNodes(keys, valus, _nodeNS);

                    //记录失败的变量名
                    if (!result)
                    {
                        //循环添加变量
                        foreach (var nodeItem in values)
                        {
                            _logOperator.AddLog(LogType.userLog, "任务组OPC变量写入", "写入变量失败", string.Format("{0}", nodeItem.Key));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //记录连接失败原因
                    _logOperator.AddLog(LogType.userLog, "任务组OPC变量写入", "写入变量失败", string.Format("写入变量异常!原因为: {0} 失败的变量列表为", ex.Message));
                    //
                    //循环添加变量
                    foreach (var nodeItem in values)
                    {
                        _logOperator.AddLog(LogType.userLog, "任务组OPC变量写入", "写入变量失败", string.Format("{0}", nodeItem.Key));
                    }
                }

             
            }

            return result;
        }
    }
}
