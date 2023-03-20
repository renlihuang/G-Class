using CS.Base.AppSet;
using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.CollectSfc;
using MESwebservice.Mescall;
using MESwebservice.PoleArrival;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 焊前寻址
/// </summary>
namespace DCS.TASKITEM.DataGather
{
    internal class PreWeldAddrGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        static bool _outStaionResult = false;
        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public PreWeldAddrGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        RequestToHttpHelper _requestToHttpHelper;
        //采集任务
        static CollectTaskContext _collectTaskContext;
        //需要采集的参数，根据业务增加
        string plcDataStatus;
        string mesDataStatus;
        string virtualCode;
        string Apihost;
        //日志帮助类
        static ILogOperator _log;
        List<PreWeldAddrEntity> _preWeldAddrs = new List<PreWeldAddrEntity>();


        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            plcDataStatus = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            mesDataStatus = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            // virtualCode = _collectTaskContext.DataMap.GetDataByKey("VirtualCode");
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();

            string tagName = "\"TMCSeekSiteDB\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[tagName + $"\"ModuleCode1\""] = null;
            data[tagName + $"\"ModuleCode2\""] = null;
            for (int i = 0; i < 36; i++)
            {
                data[tagName + $"\"Static_1\"[{i}].xvalue"] = null;
                data[tagName + $"\"Static_1\"[{i}].yvalue"] = null;
                data[tagName + $"\"Static_1\"[{i}].zvalue"] = null;
                data[tagName + $"\"Static_1\"[{i}].LocationCode"] = null;
            }

            if (opc.ReadNodes(data))
            {
                foreach (var item in data)
                {
                    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                }
                int PlcFalg = Convert.ToInt16(data[plcDataStatus]);
                int MesFalg = Convert.ToInt16(data[mesDataStatus]);
                // string VirtualCode = data[virtualCode].ToString();



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
                            //

                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //保存出站结果
                        _outStaionResult = true;
                        //初始化实体类
                        _preWeldAddrs.Clear();
                        for (int i = 0; i < 36; i++)
                        {
                            PreWeldAddrEntity preWeldAddr = new PreWeldAddrEntity();
                            preWeldAddr.ModuleCode1 = data[tagName + $"\"ModuleCode1\""].ToString();
                            preWeldAddr.ModuleCode2 = data[tagName + $"\"ModuleCode2\""].ToString();
                            preWeldAddr.SateData = data[tagName + $"\"Static_1\"[{i}].xvalue"].ToString();
                            preWeldAddr.RangeValue = data[tagName + $"\"Static_1\"[{i}].yvalue"].ToString();
                            preWeldAddr.RangeDiff = data[tagName + $"\"Static_1\"[{i}].zvalue"].ToString();
                            preWeldAddr.PoorValue = data[tagName + $"\"Static_1\"[{i}].LocationCode"].ToString();
                            _preWeldAddrs.Add(preWeldAddr);
                        }
                        //上传追溯数据
                        #region 批量插入数据
                        for (int i = 0; i < _preWeldAddrs.Count(); i++)
                        {
                            AddEntityAync(_preWeldAddrs[i]);
                        }
                        #endregion

                    }
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
                        _log.AddUserLog("焊前寻址", "焊前寻址", string.Format("焊前寻址resul写入失败"));
                    }
                    else
                    {

                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("焊前寻址", "焊前寻址", string.Format("焊前寻址resul写入2失败"));
                        }
                        else
                        {
                            //标记结束出站
                            _isOutStation = false;
                        }
                    }

                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("焊前寻址", "焊前寻址", string.Format("焊前寻址标志位写0失败"));
                    }
                    _isOutStation = false;
                }
                if (PlcFalg == 3)
                {

                    _isOutStation = false;

                    writeTags[mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("焊前寻址", "焊前寻址", string.Format("焊前寻址标志位写4失败"));
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
        HttpResponseResultModel<string> AddEntityAync(PreWeldAddrEntity preWeldAddrEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/PreWeldAddr/insert",
                Data = preWeldAddrEntity
            }).Result;
        }


        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}
