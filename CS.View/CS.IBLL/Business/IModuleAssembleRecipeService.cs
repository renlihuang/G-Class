using CS.Base.HttpHelper;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using DCS.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.IBLL.Basic
{
    public interface IRecipeService
    {
        /// <summary>
        /// 获取所有模组列表
        /// </summary>
        /// <returns></returns>
        Task<List<ProductEntity>> GetRecipeListAsync();

        /// <summary>
        /// 获取所有模组列表
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseResultModel<List<ProductEntity>>> GetRecipeListByIDAsync(List<long> ids);

        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> AddRecipeAync(ProductofLineEntity entity);

        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateRecipeAync(long id,ProductofLineEntity entity);


        /// <summary>
        /// 添加配方
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteRecipeAync(long id);

        /// <summary>
        /// 查询配方列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<ProductofLineEntity>> GetRecipeListAync(int pageIndex, int pageSize,ModuleAssembleRecipeCondition condition);



    }
}
