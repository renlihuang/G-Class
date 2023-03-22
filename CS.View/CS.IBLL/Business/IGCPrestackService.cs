﻿using CS.Base.HttpHelper;
using CS.Model.Business.Entiry;
using CS.Model.Business.QueryCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.IBLL.Business
{
    public interface IGCPrestackService
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<GCPrestackEntity>> GetGCPrestackAsync(int pageIndex, int pageSize, GCPrestackCondition condition);

        /// <summary>
        /// 添加参数名
        /// </summary>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> AddGCPrestackAAsync(GCPrestackEntity entiry);

        /// <summary>
        /// 更新参数名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> UpdateGCPrestackAAsync(long id, GCPrestackEntity entiry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameEntiry"></param>
        /// <returns></returns>
        Task<bool> DeteleGCPrestackAAsync(long id);
    }
}
