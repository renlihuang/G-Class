using DCS.BASE;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using System;
using System.Collections.Generic;
/// <summary>
/// 模组堆叠
/// </summary>
namespace DCS.TASKITEM.DataGather
{
    internal class PoleTestGather : IPeriodicTask
    {
        /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        static bool _outStaionResult = false;
        //csvlist
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();
        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public PoleTestGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        RequestToHttpHelper _requestToHttpHelper;
        //采集任务
        static CollectTaskContext _collectTaskContext;
        //需要采集的参数，根据业务增加
        string plcDataStatus;
        string mesDataStatus;
        string Apihost;
        //日志帮助类
        static ILogOperator _log;
        List<CcsHandInstallEntity> _cssHandInstall = new List<CcsHandInstallEntity>();


        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            plcDataStatus = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            mesDataStatus = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();

            string tagName = "\"TMCModuleStackingDB\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            data[tagName + $"\"VirtualCode\""] = null;
            for (int i = 0; i < 36; i++)
            {
                data[tagName + $"\"BatteryCoreCode\"[{i}]"] = null;
            }

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

                string VirtualCode = data[tagName + $"\"VirtualCode\""].ToString();

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    _cssHandInstall.Clear();
                    for (int i = 0; i < 36; i++)
                    {
                        CcsHandInstallEntity ccsHandInstall = new CcsHandInstallEntity();
                        ccsHandInstall.ModuleCode = data[tagName + $"\"BatteryCoreCode\"[{i}]"].ToString();
                        ccsHandInstall.VirtualCode = VirtualCode;
                        _cssHandInstall.Add(ccsHandInstall);
                    }

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

                        //上传追溯数据
                        //for (int i = 0; i < _cssHandInstall.Count; i++)
                        //{
                        //    AddEntityAync(_cssHandInstall[i]);
                        //}
                        AddEntityAync(_cssHandInstall);
                    }
                    dicLst.Clear();
                    dicLst.Add(data);
                    _log.ToCSVData(dicLst, "","模组堆叠");
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
                        _log.AddUserLog("模组堆叠", "模组堆叠", string.Format("模组堆叠resul写入失败"));
                    }
                    else
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("模组堆叠", "模组堆叠", string.Format("模组堆叠resul写入2失败"));
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
                        _log.AddUserLog("模组堆叠", "模组堆叠", string.Format("模组堆叠标志位写0失败"));
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
                            _log.AddUserLog("模组堆叠", "模组堆叠", string.Format("模组堆叠标志位写4失败"));
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
        HttpResponseResultModel<string> AddEntityAync(List<CcsHandInstallEntity> ccsHandInstallEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/CcsHandInstall/InsertBatch",
                Data = ccsHandInstallEntity
            }).Result;
        }


        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}
