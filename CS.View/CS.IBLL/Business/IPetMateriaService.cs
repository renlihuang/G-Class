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
    public interface IPetMateriaService
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<PetMateriaEntity>> GetPetMateriaAsync(int pageIndex, int pageSize, PetMateriaCondition condition);

        /// <summary>
        /// 添加参数名
        /// </summary>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> AddPetMateriaAsync(PetMateriaEntity entiry);

        /// <summary>
        /// 更新参数名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> UpdatePetMateriaAsync(long id, PetMateriaEntity entiry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> DetelePetMateriaAsync(long id);

    }
}
