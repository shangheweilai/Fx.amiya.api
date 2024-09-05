﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Background.Api.Vo.HospitalContentplatformCode.Input;
using Fx.Amiya.Background.Api.Vo.HospitalContentplatformCode.Result;
using Fx.Amiya.Background.Api.Vo.ThirdPartContentplatformInfo.Input;
using Fx.Amiya.Dto.ContentPlateFormOrder;
using Fx.Amiya.Dto.ContentPlatFormOrderSend;
using Fx.Amiya.Dto.HospitalContentplatformCode.Input;
using Fx.Amiya.Dto.OperationLog;
using Fx.Amiya.IService;
using Fx.Amiya.Service;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Common.Utils;
using Fx.Open.Infrastructure.Web;
using Jd.Api.Util;
using jos_sdk_net.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 三方平台医院编码
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class HospitalContentplatformCodeController : ControllerBase
    {
        private IHospitalContentplatformCodeService hospitalContentplatformCodeService;
        private readonly IContentPlateFormOrderService contentPlateFormOrderService;
        private IHttpContextAccessor _httpContextAccessor;
        private IOperationLogService operationLogService;
        private IContentPlatformOrderSendService contentPlatformOrderSendService;
        private IThirdPartContentplatformInfoService thirdPartContentplatformInfoService;
        private IHospitalEmployeeService hospitalEmployeeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hospitalContentplatformCodeService"></param>
        /// <param name="contentPlateFormOrderService"></param>
        /// <param name="wxAppConfigService"></param>
        public HospitalContentplatformCodeController(IHospitalContentplatformCodeService hospitalContentplatformCodeService, IContentPlateFormOrderService contentPlateFormOrderService, IHttpContextAccessor httpContextAccessor, IContentPlatformOrderSendService contentPlatformOrderSendService, IOperationLogService operationLogService, IThirdPartContentplatformInfoService thirdPartContentplatformInfoService, IHospitalEmployeeService hospitalEmployeeService)
        {
            this.hospitalContentplatformCodeService = hospitalContentplatformCodeService;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this._httpContextAccessor = httpContextAccessor;
            this.operationLogService = operationLogService;
            this.contentPlatformOrderSendService = contentPlatformOrderSendService;
            this.thirdPartContentplatformInfoService = thirdPartContentplatformInfoService;
            this.hospitalEmployeeService = hospitalEmployeeService;
        }

        /// <summary>
        /// 管理端根据条件获取三方平台医院编码信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<FxPageInfo<HospitalContentplatformCodeVo>>> GetListWithPageAsync([FromQuery] QueryHospitalContentplatformCodeVo query)
        {
            try
            {
                QueryHospitalContentplatformCodeDto queryDto = new QueryHospitalContentplatformCodeDto();
                queryDto.StartDate = query.StartDate;
                queryDto.EndDate = query.EndDate;
                queryDto.PageNum = query.PageNum;
                queryDto.KeyWord = query.KeyWord;
                queryDto.PageSize = query.PageSize;


                queryDto.ThirdPartContentplatformInfoId = query.ThirdPartContentplatformInfoId;
                queryDto.HospitalId = query.HospitalId;
                var q = await hospitalContentplatformCodeService.GetListAsync(queryDto);
                var hospitalContentplatformCode = from d in q.List
                                                  select new HospitalContentplatformCodeVo
                                                  {
                                                      Id = d.Id,
                                                      CreateDate = d.CreateDate,
                                                      UpdateDate = d.UpdateDate,
                                                      Valid = d.Valid,
                                                      DeleteDate = d.DeleteDate,
                                                      ThirdPartContentplatformInfoId = d.ThirdPartContentplatformInfoId,
                                                      ThirdPartContentplatformInfoName = d.ThirdPartContentplatformInfoName,
                                                      HospitalId = d.HospitalId,
                                                      HospitalName = d.HospitalName,
                                                      Code = d.Code
                                                  };

                FxPageInfo<HospitalContentplatformCodeVo> pageInfo = new FxPageInfo<HospitalContentplatformCodeVo>();
                pageInfo.TotalCount = q.TotalCount;
                pageInfo.List = hospitalContentplatformCode;

                return ResultData<FxPageInfo<HospitalContentplatformCodeVo>>.Success().AddData("hospitalContentplatformCode", pageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<HospitalContentplatformCodeVo>>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 添加三方平台医院编码
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData> AddAsync(AddHospitalContentplatformCodeVo addVo)
        {

            try
            {
                AddHospitalContentplatformCodeDto addDto = new AddHospitalContentplatformCodeDto();
                addDto.HospitalId = addVo.HospitalId;
                addDto.ThirdPartContentplatformInfoId = addVo.ThirdPartContentplatformInfoId;
                addDto.Code = addVo.Code;
                await hospitalContentplatformCodeService.AddAsync(addDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 根据三方平台医院编码编号获取三方平台医院编码信息
        /// </summary>
        /// <param name="id">三方平台医院编码编号</param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<HospitalContentplatformCodeVo>> GetByIdAsync(string id)
        {
            try
            {
                var hospitalContentplatformCode = await hospitalContentplatformCodeService.GetByIdAsync(id);
                HospitalContentplatformCodeVo hospitalContentplatformCodeVo = new HospitalContentplatformCodeVo();
                hospitalContentplatformCodeVo.Id = hospitalContentplatformCode.Id;
                hospitalContentplatformCodeVo.CreateDate = hospitalContentplatformCode.CreateDate;
                hospitalContentplatformCodeVo.Valid = hospitalContentplatformCode.Valid;
                hospitalContentplatformCodeVo.HospitalId = hospitalContentplatformCode.HospitalId;
                hospitalContentplatformCodeVo.HospitalName = hospitalContentplatformCode.HospitalName;
                hospitalContentplatformCodeVo.ThirdPartContentplatformInfoId = hospitalContentplatformCode.ThirdPartContentplatformInfoId;
                hospitalContentplatformCodeVo.ThirdPartContentplatformInfoName = hospitalContentplatformCode.ThirdPartContentplatformInfoName;
                hospitalContentplatformCodeVo.Code = hospitalContentplatformCode.Code;
                return ResultData<HospitalContentplatformCodeVo>.Success().AddData("hospitalContentplatformCode", hospitalContentplatformCodeVo);
            }
            catch (Exception ex)
            {
                return ResultData<HospitalContentplatformCodeVo>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 修改三方平台医院编码信息
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData> UpdateAsync(UpdateHospitalContentplatformCodeVo updateVo)
        {
            try
            {
                UpdateHospitalContentplatformCodeDto updateDto = new UpdateHospitalContentplatformCodeDto();
                updateDto.Id = updateVo.Id;
                updateDto.HospitalId = updateVo.HospitalId;
                updateDto.ThirdPartContentplatformInfoId = updateVo.ThirdPartContentplatformInfoId;
                updateDto.Code = updateVo.Code;
                await hospitalContentplatformCodeService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 作废三方平台医院编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [FxInternalAuthorize]
        public async Task<ResultData> DeleteAsync(string id)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int empId = Convert.ToInt32(employee.Id);
                await hospitalContentplatformCodeService.DeleteAsync(id, empId);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 管理端根据医院id和三方平台id进行查重-朗姿
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("getIsRepeateByHospitalIdAndThirdPartIdToLangZi")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<ThirdPartContentPlatformInfoToLangZiResultVo>> GetIsRepeateByHospitalIdAndThirdPartIdToLangZiAsync([FromQuery] QueryIsRepeateByHospitalIdAndThirdPartIdVo query)
        {
            try
            {
                var thirdcontentPlatformInfo = await thirdPartContentplatformInfoService.GetByNameAsync("朗姿");
                var url = thirdcontentPlatformInfo.ApiUrl;
                QuerySendOrderDataByLangZiVo queryData = new QuerySendOrderDataByLangZiVo();
                queryData.FWSID = "E-31-31446";
                queryData.USERID = "INTAMY";
                var hospitalContentPlatformCode = await hospitalContentplatformCodeService.GetByHospitalIdAndThirdPartContentPlatformIdAsync(query.HospitalId, query.ThirdPartContentplatformInfoId);
                queryData.JGBM = hospitalContentPlatformCode.Code;

                queryData.PDBH = query.SendOrderId.ToString();
                var order = await contentPlateFormOrderService.GetByOrderIdAsync(query.OrderId);
                var sendInfo = await contentPlatformOrderSendService.GetByIdAsync(Convert.ToInt32(query.SendOrderId.ToString()));
                if (sendInfo.IsSpecifyHospitalEmployee == true)
                {
                    queryData.PDYSID = sendInfo.HospitalEmployeeId.ToString();
                    var hospitalEmpInfo = await hospitalEmployeeService.GetByIdAsync(sendInfo.HospitalEmployeeId);
                    queryData.PDYSNM = hospitalEmpInfo.Name;
                }
                queryData.KUNAM = order.CustomerName;
                queryData.REGION = order.City;
                queryData.TEL1 = order.Phone;
                queryData.PDRQ = order.SendDate.Value;
                var data = JsonConvert.SerializeObject(queryData);
                var getResult = await HttpUtil.HTTPJsonGetHasBodyAsync(url, data);
                //var getResult = "";
                var result = JsonConvert.DeserializeObject<ThirdPartContentPlatformInfoToLangZiResultVo>(getResult);
                switch (result.RESULT)
                {
                    case "0":
                        result.REMSG += "；无重复";
                        break;
                    case "1":
                        result.REMSG += "；已被其他通路建档";
                        break;
                    case "2":
                        result.REMSG += "；已被所在通路建档";
                        break;
                }


                return ResultData<ThirdPartContentPlatformInfoToLangZiResultVo>.Success().AddData("hospitalContentplatformCode", result);
            }
            catch (Exception ex)
            {
                return ResultData<ThirdPartContentPlatformInfoToLangZiResultVo>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 管理端根据医院id和派单编号进行改单-朗姿
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut("updateOrderStatusByLangZi")]
        public async Task<ResultData<UpdateOrderStatusResultVo>> UpdateOrderStatusByLangZiAsync([FromBody] UpdateOrderStatusVo updateVo)
        {
            OperationAddDto operationLog = new OperationAddDto();
            operationLog.Parameters = JsonConvert.SerializeObject(updateVo);

            try
            {
                UpdateOrderStatusResultVo result = new UpdateOrderStatusResultVo();
                int orderStatus = 0;
                var thirdcontentPlatformInfo = await thirdPartContentplatformInfoService.GetByNameAsync("朗姿");
                string SignMsg = thirdcontentPlatformInfo.Sign;
                string SignKey = "JDRQ=" + updateVo.JDRQ + "&JGBM=" + updateVo.JGBM + "&JGWZID=" + updateVo.JGWZID + "&JGWZNM=" + updateVo.JGWZNM + "&PDBH=" + updateVo.PDBH + "&RepeateOrderPicture=" + updateVo.RepeateOrderPicture + "&SFCD=" + updateVo.SFCD.ToString().ToLower() + "&SFJD=" + updateVo.SFJD.ToString().ToLower() + "&YL1=" + updateVo.YL1 + "&YL2=" + updateVo.YL2;
                var signData = MD5Helper.Get32MD5One(SignKey + SignMsg);
                if (signData != updateVo.Sign)
                {
                    throw new Exception("签名验证失败，返回签名:'" + updateVo.Sign + "'验证有误，请重新确认上传参数进行验证！");
                }

                if (updateVo.SFJD == true)
                {
                    if (updateVo.SFCD == true)
                    {
                        orderStatus = (int)ContentPlateFormOrderStatus.RepeatOrderProfundity;
                    }
                    else
                    {
                        orderStatus = (int)ContentPlateFormOrderStatus.ConfirmOrder;
                    }
                }
                else
                {
                    orderStatus = (int)ContentPlateFormOrderStatus.RepeatOrder;
                }
                var hospitalContentPlatformCode = await hospitalContentplatformCodeService.GetByHospitalCodeAndThirdPartContentPlatformIdAsync(updateVo.JGBM, "0fca2b4b-c023-4f7d-9675-b6acf8fd8b31");
                var hospitalId = hospitalContentPlatformCode.HospitalId;
                var contentPlatformOrderSendInfo = await contentPlatformOrderSendService.GetByIdAsync(Convert.ToInt32(updateVo.PDBH));
                if (contentPlatformOrderSendInfo == null)
                {
                    throw new Exception("派单编号错误，请重新请求接口！");
                }
                if (contentPlatformOrderSendInfo.HospitalId != hospitalId)
                {
                    throw new Exception("当前操作医院账户非指定派单医院，请重新请求接口！");
                }
                //改派单状态
                UpdateContentPlatFormOrderSendByLangZiDto updateContentPlatFormOrderSendByLangZiDto = new UpdateContentPlatFormOrderSendByLangZiDto();
                updateContentPlatFormOrderSendByLangZiDto.Id = Convert.ToInt32(updateVo.PDBH);
                updateContentPlatFormOrderSendByLangZiDto.HospitalRemark = updateVo.YL1 + ";" + updateVo.YL2 + ";";
                updateContentPlatFormOrderSendByLangZiDto.OrderStatus = orderStatus;
                updateContentPlatFormOrderSendByLangZiDto.IsRepeatProfundityOrder = updateVo.SFCD;
                await contentPlatformOrderSendService.UpdateByLangZiAsync(updateContentPlatFormOrderSendByLangZiDto);

                //改订单状态
                var orderId = contentPlatformOrderSendInfo.ContentPlatFormOrderId;
                UpdateOrderByLangZiDto updateOrderByLangZiDto = new UpdateOrderByLangZiDto();
                updateOrderByLangZiDto.OrderId = orderId;
                updateOrderByLangZiDto.HospitalConsulationEmployeeName = updateVo.JGWZNM;
                updateOrderByLangZiDto.OrderStatus = orderStatus;
                updateOrderByLangZiDto.IsRepeateOrder = updateVo.SFCD;

                DateTime updateDate = DateTime.ParseExact(updateVo.JDRQ, "yyyyMMdd", CultureInfo.InvariantCulture);
                updateOrderByLangZiDto.UpdateDate = updateDate;
                updateOrderByLangZiDto.RepeateOrderPicture = updateVo.RepeateOrderPicture;
                await contentPlateFormOrderService.UpdateOrderByLangZiAsync(updateOrderByLangZiDto);
                result.JGBM = updateVo.JGBM;
                result.PDBH = updateVo.PDBH;
                return ResultData<UpdateOrderStatusResultVo>.Success().AddData("updateOrderStatusResult", result);
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                return ResultData<UpdateOrderStatusResultVo>.Fail(ex.Message);
            }
            finally
            {
                operationLog.Source = (int)RequestSource.AmiyaBackground;
                operationLog.RequestType = (int)RequestType.Update;
                operationLog.RouteAddress = _httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }

    }
}