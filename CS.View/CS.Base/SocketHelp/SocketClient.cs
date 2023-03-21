using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CS.Base.SocketHelp
{
    /// <summary>
    /// 简单的socket客户端
    /// </summary>
    public class SocketClient
    {
        protected Socket _clientSocket = null;
        protected Thread _recvThread = null;
        protected bool _isConected = false;
        protected bool _isRunning = false;
        protected string _IpAddress = "";
        protected short _nPort = 0;

        /// <summary>
        /// 定义接收委托
        /// </summary>
        /// <param name="strRev"></param>
        public delegate void OnNetWorkEvent(string strRev);

        public event OnNetWorkEvent RecvEvent;

        /// <summary>
        /// 重连打开连接
        /// </summary>
        /// <returns></returns>
        private bool Open()
        {
            return Open(_IpAddress, _nPort);
        }
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="IpAddr"></param>
        /// <param name="_nPort"></param>
        /// <returns></returns>
        public bool Open(string IpAddr, short _nPort)
        {

            if (!_isRunning)
            {
                _isRunning = true;
                _recvThread = new Thread(ReavThreadProc);
                _recvThread.IsBackground = true;
                _recvThread.Start();
                _IpAddress = IpAddr;
                this._nPort = _nPort;
            }

            if (!_isConected)
            {
                IPAddress ip = IPAddress.Parse(IpAddr);
                IPEndPoint ipe = new IPEndPoint(ip, _nPort);
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    if (_clientSocket != null)
                    {
                        _clientSocket.Connect(ipe);
                        if (_clientSocket.Connected)
                        {
                            _isConected = true;
                        }
                    }
                }
                catch
                { }
            }
            return _isConected;
        }

        public void Close()
        {
            if (_isConected)
            {
                RecvEvent = null;
                _isConected = false;
                _clientSocket.Close();
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReavThreadProc()
        {
            while (_isRunning)
            {
                if (_isConected)
                {
                    if (_clientSocket.Connected)
                    {
                        byte[] ReadBytes = new byte[4096];
                        try
                        {
                            int ReadByteSize = _clientSocket.Receive(ReadBytes);
                            if (ReadByteSize > 0)
                            {
                                string recStr = Encoding.ASCII.GetString(ReadBytes, 0, ReadByteSize);
                                //调用回调
                                if (RecvEvent != null)
                                {
                                    RecvEvent(recStr);
                                }
                            }
                            else
                            {
                                //接受字节小于0说明连接断开
                                Open();
                                Thread.Sleep(1000);
                            }
                        }
                        catch
                        {
                            _isConected = false;
                        }
                    }
                }
                else
                {
                    //内部打开
                    Open();
                    Thread.Sleep(1000);
                }
            }
        }


        /// <summary>
        /// 转换接收数据为为DataTable
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="ReadString"></param>
        /// <returns></returns>
        public static int ReavStringToTable(DataTable Table, string ReadString)
        {
            //DataTable行数
            int RowCount = 0;
            string Text = "";
            //去掉字符串中的引号
            foreach (char Char in ReadString)
            {
                if (Char != '"')
                {
                    Text += Char;
                }
            }

            //是否添加列
            bool blIsAddColum = false;
            if (Table.Columns.Count == 0)
            {
                blIsAddColum = true;
            }

            //去空格
            Text.Trim();
            string[] strList = Text.Split(',');

            int ColumnCount = 0;
            foreach (string StrItem in strList)
            {
                StrItem.Replace("\"", "");
                int Index = StrItem.IndexOf(":=");
                string key = StrItem.Substring(0, Index);
                string Value = StrItem.Substring(Index + 2, StrItem.Length - Index - 2);

                if (blIsAddColum)
                {
                    Table.Columns.Add(key.Trim());
                }
                //添加行
                if (ColumnCount == 0)
                {
                    Table.Rows.Add();
                }

                RowCount = Table.Rows.Count - 1;
                Table.Rows[RowCount][ColumnCount] = Value.Trim();
                ColumnCount++;
            }

            return Table.Rows.Count;
        }
    }
}
