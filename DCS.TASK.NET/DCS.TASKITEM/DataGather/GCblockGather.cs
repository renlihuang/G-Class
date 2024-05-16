using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.BlockMiAssembleAndCollectData;
using MESwebservice.Mescall;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCS.TASKITEM.DataGather
{
    /// <summary>
    /// 模组堆叠
    /// </summary>
    internal class GCblockGather : IPeriodicTask
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
        public GCblockGather(RequestToHttpHelper requestToHttpHelper)
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

            string tagName = "\"GCblockDB\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            for (int i = 0; i < 48; i++)
            {
                data[tagName + $"\"BatteryCoreCode\"[{i}]"] = null;
            }
            data[tagName + "VirtualCode"] = null;

            if (opc.ReadNodes(data))
            {
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}
                _collectTaskContext.TaskMsgOperator.SetPairText("PLC标志位", data[tagName + "dataStatus_PLC"].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("MES标志位", data[tagName + "dataStatus_MES"].ToString());

                int PlcFalg = Convert.ToInt16(data[plcDataStatus]);
                int MesFalg = Convert.ToInt16(data[mesDataStatus]);

                List<OCVcodeEntity> list = new List<OCVcodeEntity>();
                list.Clear();

                for (int i = 0; i < 30; i++)
                {
                    OCVcodeEntity entity = new OCVcodeEntity();
                    entity.BatteryCode = data[tagName + $"\"BatteryCoreCode\"[{i}]"].ToString() + ";";
                    list.Add(entity);
                }

                var aa = JsonHelper.SerializeObject(list);

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", "触发采集，plc上传数据为：" + aa.ToJson());
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
                                list.Clear();
                                if (list.Count != 30)
                                {
                                    _collectTaskContext.TaskMsgOperator.SetPairText("PLC上传电芯条码检查失败，数据不足30条", messfc);
                                    _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("PLC上传电芯条码检查失败，电芯数量不满30，sfc：" + messfc));
                                    _outStaionResult = false;
                                    break;
                                }
                                if (list.GroupBy(x => x.BatteryCode).Where(g => g.Count() > 1).Count() >= 1)
                                {
                                    _collectTaskContext.TaskMsgOperator.SetPairText("PLC上传电芯条码检查失败，电芯条码有重复", messfc);
                                    _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("PLC上传电芯条码检查失败，数据有重复，sfc：" + messfc));
                                    _outStaionResult = false;
                                    break;
                                }

                                //进站
                                try
                                {
                                    BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                    MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "Block入packing进站", messfc);
                                    code = miFindCustomAndSfcDataResponse.@return.code;
                                    msg = miFindCustomAndSfcDataResponse.@return.message;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                if (!checkCode("BLOCK申请虚拟条码", "BLOCK进站接口", code, msg, messfc))
                                {
                                    _outStaionResult = false;
                                    _isOutStation = true;
                                    break;
                                }
                                else
                                {
                                    _outStaionResult = true;
                                }

                                //模组装配
                                try
                                {
                                    //messfc = "001MEAVN000002C770300016";
                                    BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                    miAssmebleAndCollectDataForSfcResponse collectResponse = BlockCall.BlockMiAssemble(@AppConfig.WebserviceiniPath, "Block模组装配电芯", list, messfc);
                                    code = collectResponse.@return.code;
                                    msg = collectResponse.@return.message;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                if (!checkCode("BLOCK申请虚拟条码", "模组装配电芯接口", code, msg, messfc))
                                {
                                    _outStaionResult = false;
                                    _isOutStation = true;
                                    break;
                                }
                                else
                                {
                                    _outStaionResult = true;
                                }

                                // 收数
                                try
                                {
                                    BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                    MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse1 = BlockCall.VirtualCodeDatacoll(@AppConfig.WebserviceiniPath, "释放虚拟模组号收数", messfc, list);
                                    code = sfcResponse1.@return.code;
                                    msg = sfcResponse1.@return.message;
                                    if (!checkCode("BLOCK申请虚拟条码", "释放虚拟模组号收数接口", code, msg, messfc))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    else
                                    {
                                        _outStaionResult = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }

                            #endregion ocv mes交互
                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //初始化实体类

                        GCblockEntity block = new GCblockEntity();
                        block.BatteryCoreCode = aa;
                        block.VirtualCode = messfc;

                        //上传追溯数据
                        AddEntityAync(block);
                        dicLst.Clear();
                        data.Add("Date", DateTime.Now.ToString());
                        dicLst.Add(data);
                        _log.ToCSVData(dicLst, messfc, "block虚拟条码");
                        //根据是否出站成功来回写标准位
                        if (_outStaionResult)
                        {
                            writeResult = 1;
                        }
                        else
                        {
                            writeResult = 2;
                        }
                        writeTags[mesDataStatus] = Convert.ToInt16(writeResult);
                        //写入PLC变量
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("BLOCK申请虚拟条码resul写入失败"));
                        }
                        _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", "MES触发写入，mes结果：" + writeResult);
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
                    _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", "PLC触发3");
                    messfc = "";

                    _isOutStation = false;

                    writeTags[mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("BLOCK申请虚拟条码标志位写4失败"));
                        }
                        _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", "MES触发写入4");
                    }
                }
            }
        }

        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        private HttpResponseResultModel<GCblockEntity> AddEntityAync(GCblockEntity blockEntity)
        {
            return _requestToHttpHelper.PostAsync<GCblockEntity>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/GCblock/insert",
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