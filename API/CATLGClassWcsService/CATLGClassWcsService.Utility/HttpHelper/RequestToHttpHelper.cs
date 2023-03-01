using CATLGClassWcsService.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace CATLGClassWcsService.Utility
{
    /// <summary>
    /// 请求Http帮助类
    /// </summary>
    public class RequestToHttpHelper
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RequestToHttpHelper(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
        {
            return httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Get类型请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<T>> GetAsync<T>(HttpRequestModel requestModel)
        {
            var client = CreateClient();
            client.BaseAddress = new Uri($"{requestModel.Host}");
            AddAuthorizationHeader(client, requestModel.Token, requestModel.TokenType);
            var cancelSource = new CancellationTokenSource(requestModel.TimeOut);
            try
            {
                var response = await client.GetAsync(requestModel.Path, cancelSource.Token).ConfigureAwait(false); ;
                //  var response = await client.GetAsync(requestModel.Path).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var isSuccessStatusCode = response.IsSuccessStatusCode;
                var statusCode = response.StatusCode;
                return new HttpResponseResultModel<T>
                {
                    HttpStatusCode = statusCode,
                    IsSuccess = isSuccessStatusCode,
                    BackResult = JsonHelper.DeserializeObject<T>(content)
                };
            }
            finally
            {
                cancelSource.Dispose();
            }
        }


        /// <summary>
        /// Post类型请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<T>> PostAsync<T>(HttpRequestModel requestModel)
        {
            var client = CreateClient();
            client.BaseAddress = new Uri($"{requestModel.Host}");
            AddAuthorizationHeader(client, requestModel.Token, requestModel.TokenType);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestModel.Path);
            string json = JsonHelper.SerializeObject(requestModel.Data);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, requestModel.ContentType);
            var cancelSource = new CancellationTokenSource(requestModel.TimeOut);
            try
            {
                HttpResponseMessage response = await client.SendAsync(request, cancelSource.Token).ConfigureAwait(false);
                var statusCode = response.StatusCode;
                var isSuccessStatusCode = response.IsSuccessStatusCode;
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new HttpResponseResultModel<T>
                {
                    HttpStatusCode = statusCode,
                    IsSuccess = isSuccessStatusCode,
                    BackResult = JsonHelper.DeserializeObject<T>(content)
                };
            }
            finally
            {
                cancelSource.Dispose();
            }
        }

        /// <summary>
        /// Post类型的请求(非json)
        /// application/x-www-form-urlencoded 类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<T>> PostAsFormUrlEncodedAsync<T>(HttpRequestModel requestModel)
        {
            var client = CreateClient();
            client.BaseAddress = new Uri($"{requestModel.Host}");
            AddAuthorizationHeader(client, requestModel.Token, requestModel.TokenType);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestModel.Path);
            // DATA 如下request
            //List<KeyValuePair<string, string>> request = new List<KeyValuePair<string, string>>();//构建
            //request.Add(new KeyValuePair<string, string>("AutoCommit", "false"));
            //request.Add(new KeyValuePair<string, string>("ConsumerId", consumerId));
            if (requestModel.Data != null)
            {
                request.Content = new FormUrlEncodedContent(requestModel.KeyValuePairData);
            }
            var cancelSource = new CancellationTokenSource(requestModel.TimeOut);
            try
            {
                HttpResponseMessage response = await client.SendAsync(request, cancelSource.Token).ConfigureAwait(false);
                var statusCode = response.StatusCode;
                var isSuccessStatusCode = response.IsSuccessStatusCode;
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new HttpResponseResultModel<T>
                {
                    HttpStatusCode = statusCode,
                    IsSuccess = isSuccessStatusCode,
                    BackResult = JsonHelper.DeserializeObject<T>(content)
                };
            }
            finally
            {
                cancelSource.Dispose();
            }
        }

        /// <summary>
        /// Put类型请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<T>> PutAsync<T>(HttpRequestModel requestModel)
        {
            var client = CreateClient();
            client.BaseAddress = new Uri($"{requestModel.Host}");
            AddAuthorizationHeader(client, requestModel.Token, requestModel.TokenType);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestModel.Path);
            string json = JsonHelper.SerializeObject(requestModel.Data);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, requestModel.ContentType);
            var cancelSource = new CancellationTokenSource(requestModel.TimeOut);
            try
            {
                HttpResponseMessage response = await client.SendAsync(request, cancelSource.Token).ConfigureAwait(false);
                var statusCode = response.StatusCode;
                var isSuccessStatusCode = response.IsSuccessStatusCode;
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new HttpResponseResultModel<T>
                {
                    HttpStatusCode = statusCode,
                    IsSuccess = isSuccessStatusCode,
                    BackResult = JsonHelper.DeserializeObject<T>(content)
                };
            }
            finally
            {
                cancelSource.Dispose();
            }
        }


        /// <summary>
        /// Delete类型请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<T>> DeleteAsync<T>(HttpRequestModel requestModel)
        {
            var client = CreateClient();
            client.BaseAddress = new Uri($"{requestModel.Host}");
            AddAuthorizationHeader(client, requestModel.Token, requestModel.TokenType);
            var cancelSource = new CancellationTokenSource(requestModel.TimeOut);
            try
            {
                var response = await client.DeleteAsync(requestModel.Path, cancelSource.Token).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var statusCode = response.StatusCode;
                var isSuccessStatusCode = response.IsSuccessStatusCode;
                return new HttpResponseResultModel<T>
                {
                    HttpStatusCode = statusCode,
                    IsSuccess = isSuccessStatusCode,
                    BackResult = JsonHelper.DeserializeObject<T>(content)
                };
            }
            finally
            {
                cancelSource.Dispose();
            }
        }


        private void AddAuthorizationHeader(HttpClient client, string token, string tokenType)
        {
            if (!string.IsNullOrEmpty(token))
            {
                //tokenType 后面空格不能少
                client.DefaultRequestHeaders.Add("Authorization", $"{tokenType} " + token);
            }

        }
    }
}
