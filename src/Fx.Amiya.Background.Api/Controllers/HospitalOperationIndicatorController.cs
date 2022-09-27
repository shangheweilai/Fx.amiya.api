﻿using Fx.Amiya.Background.Api.Vo.HospitalOperationIndicator;
using Fx.Authorization.Attributes;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 机构运营指标数据
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class HospitalOperationIndicatorController : ControllerBase
    {
        //private IGreatHospitalOperationHealthService greatHospitalOperationHealthService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="greatHospitalOperationHealthService"></param>
        public HospitalOperationIndicatorController(
            // IGreatHospitalOperationHealthService greatHospitalOperationHealthService
            )
        {
            //this.greatHospitalOperationHealthService = greatHospitalOperationHealthService;
        }


        /// <summary>
        /// 获取机构运营指标数据列表
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="indicatorsId">归属指标id</param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        public async Task<ResultData<List<HospitalOperationIndicatorVo>>> GetListWithPageAsync(string keyword, string valid,int pageNum,int pageSize)
        {
            try
            {
                //  var q = await greatHospitalOperationHealthService.GetListAsync(keyword, indicatorsId);

                //var greatHospitalOperationHealth = from d in q.List
                //              select new GreatHospitalOperationHealthVo
                //              {
                //                  Id = d.Id,
                //                  ExpressCode = d.ExpressCode,
                //                  ExpressName = d.ExpressName,
                //                  Valid = d.Valid
                //              };

                List<HospitalOperationIndicatorVo> hospitalOperationIndicatorPageInfo = new List<HospitalOperationIndicatorVo>();

                return ResultData<List<HospitalOperationIndicatorVo>>.Success().AddData("hospitalOperationIndicatorListInfo", hospitalOperationIndicatorPageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<List<HospitalOperationIndicatorVo>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 添加机构指标数据
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData> AddAsync(AddHospitalOperationIndicatorVo addVo)
        {
            try
            {
                //AddExpressDto addDto = new AddExpressDto();
                //addDto.ExpressCode = addVo.ExpressCode;
                //addDto.ExpressName = addVo.ExpressName;
                //addDto.Valid = addVo.Valid;

                //await greatHospitalOperationHealthService.AddAsync(addDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 根据机构指标id获取指标信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        public async Task<ResultData<HospitalOperationIndicatorVo>> GetByIdAsync(string id)
        {
            try
            {
                //var greatHospitalOperationHealth = await greatHospitalOperationHealthService.GetByIdAsync(id);
                HospitalOperationIndicatorVo hospitalOperationIndicatorVo = new HospitalOperationIndicatorVo();
                //greatHospitalOperationHealthVo.Id = greatHospitalOperationHealth.Id;
                //greatHospitalOperationHealthVo.ExpressCode = greatHospitalOperationHealth.ExpressCode;
                //greatHospitalOperationHealthVo.ExpressName = greatHospitalOperationHealth.ExpressName;
                //greatHospitalOperationHealthVo.Valid = greatHospitalOperationHealth.Valid;

                return ResultData<HospitalOperationIndicatorVo>.Success().AddData("hospitalOperationIndicatorInfo", hospitalOperationIndicatorVo);
            }
            catch (Exception ex)
            {
                return ResultData<HospitalOperationIndicatorVo>.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 修改机构运营指标数据
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultData> UpdateAsync(UpdateHospitalOperationIndicatorVo updateVo)
        {
            try
            {
                //UpdateExpressDto updateDto = new UpdateExpressDto();
                //updateDto.Id = updateVo.Id;
                //updateDto.ExpressName = updateVo.ExpressName;
                //updateDto.ExpressCode = updateVo.ExpressCode;
                //updateDto.Valid = updateVo.Valid;
                //await greatHospitalOperationHealthService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 删除机构运营指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultData> DeleteAsync(string id)
        {
            try
            {
                //await greatHospitalOperationHealthService.DeleteAsync(id);
                return ResultData.Success();
            }
            catch (Exception ex)
            {

                return ResultData.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 获取机构运营指标名称列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("nameList")]
        public async Task<ResultData<List<IndicatorNameVo>>> IndicatorNAmeList() {
            try
            {
                List<IndicatorNameVo> indicatorNameList = new List<IndicatorNameVo>();
                return ResultData<List<IndicatorNameVo>>.Success().AddData("indicatorNameList", indicatorNameList);
            }
            catch (Exception ex)
            {
                return ResultData<List<IndicatorNameVo>>.Fail(ex.Message);
            }
        }
    }
}
