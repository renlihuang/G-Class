using System;
using System.Collections.Generic;
using System.Text;

namespace DCS.BASE
{
    public class ResultModel<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public T BackResult { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}
