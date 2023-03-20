using CS.Base.AppSet;
using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.AutoWeight;
using MESwebservice.Mescall;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DCS.TASKITEM.DataGather
{
    /// <summary>
    /// 自动称重
    /// </summary>
    internal class AutoWeighGather : IPeriodicTask
    {

        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        static bool _outStaionResult = false;

        /// csvlist
        /// </summary>
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();
        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public AutoWeighGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        RequestToHttpHelper _requestToHttpHelper;
        //采集任务
        static CollectTaskContext _collectTaskContext;
        //需要采集的参数，根据业务增加
        string _plcDataStatus;
        string _mesDataStatus;
        string _moudlecode;
        string _weight;
        string _packCode;
        string _Apihost;
        int code;
        string msg;
        //心跳变量
        int heartstat;
        //日志帮助类
        static ILogOperator _log;


        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            //_plcDataStatus = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            //_mesDataStatus = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            //_moudlecode = _collectTaskContext.DataMap.GetDataByKey("ModuleCode");
            //_weight = _collectTaskContext.DataMap.GetDataByKey("Weight");
            //_packCode = _collectTaskContext.DataMap.GetDataByKey("PackCode");
            _Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            //dataCollectForModuleTestResponse response = WeightCall.CheckWeight(@AppConfig.WebserviceiniPath, "模组称重收数");
            //code = response.@return.code;
            //msg = response.@return.message;

            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();


            string tagName = "\"TMCModeleWeightDB\".";
            _mesDataStatus = tagName + "dataStatus_MES";
            data[tagName + "dataStatus_PLC"] = null;
            data[tagName + "dataStatus_MES"] = null;
            data[tagName + "dataStatus_Heart"] = null;
            data[tagName + "Weight"] = null;
            data[tagName + "PackCode"] = null;

            //_collectTaskContext.TaskMsgOperator.SetPairText("错误代码", msg);
            Thread.Sleep(1200);
            if (opc.ReadNodes(data))
            {
                heartstat = Convert.ToInt16(data[tagName + "dataStatus_Heart"].ToString());
                if (heartstat == 1)
                {
                    heartstat = 0;
                }
                else
                {
                    heartstat = 1;
                }
                writeTags[tagName + "dataStatus_Heart"] = Convert.ToInt16(heartstat);
                //写入PLC变量

                if (!opc.WriteNodes(writeTags))
                {
                    _log.AddUserLog("自动称重", "自动称重心跳", string.Format("自动称重心跳写入失败"));
                }
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}

                int PlcFalg = Convert.ToInt16(data[tagName + "dataStatus_PLC"]);
                int MesFalg = Convert.ToInt16(data[tagName + "dataStatus_MES"]);
                string weight = data[tagName + "Weight"].ToString();
                string packCode = data[tagName + "PackCode"].ToString();
                _collectTaskContext.TaskMsgOperator.SetPairText("心跳值", heartstat.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_PLC\"".ToString(), PlcFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_MES\"".ToString(), MesFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("重量值:", weight);
                _collectTaskContext.TaskMsgOperator.SetPairText("PACK条码:", packCode);
                _collectTaskContext.TaskMsgOperator.SetPairText("当前进度", MesFalg.ToString() == "1" ? "mes处理成功" : "等待plc值");
                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    #region ocv mes交互
                    // 收数
                    try
                    {
                        dataCollectForModuleTestResponse response = WeightCall.CheckWeight(@AppConfig.WebserviceiniPath, "模组称重收数");
                        code = response.@return.code;
                        msg = response.@return.message;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (code == 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("自动称重收数接口调用成功", "");
                    }
                    else if (code > 0)
                    {
                        _collectTaskContext.TaskMsgOperator.SetPairText("自动称重收数接口调用失败", "");
                        _collectTaskContext.TaskMsgOperator.SetText("CODE: " + code);
                        _collectTaskContext.TaskMsgOperator.SetPairText("自动称重收数返回代码：", code.ToString());
                        _collectTaskContext.TaskMsgOperator.SetPairText("自动称重收数返回信息", msg);
                        //_outStaionResult = false;
                    }
                    else if (code == -1)
                    {
                        _collectTaskContext.TaskMsgOperator.SetText("请检查网络");
                    }
                    #endregion 

                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        //验证数据和业务逻辑写在这个里面
                        do
                        {
                            //

                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //保存出站结果
                        _outStaionResult = true;
                        //初始化实体类
                        AutoWeightEntity autoWeight = new AutoWeightEntity();
                        autoWeight.Weight = weight;
                        autoWeight.PackCode = packCode;

                        //上传追溯数据
                        AddEntityAync(autoWeight);
                    }
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst, "","自动称重");
                    //根据是否出站成功来回写标准位
                    if (_outStaionResult)
                    {
                        writeResult = 1;
                    }
                    else
                    {
                        writeResult = 2;
                    }
                    writeTags[_mesDataStatus] = Convert.ToInt16(writeResult);
                    //写入PLC变量
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("自动称重", "自动称重", string.Format("自动称重resul写入失败"));
                    }
                    //else
                    //{

                    //    if (!opc.WriteNodes(writeTags))
                    //    {
                    //        _log.AddUserLog("自动称重", "自动称重", string.Format("自动称重resul写入2失败"));
                    //    }
                    //    else
                    //    {
                    //        //标记结束出站
                    //        _isOutStation = false;
                    //    }
                    //}


                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[_mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("自动称重", "自动称重", string.Format("自动称重标志位写0失败"));
                    }
                    _isOutStation = false;
                }
                if (PlcFalg == 3)
                {

                    _isOutStation = false;

                    writeTags[_mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("自动称重", "自动称重", string.Format("自动称重标志位写4失败"));
                        }
                    }

                }
            }

        }
        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="autoWeightEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<AutoWeightEntity> AddEntityAync(AutoWeightEntity autoWeightEntity)
        {
            return _requestToHttpHelper.PostAsync<AutoWeightEntity>(new HttpRequestModel
            {
                Host = _Apihost,
                Path = "/AutoWeight/insert",
                Data = autoWeightEntity
            }).Result;
        }


        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}
