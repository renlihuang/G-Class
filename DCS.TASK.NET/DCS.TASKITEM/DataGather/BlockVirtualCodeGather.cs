using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.BlockCheckinvent;
//using MESwebservice.BlockdataCollect;
using MESwebservice.BlockMiAssembleAndCollectData;
using MESwebservice.BlockReleaseSfc;
using MESwebservice.BlocksfcComplete;
using MESwebservice.GetBarCodeInfo;
using MESwebservice.Mescall;
using MESwebservice.ShimEntry;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// block虚拟条码
/// </summary>
namespace DCS.TASKITEM.DataGather
{
    public class BlockVirtualCodeGather : IPeriodicTask
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
        public BlockVirtualCodeGather(RequestToHttpHelper requestToHttpHelper)
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
        int code;
        string msg;
        string messfc;
        string mesinvn;
        string modletype;

        string pnType; // 用于在进站收数接口判断资源号

        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;
            //plcDataStatus = _collectTaskContext.DataMap.GetDataByKey("PlcDataStatus");
            //mesDataStatus = _collectTaskContext.DataMap.GetDataByKey("MesDataStatus");
            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            // 获取Ocv电压数据

            // 30
            // string aaa1 = "[{'batteryCoreCode':'001CE4C0000002C5M0100853;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100829;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101495;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101450;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100208;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101964;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101590;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101409;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102097;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102254;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101558;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101365;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101422;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101356;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101487;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100293;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101475;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100601;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101596;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101364;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100647;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102148;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100029;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101615;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100046;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100828;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100792;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102045;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101490;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101299;','virtualCode':null}]";

            //  // 21
            //  string aaa2 = "[{'batteryCoreCode':'001CE4C0000002C5M0100061;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101404;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100766;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101822;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102016;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101825;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100304;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101924;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101598;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101979;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101552;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101425;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101384;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101651;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101279;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102036;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101139;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101529;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0102940;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0102947;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101333;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101410;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101471;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101629;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0101850;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100847;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102021;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0101523;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5M0100778;','virtualCode':null},{'batteryCoreCode':'001CE4C0000002C5L0102034;','virtualCode':null}]";

            ////  HttpResponseResultModel<BlockEntity> httpResponseResultModel = GetBlockData(4068778807736274944);
            // List<BlockEntity> ids2 = JsonHelper.DeserializeObject<List<BlockEntity>>(aaa1);
            //List<string> ids3 = new List<string>();
            //for (int i = 0; i < 30; i++)
            //{
            //    ids3.Add(ids2[i].BatteryCoreCode);
            //}

            //  miAssmebleAndCollectDataForSfcResponse collectResponse1 = BlockCall.BlockMiAssemble(@AppConfig.WebserviceiniPath, "Block模组装配电芯", ids2, "001MEAVN000002C7G0500021");
            //HttpResponseResultModel<List<BatteryCoreOcvTestEntity>> batteryData1 = GetOcvdataAync(ids3);
            //BlockCall blockCall1 = new BlockCall(_collectTaskContext);
            //MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse2 = BlockCall.VirtualCodeDatacoll(@AppConfig.WebserviceiniPath, "释放虚拟模组号收数", messfc, batteryData1.BackResult);

            // 根据模组号获取模组号对应的模组号和pn号
            // HttpResponseResultModel<List<ModuleAndPnEntity>> pnResult1 = GetModuleAndPnAync("001MEAVN000002C7M0500005");

            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();


            string tagName = "\"TMCblockDB\".";
            plcDataStatus = tagName + "dataStatus_PLC";
            mesDataStatus = tagName + "dataStatus_MES";
            data[plcDataStatus] = null;
            data[mesDataStatus] = null;
            for (int i = 0; i < 30; i++)
            {
                data[tagName + $"\"BatteryCoreCode\"[{i}]"] = null;
            }
            //data[tagName + $"\"VirtualCode\""] = null;
            data[tagName + "VirtualCode"] = null;
            data[tagName + "ModuleType"] = null;


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

