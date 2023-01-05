﻿using Fx.Amiya.Background.Api.Vo;
using Fx.Amiya.Background.Api.Vo.ExpressInfo;
using Fx.Amiya.Background.Api.Vo.RecommendHospital;
using Fx.Amiya.Background.Api.Vo.ReconciliationDocuments;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.IService;
using Fx.Authorization.Attributes;
using Fx.Common;
using Fx.Open.Infrastructure.Web;
using Jd.Api.Util;
using jos_sdk_net.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Controllers
{
    /// <summary>
    /// 财务对账单（仿美呗版）板块数据接口
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ReconciliationDocumentsController : ControllerBase
    {
        private IReconciliationDocumentsService reconciliationDocumentsService;
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reconciliationDocumentsService"></param>
        public ReconciliationDocumentsController(
            IHttpContextAccessor httpContextAccessor,
             IReconciliationDocumentsService reconciliationDocumentsService
            )
        {
            this.reconciliationDocumentsService = reconciliationDocumentsService;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// 获取财务对账单信息列表(管理端与机构端公用)
        /// </summary>
        /// <param name="returnBackPricePercent">返款比例</param>
        /// <param name="reconciliationState">对账单状态（0：已提交，1:待确认,2:问题账单,3:对账完成，4：回款完成）</param>
        /// <param name="startDate">创建时间（开始）</param>
        /// <param name="endDate">创建时间（结束）</param>
        /// <param name="startDealDate">成交时间（开始）</param>
        /// <param name="endDealDate">成交时间（结束）</param>
        /// <param name="keyword">关键词（客户姓名，手机号）</param>
        /// <param name="hospitalId">医院id（空值查询所有医院）</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<FxPageInfo<ReconciliationDocumentsVo>>> GetListAsync(decimal? returnBackPricePercent, int? reconciliationState, DateTime? startDate, DateTime? endDate, DateTime? startDealDate, DateTime? endDealDate, string keyword, int? hospitalId, int pageNum, int pageSize)
        {
            try
            {
                var q = await reconciliationDocumentsService.GetListWithPageAsync(returnBackPricePercent, reconciliationState, startDate, endDate, startDealDate, endDealDate, keyword, hospitalId, pageNum, pageSize);

                var reconciliationDocuments = from d in q.List
                                              select new ReconciliationDocumentsVo
                                              {
                                                  Id = d.Id,
                                                  HospitalId = d.HospitalId,
                                                  HospitalName = d.HospitalName,
                                                  CustomerName = d.CustomerName,
                                                  CustomerPhone = d.CustomerPhone,
                                                  DealGoods = d.DealGoods,
                                                  DealDate = d.DealDate,
                                                  TotalDealPrice = d.TotalDealPrice,
                                                  ReturnBackPricePercent = d.ReturnBackPricePercent,
                                                  SystemUpdatePricePercent = d.SystemUpdatePricePercent,
                                                  QuestionReason = d.QuestionReason,
                                                  Remark = d.Remark,
                                                  ReconciliationState = d.ReconciliationState,
                                                  ReconciliationStateText = d.ReconciliationStateText,
                                                  CreateBy = d.CreateBy,
                                                  CreateByName = d.CreateByName,
                                                  CreateDate = d.CreateDate,
                                                  UpdateDate = d.UpdateDate,
                                                  DeleteDate = d.DeleteDate,
                                                  Valid = d.Valid,
                                                  ReturnBackPrice = d.TotalDealPrice * d.ReturnBackPricePercent / 100,
                                                  SystemUpdatePrice = d.TotalDealPrice * d.SystemUpdatePricePercent / 100,
                                                  ReturnBackTotalPrice = (d.SystemUpdatePricePercent + d.ReturnBackPricePercent) * d.TotalDealPrice / 100
                                              };

                FxPageInfo<ReconciliationDocumentsVo> reconciliationDocumentsResult = new FxPageInfo<ReconciliationDocumentsVo>();
                reconciliationDocumentsResult.List = reconciliationDocuments.ToList();
                reconciliationDocumentsResult.TotalCount = q.TotalCount;
                reconciliationDocumentsResult.PageSize = pageSize;
                reconciliationDocumentsResult.CurrentPageIndex = pageNum;
                return ResultData<FxPageInfo<ReconciliationDocumentsVo>>.Success().AddData("reconciliationDocumentsInfo", reconciliationDocumentsResult);
            }
            catch (Exception ex)
            {
                return ResultData<FxPageInfo<ReconciliationDocumentsVo>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 财务对账单模板导出（管理端）
        /// </summary>
        /// <param name="returnBackPricePercent">返款比例</param>
        /// <param name="reconciliationState">对账单状态（0：已提交，1:待确认,2:问题账单,3:对账完成，4：回款完成）</param>
        /// <param name="startDate">创建时间（开始）</param>
        /// <param name="endDate">创建时间（结束）</param>
        /// <param name="startDealDate">成交时间（开始）</param>
        /// <param name="endDealDate">成交时间（结束）</param>
        /// <param name="keyword">关键词（客户姓名，手机号）</param>
        /// <param name="hospitalId">医院id（空值查询所有医院）</param>
        /// <returns></returns>
        [HttpGet("internalExportReconciliationDocuments")]
        [FxInternalAuthorize]
        public async Task<FileStreamResult> InternalxportReconciliationDocuments(decimal? returnBackPricePercent, int? reconciliationState, DateTime? startDate, DateTime? endDate, DateTime? startDealDate, DateTime? endDealDate, string keyword, int? hospitalId)
        {
            if (!startDate.HasValue && !endDate.HasValue)
            { throw new Exception("请选择时间进行查询"); }
            if (startDate.HasValue && endDate.HasValue)
            {
                if ((endDate.Value - startDate.Value).TotalDays > 31)
                {
                    throw new Exception("开始时间与结束时间不能超过一个月，请重新选择后再进行查询！");
                }
            }
            var res = new List<ReconciliationDocumentsVo>();
            var q = await reconciliationDocumentsService.ExportListWithPageAsync(returnBackPricePercent, reconciliationState, startDate, endDate, startDealDate, endDealDate, keyword, hospitalId);

            var reconciliationDocuments = from d in q
                                          select new ReconciliationDocumentsVo
                                          {
                                              Id = d.Id,
                                              HospitalId = d.HospitalId,
                                              HospitalName = d.HospitalName,
                                              CustomerName = d.CustomerName,
                                              CustomerPhone = d.CustomerPhone,
                                              DealGoods = d.DealGoods,
                                              DealDate = d.DealDate,
                                              TotalDealPrice = d.TotalDealPrice,
                                              ReturnBackPricePercent = d.ReturnBackPricePercent,
                                              SystemUpdatePricePercent = d.SystemUpdatePricePercent,
                                              QuestionReason = d.QuestionReason,
                                              Remark = d.Remark,
                                              ReconciliationState = d.ReconciliationState,
                                              ReconciliationStateText = d.ReconciliationStateText,
                                              CreateBy = d.CreateBy,
                                              CreateByName = d.CreateByName,
                                              CreateDate = d.CreateDate,
                                              UpdateDate = d.UpdateDate,
                                              DeleteDate = d.DeleteDate,
                                              Valid = d.Valid,
                                              ReturnBackPrice = d.TotalDealPrice * d.ReturnBackPricePercent / 100,
                                              SystemUpdatePrice = d.TotalDealPrice * d.SystemUpdatePricePercent / 100,
                                              ReturnBackTotalPrice = (d.SystemUpdatePricePercent + d.ReturnBackPricePercent) * d.TotalDealPrice / 100
                                          };

            res = reconciliationDocuments.ToList();
            var stream = ExportExcelHelper.ExportExcel(res);
            var result = File(stream, "application/vnd.ms-excel", $"" + startDate.Value.ToString("yyyy年MM月dd日") + "-" + endDate.Value.ToString("yyyy年MM月dd日") + "财务对账单.xls");
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            return result;
        }

        /// <summary>
        /// 添加财务对账单信息（机构端）
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [FxTenantAuthorize]
        [HttpPost]
        public async Task<ResultData> AddAsync(AddReconciliationDocumentsVo addVo)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaHospitalEmployeeIdentity;
                int hospitalEmpId = Convert.ToInt32(employee.Id);
                int hospitalId = employee.HospitalId;
                List<AddReconciliationDocumentsDto> AddReconciliationDocumentsDtoList = new List<AddReconciliationDocumentsDto>();
                AddReconciliationDocumentsDto addDto = new AddReconciliationDocumentsDto();
                addDto.HospitalId = hospitalId;
                addDto.CustomerName = addVo.CustomerName;
                addDto.CustomerPhone = addVo.CustomerPhone;
                addDto.DealGoods = addVo.DealGoods;
                addDto.DealDate = addVo.DealDate;
                addDto.TotalDealPrice = addVo.TotalDealPrice;
                addDto.ReturnBackPricePercent = addVo.ReturnBackPricePercent;
                addDto.SystemUpdatePricePercent = addVo.SystemUpdatePricePercent;
                addDto.Remark = addVo.Remark;
                addDto.ReconciliationState = (int)ReconciliationDocumentsStateEnum.Submited;
                addDto.CreateBy = hospitalEmpId;
                AddReconciliationDocumentsDtoList.Add(addDto);
                await reconciliationDocumentsService.AddAsync(AddReconciliationDocumentsDtoList);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 根据财务对账单编号获取财务对账单信息（管理端与机构端公用）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData<ReconciliationDocumentsVo>> GetByIdAsync(string id)
        {
            try
            {
                var reconciliationDocuments = await reconciliationDocumentsService.GetByIdAsync(id);
                ReconciliationDocumentsVo reconciliationDocumentsVo = new ReconciliationDocumentsVo();
                reconciliationDocumentsVo.Id = reconciliationDocuments.Id;
                reconciliationDocumentsVo.HospitalId = reconciliationDocuments.HospitalId;
                reconciliationDocumentsVo.CustomerName = reconciliationDocuments.CustomerName;
                reconciliationDocumentsVo.CustomerPhone = reconciliationDocuments.CustomerPhone;
                reconciliationDocumentsVo.DealGoods = reconciliationDocuments.DealGoods;
                reconciliationDocumentsVo.DealDate = reconciliationDocuments.DealDate;
                reconciliationDocumentsVo.TotalDealPrice = reconciliationDocuments.TotalDealPrice;
                reconciliationDocumentsVo.ReturnBackPricePercent = reconciliationDocuments.ReturnBackPricePercent;
                reconciliationDocumentsVo.SystemUpdatePricePercent = reconciliationDocuments.SystemUpdatePricePercent;
                reconciliationDocumentsVo.Remark = reconciliationDocuments.Remark;
                reconciliationDocumentsVo.ReconciliationState = reconciliationDocuments.ReconciliationState;
                reconciliationDocumentsVo.CreateBy = reconciliationDocuments.CreateBy;

                return ResultData<ReconciliationDocumentsVo>.Success().AddData("reconciliationDocumentsInfo", reconciliationDocumentsVo);
            }
            catch (Exception ex)
            {
                return ResultData<ReconciliationDocumentsVo>.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 修改财务对账单信息（机构端）
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        [FxTenantAuthorize]
        public async Task<ResultData> UpdateAsync(UpdateReconciliationDocumentsVo updateVo)
        {
            try
            {
                var employee = _httpContextAccessor.HttpContext.User as FxAmiyaHospitalEmployeeIdentity;
                int hospitalId = employee.HospitalId;
                int hospitalEmpId = Convert.ToInt32(employee.Id);
                UpdateReconciliationDocumentsDto updateDto = new UpdateReconciliationDocumentsDto();

                updateDto.Id = updateVo.Id;
                updateDto.HospitalId = hospitalId;
                updateDto.CustomerName = updateVo.CustomerName;
                updateDto.CustomerPhone = updateVo.CustomerPhone;
                updateDto.DealGoods = updateVo.DealGoods;
                updateDto.DealDate = updateVo.DealDate;
                updateDto.TotalDealPrice = updateVo.TotalDealPrice;
                updateDto.ReturnBackPricePercent = updateVo.ReturnBackPricePercent;
                updateDto.SystemUpdatePricePercent = updateVo.SystemUpdatePricePercent;
                updateDto.Remark = updateVo.Remark;
                updateDto.CreateBy = hospitalEmpId;
                await reconciliationDocumentsService.UpdateAsync(updateDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }



        /// <summary>
        /// 标记对账单状态（管理端与机构端公用）
        /// </summary>
        /// <param name="tagReconciliationStateVo"></param>
        /// <returns></returns>
        [HttpPut("tagReconciliationState")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData> TagReconciliationStateAsync(TagReconciliationStateVo tagReconciliationStateVo)
        {
            try
            {
                await reconciliationDocumentsService.TagReconciliationStateAsync(tagReconciliationStateVo.IdList, tagReconciliationStateVo.ReconciliationState, tagReconciliationStateVo.QuestionReason);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }
        /// <summary>
        /// 对账单批量回款
        /// </summary>
        /// <param name="reconciliationDocumentsReturnBackPriceVo"></param>
        /// <returns></returns>
        [HttpPut("reconciliationReturnBackPriceList")]
        [FxInternalOrTenantAuthroize]
        public async Task<ResultData> ReconciliationReturnBackPriceListAsync(ReconciliationDocumentsReturnBackPriceVo reconciliationDocumentsReturnBackPriceVo)
        {
            try
            {
                ReconciliationDocumentsReturnBackPriceDto reconciliationDocumentsReturnBackPriceDto = new ReconciliationDocumentsReturnBackPriceDto();
                reconciliationDocumentsReturnBackPriceDto.ReconciliationDocumentsIdList = reconciliationDocumentsReturnBackPriceVo.ReconciliationDocumentsIdList;
                reconciliationDocumentsReturnBackPriceDto.ReturnBackDate = reconciliationDocumentsReturnBackPriceVo.ReturnBackDate;
                await reconciliationDocumentsService.TagReconciliationStateAsync(reconciliationDocumentsReturnBackPriceDto);
                return ResultData.Success();
            }
            catch (Exception ex)
            {
                return ResultData.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 删除财务对账单信息（机构端）
        /// </summary>
        /// <param name="deleteVo"></param>
        /// <returns></returns>
        [HttpDelete]
        [FxTenantAuthorize]
        public async Task<ResultData> DeleteAsync(DeleteReconciliationDocumentsVo deleteVo)
        {
            try
            {
                await reconciliationDocumentsService.DeleteAsync(deleteVo.IdList);
                return ResultData.Success();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 财务对账单模板导出（机构端）
        /// </summary>
        /// <returns></returns>
        [HttpGet("exportReconciliationDocuments")]
        [FxTenantAuthorize]
        public async Task<FileStreamResult> exportReconciliationDocuments()
        {
            var res = new List<AddReconciliationDocumentsVo>();
            var exportOrderWriteOff = res.ToList();
            var stream = ExportExcelHelper.ExportExcel(exportOrderWriteOff);
            var result = File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"财务对账单模板.xlsx");
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            return result;
        }

        /// <summary>
        /// 导入财务对账单（机构端）
        /// </summary>
        /// <returns></returns>
        [HttpPut("importReconciliationDocuments")]
        [FxTenantAuthorize]
        public async Task<ResultData> ReconciliationDocumentsInPortAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                throw new Exception("请检查文件是否存在");
            var employee = _httpContextAccessor.HttpContext.User as FxAmiyaHospitalEmployeeIdentity;
            int hospitalId = employee.HospitalId;
            int hospitalEmpId = Convert.ToInt32(employee.Id);
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);//取到文件流

                using (ExcelPackage package = new ExcelPackage(stream))
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets["sheet1"];
                    if (worksheet == null)
                    {
                        throw new Exception("请另外新建一个excel文件'.xlsx'后将填写好的数据复制到新文件中上传，勿采用当前导出文件进行上传！");
                    }
                    //获取表格的列数和行数
                    int rowCount = worksheet.Dimension.Rows;
                    List<AddReconciliationDocumentsDto> addDtoList = new List<AddReconciliationDocumentsDto>();
                    for (int x = 2; x <= rowCount; x++)
                    {
                        AddReconciliationDocumentsDto addDto = new AddReconciliationDocumentsDto();
                        if (!string.IsNullOrEmpty(worksheet.Cells[x, 1].Value.ToString()))
                        {
                            addDto.CustomerName = worksheet.Cells[x, 1].Value.ToString();
                        }
                        else
                        {
                            throw new Exception("客户姓名有参数列为空，请检查表格数据！");
                        }
                        if (worksheet.Cells[x, 2].Value != null)
                        {
                            addDto.CustomerPhone = worksheet.Cells[x, 2].Value.ToString();
                        }
                        else
                        {
                            throw new Exception("客户电话有参数列为空，请检查表格数据！");
                        }
                        if (worksheet.Cells[x, 3].Value != null)
                        {
                            addDto.DealGoods = worksheet.Cells[x, 3].Value.ToString();
                        }
                        else
                        {
                            throw new Exception("成交项目有参数列为空，请检查表格数据！");
                        }
                        if (worksheet.Cells[x, 4].Value != null)
                        {
                            int tempValue;
                            if (int.TryParse(worksheet.Cells[x, 4].Value.ToString(), out tempValue))
                            {
                                var dealDate = DateTime.FromOADate(double.Parse(worksheet.Cells[x, 4].Value.ToString()));
                                addDto.DealDate = dealDate;
                            }
                            else
                            {
                                addDto.DealDate = Convert.ToDateTime(worksheet.Cells[x, 4].Value.ToString());
                            }
                        }
                        else
                        {
                            throw new Exception("成交时间有参数列为空，请检查表格数据！");
                        }
                        if (worksheet.Cells[x, 5].Value != null)
                        {
                            addDto.TotalDealPrice = Convert.ToInt32(worksheet.Cells[x, 5].Value.ToString());
                        }
                        else
                        {
                            throw new Exception("总成交金额有参数列为空，请检查表格数据！");
                        }
                        if (worksheet.Cells[x, 6].Value != null)
                        {
                            addDto.ReturnBackPricePercent = Convert.ToDecimal(worksheet.Cells[x, 6].Value.ToString());
                        }
                        else
                        {
                            throw new Exception("返款比例有参数列为空，请检查表格数据！");
                        }
                        if (worksheet.Cells[x, 7].Value != null)
                        {
                            addDto.SystemUpdatePricePercent = Convert.ToDecimal(worksheet.Cells[x, 7].Value.ToString());
                        }
                        else
                        {
                            throw new Exception("系统维护费比例有参数列为空，请检查表格数据！");
                        }
                        if (worksheet.Cells[x, 8].Value != null)
                        {
                            addDto.Remark = worksheet.Cells[x, 8].Value.ToString();
                        }
                        addDto.HospitalId = hospitalId;
                        addDto.CreateBy = hospitalEmpId;
                        addDtoList.Add(addDto);
                    }
                    await reconciliationDocumentsService.AddAsync(addDtoList);
                }
            }
            return ResultData.Success();


        }
    }
}
