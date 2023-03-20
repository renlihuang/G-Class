using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.CORE.Interface
{
    /// <summary>
    /// OPC操作
    /// </summary>
    public interface IOpcOperator
    {
        /// <summary>
        /// 设置LOG操作接口
        /// </summary>
        /// <param name="logOperator"></param>
        void SetLogOperator(ILogOperator logOperator);
        /// <summary>
        /// 命名空间索引设置
        /// </summary>
        /// <param name="ns"></param>
        void SetNodeNS(int ns);
        /// <summary>
        /// 获取连接状态
        /// </summary>
        /// <returns></returns>
        bool GetConnectStatus();

        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool Open(string url);

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        void Close();

        /// <summary>
        /// 读取PLC数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        bool ReadNodes(Dictionary<string,object> values);

        /// <summary>
        /// 写入PLC数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        bool WriteNodes(Dictionary<string, object> values);
    }
}
