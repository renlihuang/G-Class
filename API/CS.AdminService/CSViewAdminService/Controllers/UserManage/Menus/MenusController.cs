

//使用平台信息: ID:NETCORE_WebApi  描述:NETCORE_WebApi
//代码版本信息: ID:V3_1  描述:V3_2模块组件(泛型主键),不向前兼容，对应模板V3_2 添加时间:2021/10/9 9:23:36
using CSViewAdminService.UserManage.Abstractions;
using  CSViewAdminService.AppLayer;
using  CSViewAdminService.Utility;
using CSViewAdminService.Filters;
using CSViewAdminService.Core;
using CSViewAdminService.AppLayer.UserManage.ViewObject;
using CSViewAdminService.AppLayer.UserManage.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace  CSViewAdminService.Controllers
{
    
    /// <summary>
    /// 接口
    /// </summary>

     	[ApiExplorerSettings(GroupName = "UserManage")] 
    [Route("Menus")]
    [ApiController]
    [ModelValidation]
    public class  MenusController : ControllerBase
    {
        private readonly MenusApp menusApp;
        
        public MenusController(MenusApp menusApp)
        {
            this.menusApp = menusApp;
        }

          /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        [Route("Get")]
        [ProducesResponseType(typeof(MenusView), 200)]
        [HttpGet]
        public async Task<ActionResult> GetAsync(long  id)
        {
            var entity = await menusApp.GetAsync(id).ConfigureAwait(false);
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
        public async Task<ActionResult> InsertAsync([FromBody]MenusInsertDTO insertDto)
        {
            if (insertDto == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await menusApp.InsertAsync(insertDto).ConfigureAwait(false);

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
        private async Task<ActionResult> InsertBatchAsync([FromBody]List<MenusInsertDTO > insertDtoList)
        {
            if (insertDtoList == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }

            var result = await menusApp.InsertBatchAsync(insertDtoList).ConfigureAwait(false);

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
        public async Task<ActionResult> UpdateByIdAsync(long id, [FromBody]MenusUpdateDTO  updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await menusApp.UpdateAsync(id,updateDto).ConfigureAwait(false);

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

            var result = await menusApp.DeleteAsync(id).ConfigureAwait(false);
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

            var result = await menusApp.DeleteBatchAsync(idList).ConfigureAwait(false);
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
        [ProducesResponseType(typeof(QueryPagedResponseModel<MenusView>), 200)]
        [HttpGet]
        public async Task<ActionResult> GetListPagedAsync(int page = 1, int limit = 10,[FromQuery]MenusConditionDTO conditionDTO = null)
        {
           
            var queryPagedResponse = await menusApp.GetListPagedAsync(page, limit, conditionDTO, null, "ID desc").ConfigureAwait(false);
            return Ok(queryPagedResponse);
        }

        /// <summary>
        /// 查询数据(不分页) 返回表记录
        /// </summary>
        /// <param name="conditionDTO">查询条件类</param>
        /// <returns></returns>
        [Route("GetList")]
        [ProducesResponseType(typeof(List<MenusView>), 200)]
        [HttpGet]
        public async Task<ActionResult> GetListAsync([FromQuery]MenusConditionDTO conditionDTO)
        {
            var list = await menusApp.GetListAsync(conditionDTO, null, "id desc").ConfigureAwait(false);
            return Ok(list);
        }

        
		 /// <summary>
        /// 根据id集合获取多条数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("GetListByIds")]
        [ProducesResponseType(typeof(List<MenusView>), 200)]
        [HttpPost]
        public async Task<ActionResult> GetListByIdsAsync([FromBody]List<long> ids)
        {
            var list = await menusApp.GetListByIdsAsync(ids).ConfigureAwait(false);
            return Ok(list);
        }

    }
}