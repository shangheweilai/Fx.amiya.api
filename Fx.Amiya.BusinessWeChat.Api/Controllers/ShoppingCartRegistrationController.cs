using Fx.Amiya.IService;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fx.Authorization.Attributes;
using Fx.Amiya.BusinessWeChat.Api.Vo.Base;
using Fx.Common;
using Fx.Amiya.Service;
using Fx.Amiya.Dto.ShoppingCartRegistration;
using Fx.Amiya.BusinessWechat.Api.Vo.ShoppingCartRegistration;
using Fx.Amiya.BusinessWeChat.Api.Vo.ShoppingCartRegistration;
using Fx.Amiya.BusinessWeChat.Api.Vo;
using Fx.Amiya.Dto.OperationLog;
using Newtonsoft.Json;

namespace Fx.Amiya.BusinessWechat.Api.Controllers
{
    /// <summary>
    /// 小黄车登记列表接口
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [FxInternalAuthorize]
    public class ShoppingCartRegistrationController : ControllerBase
    {

        private IShoppingCartRegistrationService shoppingCartRegistrationService;
        private IHttpContextAccessor httpContextAccessor;
        private IOperationLogService operationLogService;
        private IContentPlateFormOrderService contentPlateFormOrderService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shoppingCartRegistrationService"></param>
        public ShoppingCartRegistrationController(IShoppingCartRegistrationService shoppingCartRegistrationService, IOperationLogService operationLogService, IContentPlateFormOrderService contentPlateFormOrderService, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this.operationLogService = operationLogService;
            this.shoppingCartRegistrationService = shoppingCartRegistrationService;
        }

