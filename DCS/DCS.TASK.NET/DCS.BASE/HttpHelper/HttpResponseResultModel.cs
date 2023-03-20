using System.Net;

namespace DCS.BASE
{

    /// <summary>
    ///  http请求结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpResponseResultModel<T> : ResultModel<T>
    {
        /// <summary>
        /// http码
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
