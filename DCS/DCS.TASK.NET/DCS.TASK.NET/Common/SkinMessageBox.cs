
using DCS.TASK.NET.ViewDlg;
using DCS.TASK.NET.ViewModel.Base;
using DCS.TASK.NET.ViewModel.ViewDlgViewModel;
using MaterialDesignThemes.Wpf;
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
        public static void Error(string text)
        {
            Show(text, MsgType.Error);
        }

        public static void Warning(string text)
        {
            Show(text, MsgType.Warning);
        }

        public static void Info(string text)
        {
            Show(text, MsgType.Info);
        }

        public static bool Question(string text)
        {
            return Show(text, MsgType.Question);
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        private static bool Show(string text, MsgType msgType)
        {
            //
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

            //创建对话框
            MsgBoxDialog msgBoxDialog = new MsgBoxDialog();
            //关联数据上下文
            var viewModel = new MsgBoxViewModel(msgBoxDialog)
            {
                Text = text,
                Color = color,
                Icon = icon,
                IsHide = isHide
            };
            msgBoxDialog.DataContext = viewModel;
            //显示对话框
            msgBoxDialog.ShowDialog();

            return viewModel.DialogResult;
        }
    }
}