                List<BlockEntity> list = new List<BlockEntity>();
                List<BatteryCoreOcvTestEntity> batteryCoreOcvlist = new List<BatteryCoreOcvTestEntity>();
                List<string> ids = new List<string>();
                list.Clear();
                ids.Clear();
                
                for (int i = 0; i < 30; i++)
                {
                    BlockEntity entity = new BlockEntity();
                    entity.BatteryCoreCode = data[tagName + $"\"BatteryCoreCode\"[{i}]"].ToString() + ";";
                    list.Add(entity);
                    ids.Add(data[tagName + $"\"BatteryCoreCode\"[{i}]"].ToString());
                }

                var aa = JsonHelper.SerializeObject(list);

                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码","触发采集，plc上传数据为："+ ids.ToJson());
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
                                batteryCoreOcvlist.Clear();
                                //获取Ocv电压数据
                                for (int i = 0; i < list.Count; i++)
                                {
                                    if (list[i].BatteryCoreCode != ";")
                                    {
                                        HttpResponseResultModel<List<BatteryCoreOcvTestEntity>> batteryData = GetOcvdataBySingleAync(list[i].BatteryCoreCode.Substring(0, 24));
                                        if (batteryData.BackResult.Count > 0)
                                        {
                                            batteryCoreOcvlist.Add(batteryData.BackResult[0]);
                                        }
                                    }
                                }
                                if (batteryCoreOcvlist.Count != 30)
                                {
                                    _collectTaskContext.TaskMsgOperator.SetPairText("PLC上传电芯条码检查失败，数据不足30条", messfc);
                                    _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("PLC上传电芯条码检查失败，电芯数量不满30，sfc：" + messfc));
                                    _outStaionResult = false;
                                    break;
                                }
                                if (list.GroupBy(x => x.BatteryCoreCode).Where(g => g.Count() > 1).Count() >= 1)
                                {
                                    _collectTaskContext.TaskMsgOperator.SetPairText("PLC上传电芯条码检查失败，电芯条码有重复", messfc);
                                    _log.AddUserLog("BLOCK申请虚拟条码", "BLOCK申请虚拟条码", string.Format("PLC上传电芯条码检查失败，数据有重复，sfc：" + messfc));
                                    _outStaionResult = false;
                                    break;
                                }
                                // 释放模组号
                                //messfc = "001MEAVN000002C770300016";
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

                                    //PasteBoxCall pasteBox12 = new PasteBoxCall(_collectTaskContext);
                                    //BlockCall blockCall1 = new BlockCall(_collectTaskContext);
                                    //MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "Block入packing进站", "001MEAVN000002C7L0500001");
                                    //miGetSlotDataResponse slotDataResponseA12 = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌", "001MEAVN000002C7L0500001");
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

                                // 根据模组号获取模组号对应的模组号和pn号
                                HttpResponseResultModel<List<ModuleAndPnEntity>> pnResult = GetModuleAndPnAync(messfc);
                                if (pnResult.BackResult.Count > 0)
                                {
                                    pnType = pnResult.BackResult[0].PnNo;
                                }

                                //进站
                                try
                                {
                                    BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                    MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "Block入packing进站", messfc, pnType);
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
                                    miAssmebleAndCollectDataForSfcResponse collectResponse = BlockCall.BlockMiAssemble(@AppConfig.WebserviceiniPath, "Block模组装配电芯", list, messfc, pnType);
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
                                    MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse1 = BlockCall.VirtualCodeDatacoll(@AppConfig.WebserviceiniPath, "释放虚拟模组号收数", messfc, batteryCoreOcvlist, modletype, pnType);
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
                                //判断ab模组
                                //PasteBoxCall pasteBox1 = new PasteBoxCall(_collectTaskContext);
                                //miGetSlotDataResponse slotDataResponseA1 = PasteBoxCall.GetBarCodeInfo(@AppConfig.WebserviceiniPath, "贴箱体标获取铭牌", messfc);
                                //code = slotDataResponseA1.@return.code;
                                //msg = slotDataResponseA1.@return.message;
                                //string pnab = "";
                                //if (slotDataResponseA1.@return.labelListArray != null)
                                //{
                                //    pnab = slotDataResponseA1.@return.labelListArray[0].value;
                                //}
                                if (pnType == "830100-02437")
                                {
                                    modletype = "B";
                                }
                                else
                                {
                                    modletype = "A";
                                }
                                _collectTaskContext.TaskMsgOperator.SetPairText("模组类型", modletype.ToString());
                            }
                            #endregion
                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //初始化实体类

                        BlockVirtualCodeEntity block = new BlockVirtualCodeEntity();
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
                                IniFileAPI.INIWriteValue(@AppConfig.WebserviceiniPath, "生产模式", "LastType", modletype);
                                
                            }
                            else
                            {
                                writeTags[tagName + "VirtualCode"] = "";
                                //模组码申请失败改变AB模组顺序，只有失败后才触发
                                string mestype = IniFileAPI.INIGetStringValue(@AppConfig.WebserviceiniPath, "生产模式", "Type", "");
                                if (mestype=="A")
                                {
                                    IniFileAPI.INIWriteValue(@AppConfig.WebserviceiniPath, "生产模式", "Type", "B");
                                }
                                else
                                {
                                    IniFileAPI.INIWriteValue(@AppConfig.WebserviceiniPath, "生产模式", "Type", "A");
                                }
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
        HttpResponseResultModel<BlockVirtualCodeEntity> AddEntityAync(BlockVirtualCodeEntity blockEntity)
        {
            return _requestToHttpHelper.PostAsync<BlockVirtualCodeEntity>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/Block/insert",
                Data = blockEntity
            }).Result;
        }
        /// <summary>
        /// 根据电芯单个获取电压数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<List<BatteryCoreOcvTestEntity>> GetOcvdataBySingleAync(string batterycode)
        {
            return _requestToHttpHelper.GetAsync<List<BatteryCoreOcvTestEntity>>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/BatteryCoreOcvTest/GetList?batteryCoreCode="+ batterycode + "&Result=ok",
                Data = batterycode
            }).Result;
        }

        /// <summary>
        /// 调用webapi获取电压数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<List<BatteryCoreOcvTestEntity>> GetOcvdataAync(List<string> ids)
        {
            return _requestToHttpHelper.PostAsync<List<BatteryCoreOcvTestEntity>>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/BatteryCoreOcvTest/GetListByIds",
                Data = ids
            }).Result;
        }

        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<BlockEntity> GetBlockData(long  Id)
        {
            return _requestToHttpHelper.GetAsync<BlockEntity>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/Block/Get",
                Data = Id
            }).Result;
        }

        /// <summary>
        /// 保存模组号 资源号数据
        /// </summary>
        /// <param name="batteryCoreOcvTestEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<string> AddModuleAndPnAync(ModuleAndPnEntity moduleAndPnEntity)
        {
            return _requestToHttpHelper.PostAsync<string>(new HttpRequestModel
            {
                Host = _collectTaskContext.DataMap.GetDataByKey("Apihost"),
                Path = "/ModuleAndPn/insert",
                Data = moduleAndPnEntity
            }).Result;
        }

        /// <summary>
        /// 调用api获取焊前寻址数据
        /// </summary>
        /// <param name="seekSiteEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<List<ModuleAndPnEntity>> GetModuleAndPnAync(string ModuleCode)
        {
            return _requestToHttpHelper.GetAsync<List<ModuleAndPnEntity>>(new HttpRequestModel
            {
                Host = Apihost,
                Path = $"/ModuleAndPn/GetList?ModuleCode=" + ModuleCode,
                Data = new ModuleAndPnEntity { ModuleCode = ModuleCode }
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
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "调用成功，sfc："+sfc));
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
