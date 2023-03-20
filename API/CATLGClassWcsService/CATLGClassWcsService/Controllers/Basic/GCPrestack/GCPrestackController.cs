

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2023/3/20 15:16:19
using CATLGClassWcsService.Basic.Abstractions;
using  CATLGClassWcsService.AppLayer;
using  CATLGClassWcsService.Utility;
using CATLGClassWcsService.Filters;
using CATLGClassWcsService.Core;
using CATLGClassWcsService.AppLayer.ViewObject;
using CATLGClassWcsService.AppLayer.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace  CATLGClassWcsService.Controllers
{
    
    /// <summary>
    /// 接口
    /// </summary>

     	[ApiExplorerSettings(GroupName = "Basic")] 
    [Route("GCPrestack")]
    [ApiController]
    [ModelValidation]
    public class  GCPrestackController : ControllerBase
    {
        private readonly GCPrestackApp gCPrestackApp;
        
        public GCPrestackController(GCPrestackApp gCPrestackApp)
        {
            this.gCPrestackApp = gCPrestackApp;
        }

          /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        [Route("Get")]
        [ProducesResponseType(typeof(GCPrestackView), 200)]
        [HttpGet]
        public async Task<ActionResult> GetAsync(long  id)
        {
            var entity = await gCPrestackApp.GetAsync(id).ConfigureAwait(false);
            return Ok(entity);
        }

        /// <summary>
        /// 插入新实体
        /// </summary>
        /// <param name="insertDto">对应实体数据传输对象</param>
        /// <returns></returns>
        [Route("insert")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<long>), 200)]
        public async Task<ActionResult> InsertAsync([FromBody]GCPrestackInsertDTO insertDto)
        {
            if (insertDto == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await gCPrestackApp.InsertAsync(insertDto).ConfigureAwait(false);

            return Ok(result);
        }


         /// <summary>
        /// 批量插入新实体
        /// </summary>
        /// <param name="insertEntityList">对应实体数据传输对象集合</param>
        /// <returns></returns>
        [Route("InsertBatch")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        private async Task<ActionResult> InsertBatchAsync([FromBody]List<GCPrestackInsertDTO > insertDtoList)
        {
            if (insertDtoList == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }

            var result = await gCPrestackApp.InsertBatchAsync(insertDtoList).ConfigureAwait(false);

            return Ok(result);
        }


       /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="updateDto">对应实体数据传输对象</param>
        /// <returns></returns>
        [Route("UpdateById/{id}")]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        [HttpPost]
        public async Task<ActionResult> UpdateByIdAsync(long id, [FromBody]GCPrestackUpdateDTO  updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await gCPrestackApp.UpdateAsync(id,updateDto).ConfigureAwait(false);

            return Ok(result);
        }


         

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<ActionResult> DeleteAsync([FromBody]long id)
        {
                       if (id <= 0)
                                  {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }

            var result = await gCPrestackApp.DeleteAsync(id).ConfigureAwait(false);
            return Ok(result);
        }


		 /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [Route("DeleteBatch")]
        [HttpDelete]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<ActionResult> DeleteBatchAsync([FromBody]List<long> idList)
        {
            if (idList == null || idList.Count == 0)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }

            var result = await gCPrestackApp.DeleteBatchAsync(idList).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// 查询数据(分页) 返回表记录
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <param name="conditionDTO">查询条件类</param>
        /// <returns></returns>
        [Route("GetListPaged")]
        [ProducesResponseType(typeof(QueryPagedResponseModel<GCPrestackView>), 200)]
        [HttpGet]
        public async Task<ActionResult> GetListPagedAsync(int page = 1, int limit = 10,[FromQuery]GCPrestackConditionDTO conditionDTO = null)
        {
           
            var queryPagedResponse = await gCPrestackApp.GetListPagedAsync(page, limit, conditionDTO, null, "ID desc").ConfigureAwait(false);
            return Ok(queryPagedResponse);
        }

        /// <summary>
        /// 查询数据(不分页) 返回表记录
        /// </summary>
        /// <param name="conditionDTO">查询条件类</param>
        /// <returns></returns>
        [Route("GetList")]
        [ProducesResponseType(typeof(List<GCPrestackView>), 200)]
        [HttpGet]
        public async Task<ActionResult> GetListAsync([FromQuery]GCPrestackConditionDTO conditionDTO)
        {
            var list = await gCPrestackApp.GetListAsync(conditionDTO, null, "id desc").ConfigureAwait(false);
            return Ok(list);
        }

        
		 /// <summary>
        /// 根据id集合获取多条数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("GetListByIds")]
        [ProducesResponseType(typeof(List<GCPrestackView>), 200)]
        [HttpPost]
        public async Task<ActionResult> GetListByIdsAsync([FromBody]List<long> ids)
        {
            var list = await gCPrestackApp.GetListByIdsAsync(ids).ConfigureAwait(false);
            return Ok(list);
        }

    }
}