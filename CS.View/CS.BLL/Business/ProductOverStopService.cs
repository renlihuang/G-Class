using CS.Base.AppSet;
using CS.Base.HttpHelper;
using CS.Core;
using CS.IBLL.Business;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using DCS.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.BLL.Business
{
    public class ProductOverStopService : IProductOverStopService, IAutoInjectScoped
    {
        readonly RequestToHttpHelper _requestToHttpHelper;

        public ProductOverStopService(RequestToHttpHelper requestToHttpHelper)
        {
            _requestToHttpHelper = requestToHttpHelper;
        }

        public async Task<bool> AddProductOverStopAsync(ProductOverStopEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/AutoWeight/insert";
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);
            //
            return result.IsSuccess;
        }

        public async Task<bool> DeteleProductOverStopAsync(long id)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/AutoWeight/UpdateById/" + id;
            httpRequestModel.Data = new { IsDeleted = 1 };
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }

        /// <summary>
        /// 过站信息查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<QueryPagedResponseModel<ProductOverStopEntity>> GetProductOverStopAsync(int pageIndex, int pageSize, ProductOverStopCondition condition)
        {
            QueryPagedResponseModel<ProductOverStopEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            //托盘码
            if (!string.IsNullOrEmpty(condition.ProductCode))
            {
                ConditionStringBuilder.Append($"&ProductCode={condition.ProductCode}");
            }
            // 过站信息
            if (!string.IsNullOrEmpty(condition.StandInfo))
            {
                ConditionStringBuilder.Append($"&StandInfo={condition.StandInfo}");
            }
            // 开始时间
            if (!string.IsNullOrEmpty(condition.CreateTimeStart))
            {
                ConditionStringBuilder.Append($"&CreateTimeStart={condition.CreateTimeStart}");
            }
            // 结束时间
            if (!string.IsNullOrEmpty(condition.CreateTimeEnd))
            {
                DateTime end = Convert.ToDateTime(condition.CreateTimeEnd);
                end = end.AddDays(1);
                ConditionStringBuilder.Append($"&CreateTimeEnd={end.ToString()}");
            }
            //添加模组号查询条件
            //if (!string.IsNullOrEmpty(condition.ModuleCode))
            //{
            //    ConditionStringBuilder.Append($"&ModuleCode={condition.ModuleCode}");
            //}

            //按父节点ID
            //if (condition.ID > 0)
            //{
            //    ConditionStringBuilder.Append($"&ID={condition.ID}");
            //}

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ProductOverStop/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ProductOverStopEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ProductOverStopEntity>();
            }
            return responseModel;
        }

        /// <summary>
        /// 过站统计查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<QueryPagedResponseModel<ProductOverStopEntity>> GetProductOverStopAllAsync(int pageIndex, int pageSize, ProductOverStopCondition condition)
        {
            QueryPagedResponseModel<ProductOverStopEntity> responseModel = null;
            //拼接查询字符串
            StringBuilder ConditionStringBuilder = new StringBuilder();

            // 产品类型
            if (!string.IsNullOrEmpty(condition.ProductType))
            {
                ConditionStringBuilder.Append($"&ProductType={condition.ProductType}");
                ConditionStringBuilder.Append($"&IEFlag=出站");
            }
            //托盘码
            if (!string.IsNullOrEmpty(condition.ProductCode))
            {
                ConditionStringBuilder.Append($"&ProductCode={condition.ProductCode}");
            }
            // 过站信息
            if (!string.IsNullOrEmpty(condition.StandInfo))
            {
                ConditionStringBuilder.Append($"&StandInfo={condition.StandInfo}");
            }
            // 过站结果
            if (!string.IsNullOrEmpty(condition.OverStop))
            {
                ConditionStringBuilder.Append($"&OverStop={condition.OverStop}");
            }
            // 开始时间
            if (!string.IsNullOrEmpty(condition.CreateTimeStart))
            {
                ConditionStringBuilder.Append($"&CreateTimeStart={condition.CreateTimeStart}");
            }
            // 结束时间
            if (!string.IsNullOrEmpty(condition.CreateTimeEnd))
            {
                DateTime end = Convert.ToDateTime(condition.CreateTimeEnd);
                end = end.AddDays(1);
                ConditionStringBuilder.Append($"&CreateTimeEnd={end.ToString()}");
            }

            //按父节点ID
            //if (condition.ID > 0)
            //{
            //    ConditionStringBuilder.Append($"&ID={condition.ID}");
            //}

            HttpRequestModel httpRequestModel = new HttpRequestModel();
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = $"/ProductOverStop/GetListPaged?page={pageIndex}&limit={pageSize}" + ConditionStringBuilder.ToString();
            //查询数据
            var result = await _requestToHttpHelper.GetAsync<QueryPagedResponseModel<ProductOverStopEntity>>(httpRequestModel).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                responseModel = result.BackResult;
            }
            else
            {
                responseModel = new QueryPagedResponseModel<ProductOverStopEntity>();
            }
            return responseModel;
        }

        public async Task<bool> UpdateProductOverStopAsync(long id, ProductOverStopEntity entiry)
        {
            HttpRequestModel httpRequestModel = new HttpRequestModel();
            //设置请求数据
            httpRequestModel.Host = AppConfig.BusinessApiHost;
            httpRequestModel.Path = "/BatteryCoreOcvTest/UpdateById/" + id;
            httpRequestModel.Data = entiry;
            //发送POST请求
            var result = await _requestToHttpHelper.PostAsync<HttpResponseResultModel<long>>(httpRequestModel).ConfigureAwait(false);

            return result.IsSuccess;
        }

    }
}
