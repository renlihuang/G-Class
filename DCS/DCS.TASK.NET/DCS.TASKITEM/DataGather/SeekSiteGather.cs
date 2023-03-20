using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.Mescall;
using MESwebservice.ModuleEntry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DCS.TASKITEM.DataGather
{
    internal class SeekSiteGather : IPeriodicTask
    {   /// <summary>
        /// 是否已经调用过出站
        /// </summary>
        static bool _isOutStation = false;

        /// <summary>
        /// 出站结果
        /// </summary>
        static bool _outStaionResult = false;
        /// <summary>
        /// csvlist
        /// </summary>
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();
        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public SeekSiteGather(RequestToHttpHelper requestToHttpHelper)
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
        //心跳变量
        int heartstat;
        string Module_As= String.Empty;
        string Module_Bs = String.Empty;
        int code;
        string msg;

        //日志帮助类
        static ILogOperator _log;


        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            _plcDataStatus = "\"TMCSeekSiteDB\".dataStatus_PLC";
            _mesDataStatus = "\"TMCSeekSiteDB\".dataStatus_MES";
            _Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
           // MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse= BlockCall.SeekSiteDatacoll(@AppConfig.WebserviceiniPath, "焊前寻址收数", "001MEAVN000002C7G0500020");
           //MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponse = BlockCall.SeekAddrComplete(@AppConfig.WebserviceiniPath, "焊前寻址出站", "001MEAVN000002C7G0500020");
            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Clear();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();

            string tagName = "\"TMCSeekSiteDB\".";
            data[_plcDataStatus] = null;
            data[_mesDataStatus] = null;
            data[tagName + $"\"dataStatus_Heart\""] = null;
            //data[tagName + "dataStatus_PLC"] = null;
            //data[tagName + "dataStatus_MES"] = null;
            data[tagName + "Module_acode"] = null;
            data[tagName + "Module_bcode"] = null;
            //data[tagName + $"Module_A"] = null;
            //data[tagName + $"Module_B"] = null;
            data[tagName + $"Photo_1"] = null;
            data[tagName + $"Photo_2"] = null;
            data[tagName + $"Photo_3"] = null;
            data[tagName + $"Photo_4"] = null;
            data[tagName + $"Mark"] = null;

            _collectTaskContext.TaskMsgOperator.SetMesText("1","1","1");

            //for (int i = 1; i < 121; i++)
            //{
            //    data[tagName + $"\"Moudle_A\"[{i}]"] = null;
            //    data[tagName + $"\"Moudle_B\"[{i}]"] = null;
            //}
            Thread.Sleep(1200);
            if (opc.ReadNodes(data))
            {

                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}\

                heartstat = Convert.ToInt16(data[tagName + $"\"dataStatus_Heart\""].ToString());
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
                    _log.AddUserLog("焊前寻址", "焊前寻址心跳", string.Format("焊前寻址心跳写入失败"));
                }
               
                int PlcFalg = Convert.ToInt16(data[tagName + "dataStatus_PLC"]);
                int MesFalg = Convert.ToInt16(data[tagName + "dataStatus_MES"]);
                _collectTaskContext.TaskMsgOperator.SetPairText("心跳值", heartstat.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_PLC\"".ToString(), PlcFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_MES\"".ToString(), MesFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("当前进度", MesFalg.ToString() == "1" ? "mes处理成功" : "等待plc值");
                string moudlecoea = data[tagName + "Module_acode"] == null ? "" : data[tagName + "Module_acode"].ToString();
                string moudlecoeb = data[tagName + "Module_bcode"] == null ? "" : data[tagName + "Module_bcode"].ToString();
                //float[] Module_A = data[tagName + "Module_A"] == null ? null : (float[])data[tagName + "Module_A"];
                //float[] Module_B = data[tagName + "Module_B"] == null ? null : (float[])data[tagName + "Module_B"];
                byte[] bPhoto_1= data[tagName + "Photo_1"] == null ? null : (byte[])data[tagName + "Photo_1"];
                char[] cPhoto_1 = Encoding.ASCII.GetChars(bPhoto_1);
                string Photo_1 = new string(cPhoto_1);

                byte[] bPhoto_2 = data[tagName + "Photo_2"] == null ? null : (byte[])data[tagName + "Photo_2"];
                char[] cPhoto_2 = Encoding.ASCII.GetChars(bPhoto_2);
                string Photo_2 = new string(cPhoto_2);

                byte[] bPhoto_3 = data[tagName + "Photo_3"] == null ? null : (byte[])data[tagName + "Photo_3"];
                char[] cPhoto_3 = Encoding.ASCII.GetChars(bPhoto_3);
                string Photo_3 = new string(cPhoto_3);

                byte[] bPhoto_4 = data[tagName + "Photo_4"] == null ? null : (byte[])data[tagName + "Photo_4"];
                char[] cPhoto_4 = Encoding.ASCII.GetChars(bPhoto_4);
                string Photo_4 = new string(cPhoto_4);

                byte[] bMark = data[tagName + "Mark"] == null ? null : (byte[])data[tagName + "Mark"];
                char[] cMark = Encoding.ASCII.GetChars(bMark);
                string Mark = new string(cMark);

              
                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    var mesStatus = string.Empty;
                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        do
                        {
                            #region ocv mes
                            // 判断mes状态是否启用
                             mesStatus = IniFileAPI.INIGetStringValue(@AppConfig.WebserviceiniPath, "MES状态", "MesStatus", "");
                            if (mesStatus == "ON") // ON 开
                            {
                                //moudlecoea = "001MEAVN000002C7X0500002";
                                //moudlecoeb = "001MEAVN000002C7X0500001";
                                if (string.IsNullOrEmpty(moudlecoea)|| string.IsNullOrEmpty(moudlecoeb))
                                {
                                    _collectTaskContext.TaskMsgOperator.SetPairText("提示信息", "模组条码为空，请检查数据");
                                    _outStaionResult = false;
                                    _isOutStation = true;
                                    break;
                                }
                                try
                                {
                                    PasteBoxCall pasteBoxCall = new PasteBoxCall(_collectTaskContext);
                                    //进站a
                                    miFindCustomAndSfcDataResponse miFindCustomAndSfca = PasteBoxCall.ShimEntry(@AppConfig.WebserviceiniPath, "焊前寻址进站", moudlecoea, "1");
                                    if (!checkCode("焊前寻址", "焊前寻址进站接口A", code, msg, moudlecoea))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    else
                                    {
                                        _outStaionResult = true;
                                    }
                                    //进站b
                                    miFindCustomAndSfcDataResponse miFindCustomAndSfcb = PasteBoxCall.ShimEntry(@AppConfig.WebserviceiniPath, "焊前寻址进站", moudlecoeb, "1");
                                    if (!checkCode("焊前寻址", "焊前寻址进站接口B", code, msg, moudlecoeb))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    else
                                    {
                                        _outStaionResult = true;
                                    }
                                    //  收数调用两次 
                                    BlockCall blockCall = new BlockCall(_collectTaskContext);
                                    MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse1 = BlockCall.SeekSiteDatacoll(@AppConfig.WebserviceiniPath, "焊前寻址收数", moudlecoea);
                                    code = sfcResponse1.@return.code;
                                    msg = sfcResponse1.@return.message;
                                    if (!checkCode("焊前寻址", "焊前寻址收数接口A", code, msg, moudlecoea))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    else
                                    {
                                        _outStaionResult = true;
                                    }

                                    MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse2 = BlockCall.SeekSiteDatacoll(@AppConfig.WebserviceiniPath, "焊前寻址收数", moudlecoeb);
                                    code = sfcResponse2.@return.code;
                                    msg = sfcResponse2.@return.message;
                                    if (!checkCode("焊前寻址", "焊前寻址收数接口B", code, msg, moudlecoeb))
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

                                // 出站
                                try
                                {
                                    BlockCall blockCall = new BlockCall(_collectTaskContext);
                                    MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponseA = BlockCall.SeekAddrComplete(@AppConfig.WebserviceiniPath, "焊前寻址出站", moudlecoea);
                                    code = sfcCompleteResponseA.@return.code;
                                    msg = sfcCompleteResponseA.@return.message;
                                    if (!checkCode("焊前寻址", "焊前寻址出站接口A", code, msg, moudlecoea))
                                    {
                                        _outStaionResult = false;
                                        _isOutStation = true;
                                        break;
                                    }
                                    else
                                    {
                                        _outStaionResult = true;
                                    }

                                    MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponseB = BlockCall.SeekAddrComplete(@AppConfig.WebserviceiniPath, "焊前寻址出站", moudlecoeb);
                                    code = sfcCompleteResponseB.@return.code;
                                    msg = sfcCompleteResponseB.@return.message;
                                    if (!checkCode("焊前寻址", "焊前寻址出站接口B", code, msg, moudlecoeb))
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
                            #endregion
                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //保存出站结果
                        //初始化实体类

                      
                        dicLst.Clear();
                       
                        dicLst.Add(data);
                        _log.ToCSVData(dicLst,"", "焊前寻址");
                        SeekSiteEntity seekSiteEntity = new SeekSiteEntity();
                        seekSiteEntity.MoudlecodeA = moudlecoea;
                        seekSiteEntity.MoudlecodeB = moudlecoeb;
                        seekSiteEntity.Photo_1 = Photo_1;
                        seekSiteEntity.Photo_2= Photo_2;
                        seekSiteEntity.Photo_3= Photo_3;
                        seekSiteEntity.Photo_4= Photo_4;
                        seekSiteEntity.Mark = Mark;
                        //上传追溯数据
                        AddEntityAync(seekSiteEntity);
                    }
                    //根据是否出站成功来回写标准位\                
                    if (mesStatus=="OFF")
                    {
                        _outStaionResult = true; // 关闭接口后 _outStaionResult 赋值 MES状态赋值ON后注销
                    }
                    
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
                        _log.AddUserLog("焊前寻址", "焊前寻址", string.Format("焊前寻址resul写入失败"));
                    }

                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[_mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("焊前寻址", "焊前寻址", string.Format("焊前寻址标志位写0失败"));
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
                            _log.AddUserLog("焊前寻址", "焊前寻址", string.Format("焊前寻址标志位写4失败"));
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
        HttpResponseResultModel<SeekSiteEntity> AddEntityAync(SeekSiteEntity seekSiteEntity)
        {
            return _requestToHttpHelper.PostAsync<SeekSiteEntity>(new HttpRequestModel
            {
                Host = _Apihost,
                Path = "/SeekSite/insert",
                Data = seekSiteEntity
            }).Result;
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
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "调用成功,模组号:" + sfc));
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
        public void DoUnInit()
        {
            //throw new NotImplementedException();
        }
    }
}