        /// <summary>
        /// 获取小黄车登记信息列表（分页）
        /// </summary>
        /// <param name="startDate">登记开始时间</param>
        /// <param name="endDate">登记结束时间</param>
        /// <param name="LiveAnchorId">主播id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="contentPlatFormId">内容平台id</param>
        /// <param name="isCreateOrder">录单触达</param>
        /// <param name="isSendOrder">派单触达</param>
        /// <param name="isAddWechat">是否加v</param>
        /// <param name="isWriteOff">是否核销</param>
        /// <param name="isConsultation">是否面诊</param>
        /// <param name="isReturnBackPrice">是否回款</param>
        /// <param name="minPrice">最小金额</param>
        /// <param name="maxPrice">最大金额</param>
        /// <param name="assignEmpId">接诊人员</param>
        /// <param name="createBy">创建人</param>
        /// <param name="startBadReviewTime">差评开始时间</param>
        /// <param name="endBadReviewTime">差评结束时间</param>
        /// <param name="startRefundTime">退款开始时间</param>
        /// <param name="endRefundTime">退款结束时间</param>
        /// <param name="emergencyLevel">紧急程度</param>
        /// <param name="isBadReview">是否差评</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="baseLiveAnchorId">主播基础id</param>
        /// <param name="source">客户来源</param>
        /// <returns></returns>
        [HttpGet("listWithPage")]
        public async Task<ResultData<FxPageInfo<ShoppingCartRegistrationVo>>> GetListWithPageAsync(DateTime? startDate, DateTime? endDate, int? LiveAnchorId, bool? isCreateOrder, int? createBy, bool? isSendOrder, bool? isAddWechat, bool? isWriteOff, bool? isConsultation, bool? isReturnBackPrice, string keyword, string contentPlatFormId, int pageNum, int pageSize, decimal? minPrice, decimal? maxPrice, int? assignEmpId, DateTime? startRefundTime, DateTime? endRefundTime, DateTime? startBadReviewTime, DateTime? endBadReviewTime, int? ShoppingCartRegistrationCustomerType, int? emergencyLevel, bool? isBadReview, string baseLiveAnchorId, int? source, int? belongChannel)
        {
            try
            {
                var employee = httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                var q = await shoppingCartRegistrationService.GetListWithPageAsync(startDate, endDate, LiveAnchorId, isCreateOrder, createBy, isSendOrder, employeeId, isAddWechat, isWriteOff, isConsultation, isReturnBackPrice, keyword, contentPlatFormId, pageNum, pageSize, minPrice, maxPrice, assignEmpId, startRefundTime, endRefundTime, startBadReviewTime, endBadReviewTime, ShoppingCartRegistrationCustomerType, emergencyLevel, isBadReview, baseLiveAnchorId, source, belongChannel);

                var shoppingCartRegistration = from d in q.List
                                               select new ShoppingCartRegistrationVo
                                               {
                                                   Id = d.Id,
                                                   RecordDate = d.RecordDate,
                                                   ContentPlatFormName = d.ContentPlatFormName,
                                                   LiveAnchorName = d.LiveAnchorName,
                                                   CustomerNickName = d.CustomerNickName,
                                                   HiddenPhone = d.HiddenPhone,
                                                   Price = d.Price,
                                                   Remark = d.Remark,
                                                   CreateBy = d.CreateByName,
                                                   AssignEmpName = d.AssignEmpName,
                                                   EmergencyLevelText = ServiceClass.GetShopCartRegisterEmergencyLevelText(d.EmergencyLevel),
                                                   SourceText = d.SourceText,
                                                   ShoppingCartRegistrationCustomerTypeText = d.ShoppingCartRegistrationCustomerTypeText,
                                                   GetCustomerTypeText = d.GetCustomerTypeText,
                                                   BelongChannelName = d.BelongChannelName,
                                                   CustomerWechatNo=d.CustomerWechatNo,
                                                   //LiveAnchorWechatNo = d.LiveAnchorWechatNo,
                                                   //Phone = d.Phone,
                                                   //EncryptPhone = d.EncryptPhone,
                                                   //SubPhone = d.SubPhone,
                                                   //HiddenSubPhone = d.HiddenSubPhone,
                                                   //EncryptSubPhone = d.EncryptSubPhone,
                                                   //IsCreateOrder = d.IsCreateOrder,
                                                   //IsSendOrder = d.IsSendOrder,
                                                   //ConsultationType = d.ConsultationType,
                                                   //ConsultationTypeText = d.ConsultationTypeText,
                                                   //IsWriteOff = d.IsWriteOff,
                                                   //IsConsultation = d.IsConsultation,
                                                   //ConsultationDate = d.ConsultationDate,
                                                   //IsAddWeChat = d.IsAddWeChat,
                                                   //IsReturnBackPrice = d.IsReturnBackPrice,
                                                   //CreateDate = d.CreateDate,
                                                   //IsReContent = d.IsReContent,
                                                   //ReContent = d.ReContent,
                                                   //RefundReason = d.RefundReason,
                                                   //BadReviewContent = d.BadReviewContent,
                                                   //BadReviewReason = d.BadReviewReason,
                                                   //BadReviewDate = d.BadReviewDate == null ? null : d.BadReviewDate,
                                                   //RefundDate = d.RefundDate,
                                                   //IsBadReview = d.IsBadReview,
                                                   //EmergencyLevel = d.EmergencyLevel,
                                                   //Source = d.Source,
                                                   //ShoppingCartRegistrationCustomerType = d.ShoppingCartRegistrationCustomerType,
                                                   //ProductType = d.ProductType,
                                                   //ProductTypeText = d.ProductTypeText,
                                                   //BaseLiveAnchorId = d.BaseLiveAnchorId,
                                                   //BaseLiveAnchorName = d.BaseLiveAnchorName,
                                                   //GetCustomerType = d.GetCustomerType,
                                                   //BelongChannel = d.BelongChannel,
                                                   //IsRiBuLuoLiving = d.IsRiBuLuoLiving
                                               };

                FxPageInfo<ShoppingCartRegistrationVo> shoppingCartRegistrationPageInfo = new FxPageInfo<ShoppingCartRegistrationVo>();
                shoppingCartRegistrationPageInfo.TotalCount = q.TotalCount;
                shoppingCartRegistrationPageInfo.List = shoppingCartRegistration;

                return ResultData<FxPageInfo<ShoppingCartRegistrationVo>>.Success().AddData("shoppingCartRegistrationInfo", shoppingCartRegistrationPageInfo);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<ShoppingCartRegistrationVo>>.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 添加小黄车登记信息
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData> AddAsync(AddShoppingCartRegistrationVo addVo)
        {
            try
            {
                //var isExistPhone = await shoppingCartRegistrationService.GetByPhoneAsync(addVo.Phone);
                //if (!string.IsNullOrEmpty(isExistPhone.Id))
                //{
                //    throw new Exception("已存在该客户手机号，无法录入，请重新填写！");
                //}

                var employee = httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                AddShoppingCartRegistrationDto addDto = new AddShoppingCartRegistrationDto();
                addDto.RecordDate = addVo.RecordDate;
                addDto.ContentPlatFormId = addVo.ContentPlatFormId;
                addDto.LiveAnchorId = addVo.LiveAnchorId;
                addDto.GetCustomerType = addVo.GetCustomerType;
                addDto.LiveAnchorWechatNo = addVo.LiveAnchorWechatNo;
                addDto.CustomerNickName = addVo.CustomerNickName;
                addDto.Phone = addVo.Phone;
                addDto.SubPhone = addVo.SubPhone;
                addDto.Price = addVo.Price;
                addDto.ConsultationType = addVo.ConsultationType;
                addDto.ShoppingCartRegistrationCustomerType = addVo.ShoppingCartRegistrationCustomerType;
                addDto.IsWriteOff = addVo.IsWriteOff;
                addDto.IsAddWeChat = addVo.IsAddWeChat;
                addDto.ConsultationDate = addVo.ConsultationDate;
                addDto.IsReturnBackPrice = addVo.IsReturnBackPrice;
                addDto.Remark = addVo.Remark;
                addDto.CreateBy = employeeId;
                addDto.AssignEmpId = addVo.AssignEmpId;
                addDto.ReContent = addVo.ReContent;
                addDto.IsReContent = addVo.IsReContent;
                addDto.RefundDate = addVo.RefundDate;
                addDto.RefundReason = addVo.RefundReason;
                addDto.BadReviewContent = addVo.BadReviewContent;
                addDto.BadReviewDate = addVo.BadReviewDate;
                addDto.BadReviewReason = addVo.BadReviewReason;
                addDto.IsBadReview = addVo.IsBadReview;
                addDto.EmergencyLevel = addVo.EmergencyLevel;
                addDto.Source = addVo.Source;
                addDto.ProductType = addVo.ProductType;
                addDto.IsConsultation = addVo.IsConsultation;
                addDto.BelongChannel = addVo.BelongChannel;
                addDto.AddWechatPicture = addVo.AddWechatPicture;
                addDto.CluePicture = addVo.CluePicture;
                addDto.IsRiBuLuoLiving = addVo.IsRiBuLuoLiving;
                addDto.CustomerWechatNo = addVo.CustomerWechatNo;
                addDto.FromTitle = addVo.FromTitle;
                addDto.IsRepeateCreateOrder = addVo.IsRepeateCreateOrder;
                var contentPlatFormOrder = await contentPlateFormOrderService.GetOrderListByPhoneAsync(addVo.Phone);
                var isSendOrder = contentPlatFormOrder.Where(x => x.OrderStatus != (int)ContentPlateFormOrderStatus.HaveOrder).Count();
                if (contentPlatFormOrder.Count > 0)
                {
                    addDto.IsCreateOrder = true;
                }
                if (isSendOrder > 0)
                {
                    addDto.IsSendOrder = true;
                }

                await shoppingCartRegistrationService.AddAsync(addDto);

                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 指派小黄车登记信息
        /// </summary>
        /// <param name="assignVo"></param>
        /// <returns></returns>
        [HttpPut("assign")]
        public async Task<ResultData> AssignAsync(AssignVo assignVo)
        {
            try
            {
                await shoppingCartRegistrationService.AssignAsync(assignVo.Id, assignVo.AssignBy);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 删除小黄车登记信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultData> DeleteAsync(string id)
        {
            OperationAddDto operationLog = new OperationAddDto();
            try
            {
                var employee = httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
                int employeeId = Convert.ToInt32(employee.Id);
                operationLog.OperationBy = employeeId;
                var data = await shoppingCartRegistrationService.GetByIdAsync(id);
                operationLog.Parameters = JsonConvert.SerializeObject(data);
                await shoppingCartRegistrationService.DeleteAsync(id);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                operationLog.Message = ex.Message;
                operationLog.Code = -1;
                return ResultData.Fail(ex.Message);
            }
            finally
            {
                operationLog.Source = (int)RequestSource.AmiyaBusinessWechat;
                operationLog.RequestType = (int)RequestType.Update;
                operationLog.RouteAddress = httpContextAccessor.HttpContext.Request.Path;
                await operationLogService.AddOperationLogAsync(operationLog);
            }
        }

        /// <summary>
        /// 根据小黄车登记手机号获取小黄车登记信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="liveAnchorId">主播IP账户id</param>
        /// <returns></returns>
        [HttpGet("byPhoneAndLiveAnchorId")]
        public async Task<ResultData<ShoppingCartRegistrationVo>> GetByPhoneAndLiveAnchorIdAsync(string phone, int liveAnchorId)
        {
            try
            {
                var shoppingCartRegistration = await shoppingCartRegistrationService.GetAddOrderPriceByPhoneAndLiveAnchorIdAsync(phone, liveAnchorId);
                ShoppingCartRegistrationVo shoppingCartRegistrationVo = new ShoppingCartRegistrationVo();
                shoppingCartRegistrationVo.Id = shoppingCartRegistration.Id;
                shoppingCartRegistrationVo.Price = shoppingCartRegistration.Price;

                return ResultData<ShoppingCartRegistrationVo>.Success().AddData("shoppingCartRegistrationInfo", shoppingCartRegistrationVo);
            }
            catch (Exception ex)
            {
                return ResultData<ShoppingCartRegistrationVo>.Fail(ex.Message);
            }
        }

        #region 【枚举下拉框】
        /// <summary>
        /// 客户来源列表（短视频，直播间等）
        /// </summary>
        /// <returns></returns>
        [HttpGet("customerSourceList")]
        public async Task<ResultData<List<BaseKeyAndValueVo<int>>>> GetCustomerSourceListAsync(string contentPlatFormId, int? channel)
        {
            var nameList = shoppingCartRegistrationService.GetCustomerSourceList(contentPlatFormId, channel);
            var result = nameList.Select(e => new BaseKeyAndValueVo<int>
            {
                Id = e.Key,
                Name = e.Value
            }).ToList();
            return ResultData<List<BaseKeyAndValueVo<int>>>.Success().AddData("sourceList", result);

        }
        /// <summary>
        /// 客户类型列表（医美/带货顾客）
        /// </summary>
        /// <returns></returns>
        [HttpGet("customerTypeList")]
        public async Task<ResultData<List<BaseKeyAndValueVo<int>>>> GetCustomerTypeListAsync()
        {
            var nameList = shoppingCartRegistrationService.GetCustomerTypeList();
            var result = nameList.Select(e => new BaseKeyAndValueVo<int>
            {
                Id = e.Key,
                Name = e.Value
            }).ToList();
            return ResultData<List<BaseKeyAndValueVo<int>>>.Success().AddData("sourceList", result);

        }
        /// <summary>
        /// 获取归属部门列表（直播前、中、后）
        /// </summary>
        /// <returns></returns>
        [HttpGet("shoppingCartGetBelongChannelList")]
        public async Task<ResultData<List<BaseIdAndNameVo<int>>>> GetBelongChannelListAsync()
        {
            var nameList = shoppingCartRegistrationService.GetBelongDepartmentList();
            var result = nameList.Select(e => new BaseIdAndNameVo<int>
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            return ResultData<List<BaseIdAndNameVo<int>>>.Success().AddData("belongChannelList", result);
        }

        /// <summary>
        /// 输出线索类型列表（一级线索，二级线索，三级线索）
        /// </summary>
        /// <returns></returns>
        [HttpGet("emergencyLevels")]
        public ResultData<List<EmergencyLevelVo>> GetEmergencyLevel()
        {
            var emergencyLevel = from d in shoppingCartRegistrationService.GetEmergencyLevelList()
                                 select new EmergencyLevelVo
                                 {
                                     EmergencyLevel = d.EmergencyLevel,
                                     EmergencyLevelText = d.EmergencyText
                                 };
            return ResultData<List<EmergencyLevelVo>>.Success().AddData("emergencyLevels", emergencyLevel.ToList());
        }


        /// <summary>
        /// 获客方式列表（自主获客、组内分诊）
        /// </summary>
        /// <returns></returns>
        [HttpGet("shoppingCartGetCustomerTypeList")]
        public async Task<ResultData<List<BaseIdAndNameVo<int>>>> GetShoppingCartGetCustomerTypeListAsync()
        {
            var nameList = shoppingCartRegistrationService.GetShoppingCartGetCustomerTypeText();
            var result = nameList.Select(e => new BaseIdAndNameVo<int>
            {
                Id = e.Key,
                Name = e.Value
            }).ToList();
            return ResultData<List<BaseIdAndNameVo<int>>>.Success().AddData("typeList", result);
        }

        /// <summary>
        /// 获取带货产品类型列表(彩妆、护肤、饰品、其他)---选择“客户来源”为“产品转化”时使用
        /// </summary>
        /// <returns></returns>
        [HttpGet("shoppingCartTakeGoodsProductTypeList")]
        public async Task<ResultData<List<BaseIdAndNameVo<int>>>> GetShoppingCartTakeGoodsProductTypeListAsync()
        {
            var nameList = shoppingCartRegistrationService.GetShoppingCartTakeGoodsProductTypeList();
            var result = nameList.Select(e => new BaseIdAndNameVo<int>
            {
                Id = e.Key,
                Name = e.Value
            }).ToList();
            return ResultData<List<BaseIdAndNameVo<int>>>.Success().AddData("sourceList", result);

        }
        #endregion
    }
}
