using CS.Base.AppSet;
using DCS.BASE;
using DCS.BASE.IniFile;
using DCS.CORE;
using DCS.CORE.Interface;
using DCS.MODEL.Entiry;
using MESwebservice.BlockMiAssembleAndCollectData;
using MESwebservice.BlockReleaseSfc;
using MESwebservice.Mescall;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DCS.TASKITEM.DataGather
{
    internal class BusbarWeldGather : IPeriodicTask
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
        /// csvlist
        /// </summary>
        private static List<Dictionary<string, object>> dicLst = new List<Dictionary<string, object>>();
        /// <summary>
        /// 构造函数注册请求Http帮助类
        /// </summary>
        /// <param name="requestToHttpHelper"></param>
        public BusbarWeldGather(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }
        RequestToHttpHelper _requestToHttpHelper;
        //采集任务
        static CollectTaskContext _collectTaskContext;
        //需要采集的参数，根据业务增加
        string _plcDataStatus;
        string _mesDataStatus;
        string Apihost;
        //心跳变量
        int heartstat;
        //日志帮助类
        static ILogOperator _log;
        int code;
        string msg;
        string messfc;
        string mesinvn;
        string modletype;
        string OffsetA;
        string OffsetB;

        public void DoInit(TimedTaskContext taskContext)
        {

            _collectTaskContext = taskContext as CollectTaskContext;

            Apihost = _collectTaskContext.DataMap.GetDataByKey("Apihost");
            _log = _collectTaskContext.LogOperator;
        }

        public void DoTask()
        {
            // miAssmebleAndCollectDataForSfcResponse collectResponse1= BlockCall.BusbarMiAssemble(@AppConfig.WebserviceiniPath, "Busbar装配模组号", "001MEAVN000002C7B0500004", "001MEAVN000002C7B0500005", "001PE0K4000002C7B0500009");
            var opc = _collectTaskContext.OpcOperator;


            Dictionary<string, object> data = new Dictionary<string, object>();
            //写入字典
            Dictionary<string, object> writeTags = new Dictionary<string, object>();

            string tagName = "\"TMCBusbarDB\".";

            data[tagName + $"\"dataStatus_PLC\""] = null;
            data[tagName + $"\"dataStatus_MES\""] = null;
            data[tagName + $"\"dataStatus_Heart\""] = null;
            _mesDataStatus = tagName + $"\"dataStatus_MES\"";
            data[tagName + $"\"BusbarSpeed\""] = null;

            //for (int i = 0; i < 6; i++)
            //{
            //    data[tagName + $"\"setvalue_Group1\".pos{i + 1}"] = null;
            //}
            //for (int i = 0; i < 6; i++)
            //{
            //    data[tagName + $"\"setvalue_Group2\".pos{i + 1}"] = null;
            //}
            //for (int i = 0; i < 6; i++)
            //{
            //    data[tagName + $"\"setvalue_Group3\".pos{i + 1}"] = null;
            //}
            //for (int i = 0; i < 6; i++)
            //{
            //    data[tagName + $"\"setvalue_Group4\".pos{i + 1}"] = null;
            //}
            //for (int i = 0; i < 6; i++)
            //{
            //    data[tagName + $"\"setvalue_Group5\".pos{i + 1}"] = null;
            //}

            #region 功率相关
            // CenterPower_group
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"CenterPower_group{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"CenterPower_group{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"CenterPower_group{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"CenterPower_group{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"CenterPower_group{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"CenterPower_group{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }
            // CenterPower_MAX
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"CenterPower_MAX{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"CenterPower_MAX{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"CenterPower_MAX{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"CenterPower_MAX{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"CenterPower_MAX{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"CenterPower_MAX{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }
            // CenterPower_MIN
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"CenterPower_MIN{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"CenterPower_MIN{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"CenterPower_MIN{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"CenterPower_MIN{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"CenterPower_MIN{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"CenterPower_MIN{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }
            // RingPower_group
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"RingPower_group{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"RingPower_group{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"RingPower_group{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"RingPower_group{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"RingPower_group{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"RingPower_group{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }
            // RingPower_MAX
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"RingPower_MAX{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"RingPower_MAX{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"RingPower_MAX{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"RingPower_MAX{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"RingPower_MAX{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"RingPower_MAX{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }
            // RingPower_MIN
            for (int a = 0; a < 5; a++)
            {
                for (int b = 0; b < 4; b++)
                {
                    data[tagName + $"\"RingPower_MIN{a + 1}\".pos1.hole{b + 1}"] = null;
                }
                for (int c = 0; c < 4; c++)
                {
                    data[tagName + $"\"RingPower_MIN{a + 1}\".pos2.hole{c + 1}"] = null;
                }
                for (int d = 0; d < 4; d++)
                {
                    data[tagName + $"\"RingPower_MIN{a + 1}\".pos3.hole{d + 1}"] = null;
                }
                for (int e = 0; e < 4; e++)
                {
                    data[tagName + $"\"RingPower_MIN{a + 1}\".pos4.hole{e + 1}"] = null;
                }
                for (int f = 0; f < 4; f++)
                {
                    data[tagName + $"\"RingPower_MIN{a + 1}\".pos5.hole{f + 1}"] = null;
                }
                for (int g = 0; g < 4; g++)
                {
                    data[tagName + $"\"RingPower_MIN{a + 1}\".pos6.hole{g + 1}"] = null;
                }
            }
            #endregion
            data[tagName + $"\"Module-acode\""] = null;
            data[tagName + $"\"Module-bcode\""] = null;
            data[tagName + $"\"CurrentUserName\""] = null;
            data[tagName + $"\"RFID\""] = null;



            data[tagName + $"\"Phto_A_offset\""] = null;
            data[tagName + $"\"Phto_B_offset\""] = null;
            for (int i = 0; i < 121; i++)
            {
                data[tagName + $"\"Module_A\"[{i}]"] = null;
            }
            for (int i = 0; i < 121; i++)
            {
                data[tagName + $"\"Module_B\"[{i}]"] = null;
            }
            //data[tagName + $"\"Module_A\""] = null;
            //data[tagName + "Module_B"] = null;
            data[tagName + $"Mark"] = null;
            data[tagName + $"photo_1"] = null;
            data[tagName + $"photo_2"] = null;
            data[tagName + $"photo_3"] = null;
            data[tagName + $"photo_4"] = null;


            data[tagName + $"\"Pack-code\""] = null;

            if (opc.ReadNodes(data))
            {
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
                    _log.AddUserLog("Busbar焊接", "Busbar焊接心跳", string.Format("Busbar焊接心跳写入失败"));
                }
                _collectTaskContext.TaskMsgOperator.SetPairText("心跳值", heartstat.ToString());
                //foreach (var item in data)
                //{
                //    _collectTaskContext.TaskMsgOperator.SetPairText(item.Key, item.Value.ToString());
                //}
                int PlcFalg = Convert.ToInt16(data[tagName + $"\"dataStatus_PLC\""].ToString());
                int MesFalg = Convert.ToInt16(data[tagName + $"\"dataStatus_MES\""].ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_PLC\"".ToString(), PlcFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText(tagName + $"\"dataStatus_MES\"".ToString(), MesFalg.ToString());
                _collectTaskContext.TaskMsgOperator.SetPairText("当前进度", MesFalg.ToString() == "1" ? "mes处理成功" : "等待plc值");

                string BusbarSpeed = data[tagName + $"\"BusbarSpeed\""].ToString();
                string Phto_A_offset = data[tagName + $"\"Phto_A_offset\""].ToString();
                string Phto_B_offset = data[tagName + $"\"Phto_B_offset\""].ToString();
                string photo1 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_1"])));
                string photo2 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_2"])));
                string photo3 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_3"])));
                string photo4 = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "photo_4"])));
                string Mark = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "Mark"])));
                string ModuleACode = data[tagName + $"\"Module-acode\""].ToString();
                string ModuleBCode = data[tagName + $"\"Module-bcode\""].ToString();
                string PackCode = data[tagName + $"\"Pack-code\""].ToString();
                _collectTaskContext.TaskMsgOperator.SetPairText("pack条码", PackCode);

                for (int i = 0; i < 120; i++)
                {
                    OffsetA += data[tagName + $"\"Module_A\"[{i}]"].ToString() + ";"; // 测距值
                    OffsetB += data[tagName + $"\"Module_B\"[{i}]"].ToString() + ";"; // 补偿后测距值
                }


                // string OffsetBCode = new string(Encoding.ASCII.GetChars((byte[])(data[tagName + "Module_B"]))); // 补偿后测距值

                //string setvalue_Group1 = "";
                //for (int i = 0; i < 6; i++)
                //{
                //    setvalue_Group1 += data[tagName + $"\"setvalue_Group1\".pos{i + 1}"].ToString() + ";";
                //}
                //string setvalue_Group2 = "";
                //for (int i = 0; i < 6; i++)
                //{
                //    setvalue_Group2 += data[tagName + $"\"setvalue_Group2\".pos{i + 1}"].ToString() + ";";
                //}
                //string setvalue_Group3 = "";
                //for (int i = 0; i < 6; i++)
                //{
                //    setvalue_Group3 += data[tagName + $"\"setvalue_Group3\".pos{i + 1}"].ToString() + ";";
                //}
                //string setvalue_Group4 = "";
                //for (int i = 0; i < 6; i++)
                //{
                //    setvalue_Group4 += data[tagName + $"\"setvalue_Group4\".pos{i + 1}"].ToString() + ";";
                //}
                //string setvalue_Group5 = "";
                //for (int i = 0; i < 6; i++)
                //{
                //    setvalue_Group5 += data[tagName + $"\"setvalue_Group5\".pos{i + 1}"].ToString() + ";";
                //}

                #region 功率相关
                #region CenterPower_group
                string CenterPower_group1 = "";
                string CenterPower_group1_pos1 = "";
                string CenterPower_group1_pos2 = "";
                string CenterPower_group1_pos3 = "";
                string CenterPower_group1_pos4 = "";
                string CenterPower_group1_pos5 = "";
                string CenterPower_group1_pos6 = "";
                string CenterPower_group2 = "";
                string CenterPower_group2_pos1 = "";
                string CenterPower_group2_pos2 = "";
                string CenterPower_group2_pos3 = "";
                string CenterPower_group2_pos4 = "";
                string CenterPower_group2_pos5 = "";
                string CenterPower_group2_pos6 = "";
                string CenterPower_group3 = "";
                string CenterPower_group3_pos1 = "";
                string CenterPower_group3_pos2 = "";
                string CenterPower_group3_pos3 = "";
                string CenterPower_group3_pos4 = "";
                string CenterPower_group3_pos5 = "";
                string CenterPower_group3_pos6 = "";
                string CenterPower_group4 = "";
                string CenterPower_group4_pos1 = "";
                string CenterPower_group4_pos2 = "";
                string CenterPower_group4_pos3 = "";
                string CenterPower_group4_pos4 = "";
                string CenterPower_group4_pos5 = "";
                string CenterPower_group4_pos6 = "";
                string CenterPower_group5 = "";
                string CenterPower_group5_pos1 = "";
                string CenterPower_group5_pos2 = "";
                string CenterPower_group5_pos3 = "";
                string CenterPower_group5_pos4 = "";
                string CenterPower_group5_pos5 = "";
                string CenterPower_group5_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    CenterPower_group1_pos1 += data[tagName + $"\"CenterPower_group1\".pos1.hole{b + 1}"] + ";";
                    CenterPower_group1_pos2 += data[tagName + $"\"CenterPower_group1\".pos2.hole{b + 1}"] + ";";
                    CenterPower_group1_pos3 += data[tagName + $"\"CenterPower_group1\".pos3.hole{b + 1}"] + ";";
                    CenterPower_group1_pos4 += data[tagName + $"\"CenterPower_group1\".pos4.hole{b + 1}"] + ";";
                    CenterPower_group1_pos5 += data[tagName + $"\"CenterPower_group1\".pos5.hole{b + 1}"] + ";";
                    CenterPower_group1_pos6 += data[tagName + $"\"CenterPower_group1\".pos6.hole{b + 1}"] + ";";

                    CenterPower_group2_pos1 += data[tagName + $"\"CenterPower_group2\".pos1.hole{b + 1}"] + ";";
                    CenterPower_group2_pos2 += data[tagName + $"\"CenterPower_group2\".pos2.hole{b + 1}"] + ";";
                    CenterPower_group2_pos3 += data[tagName + $"\"CenterPower_group2\".pos3.hole{b + 1}"] + ";";
                    CenterPower_group2_pos4 += data[tagName + $"\"CenterPower_group2\".pos4.hole{b + 1}"] + ";";
                    CenterPower_group2_pos5 += data[tagName + $"\"CenterPower_group2\".pos5.hole{b + 1}"] + ";";
                    CenterPower_group2_pos6 += data[tagName + $"\"CenterPower_group2\".pos6.hole{b + 1}"] + ";";

                    CenterPower_group3_pos1 += data[tagName + $"\"CenterPower_group3\".pos1.hole{b + 1}"] + ";";
                    CenterPower_group3_pos2 += data[tagName + $"\"CenterPower_group3\".pos2.hole{b + 1}"] + ";";
                    CenterPower_group3_pos3 += data[tagName + $"\"CenterPower_group3\".pos3.hole{b + 1}"] + ";";
                    CenterPower_group3_pos4 += data[tagName + $"\"CenterPower_group3\".pos4.hole{b + 1}"] + ";";
                    CenterPower_group3_pos5 += data[tagName + $"\"CenterPower_group3\".pos5.hole{b + 1}"] + ";";
                    CenterPower_group3_pos6 += data[tagName + $"\"CenterPower_group3\".pos6.hole{b + 1}"] + ";";

                    CenterPower_group4_pos1 += data[tagName + $"\"CenterPower_group4\".pos1.hole{b + 1}"] + ";";
                    CenterPower_group4_pos2 += data[tagName + $"\"CenterPower_group4\".pos2.hole{b + 1}"] + ";";
                    CenterPower_group4_pos3 += data[tagName + $"\"CenterPower_group4\".pos3.hole{b + 1}"] + ";";
                    CenterPower_group4_pos4 += data[tagName + $"\"CenterPower_group4\".pos4.hole{b + 1}"] + ";";
                    CenterPower_group4_pos5 += data[tagName + $"\"CenterPower_group4\".pos5.hole{b + 1}"] + ";";
                    CenterPower_group4_pos6 += data[tagName + $"\"CenterPower_group4\".pos6.hole{b + 1}"] + ";";

                    CenterPower_group5_pos1 += data[tagName + $"\"CenterPower_group5\".pos1.hole{b + 1}"] + ";";
                    CenterPower_group5_pos2 += data[tagName + $"\"CenterPower_group5\".pos2.hole{b + 1}"] + ";";
                    CenterPower_group5_pos3 += data[tagName + $"\"CenterPower_group5\".pos3.hole{b + 1}"] + ";";
                    CenterPower_group5_pos4 += data[tagName + $"\"CenterPower_group5\".pos4.hole{b + 1}"] + ";";
                    CenterPower_group5_pos5 += data[tagName + $"\"CenterPower_group5\".pos5.hole{b + 1}"] + ";";
                    CenterPower_group5_pos6 += data[tagName + $"\"CenterPower_group5\".pos6.hole{b + 1}"] + ";";

                }
                CenterPower_group1 = CenterPower_group1_pos1 + CenterPower_group1_pos2
                    + CenterPower_group1_pos3 + CenterPower_group1_pos4 + CenterPower_group1_pos5
                    + CenterPower_group1_pos6;
                CenterPower_group2 = CenterPower_group2_pos1 + CenterPower_group2_pos2
                    + CenterPower_group2_pos3 + CenterPower_group2_pos4 + CenterPower_group2_pos5
                    + CenterPower_group2_pos6;
                CenterPower_group3 = CenterPower_group3_pos1 + CenterPower_group3_pos2
                    + CenterPower_group3_pos3 + CenterPower_group3_pos4 + CenterPower_group3_pos5
                    + CenterPower_group3_pos6;
                CenterPower_group4 = CenterPower_group4_pos1 + CenterPower_group4_pos2
                    + CenterPower_group4_pos3 + CenterPower_group4_pos4 + CenterPower_group4_pos5
                    + CenterPower_group4_pos6;
                CenterPower_group5 = CenterPower_group5_pos1 + CenterPower_group5_pos2
                    + CenterPower_group5_pos3 + CenterPower_group5_pos4 + CenterPower_group5_pos5
                    + CenterPower_group5_pos6;
                #endregion

                #region CenterPower_MAX
                string CenterPower_MAX1 = "";
                string CenterPower_MAX1_pos1 = "";
                string CenterPower_MAX1_pos2 = "";
                string CenterPower_MAX1_pos3 = "";
                string CenterPower_MAX1_pos4 = "";
                string CenterPower_MAX1_pos5 = "";
                string CenterPower_MAX1_pos6 = "";
                string CenterPower_MAX2 = "";
                string CenterPower_MAX2_pos1 = "";
                string CenterPower_MAX2_pos2 = "";
                string CenterPower_MAX2_pos3 = "";
                string CenterPower_MAX2_pos4 = "";
                string CenterPower_MAX2_pos5 = "";
                string CenterPower_MAX2_pos6 = "";
                string CenterPower_MAX3 = "";
                string CenterPower_MAX3_pos1 = "";
                string CenterPower_MAX3_pos2 = "";
                string CenterPower_MAX3_pos3 = "";
                string CenterPower_MAX3_pos4 = "";
                string CenterPower_MAX3_pos5 = "";
                string CenterPower_MAX3_pos6 = "";
                string CenterPower_MAX4 = "";
                string CenterPower_MAX4_pos1 = "";
                string CenterPower_MAX4_pos2 = "";
                string CenterPower_MAX4_pos3 = "";
                string CenterPower_MAX4_pos4 = "";
                string CenterPower_MAX4_pos5 = "";
                string CenterPower_MAX4_pos6 = "";
                string CenterPower_MAX5 = "";
                string CenterPower_MAX5_pos1 = "";
                string CenterPower_MAX5_pos2 = "";
                string CenterPower_MAX5_pos3 = "";
                string CenterPower_MAX5_pos4 = "";
                string CenterPower_MAX5_pos5 = "";
                string CenterPower_MAX5_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    CenterPower_MAX1_pos1 += data[tagName + $"\"CenterPower_MAX1\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MAX1_pos2 += data[tagName + $"\"CenterPower_MAX1\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MAX1_pos3 += data[tagName + $"\"CenterPower_MAX1\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MAX1_pos4 += data[tagName + $"\"CenterPower_MAX1\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MAX1_pos5 += data[tagName + $"\"CenterPower_MAX1\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MAX1_pos6 += data[tagName + $"\"CenterPower_MAX1\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MAX2_pos1 += data[tagName + $"\"CenterPower_MAX2\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MAX2_pos2 += data[tagName + $"\"CenterPower_MAX2\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MAX2_pos3 += data[tagName + $"\"CenterPower_MAX2\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MAX2_pos4 += data[tagName + $"\"CenterPower_MAX2\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MAX2_pos5 += data[tagName + $"\"CenterPower_MAX2\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MAX2_pos6 += data[tagName + $"\"CenterPower_MAX2\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MAX3_pos1 += data[tagName + $"\"CenterPower_MAX3\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MAX3_pos2 += data[tagName + $"\"CenterPower_MAX3\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MAX3_pos3 += data[tagName + $"\"CenterPower_MAX3\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MAX3_pos4 += data[tagName + $"\"CenterPower_MAX3\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MAX3_pos5 += data[tagName + $"\"CenterPower_MAX3\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MAX3_pos6 += data[tagName + $"\"CenterPower_MAX3\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MAX4_pos1 += data[tagName + $"\"CenterPower_MAX4\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MAX4_pos2 += data[tagName + $"\"CenterPower_MAX4\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MAX4_pos3 += data[tagName + $"\"CenterPower_MAX4\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MAX4_pos4 += data[tagName + $"\"CenterPower_MAX4\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MAX4_pos5 += data[tagName + $"\"CenterPower_MAX4\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MAX4_pos6 += data[tagName + $"\"CenterPower_MAX4\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MAX5_pos1 += data[tagName + $"\"CenterPower_MAX5\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MAX5_pos2 += data[tagName + $"\"CenterPower_MAX5\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MAX5_pos3 += data[tagName + $"\"CenterPower_MAX5\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MAX5_pos4 += data[tagName + $"\"CenterPower_MAX5\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MAX5_pos5 += data[tagName + $"\"CenterPower_MAX5\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MAX5_pos6 += data[tagName + $"\"CenterPower_MAX5\".pos6.hole{b + 1}"] + ";";

                }
                CenterPower_MAX1 = CenterPower_MAX1_pos1 + CenterPower_MAX1_pos2
                    + CenterPower_MAX1_pos3 + CenterPower_MAX1_pos4 + CenterPower_MAX1_pos5
                    + CenterPower_MAX1_pos6;
                CenterPower_MAX2 = CenterPower_MAX2_pos1 + CenterPower_MAX2_pos2
                    + CenterPower_MAX2_pos3 + CenterPower_MAX2_pos4 + CenterPower_MAX2_pos5
                    + CenterPower_MAX2_pos6;
                CenterPower_MAX3 = CenterPower_MAX3_pos1 + CenterPower_MAX3_pos2
                    + CenterPower_MAX3_pos3 + CenterPower_MAX3_pos4 + CenterPower_MAX3_pos5
                    + CenterPower_MAX3_pos6;
                CenterPower_MAX4 = CenterPower_MAX4_pos1 + CenterPower_MAX4_pos2
                    + CenterPower_MAX4_pos3 + CenterPower_MAX4_pos4 + CenterPower_MAX4_pos5
                    + CenterPower_MAX4_pos6;
                CenterPower_MAX5 = CenterPower_MAX5_pos1 + CenterPower_MAX5_pos2
                    + CenterPower_MAX5_pos3 + CenterPower_MAX5_pos4 + CenterPower_MAX5_pos5
                    + CenterPower_MAX5_pos6;
                #endregion

                #region CenterPower_MIN
                string CenterPower_MIN1 = "";
                string CenterPower_MIN1_pos1 = "";
                string CenterPower_MIN1_pos2 = "";
                string CenterPower_MIN1_pos3 = "";
                string CenterPower_MIN1_pos4 = "";
                string CenterPower_MIN1_pos5 = "";
                string CenterPower_MIN1_pos6 = "";
                string CenterPower_MIN2 = "";
                string CenterPower_MIN2_pos1 = "";
                string CenterPower_MIN2_pos2 = "";
                string CenterPower_MIN2_pos3 = "";
                string CenterPower_MIN2_pos4 = "";
                string CenterPower_MIN2_pos5 = "";
                string CenterPower_MIN2_pos6 = "";
                string CenterPower_MIN3 = "";
                string CenterPower_MIN3_pos1 = "";
                string CenterPower_MIN3_pos2 = "";
                string CenterPower_MIN3_pos3 = "";
                string CenterPower_MIN3_pos4 = "";
                string CenterPower_MIN3_pos5 = "";
                string CenterPower_MIN3_pos6 = "";
                string CenterPower_MIN4 = "";
                string CenterPower_MIN4_pos1 = "";
                string CenterPower_MIN4_pos2 = "";
                string CenterPower_MIN4_pos3 = "";
                string CenterPower_MIN4_pos4 = "";
                string CenterPower_MIN4_pos5 = "";
                string CenterPower_MIN4_pos6 = "";
                string CenterPower_MIN5 = "";
                string CenterPower_MIN5_pos1 = "";
                string CenterPower_MIN5_pos2 = "";
                string CenterPower_MIN5_pos3 = "";
                string CenterPower_MIN5_pos4 = "";
                string CenterPower_MIN5_pos5 = "";
                string CenterPower_MIN5_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    CenterPower_MIN1_pos1 += data[tagName + $"\"CenterPower_MIN1\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MIN1_pos2 += data[tagName + $"\"CenterPower_MIN1\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MIN1_pos3 += data[tagName + $"\"CenterPower_MIN1\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MIN1_pos4 += data[tagName + $"\"CenterPower_MIN1\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MIN1_pos5 += data[tagName + $"\"CenterPower_MIN1\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MIN1_pos6 += data[tagName + $"\"CenterPower_MIN1\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MIN2_pos1 += data[tagName + $"\"CenterPower_MIN2\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MIN2_pos2 += data[tagName + $"\"CenterPower_MIN2\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MIN2_pos3 += data[tagName + $"\"CenterPower_MIN2\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MIN2_pos4 += data[tagName + $"\"CenterPower_MIN2\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MIN2_pos5 += data[tagName + $"\"CenterPower_MIN2\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MIN2_pos6 += data[tagName + $"\"CenterPower_MIN2\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MIN3_pos1 += data[tagName + $"\"CenterPower_MIN3\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MIN3_pos2 += data[tagName + $"\"CenterPower_MIN3\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MIN3_pos3 += data[tagName + $"\"CenterPower_MIN3\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MIN3_pos4 += data[tagName + $"\"CenterPower_MIN3\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MIN3_pos5 += data[tagName + $"\"CenterPower_MIN3\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MIN3_pos6 += data[tagName + $"\"CenterPower_MIN3\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MIN4_pos1 += data[tagName + $"\"CenterPower_MIN4\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MIN4_pos2 += data[tagName + $"\"CenterPower_MIN4\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MIN4_pos3 += data[tagName + $"\"CenterPower_MIN4\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MIN4_pos4 += data[tagName + $"\"CenterPower_MIN4\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MIN4_pos5 += data[tagName + $"\"CenterPower_MIN4\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MIN4_pos6 += data[tagName + $"\"CenterPower_MIN4\".pos6.hole{b + 1}"] + ";";

                    CenterPower_MIN5_pos1 += data[tagName + $"\"CenterPower_MIN5\".pos1.hole{b + 1}"] + ";";
                    CenterPower_MIN5_pos2 += data[tagName + $"\"CenterPower_MIN5\".pos2.hole{b + 1}"] + ";";
                    CenterPower_MIN5_pos3 += data[tagName + $"\"CenterPower_MIN5\".pos3.hole{b + 1}"] + ";";
                    CenterPower_MIN5_pos4 += data[tagName + $"\"CenterPower_MIN5\".pos4.hole{b + 1}"] + ";";
                    CenterPower_MIN5_pos5 += data[tagName + $"\"CenterPower_MIN5\".pos5.hole{b + 1}"] + ";";
                    CenterPower_MIN5_pos6 += data[tagName + $"\"CenterPower_MIN5\".pos6.hole{b + 1}"] + ";";

                }
                CenterPower_MIN1 = CenterPower_MIN1_pos1 + CenterPower_MIN1_pos2
                    + CenterPower_MIN1_pos3 + CenterPower_MIN1_pos4 + CenterPower_MIN1_pos5
                    + CenterPower_MIN1_pos6;
                CenterPower_MIN2 = CenterPower_MIN2_pos1 + CenterPower_MIN2_pos2
                    + CenterPower_MIN2_pos3 + CenterPower_MIN2_pos4 + CenterPower_MIN2_pos5
                    + CenterPower_MIN2_pos6;
                CenterPower_MIN3 = CenterPower_MIN3_pos1 + CenterPower_MIN3_pos2
                    + CenterPower_MIN3_pos3 + CenterPower_MIN3_pos4 + CenterPower_MIN3_pos5
                    + CenterPower_MIN3_pos6;
                CenterPower_MIN4 = CenterPower_MIN4_pos1 + CenterPower_MIN4_pos2
                    + CenterPower_MIN4_pos3 + CenterPower_MIN4_pos4 + CenterPower_MIN4_pos5
                    + CenterPower_MIN4_pos6;
                CenterPower_MIN5 = CenterPower_MIN5_pos1 + CenterPower_MIN5_pos2
                    + CenterPower_MIN5_pos3 + CenterPower_MIN5_pos4 + CenterPower_MIN5_pos5
                    + CenterPower_MIN5_pos6;
                #endregion

                #region RingPower_group
                string RingPower_group1 = "";
                string RingPower_group1_pos1 = "";
                string RingPower_group1_pos2 = "";
                string RingPower_group1_pos3 = "";
                string RingPower_group1_pos4 = "";
                string RingPower_group1_pos5 = "";
                string RingPower_group1_pos6 = "";
                string RingPower_group2 = "";
                string RingPower_group2_pos1 = "";
                string RingPower_group2_pos2 = "";
                string RingPower_group2_pos3 = "";
                string RingPower_group2_pos4 = "";
                string RingPower_group2_pos5 = "";
                string RingPower_group2_pos6 = "";
                string RingPower_group3 = "";
                string RingPower_group3_pos1 = "";
                string RingPower_group3_pos2 = "";
                string RingPower_group3_pos3 = "";
                string RingPower_group3_pos4 = "";
                string RingPower_group3_pos5 = "";
                string RingPower_group3_pos6 = "";
                string RingPower_group4 = "";
                string RingPower_group4_pos1 = "";
                string RingPower_group4_pos2 = "";
                string RingPower_group4_pos3 = "";
                string RingPower_group4_pos4 = "";
                string RingPower_group4_pos5 = "";
                string RingPower_group4_pos6 = "";
                string RingPower_group5 = "";
                string RingPower_group5_pos1 = "";
                string RingPower_group5_pos2 = "";
                string RingPower_group5_pos3 = "";
                string RingPower_group5_pos4 = "";
                string RingPower_group5_pos5 = "";
                string RingPower_group5_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    RingPower_group1_pos1 += data[tagName + $"\"RingPower_group1\".pos1.hole{b + 1}"] + ";";
                    RingPower_group1_pos2 += data[tagName + $"\"RingPower_group1\".pos2.hole{b + 1}"] + ";";
                    RingPower_group1_pos3 += data[tagName + $"\"RingPower_group1\".pos3.hole{b + 1}"] + ";";
                    RingPower_group1_pos4 += data[tagName + $"\"RingPower_group1\".pos4.hole{b + 1}"] + ";";
                    RingPower_group1_pos5 += data[tagName + $"\"RingPower_group1\".pos5.hole{b + 1}"] + ";";
                    RingPower_group1_pos6 += data[tagName + $"\"RingPower_group1\".pos6.hole{b + 1}"] + ";";

                    RingPower_group2_pos1 += data[tagName + $"\"RingPower_group2\".pos1.hole{b + 1}"] + ";";
                    RingPower_group2_pos2 += data[tagName + $"\"RingPower_group2\".pos2.hole{b + 1}"] + ";";
                    RingPower_group2_pos3 += data[tagName + $"\"RingPower_group2\".pos3.hole{b + 1}"] + ";";
                    RingPower_group2_pos4 += data[tagName + $"\"RingPower_group2\".pos4.hole{b + 1}"] + ";";
                    RingPower_group2_pos5 += data[tagName + $"\"RingPower_group2\".pos5.hole{b + 1}"] + ";";
                    RingPower_group2_pos6 += data[tagName + $"\"RingPower_group2\".pos6.hole{b + 1}"] + ";";

                    RingPower_group3_pos1 += data[tagName + $"\"RingPower_group3\".pos1.hole{b + 1}"] + ";";
                    RingPower_group3_pos2 += data[tagName + $"\"RingPower_group3\".pos2.hole{b + 1}"] + ";";
                    RingPower_group3_pos3 += data[tagName + $"\"RingPower_group3\".pos3.hole{b + 1}"] + ";";
                    RingPower_group3_pos4 += data[tagName + $"\"RingPower_group3\".pos4.hole{b + 1}"] + ";";
                    RingPower_group3_pos5 += data[tagName + $"\"RingPower_group3\".pos5.hole{b + 1}"] + ";";
                    RingPower_group3_pos6 += data[tagName + $"\"RingPower_group3\".pos6.hole{b + 1}"] + ";";

                    RingPower_group4_pos1 += data[tagName + $"\"RingPower_group4\".pos1.hole{b + 1}"] + ";";
                    RingPower_group4_pos2 += data[tagName + $"\"RingPower_group4\".pos2.hole{b + 1}"] + ";";
                    RingPower_group4_pos3 += data[tagName + $"\"RingPower_group4\".pos3.hole{b + 1}"] + ";";
                    RingPower_group4_pos4 += data[tagName + $"\"RingPower_group4\".pos4.hole{b + 1}"] + ";";
                    RingPower_group4_pos5 += data[tagName + $"\"RingPower_group4\".pos5.hole{b + 1}"] + ";";
                    RingPower_group4_pos6 += data[tagName + $"\"RingPower_group4\".pos6.hole{b + 1}"] + ";";

                    RingPower_group5_pos1 += data[tagName + $"\"RingPower_group5\".pos1.hole{b + 1}"] + ";";
                    RingPower_group5_pos2 += data[tagName + $"\"RingPower_group5\".pos2.hole{b + 1}"] + ";";
                    RingPower_group5_pos3 += data[tagName + $"\"RingPower_group5\".pos3.hole{b + 1}"] + ";";
                    RingPower_group5_pos4 += data[tagName + $"\"RingPower_group5\".pos4.hole{b + 1}"] + ";";
                    RingPower_group5_pos5 += data[tagName + $"\"RingPower_group5\".pos5.hole{b + 1}"] + ";";
                    RingPower_group5_pos6 += data[tagName + $"\"RingPower_group5\".pos6.hole{b + 1}"] + ";";

                }
                RingPower_group1 = RingPower_group1_pos1 + RingPower_group1_pos2
                    + RingPower_group1_pos3 + RingPower_group1_pos4 + RingPower_group1_pos5
                    + RingPower_group1_pos6;
                RingPower_group2 = RingPower_group2_pos1 + RingPower_group2_pos2
                    + RingPower_group2_pos3 + RingPower_group2_pos4 + RingPower_group2_pos5
                    + RingPower_group2_pos6;
                RingPower_group3 = RingPower_group3_pos1 + RingPower_group3_pos2
                    + RingPower_group3_pos3 + RingPower_group3_pos4 + RingPower_group3_pos5
                    + RingPower_group3_pos6;
                RingPower_group4 = RingPower_group4_pos1 + RingPower_group4_pos2
                    + RingPower_group4_pos3 + RingPower_group4_pos4 + RingPower_group4_pos5
                    + RingPower_group4_pos6;
                RingPower_group5 = RingPower_group5_pos1 + RingPower_group5_pos2
                    + RingPower_group5_pos3 + RingPower_group5_pos4 + RingPower_group5_pos5
                    + RingPower_group5_pos6;
                #endregion

                #region RingPower_MAX
                string RingPower_MAX1 = "";
                string RingPower_MAX1_pos1 = "";
                string RingPower_MAX1_pos2 = "";
                string RingPower_MAX1_pos3 = "";
                string RingPower_MAX1_pos4 = "";
                string RingPower_MAX1_pos5 = "";
                string RingPower_MAX1_pos6 = "";
                string RingPower_MAX2 = "";
                string RingPower_MAX2_pos1 = "";
                string RingPower_MAX2_pos2 = "";
                string RingPower_MAX2_pos3 = "";
                string RingPower_MAX2_pos4 = "";
                string RingPower_MAX2_pos5 = "";
                string RingPower_MAX2_pos6 = "";
                string RingPower_MAX3 = "";
                string RingPower_MAX3_pos1 = "";
                string RingPower_MAX3_pos2 = "";
                string RingPower_MAX3_pos3 = "";
                string RingPower_MAX3_pos4 = "";
                string RingPower_MAX3_pos5 = "";
                string RingPower_MAX3_pos6 = "";
                string RingPower_MAX4 = "";
                string RingPower_MAX4_pos1 = "";
                string RingPower_MAX4_pos2 = "";
                string RingPower_MAX4_pos3 = "";
                string RingPower_MAX4_pos4 = "";
                string RingPower_MAX4_pos5 = "";
                string RingPower_MAX4_pos6 = "";
                string RingPower_MAX5 = "";
                string RingPower_MAX5_pos1 = "";
                string RingPower_MAX5_pos2 = "";
                string RingPower_MAX5_pos3 = "";
                string RingPower_MAX5_pos4 = "";
                string RingPower_MAX5_pos5 = "";
                string RingPower_MAX5_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    RingPower_MAX1_pos1 += data[tagName + $"\"RingPower_MAX1\".pos1.hole{b + 1}"] + ";";
                    RingPower_MAX1_pos2 += data[tagName + $"\"RingPower_MAX1\".pos2.hole{b + 1}"] + ";";
                    RingPower_MAX1_pos3 += data[tagName + $"\"RingPower_MAX1\".pos3.hole{b + 1}"] + ";";
                    RingPower_MAX1_pos4 += data[tagName + $"\"RingPower_MAX1\".pos4.hole{b + 1}"] + ";";
                    RingPower_MAX1_pos5 += data[tagName + $"\"RingPower_MAX1\".pos5.hole{b + 1}"] + ";";
                    RingPower_MAX1_pos6 += data[tagName + $"\"RingPower_MAX1\".pos6.hole{b + 1}"] + ";";

                    RingPower_MAX2_pos1 += data[tagName + $"\"RingPower_MAX2\".pos1.hole{b + 1}"] + ";";
                    RingPower_MAX2_pos2 += data[tagName + $"\"RingPower_MAX2\".pos2.hole{b + 1}"] + ";";
                    RingPower_MAX2_pos3 += data[tagName + $"\"RingPower_MAX2\".pos3.hole{b + 1}"] + ";";
                    RingPower_MAX2_pos4 += data[tagName + $"\"RingPower_MAX2\".pos4.hole{b + 1}"] + ";";
                    RingPower_MAX2_pos5 += data[tagName + $"\"RingPower_MAX2\".pos5.hole{b + 1}"] + ";";
                    RingPower_MAX2_pos6 += data[tagName + $"\"RingPower_MAX2\".pos6.hole{b + 1}"] + ";";

                    RingPower_MAX3_pos1 += data[tagName + $"\"RingPower_MAX3\".pos1.hole{b + 1}"] + ";";
                    RingPower_MAX3_pos2 += data[tagName + $"\"RingPower_MAX3\".pos2.hole{b + 1}"] + ";";
                    RingPower_MAX3_pos3 += data[tagName + $"\"RingPower_MAX3\".pos3.hole{b + 1}"] + ";";
                    RingPower_MAX3_pos4 += data[tagName + $"\"RingPower_MAX3\".pos4.hole{b + 1}"] + ";";
                    RingPower_MAX3_pos5 += data[tagName + $"\"RingPower_MAX3\".pos5.hole{b + 1}"] + ";";
                    RingPower_MAX3_pos6 += data[tagName + $"\"RingPower_MAX3\".pos6.hole{b + 1}"] + ";";

                    RingPower_MAX4_pos1 += data[tagName + $"\"RingPower_MAX4\".pos1.hole{b + 1}"] + ";";
                    RingPower_MAX4_pos2 += data[tagName + $"\"RingPower_MAX4\".pos2.hole{b + 1}"] + ";";
                    RingPower_MAX4_pos3 += data[tagName + $"\"RingPower_MAX4\".pos3.hole{b + 1}"] + ";";
                    RingPower_MAX4_pos4 += data[tagName + $"\"RingPower_MAX4\".pos4.hole{b + 1}"] + ";";
                    RingPower_MAX4_pos5 += data[tagName + $"\"RingPower_MAX4\".pos5.hole{b + 1}"] + ";";
                    RingPower_MAX4_pos6 += data[tagName + $"\"RingPower_MAX4\".pos6.hole{b + 1}"] + ";";

                    RingPower_MAX5_pos1 += data[tagName + $"\"RingPower_MAX5\".pos1.hole{b + 1}"] + ";";
                    RingPower_MAX5_pos2 += data[tagName + $"\"RingPower_MAX5\".pos2.hole{b + 1}"] + ";";
                    RingPower_MAX5_pos3 += data[tagName + $"\"RingPower_MAX5\".pos3.hole{b + 1}"] + ";";
                    RingPower_MAX5_pos4 += data[tagName + $"\"RingPower_MAX5\".pos4.hole{b + 1}"] + ";";
                    RingPower_MAX5_pos5 += data[tagName + $"\"RingPower_MAX5\".pos5.hole{b + 1}"] + ";";
                    RingPower_MAX5_pos6 += data[tagName + $"\"RingPower_MAX5\".pos6.hole{b + 1}"] + ";";

                }
                RingPower_MAX1 = RingPower_MAX1_pos1 + RingPower_MAX1_pos2
                    + RingPower_MAX1_pos3 + RingPower_MAX1_pos4 + RingPower_MAX1_pos5
                    + RingPower_MAX1_pos6;
                RingPower_MAX2 = RingPower_MAX2_pos1 + RingPower_MAX2_pos2
                    + RingPower_MAX2_pos3 + RingPower_MAX2_pos4 + RingPower_MAX2_pos5
                    + RingPower_MAX2_pos6;
                RingPower_MAX3 = RingPower_MAX3_pos1 + RingPower_MAX3_pos2
                    + RingPower_MAX3_pos3 + RingPower_MAX3_pos4 + RingPower_MAX3_pos5
                    + RingPower_MAX3_pos6;
                RingPower_MAX4 = RingPower_MAX4_pos1 + RingPower_MAX4_pos2
                    + RingPower_MAX4_pos3 + RingPower_MAX4_pos4 + RingPower_MAX4_pos5
                    + RingPower_MAX4_pos6;
                RingPower_MAX5 = RingPower_MAX5_pos1 + RingPower_MAX5_pos2
                    + RingPower_MAX5_pos3 + RingPower_MAX5_pos4 + RingPower_MAX5_pos5
                    + RingPower_MAX5_pos6;
                #endregion

                #region RingPower_MIN
                string RingPower_MIN1 = "";
                string RingPower_MIN1_pos1 = "";
                string RingPower_MIN1_pos2 = "";
                string RingPower_MIN1_pos3 = "";
                string RingPower_MIN1_pos4 = "";
                string RingPower_MIN1_pos5 = "";
                string RingPower_MIN1_pos6 = "";
                string RingPower_MIN2 = "";
                string RingPower_MIN2_pos1 = "";
                string RingPower_MIN2_pos2 = "";
                string RingPower_MIN2_pos3 = "";
                string RingPower_MIN2_pos4 = "";
                string RingPower_MIN2_pos5 = "";
                string RingPower_MIN2_pos6 = "";
                string RingPower_MIN3 = "";
                string RingPower_MIN3_pos1 = "";
                string RingPower_MIN3_pos2 = "";
                string RingPower_MIN3_pos3 = "";
                string RingPower_MIN3_pos4 = "";
                string RingPower_MIN3_pos5 = "";
                string RingPower_MIN3_pos6 = "";
                string RingPower_MIN4 = "";
                string RingPower_MIN4_pos1 = "";
                string RingPower_MIN4_pos2 = "";
                string RingPower_MIN4_pos3 = "";
                string RingPower_MIN4_pos4 = "";
                string RingPower_MIN4_pos5 = "";
                string RingPower_MIN4_pos6 = "";
                string RingPower_MIN5 = "";
                string RingPower_MIN5_pos1 = "";
                string RingPower_MIN5_pos2 = "";
                string RingPower_MIN5_pos3 = "";
                string RingPower_MIN5_pos4 = "";
                string RingPower_MIN5_pos5 = "";
                string RingPower_MIN5_pos6 = "";

                for (int b = 0; b < 4; b++)
                {
                    RingPower_MIN1_pos1 += data[tagName + $"\"RingPower_MIN1\".pos1.hole{b + 1}"] + ";";
                    RingPower_MIN1_pos2 += data[tagName + $"\"RingPower_MIN1\".pos2.hole{b + 1}"] + ";";
                    RingPower_MIN1_pos3 += data[tagName + $"\"RingPower_MIN1\".pos3.hole{b + 1}"] + ";";
                    RingPower_MIN1_pos4 += data[tagName + $"\"RingPower_MIN1\".pos4.hole{b + 1}"] + ";";
                    RingPower_MIN1_pos5 += data[tagName + $"\"RingPower_MIN1\".pos5.hole{b + 1}"] + ";";
                    RingPower_MIN1_pos6 += data[tagName + $"\"RingPower_MIN1\".pos6.hole{b + 1}"] + ";";

                    RingPower_MIN2_pos1 += data[tagName + $"\"RingPower_MIN2\".pos1.hole{b + 1}"] + ";";
                    RingPower_MIN2_pos2 += data[tagName + $"\"RingPower_MIN2\".pos2.hole{b + 1}"] + ";";
                    RingPower_MIN2_pos3 += data[tagName + $"\"RingPower_MIN2\".pos3.hole{b + 1}"] + ";";
                    RingPower_MIN2_pos4 += data[tagName + $"\"RingPower_MIN2\".pos4.hole{b + 1}"] + ";";
                    RingPower_MIN2_pos5 += data[tagName + $"\"RingPower_MIN2\".pos5.hole{b + 1}"] + ";";
                    RingPower_MIN2_pos6 += data[tagName + $"\"RingPower_MIN2\".pos6.hole{b + 1}"] + ";";

                    RingPower_MIN3_pos1 += data[tagName + $"\"RingPower_MIN3\".pos1.hole{b + 1}"] + ";";
                    RingPower_MIN3_pos2 += data[tagName + $"\"RingPower_MIN3\".pos2.hole{b + 1}"] + ";";
                    RingPower_MIN3_pos3 += data[tagName + $"\"RingPower_MIN3\".pos3.hole{b + 1}"] + ";";
                    RingPower_MIN3_pos4 += data[tagName + $"\"RingPower_MIN3\".pos4.hole{b + 1}"] + ";";
                    RingPower_MIN3_pos5 += data[tagName + $"\"RingPower_MIN3\".pos5.hole{b + 1}"] + ";";
                    RingPower_MIN3_pos6 += data[tagName + $"\"RingPower_MIN3\".pos6.hole{b + 1}"] + ";";

                    RingPower_MIN4_pos1 += data[tagName + $"\"RingPower_MIN4\".pos1.hole{b + 1}"] + ";";
                    RingPower_MIN4_pos2 += data[tagName + $"\"RingPower_MIN4\".pos2.hole{b + 1}"] + ";";
                    RingPower_MIN4_pos3 += data[tagName + $"\"RingPower_MIN4\".pos3.hole{b + 1}"] + ";";
                    RingPower_MIN4_pos4 += data[tagName + $"\"RingPower_MIN4\".pos4.hole{b + 1}"] + ";";
                    RingPower_MIN4_pos5 += data[tagName + $"\"RingPower_MIN4\".pos5.hole{b + 1}"] + ";";
                    RingPower_MIN4_pos6 += data[tagName + $"\"RingPower_MIN4\".pos6.hole{b + 1}"] + ";";

                    RingPower_MIN5_pos1 += data[tagName + $"\"RingPower_MIN5\".pos1.hole{b + 1}"] + ";";
                    RingPower_MIN5_pos2 += data[tagName + $"\"RingPower_MIN5\".pos2.hole{b + 1}"] + ";";
                    RingPower_MIN5_pos3 += data[tagName + $"\"RingPower_MIN5\".pos3.hole{b + 1}"] + ";";
                    RingPower_MIN5_pos4 += data[tagName + $"\"RingPower_MIN5\".pos4.hole{b + 1}"] + ";";
                    RingPower_MIN5_pos5 += data[tagName + $"\"RingPower_MIN5\".pos5.hole{b + 1}"] + ";";
                    RingPower_MIN5_pos6 += data[tagName + $"\"RingPower_MIN5\".pos6.hole{b + 1}"] + ";";

                }
                RingPower_MIN1 = RingPower_MIN1_pos1 + RingPower_MIN1_pos2
                    + RingPower_MIN1_pos3 + RingPower_MIN1_pos4 + RingPower_MIN1_pos5
                    + RingPower_MIN1_pos6;
                RingPower_MIN2 = RingPower_MIN2_pos1 + RingPower_MIN2_pos2
                    + RingPower_MIN2_pos3 + RingPower_MIN2_pos4 + RingPower_MIN2_pos5
                    + RingPower_MIN2_pos6;
                RingPower_MIN3 = RingPower_MIN3_pos1 + RingPower_MIN3_pos2
                    + RingPower_MIN3_pos3 + RingPower_MIN3_pos4 + RingPower_MIN3_pos5
                    + RingPower_MIN3_pos6;
                RingPower_MIN4 = RingPower_MIN4_pos1 + RingPower_MIN4_pos2
                    + RingPower_MIN4_pos3 + RingPower_MIN4_pos4 + RingPower_MIN4_pos5
                    + RingPower_MIN4_pos6;
                RingPower_MIN5 = RingPower_MIN5_pos1 + RingPower_MIN5_pos2
                    + RingPower_MIN5_pos3 + RingPower_MIN5_pos4 + RingPower_MIN5_pos5
                    + RingPower_MIN5_pos6;
                #endregion 
                #endregion

                string Module_acode = data[tagName + $"\"Module-acode\""].ToString();
                string Module_bcode = data[tagName + $"\"Module-bcode\""].ToString();
                string CurrentUserName = data[tagName + $"\"CurrentUserName\""].ToString();
                string rfid = data[tagName + $"\"RFID\""].ToString();



                //string Setvalue_Group1 = string.Empty;
                //string Setvalue_Group2 = string.Empty;
                //string Setvalue_Group3 = string.Empty;
                //string Setvalue_Group4 = string.Empty;
                //string Setvalue_Group5 = string.Empty;
                //for (int i = 0; i < 6; i++)
                //{
                //    Setvalue_Group1 += data[tagName + $"\"setvalue_Group1\".pos{i + 1}"].ToString() + ";";
                //    Setvalue_Group2 += data[tagName + $"\"setvalue_Group2\".pos{i + 1}"].ToString() + ";";
                //    Setvalue_Group3 += data[tagName + $"\"setvalue_Group3\".pos{i + 1}"].ToString() + ";";
                //    Setvalue_Group4 += data[tagName + $"\"setvalue_Group4\".pos{i + 1}"].ToString() + ";";
                //    Setvalue_Group5 += data[tagName + $"\"setvalue_Group5\".pos{i + 1}"].ToString() + ";";
                //}
                //Setvalue_Group1 = Setvalue_Group1.Substring(0, Setvalue_Group1.Length - 1);
                //Setvalue_Group2 = Setvalue_Group2.Substring(0, Setvalue_Group2.Length - 1);
                //Setvalue_Group3 = Setvalue_Group3.Substring(0, Setvalue_Group3.Length - 1);
                //Setvalue_Group4 = Setvalue_Group4.Substring(0, Setvalue_Group4.Length - 1);
                //Setvalue_Group5 = Setvalue_Group5.Substring(0, Setvalue_Group5.Length - 1);


                //是否采集完成
                bool isFinish = MesFalg == 4 || MesFalg == 0;

                //触发采集信号
                if (PlcFalg == 1 && isFinish)
                {
                    //写入结果
                    short writeResult = -1;
                    if (!_isOutStation)
                    {
                        //初始化实体类
                        dicLst.Clear();
                        dicLst.Add(data);
                        _log.ToCSVData(dicLst, "", "Busbar焊接");
                        BusbarWeldEntity busbar = new BusbarWeldEntity();
                        busbar.BusbarSpeed = BusbarSpeed;
                        busbar.Phto_A_offset = Phto_A_offset;
                        busbar.Phto_B_offset = Phto_B_offset;
                        busbar.Photo1 = photo1;
                        busbar.Photo2 = photo2;
                        busbar.Photo3 = photo3;
                        busbar.Photo4 = photo4;
                        busbar.Mark = Mark;
                        //busbar.Setvalue_Group1 = Setvalue_Group1;
                        //busbar.Setvalue_Group2 = Setvalue_Group2;
                        //busbar.Setvalue_Group3 = Setvalue_Group3;
                        //busbar.Setvalue_Group4 = Setvalue_Group4;
                        //busbar.Setvalue_Group5 = Setvalue_Group5;
                        busbar.Module_A = OffsetA; // 测距值
                        busbar.Module_B = OffsetB; // 补偿后测距值
                        busbar.Module_acode = Module_acode;
                        busbar.Module_bcode = Module_bcode;
                        busbar.CurrentUserName = CurrentUserName;
                        busbar.RFID = rfid;
                        busbar.PackCode = PackCode;
                        busbar.CenterPower_group1 = CenterPower_group1;
                        busbar.CenterPower_group2 = CenterPower_group2;
                        busbar.CenterPower_group3 = CenterPower_group3;
                        busbar.CenterPower_group4 = CenterPower_group4;
                        busbar.CenterPower_group5 = CenterPower_group5;
                        busbar.CenterPower_MAX1 = CenterPower_MAX1;
                        busbar.CenterPower_MAX2 = CenterPower_MAX2;
                        busbar.CenterPower_MAX3 = CenterPower_MAX3;
                        busbar.CenterPower_MAX4 = CenterPower_MAX4;
                        busbar.CenterPower_MAX5 = CenterPower_MAX5;
                        busbar.CenterPower_MIN1 = CenterPower_MIN1;
                        busbar.CenterPower_MIN2 = CenterPower_MIN2;
                        busbar.CenterPower_MIN3 = CenterPower_MIN3;
                        busbar.CenterPower_MIN4 = CenterPower_MIN4;
                        busbar.CenterPower_MIN5 = CenterPower_MIN5;

                        busbar.RingPower_group1 = RingPower_group1;
                        busbar.RingPower_group2 = RingPower_group2;
                        busbar.RingPower_group3 = RingPower_group3;
                        busbar.RingPower_group4 = RingPower_group4;
                        busbar.RingPower_group5 = RingPower_group5;
                        busbar.RingPower_MAX1 = RingPower_MAX1;
                        busbar.RingPower_MAX2 = RingPower_MAX2;
                        busbar.RingPower_MAX3 = RingPower_MAX3;
                        busbar.RingPower_MAX4 = RingPower_MAX4;
                        busbar.RingPower_MAX5 = RingPower_MAX5;
                        busbar.RingPower_MIN1 = RingPower_MIN1;
                        busbar.RingPower_MIN2 = RingPower_MIN2;
                        busbar.RingPower_MIN3 = RingPower_MIN3;
                        busbar.RingPower_MIN4 = RingPower_MIN4;
                        busbar.RingPower_MIN5 = RingPower_MIN5;


                        //验证数据和业务逻辑写在这个里面
                        do
                        {
                            #region mes交互
                            // 判断mes状态是否启用
                            var mesStatus = IniFileAPI.INIGetStringValue(@AppConfig.WebserviceiniPath, "MES状态", "MesStatus", "");
                            if (mesStatus == "ON") // ON 开
                            {

                                ////进站
                                //try
                                //{
                                //    MESwebservice.FindCusAndSfc.miFindCustomAndSfcDataResponse miFindCustomAndSfcDataResponse = BlockCall.ShimEntry(@AppConfig.WebserviceiniPath, "Busbar进站", messfc);
                                //    code = miFindCustomAndSfcDataResponse.@return.code;
                                //    msg = miFindCustomAndSfcDataResponse.@return.message;
                                //}
                                //catch (Exception ex)
                                //{
                                //    throw ex;
                                //}
                                //if (!checkCode("Busbar焊接", "Busbar进站接口", code, msg, messfc))
                                //{
                                //    _outStaionResult = false;
                                //    _isOutStation = true;
                                //    return;
                                //}

                                //收数
                                try
                                {
                                    //调用两次
                                    BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                    MESwebservice.AutoWeight.dataCollectForSfcExResponse sfcResponse = BlockCall.BusbarDatacoll(@AppConfig.WebserviceiniPath, "Busbar收数", messfc, busbar);
                                    code = sfcResponse.@return.code;
                                    msg = sfcResponse.@return.message;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                if (!checkCode("Busbar焊接", "Busbar收数接口", code, msg, messfc))
                                {
                                    _outStaionResult = false;
                                    _isOutStation = true;
                                    return;
                                }

                                // 出站
                                try
                                {
                                    BlockCall blockCall = new BlockCall(_collectTaskContext, _requestToHttpHelper);
                                    MESwebservice.AutoWeight.sfcCompleteResponse sfcCompleteResponseA = BlockCall.BusbarComplete(@AppConfig.WebserviceiniPath, "Busbar出站", messfc);
                                    code = sfcCompleteResponseA.@return.code;
                                    msg = sfcCompleteResponseA.@return.message;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                if (!checkCode("Busbar焊接", "Busbar出站接口", code, msg, messfc))
                                {
                                    _outStaionResult = false;
                                    _isOutStation = true;
                                    return;
                                }
                            }
                            #endregion

                        } while (false);

                        //表示已经调用过出战
                        _isOutStation = true;
                        //保存出站结果
                        _outStaionResult = true;

                        //上传追溯数据
                        HttpResponseResultModel<BusbarWeldEntity> result = AddEntityAync(busbar);
                        string s = busbar.ToJson();
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", s);
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
                    writeTags[_mesDataStatus] = Convert.ToInt16(writeResult);
                    //写入PLC变量
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接resul写入失败"));
                    }
                    else
                    {

                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接resul写入2失败"));
                        }
                        else
                        {
                            //标记结束出站
                            _isOutStation = false;
                        }
                    }


                }
                if (PlcFalg == 3)
                {

                    _isOutStation = false;

                    writeTags[_mesDataStatus] = Convert.ToInt16(4);
                    if (MesFalg != 4)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写4失败"));
                        }
                    }

                }
                if (PlcFalg == 0 && MesFalg != 0)
                {
                    writeTags[_mesDataStatus] = Convert.ToInt16(0);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写0失败"));
                    }
                    _isOutStation = false;
                }
                bool isget = MesFalg == 8 || MesFalg == 0;
                //busbar写入数据相关
                if (PlcFalg == 5 && isget)
                {

                    short writeResult = -1;
                    //模组号可以上面取一下 MoudlecodeA = Module_acode, MoudlecodeB = Module_bcode
                    HttpResponseResultModel<List<SeekSiteEntity>> weldData = GetDataAync(new SeekSiteEntity { MoudlecodeA = Module_acode, MoudlecodeB = Module_bcode });
                    List<SeekSiteEntity> weldDatas = weldData.BackResult;
                    if (weldDatas.Count > 0)
                    {
                        char[] cPhoto_1 = weldDatas[0].Photo_1.ToCharArray();
                        char[] cPhoto_2 = weldDatas[0].Photo_2.ToCharArray();
                        char[] cPhoto_3 = weldDatas[0].Photo_3.ToCharArray();
                        char[] cPhoto_4 = weldDatas[0].Photo_4.ToCharArray();
                        char[] cMark = weldDatas[0].Mark.ToCharArray();
                        for (int i = 0; i < cPhoto_1.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_1\"[{i}]"] = Convert.ToByte(cPhoto_1[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        writeTags.Clear();

                        for (int i = 0; i < cPhoto_2.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_2\"[{i}]"] = Convert.ToByte(cPhoto_2[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        writeTags.Clear();
                        for (int i = 0; i < cPhoto_3.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_3\"[{i}]"] = Convert.ToByte(cPhoto_3[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        writeTags.Clear();
                        for (int i = 0; i < cPhoto_4.Length; i++)
                        {
                            writeTags[tagName + $"\"photo_4\"[{i}]"] = Convert.ToByte(cPhoto_4[i]);
                        }
                        for (int i = 0; i < cMark.Length; i++)
                        {
                            writeTags[tagName + $"\"Mark\"[{i}]"] = Convert.ToByte(cMark[i]);
                        }
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("Buabar焊接写入失败"));
                        }
                        else
                        {
                            //标记结束出站
                            _isOutStation = false;
                        }
                        writeTags.Clear();
                    }
                    else
                    {
                        _log.AddUserLog("Buabar焊接", "Buabar焊接", string.Format("焊钳寻址数据获取失败,请检查模组码是否正确!"));
                    }
                    //表示已经调用过出战
                    _isOutStation = true;
                    //保存出站结果
                    _outStaionResult = true;



                    //根据是否出站成功来回写标准位
                    if (_outStaionResult)
                    {
                        writeResult = 5;
                    }
                    else
                    {
                        writeResult = 6;
                    }
                    writeTags[_mesDataStatus] = Convert.ToInt16(writeResult);
                    if (!opc.WriteNodes(writeTags))
                    {
                        _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写入" + writeResult + "失败"));
                    }
                }
                if (PlcFalg == 7)
                {

                    _isOutStation = false;

                    writeTags[_mesDataStatus] = Convert.ToInt16(8);
                    if (MesFalg != 8)
                    {
                        if (!opc.WriteNodes(writeTags))
                        {
                            _log.AddUserLog("Busbar焊接", "Busbar焊接", string.Format("Busbar焊接标志位写8失败"));
                        }
                    }

                }
            }

        }
        /// <summary>
        /// 调用webapi插入数据
        /// </summary>
        /// <param name="busbarWeldEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<BusbarWeldEntity> AddEntityAync(BusbarWeldEntity busbarWeldEntity)
        {
            return _requestToHttpHelper.PostAsync<BusbarWeldEntity>(new HttpRequestModel
            {
                Host = Apihost,
                Path = "/BusbarWeld/insert",
                Data = busbarWeldEntity
            }).Result;
        }
        /// <summary>
        /// 调用api获取焊前寻址数据
        /// </summary>
        /// <param name="seekSiteEntity"></param>
        /// <returns></returns>
        HttpResponseResultModel<List<SeekSiteEntity>> GetDataAync(SeekSiteEntity seekSiteEntity)
        {
            return _requestToHttpHelper.GetAsync<List<SeekSiteEntity>>(new HttpRequestModel
            {
                Host = Apihost,
                Path = $"/SeekSite/GetList?MoudlecodeA=" + seekSiteEntity.MoudlecodeA + "&MoudlecodeB=" + seekSiteEntity.MoudlecodeB,
                Data = seekSiteEntity
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
        /// <param name="sfc">模组号</param>
        public bool checkCode(string siteName, string interfaceName, int code, string msg, string sfc)
        {
            if (code == 0)
            {
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "调用成功", "");
                _log.AddUserLog(siteName, siteName, string.Format(interfaceName + "调用成功"));
            }
            else if (code > 0)
            {
                _collectTaskContext.TaskMsgOperator.SetPairText(interfaceName + "调用失败", "");
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
