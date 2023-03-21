using CS.Base.HttpHelper;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.IBLL.Business
{
    public interface IProductService
    {
        Task<QueryPagedResponseModel<ProductEntity>> GetProductListAsync(int pageIndex, int pageSize, ProductCondition condition);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> AddProductAsync(ProductEntity productEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> UpdateProductAsync(long id, ProductEntity productEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTypeEntity"></param>
        /// <returns></returns>
        Task<bool> DeleteProductAsync(long id);

    }
}
