using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS.View.View.UICoreView
{
    /// <summary>
    /// 输入对象ITEM
    /// </summary>
    class InputTextItem
    {
        /// <summary>
        /// 输入文字
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public int Timestamp { get; set; }
    }

    /// <summary>
    /// SanCardLoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SanCardLoginWindow : Window
    {
        /// <summary>
        /// 输入队列
        /// </summary>
        List<InputTextItem> _inputTextItems = new List<InputTextItem>();

        /// <summary>
        /// 当前输入文字
        /// </summary>
        InputTextItem _inputTextItem = null;

        public SanCardLoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //设置焦点
            Action action = () => 
            {
                Keyboard.Focus(this.keyDownText);
            };

            this.Dispatcher.BeginInvoke(action);

        }

        /// <summary>
        /// 键盘输入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyDownText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Enter)
            {
                if (_inputTextItems.Count > 2)
                {
                    //是否是第一个
                    bool isFirst = false;
                    //是否是键盘输入
                    bool isKeyBoardInput = false;
                    //第一个元素
                    InputTextItem nextItem = null;
                    //保存已经缓存字符
                    StringBuilder stringBuilder = new StringBuilder();
                    //循环判断字符串
                    foreach (InputTextItem item in _inputTextItems)
                    {
                        if (!isFirst)
                        {
                            isFirst = true;
                            //获取第一个数据
                            nextItem = item;
                        }
                        else
                        {
                            //计算两个字符串间隔
                            int interval = item.Timestamp - nextItem.Timestamp;
                            //默认为键盘输入
                            isKeyBoardInput = true;
                            //大于50ms就认为人工键盘输入
                            if (interval <= 50)
                            {
                                isKeyBoardInput = false;
                                //保存字符串
                                stringBuilder.Append(nextItem.Text);
                            }
                            //设置下一个
                            nextItem = item;
                        }
                    }
                    //清空字符串缓存
                    _inputTextItems.Clear();
                    //清空引用
                    _inputTextItem = null;

                    if (!isKeyBoardInput)
                    {
                        //保存字符串
                        stringBuilder.Append(nextItem.Text);
                    }
                    //设置显示文字
                    if (stringBuilder.Length > 0)
                    {
                        keyDownText.Text = stringBuilder.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 文字输入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keyDownText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_inputTextItem != null)
            {
                int interval = Environment.TickCount - _inputTextItem.Timestamp;
                //小于50MS认为扫描器输入
                if (interval <= 50)
                {
                    _inputTextItem = new InputTextItem
                    {
                        Text = e.Text,
                        Timestamp = Environment.TickCount
                    };
                    //加入列表
                    _inputTextItems.Add(_inputTextItem);
                }
                else
                {
                    _inputTextItem.Text = e.Text;
                    _inputTextItem.Timestamp = Environment.TickCount;
                }
            }
            else
            {
                _inputTextItem = new InputTextItem
                {
                    Text = e.Text,
                    Timestamp = Environment.TickCount
                };
                //加入列表
                _inputTextItems.Add(_inputTextItem);
            }

            //阻止字符串输入
            e.Handled = true;
        }

        private void keyDownText_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.CanExecute = false;
                e.Handled = true;
            }
        }
    }
}
