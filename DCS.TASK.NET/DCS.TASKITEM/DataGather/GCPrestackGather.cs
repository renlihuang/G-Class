using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.BlockReleaseSfc;
using MESwebservice.Mescall;
using System;
using System.Collections.Generic;

namespace DCS.TASKITEM.DataGather
{
    /// <summary>
    /// 模组预堆叠
    /// </summary>
    internal class GCPrestackGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        private static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        private static bool _outStaionResult = false;

        //csvlist
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();

        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public GCPrestackGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        private RequestToHttpHelper _requestToHttpHelper;

        //采集任务
        private static CollectTaskContext _collectTaskContext;

        //需要采集的参数，根据业务增加
        private string plcDataStatus;

        private string mesDataStatus;
        private string Apihost;

        //日志帮助类
        private static ILogOperator _log;

        private int code;
        private string msg;
        private string messfc;
        private string mesinvn;
        private string modletype;

        private string pnType; // 用于在进站收数接口判断资源号

        public void DoInit(TimedTaskContext taskContext)
        {
            _collectTaskContext = taskContext as CollectTaskContext;
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            var opc = _collectTaskContext.OpcOperator;

            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();

            string tagName = "\"GCPrestackDB\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[tagName + "VirtualCode"] = null;
            data[tagName + "MoudleType"] = null;

            if (opc.ReadNodes(data))
            {
                _collectTaskContext.TaskMsgOperator.SetPairText("PLC标志位", data[tagName + "dataStatus_PLC"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("MES标志位", data[tagName + "dataStatus_MES"].ToString());

                int PlcFalg = Convert.ToInt16(data[plcDataStatus]);
                int MesFalg = Convert.ToInt16(data[mesDataStatus]);

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        //验证数据和业务逻辑写在这个里面
                        do
                        {
                            #region ocv mes交互

                            // 判断mes状态是否启用
                            var mesStatus = IniFileAPI.INIGetStringValue(@AppConfig.WebserviceiniPath, "MES状态", "MesStatus", "");
                            if (mesStatus == "ON") // ON 开
                            {
                                try
                                {
                                    BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                    miReleaseSfcWithActivityResponse releaseResponse = BlockCall.BlockReleaseSfc(@AppConfig.WebserviceiniPath, "Block释放模组号");
                                    code = releaseResponse.@return.code;
                                    msg = releaseResponse.@return.message;
                                    if (releaseResponse.@return.sfcArray != null)
                                    {
                                        messfc = releaseResponse.@return.sfcArray[0].sfc.ToString();
                                        _collectTaskContext.TaskMsgOperator.SetPairText("虚拟模组条码", messfc.ToString());
                                    }
                                    else
                                    {
                                        messfc = "-1";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (!checkCode("BLOCK申请虚拟条码", "BLOCK申请虚拟条码释放模组号接口", code, msg, messfc))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    throw ex;
                                }
                                if (!checkCode("BLOCK申请虚拟条码", "BLOCK申请虚拟条码释放模组号接口", code, msg, messfc))
                                {
                                    _outStaionResult = false;
                                    _isOutStation = true;
                                    break;
                                }

                                _collectTaskContext.TaskMsgOperator.SetPairText("模组类型", modletype.ToString());
                            }

                            #endregion ocv mes交互
                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //初始化实体类
                        GCPrestackEntity  gCPrestack =  new GCPrestackEntity();
                        gCPrestack.VirtualCode = messfc;
                        gCPrestack.WorkOrder = messfc;
                        gCPrestack.MoudleType = messfc;
                        gCPrestack.Resource = "";
                        //上传追溯数据
                        AddEntityAync(gCPrestack);
                        dicLst.Clear();
                        data.Add("Date", DateTime.Now.ToString());
                        dicLst.Add(data);
                        _log.ToCSVData(dicLst, messfc, "模组预堆叠");
                        //根据是否出站成功来回写标准位
                        if (_outStaionResult)
                        {
                            writeResult = 1;
                        }
                        else
                        {
                            writeResult = 2;
                        }
                        if (string.IsNullOrEmpty(messfc))
                        {
                            writeTags[tagName + "VirtualCode"] = "-1";
                        }
                        else
                        {
                            if (_outStaionResult)
                            {
                                writeTags[tagName + "VirtualCode"] = messfc;
                                writeTags[tagName + "ModuleType"] = modletype;
                            }
                            else
                            {
                                writeTags[tagName + "VirtualCode"] = "";
                            }
                        }
                        writeTags[mesDataStatus] = Convert.ToInt16(writeResult);
                        //写入PLC变量
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("BLOCK申请虚拟条码resul写入失败"));
                        }
                        //标记结束出站
                        _isOutStation = true;
                    }
                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("BLOCK申请虚拟条码标志位写0失败"));
                    }
                    _isOutStation = false;
                }
                if (PlcFalg == 3)
                {
                    messfc = "";

                    _isOutStation = false;

                    writeTags[mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("BLOCK申请虚拟条码标志位写4失败"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        private HttpResponseResultModel<GCPrestackEntity> AddEntityAync(GCPrestackEntity blockEntity)
        {
            return _requestToHttpHelper.PostAsync<GCPrestackEntity>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/GCPrestack/insert",
                Data = blockEntity
            }).Result;
        }

        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 接口返回信息方法
        /// </summary>
        /// <param name="siteName">站点名称</param>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="code">接口返回代码</param>
        /// <param name="msg">接口返回信息</param>
        public bool checkCode(string siteName, string interfaceName, int code, string msg, string sfc)
        {
            if (code == 0)
            {
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "调用成功", sfc);
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "调用成功，sfc：" + sfc));
            }
            else if (code > 0)
            {
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "调用失败", sfc);
                _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "返回代码：", code.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "返回信息", msg);
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "异常，异常代码为{0}，返回信息为{1}，错误模组号为{2}", code.ToString(), msg, sfc));
                return false;
            }
            else if (code == -1)
            {
                _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "异常，请检查网络"));
                return false;
            }
            return true;
        }
    }
}