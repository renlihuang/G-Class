using CS.View.FrameControl;
using CS.View.ViewModel;
using CS.View.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.View.Common
{
    enum MsgType
    { 
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("错误")]
        Error,
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("错误")]
        Warning,
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("提示信息")]
        Info,
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Description("询问")]
        Question,
    }
    /// <summary>
    /// 显示提示框
    /// </summary>
    class SkinMessageBox
    {
        public static async void Error(string text)
        {
            await Show(text, MsgType.Error);
        }

        public static async void Warning(string text)
        {
            await Show(text, MsgType.Warning);
        }

        public static async void Info(string text)
        {
            await Show(text, MsgType.Info);
        }

        public static async Task<bool> Question(string text)
        {
            return await Show(text, MsgType.Question);
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        private static async Task<bool> Show(string text, MsgType msgType)
        {
            string icon = string.Empty;
            string color = string.Empty;
            bool isHide = true;

            switch (msgType)
            {
                case MsgType.Error:
                    icon = "CommentRemoveOutline";
                    color = "#FF4500";
                    break;
                case MsgType.Warning:
                    icon = "CommentWarning";
                    color = "#FF8247";
                    break;
                case MsgType.Info:
                    icon = "CommentProcessingOutline";
                    color = "#1C86EE";
                    break;
                case MsgType.Question:
                    icon = "CommentQuestionOutline";
                    color = "#20B2AA";
                    isHide = false;
                    break;
            }

            BaseMsgDialog msgDialog = new BaseMsgDialog();
            msgDialog.BindDataContex<MsgBox, MsgBoxViewModel>(new MsgBox(), new MsgBoxViewModel() { Text = text, Color = color, Icon = icon, IsHide = isHide });
            //显示对话框
            bool result = await msgDialog.ShowDialog();

            return result;
        }
    }
}
