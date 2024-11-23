using Fx.Amiya.DbModels.Model;
using Fx.Amiya.Dto;
using Fx.Amiya.Dto.Bill;
using Fx.Amiya.Dto.FinancialBoard;
using Fx.Amiya.Dto.ReconciliationDocuments;
using Fx.Amiya.Dto.UpdateCreateBillAndCompany;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Fx.Infrastructure.DataAccess;
using jos_sdk_net.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Service
{
    public class BillService : IBillService
    {
        private readonly IDalBill dalBill;
        private readonly IUnitOfWork unitOfWork;
        private IReconciliationDocumentsService reconciliationDocumentsService;
        private IRecommandDocumentSettleService recommandDocumentSettleService;
        private IAmiyaEmployeeService amiyaEmployeeService;
        private IContentPlatFormOrderDealInfoService contentPlatFormOrderDealInfoService;
        private IBillReturnBackPriceDataService billReturnBackPriceDataService;
        private IOrderService orderService;
        private IContentPlateFormOrderService contentPlateFormOrderService;
        private ICustomerHospitalConsumeService customerHospitalConsumeService;
        private ILiveAnchorService liveAnchorService;
        private readonly IDalHospitalInfo dalHospitalInfo;
        private readonly IDalCompanyBaseInfo dalCompanyBaseInfo;
        private readonly IDalRecommandDocumentSettle dalRecommandDocumentSettle;

        public BillService(IDalBill dalBill,
            IReconciliationDocumentsService reconciliationDocumentsService,
            IContentPlatFormOrderDealInfoService contentPlatFormOrderDealInfoService,
            IRecommandDocumentSettleService recommandDocumentSettleService,
            ILiveAnchorService liveAnchorService,
            IBillReturnBackPriceDataService billReturnBackPriceDataService,
            IAmiyaEmployeeService amiyaEmployeeService,
            IUnitOfWork unitOfWork, IOrderService orderService, IContentPlateFormOrderService contentPlateFormOrderService, ICustomerHospitalConsumeService customerHospitalConsumeService, IDalHospitalInfo dalHospitalInfo, IDalCompanyBaseInfo dalCompanyBaseInfo, IDalRecommandDocumentSettle dalRecommandDocumentSettle)
        {
            this.dalBill = dalBill;
            this.unitOfWork = unitOfWork;
            this.reconciliationDocumentsService = reconciliationDocumentsService;
            this.billReturnBackPriceDataService = billReturnBackPriceDataService;
            this.orderService = orderService;
            this.recommandDocumentSettleService = recommandDocumentSettleService;
            this.liveAnchorService = liveAnchorService;
            this.amiyaEmployeeService = amiyaEmployeeService;
            this.contentPlateFormOrderService = contentPlateFormOrderService;
            this.contentPlatFormOrderDealInfoService = contentPlatFormOrderDealInfoService;
            this.customerHospitalConsumeService = customerHospitalConsumeService;
            this.dalHospitalInfo = dalHospitalInfo;
            this.dalCompanyBaseInfo = dalCompanyBaseInfo;
            this.dalRecommandDocumentSettle = dalRecommandDocumentSettle;
        }




        /// <summary>
        /// 根据条件获取发票信息
        /// </summary>
        /// <param name="keyWord">关键词（可搜索费用备注，开票事由）</param>
        /// <param name="hospitalId">客户id</param>
        /// <param name="billType">票据类型（医美/其他）</param>
        /// <param name="returnBackState"></param>
        /// <param name="companyId">回款状态（未回款/回款中/已回款）</param>
        /// <param name="valid">是否作废（1正常，0作废）</param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<BillDto>> GetListAsync(DateTime? startDate, DateTime? endDate, int? hospitalId, bool? valid, int? billType, int? returnBackState, string companyId, string keyWord, int pageNum, int pageSize)
        {
            startDate = startDate.HasValue ? startDate.Value.Date : DateTime.Now.Date;
            endDate = endDate.HasValue ? endDate.Value.AddDays(1).Date : DateTime.Now.AddDays(1).Date;
            var bills = from d in dalBill.GetAll().Include(x => x.AmiyaEmployee).Include(x => x.HospitalInfo).Include(x => x.CompanyBaseInfo)
                        where (string.IsNullOrWhiteSpace(keyWord) || d.OtherPriceRemark.Contains(keyWord) || d.CreateBillReason.Contains(keyWord) || d.Id.Contains(keyWord))
                        && (!hospitalId.HasValue || d.HospitalId == hospitalId.Value)
                        && (string.IsNullOrEmpty(companyId) || d.CollectionCompanyId == companyId)
                        && (!billType.HasValue || d.BillType == billType.Value)
                        && (!returnBackState.HasValue || d.ReturnBackState == returnBackState.Value)
                        && (!valid.HasValue || d.Valid == valid.Value)
                        && (d.CreateDate >= startDate && d.CreateDate < endDate)
                        select new BillDto
                        {
                            Id = d.Id,
                            HospitalId = d.HospitalId,
                            HospitalName = d.HospitalInfo.Name,
                            BillPrice = d.BillPrice,
                            TaxRate = d.TaxRate,
                            TaxPrice = d.TaxPrice,
                            NotInTaxPrice = d.NotInTaxPrice,
                            OtherPrice = d.OtherPrice,
                            OtherPriceRemark = d.OtherPriceRemark,
                            CollectionCompanyId = d.CollectionCompanyId,
                            CollectionCompanyName = d.CompanyBaseInfo.Name,
                            BelongStartTime = d.BelongStartTime,
                            BelongEndTime = d.BelongEndTime,
                            BillType = d.BillType,
                            BillTypeText = ServiceClass.GetBillTypeText(d.BillType),
                            CreateBillReason = d.CreateBillReason,
                            ReturnBackState = d.ReturnBackState,
                            ReturnBackStateText = ServiceClass.GetBillReturnBackStateText(d.ReturnBackState),
                            ReturnBackPrice = d.ReturnBackPrice,
                            CreateDate = d.CreateDate,
                            CreateBy = d.CreateBy,
                            CreateByEmployeeName = d.AmiyaEmployee.Name,
                            UpdateDate = d.UpdateDate,
                            Valid = d.Valid,
                            ValidText = d.Valid == true ? "正常" : "作废",
                            DeleteDate = d.DeleteDate,
                            ReturnBackPriceDate = d.ReturnBackPriceDate
                        };
            FxPageInfo<BillDto> billPageInfo = new FxPageInfo<BillDto>();
            billPageInfo.TotalCount = await bills.CountAsync();
            billPageInfo.List = await bills.OrderByDescending(x => x.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return billPageInfo;
        }

        /// <summary>
        /// 根据条件获取发票信息
        /// </summary>
        /// <param name="keyWord">关键词（可搜索费用备注，开票事由）</param>
        /// <param name="hospitalId">客户id</param>
        /// <param name="billType">票据类型（医美/其他）</param>
        /// <param name="returnBackState"></param>
        /// <param name="companyId">回款状态（未回款/回款中/已回款）</param>
        /// <param name="valid">是否作废（1正常，0作废）</param>
        /// <returns></returns>
        public async Task<List<ExportBillDto>> ExportBillListAsync(DateTime? startDate, DateTime? endDate, int? hospitalId, bool? valid, int? billType, int? returnBackState, string companyId, string keyWord)
        {
            startDate = startDate.HasValue ? startDate.Value.Date : DateTime.Now.Date;
            endDate = endDate.HasValue ? endDate.Value.AddDays(1).Date : DateTime.Now.AddDays(1).Date;
            var bills = from d in dalBill.GetAll().Include(x => x.AmiyaEmployee).Include(x => x.HospitalInfo).Include(x => x.CompanyBaseInfo)
                        where (string.IsNullOrWhiteSpace(keyWord) || d.OtherPriceRemark.Contains(keyWord) || d.CreateBillReason.Contains(keyWord))
                        && (!hospitalId.HasValue || d.HospitalId == hospitalId.Value)
                        && (string.IsNullOrEmpty(companyId) || d.CollectionCompanyId == companyId)
                        && (!billType.HasValue || d.BillType == billType.Value)
                        && (!returnBackState.HasValue || d.ReturnBackState == returnBackState.Value)
                        && (!valid.HasValue || d.Valid == valid.Value)
                        && (d.CreateDate >= startDate && d.CreateDate < endDate)
                        select new ExportBillDto
                        {
                            Id = d.Id,
                            HospitalId = d.HospitalId,
                            HospitalName = d.HospitalInfo.Name,
                            BillPrice = d.BillPrice,
                            TaxRate = d.TaxRate,
                            TaxPrice = d.TaxPrice,
                            NotInTaxPrice = d.NotInTaxPrice,
                            OtherPrice = d.OtherPrice,
                            OtherPriceRemark = d.OtherPriceRemark,
                            CollectionCompanyId = d.CollectionCompanyId,
                            CollectionCompanyName = d.CompanyBaseInfo.Name,
                            BelongStartTime = d.BelongStartTime,
                            BelongEndTime = d.BelongEndTime,
                            BillType = d.BillType,
                            BillTypeText = ServiceClass.GetBillTypeText(d.BillType),
                            CreateBillReason = d.CreateBillReason,
                            ReturnBackState = d.ReturnBackState,
                            ReturnBackStateText = ServiceClass.GetBillReturnBackStateText(d.ReturnBackState),
                            ReturnBackPrice = d.ReturnBackPrice,
                            CreateDate = d.CreateDate,
                            CreateBy = d.CreateBy,
                            CreateByEmployeeName = d.AmiyaEmployee.Name,
                            UpdateDate = d.UpdateDate,
                            Valid = d.Valid,
                            ValidText = d.Valid == true ? "正常" : "作废",
                            DeleteDate = d.DeleteDate,
                            ReturnBackPriceDate = d.ReturnBackPriceDate
                        };
            return await bills.ToListAsync();
        }

        /// <summary>
        /// 添加发票
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddBillDto addDto)
        {
            unitOfWork.BeginTransaction();
            try
            {
                List<string> billIdList = new List<string>();
                var billBaseId = CreateOrderIdHelper.GetBillNextNumber();
                int billCount = 1;
                if (addDto.BillPrice < addDto.Details.Sum(x => x.SeparateBillPrice))
                {
                    throw new Exception("开票金额不能大于发票总金额，请核对后输入");
                }
                foreach (var x in addDto.Details)
                {
                    Bill bill = new Bill();
                    if (addDto.Details.Count > 1)
                    {
                        bill.Id = billBaseId + "0" + billCount;
                    }
                    else
                    {
                        bill.Id = billBaseId;
                    }
                    bill.DealPrice = addDto.DealPrice;
                    bill.InformationPrice = addDto.InformationPrice;
                    bill.SystemUpdatePrice = addDto.SystemUpdatePrice;
                    bill.HospitalId = addDto.HospitalId;
                    bill.BillPrice = x.SeparateBillPrice;
                    bill.TaxRate = x.TaxRate;
                    bill.TaxPrice = x.TaxPrice;
                    bill.NotInTaxPrice = x.NotInTaxPrice;
                    bill.OtherPrice = x.OtherPrice;
                    bill.OtherPriceRemark = x.OtherPriceRemark;
                    bill.CollectionCompanyId = x.CollectionCompanyId;
                    bill.BelongStartTime = addDto.BelongStartTime;
                    bill.BelongEndTime = addDto.BelongEndTime;
                    bill.BillType = addDto.BillType;
                    bill.CreateBillReason = addDto.CreateBillReason;
                    bill.ReturnBackState = (int)BillReturnBackStateTextEnum.UnReturnBack;
                    bill.CreateBy = addDto.CreateBy;
                    bill.CreateDate = addDto.CreateDate;
                    bill.Valid = true;
                    await dalBill.AddAsync(bill, true);
                    billIdList.Add(bill.Id);
                    billCount++;
                }
                if (addDto.BillType == (int)BillTypeTextEnum.BeautyClinic)
                {
                    //调用对账单表更新是否开票接口
                    ReconciliationDocumentsCreateBillDto reconciliationDocumentsCreateBillDto = new ReconciliationDocumentsCreateBillDto();
                    reconciliationDocumentsCreateBillDto.BillId = billIdList[0];
                    if (billIdList.Count > 1)
                    {
                        //加载第二个发票编号
                        reconciliationDocumentsCreateBillDto.BillId2 = billIdList[1];
                    }
                    reconciliationDocumentsCreateBillDto.ReconciliationDocumentsIdList = addDto.ReconciliationDocumentsIdList;
                    reconciliationDocumentsCreateBillDto.IsCreateBill = true;
                    await reconciliationDocumentsService.ReconciliationDocumentsCreateBillAsync(reconciliationDocumentsCreateBillDto);

                    //调用对账单表更新内容平台,成交信息,三方订单,升单 是否开票及开票公司
                    var settleList = await reconciliationDocumentsService.GetRecommandDocumentSettleListAsync(addDto.ReconciliationDocumentsIdList);
                    foreach (var item in settleList)
                    {
                        if (item.OrderFrom == (int)OrderFrom.ThirdPartyOrder)
                        {
                            UpdateCreateBillAndCompanyDto update = new UpdateCreateBillAndCompanyDto();
                            update.OrderId = item.OrderId;
                            update.IsCreateBill = true;
                            update.CreateBillCompanyId = addDto.Details[0].CollectionCompanyId;
                            await orderService.UpdateCreateBillAndBelongCompany(update);
                        }
                        if (item.OrderFrom == (int)OrderFrom.ContentPlatFormOrder)
                        {
                            UpdateCreateBillAndCompanyDto update = new UpdateCreateBillAndCompanyDto();
                            update.OrderId = item.OrderId;
                            update.OrderDetailInfoId = item.DealInfoId;
                            update.IsCreateBill = true;
                            update.CreateBillCompanyId = addDto.Details[0].CollectionCompanyId;
                            await contentPlateFormOrderService.UpdateCreateBillAndBelongCompany(update);
                        }
                        if (item.OrderFrom == (int)OrderFrom.BuyAgainOrder)
                        {
                            UpdateCreateBillAndCompanyDto update = new UpdateCreateBillAndCompanyDto();
                            update.OrderId = item.OrderId;
                            update.IsCreateBill = true;
                            update.CreateBillCompanyId = addDto.Details[0].CollectionCompanyId;
                            await customerHospitalConsumeService.UpdateCreateBillAndBelongCompany(update);
                        }
                    }

                }
                unitOfWork.Commit();
            }
            catch (Exception err)
            {
                unitOfWork.RollBack();
                throw new Exception(err.ToString());
            }
        }



        public async Task<BillDto> GetByIdAsync(string id)
        {
            var result = await dalBill.GetAll().Include(x => x.HospitalInfo).Include(x => x.AmiyaEmployee).Include(x => x.CompanyBaseInfo).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return new BillDto();
            }

            BillDto returnResult = new BillDto();
            returnResult.Id = result.Id;
            returnResult.HospitalId = result.HospitalId;
            returnResult.HospitalName = result.HospitalInfo.Name;
            returnResult.BillPrice = result.BillPrice;
            returnResult.TaxRate = result.TaxRate;
            returnResult.TaxPrice = result.TaxPrice;
            returnResult.NotInTaxPrice = result.NotInTaxPrice;
            returnResult.OtherPrice = result.OtherPrice;
            returnResult.OtherPriceRemark = result.OtherPriceRemark;
            returnResult.CollectionCompanyId = result.CollectionCompanyId;
            returnResult.CollectionCompanyName = result.CompanyBaseInfo.Name;
            returnResult.BelongStartTime = result.BelongStartTime;
            returnResult.BelongEndTime = result.BelongEndTime;
            returnResult.BillType = result.BillType;
            returnResult.BillTypeText = ServiceClass.GetBillTypeText(result.BillType);
            returnResult.CreateBillReason = result.CreateBillReason;
            returnResult.ReturnBackState = result.ReturnBackState;
            returnResult.ReturnBackStateText = ServiceClass.GetBillReturnBackStateText(result.ReturnBackState);
            returnResult.CreateDate = result.CreateDate;
            returnResult.CreateBy = result.CreateBy;
            returnResult.CreateByEmployeeName = result.AmiyaEmployee.Name;
            returnResult.UpdateDate = result.UpdateDate;
            returnResult.Valid = result.Valid;
            returnResult.ValidText = result.Valid == true ? "正常" : "作废";
            returnResult.DeleteDate = result.DeleteDate;

            var reconciliationDocuments = await reconciliationDocumentsService.GetByBillIdListAsync(id);
            returnResult.ReconciliationDocumentsDtoList = reconciliationDocuments;

            return returnResult;
        }



        /// <summary>
        /// 修改发票
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateBillDto updateDto)
        {
            var result = await dalBill.GetAll().Where(x => x.Id == updateDto.Id && x.Valid == true).FirstOrDefaultAsync();
            if (result == null)
                throw new Exception("未找到发票信息");
            result.HospitalId = updateDto.HospitalId;
            result.BillPrice = updateDto.BillPrice;
            result.TaxRate = updateDto.TaxRate;
            result.TaxPrice = updateDto.TaxPrice;
            result.NotInTaxPrice = updateDto.NotInTaxPrice;
            result.OtherPrice = updateDto.OtherPrice;
            result.OtherPriceRemark = updateDto.OtherPriceRemark;
            result.CollectionCompanyId = updateDto.CollectionCompanyId;
            result.BelongStartTime = updateDto.BelongStartTime;
            result.BelongEndTime = updateDto.BelongEndTime;
            result.BillType = updateDto.BillType;
            result.CreateDate = updateDto.CreateBillDate;
            result.CreateBillReason = updateDto.CreateBillReason;
            result.UpdateDate = DateTime.Now;
            await dalBill.UpdateAsync(result, true);
        }

        /// <summary>
        /// 作废发票
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            unitOfWork.BeginTransaction();
            try
            {

                var result = await dalBill.GetAll().SingleOrDefaultAsync(e => e.Id == id && e.Valid == true);
                if (result == null)
                    throw new Exception("未找到发票信息");
                result.Valid = false;
                await dalBill.UpdateAsync(result, true);

                if (result.BillType == (int)BillTypeTextEnum.BeautyClinic)
                {

                    var reconciliationDocuments = await reconciliationDocumentsService.GetByBillIdListAsync(id);
                    var reconciliationDocumentsList = reconciliationDocuments.Select(x => x.Id).ToList();

                    //调用对账单表更新内容平台,成交信息,三方订单,升单 是否开票及开票公司
                    var settleList = await reconciliationDocumentsService.GetRecommandDocumentSettleListAsync(reconciliationDocumentsList);
                    foreach (var item in settleList)
                    {
                        if (item.OrderFrom == (int)OrderFrom.ThirdPartyOrder)
                        {
                            UpdateCreateBillAndCompanyDto update = new UpdateCreateBillAndCompanyDto();
                            update.OrderId = item.OrderId;
                            update.IsCreateBill = false;
                            update.CreateBillCompanyId = "";
                            await orderService.UpdateCreateBillAndBelongCompany(update);
                        }
                        if (item.OrderFrom == (int)OrderFrom.ContentPlatFormOrder)
                        {
                            UpdateCreateBillAndCompanyDto update = new UpdateCreateBillAndCompanyDto();
                            update.OrderId = item.OrderId;
                            update.OrderDetailInfoId = item.DealInfoId;
                            update.IsCreateBill = false;
                            update.CreateBillCompanyId = "";
                            await contentPlateFormOrderService.UpdateCreateBillAndBelongCompany(update);
                        }
                        if (item.OrderFrom == (int)OrderFrom.BuyAgainOrder)
                        {
                            UpdateCreateBillAndCompanyDto update = new UpdateCreateBillAndCompanyDto();
                            update.OrderId = item.OrderId;
                            update.IsCreateBill = false;
                            update.CreateBillCompanyId = "";
                            await customerHospitalConsumeService.UpdateCreateBillAndBelongCompany(update);
                        }
                    }

                    //调用对账单表更新是否开票接口
                    ReconciliationDocumentsCreateBillDto reconciliationDocumentsCreateBillDto = new ReconciliationDocumentsCreateBillDto();
                    reconciliationDocumentsCreateBillDto.BillId = "";
                    reconciliationDocumentsCreateBillDto.ReconciliationDocumentsIdList = reconciliationDocuments.Select(x => x.Id).ToList();
                    reconciliationDocumentsCreateBillDto.IsCreateBill = false;
                    await reconciliationDocumentsService.ReconciliationDocumentsCreateBillAsync(reconciliationDocumentsCreateBillDto);
                }
                unitOfWork.Commit();
            }
            catch (Exception er)
            {
                unitOfWork.RollBack();
                throw new Exception(er.Message.ToString());
            }
        }

        /// <summary>
        /// 发票回款
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task ReturnBakcPriceAsync(BillReturnBackPriceDto updateDto)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var result = await dalBill.GetAll().Where(x => x.Id == updateDto.Id && x.Valid == true).FirstOrDefaultAsync();
                result.ReturnBackState = (int)BillReturnBackStateTextEnum.ReturnBacking;
                if (result == null)
                    throw new Exception("未找到发票信息");
                decimal billTotalPrice = result.BillPrice;
                if (result.OtherPrice.HasValue)
                {
                    billTotalPrice += result.OtherPrice.Value;
                }
                if (result.ReturnBackPrice.HasValue)
                {
                    result.ReturnBackPrice += updateDto.ReturnBackPrice;
                }
                else
                {
                    result.ReturnBackPrice = updateDto.ReturnBackPrice;
                }
                if (result.ReturnBackPrice == billTotalPrice)
                {
                    result.ReturnBackState = (int)BillReturnBackStateTextEnum.ReturnBackSuccessful;

                    //调用对账单表回款业务（todo;）
                    ReconciliationDocumentsReturnBackPriceDto reconciliationDocumentsReturnBackPriceDto = new ReconciliationDocumentsReturnBackPriceDto();
                    reconciliationDocumentsReturnBackPriceDto.ReturnBackDate = updateDto.ReturnBackDate;
                    var reconciliationDocuments = await reconciliationDocumentsService.GetByBillIdListAsync(updateDto.Id);
                    reconciliationDocumentsReturnBackPriceDto.ReconciliationDocumentsIdList = reconciliationDocuments.Select(x => x.Id).ToList();
                    await reconciliationDocumentsService.TagReconciliationStateAsync(reconciliationDocumentsReturnBackPriceDto);
                }
                if (result.ReturnBackPrice > billTotalPrice)
                {
                    throw new Exception("当前回款金额与已回款金额累计（" + result.ReturnBackPrice + "元）不能大于发票金额与其他费用的总和（" + billTotalPrice + "）！");
                }
                result.UpdateDate = DateTime.Now;
                result.ReturnBackPriceDate = updateDto.ReturnBackDate;
                await dalBill.UpdateAsync(result, true);

                //回款记录表插入数据(todo;)
                AddBillReturnBackPriceDataDto addBillReturnBackPriceDataDto = new AddBillReturnBackPriceDataDto();
                addBillReturnBackPriceDataDto.HospitalId = result.HospitalId;
                addBillReturnBackPriceDataDto.CompanyId = result.CollectionCompanyId;
                addBillReturnBackPriceDataDto.BillId = result.Id;
                addBillReturnBackPriceDataDto.BillPrice = result.BillPrice;
                addBillReturnBackPriceDataDto.OtherPrice = result.OtherPrice;
                addBillReturnBackPriceDataDto.ReturnBackPrice = updateDto.ReturnBackPrice;
                addBillReturnBackPriceDataDto.ReturnBackDate = updateDto.ReturnBackDate;
                addBillReturnBackPriceDataDto.ReturnBackState = result.ReturnBackState;
                addBillReturnBackPriceDataDto.CreateBy = updateDto.CreateBy;
                addBillReturnBackPriceDataDto.Remark = updateDto.Remark;
                await billReturnBackPriceDataService.AddAsync(addBillReturnBackPriceDataDto);
                unitOfWork.Commit();
            }
            catch (Exception err)
            {
                unitOfWork.RollBack();
                throw new Exception(err.Message.ToString());
            }
        }


        #region [对账单审核记录]

        /// <summary>
        /// 审核助理薪资数据
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>

        public async Task CheckReconciliationDocumentsSettleAsync(CheckReconciliationDocumentSettleDto checkDto)
        {
            await recommandDocumentSettleService.CheckAsync(checkDto);
        }
        /// <summary>
        /// 批量审核助理/行政客服的自播达人薪资数据
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        public async Task BatchCheckReconciliationDocumentsSettleAsync(BatchCheckReconciliationDocumentSettleDto checkDto)
        {
            if (checkDto.CheckType != (int)ReconciliationDocumentSettleCheckType.SelfLiveAnchorCheck)
                throw new Exception("审核类型只能为自播达人");
            var employee = await amiyaEmployeeService.GetByIdAsync(checkDto.CheckBelongEmpId.Value);
            //if (employee.PositionId != 4)
            //    throw new Exception("该员工不属于客服管理员,不能批量审核!");
            var checkRecordDataList = dalRecommandDocumentSettle.GetAll()
                 .Where(e => checkDto.IdList.Contains(e.Id))
                 .Select(e => new
                 {
                     Id = e.Id,
                     IsOldCustomer = e.IsOldCustomer,
                     CustomerServiceSettlePrice = e.CustomerServiceSettlePrice,
                     BelongEmpId = e.BelongEmpId,
                     CreateEmpId = e.CreateEmpId,
                     ContentPlatFormOrderAddOrderPrice = e.ContentPlatFormOrderAddOrderPrice
                 }).ToList();
            if (checkRecordDataList.Count() != checkDto.IdList.Count())
                throw new Exception("编号错误");
            if (checkRecordDataList.Where(e => e.BelongEmpId != e.CreateEmpId).Count() > 0)
                throw new Exception("所选数据包含稽查业绩,审核失败!");
            if (checkRecordDataList.Where(e => e.BelongEmpId != checkDto.CheckBelongEmpId).Count() > 0)
                throw new Exception("所选数据包含归属于其他客服的数据,审核失败");
            try
            {
                unitOfWork.BeginTransaction();
                foreach (var item in checkRecordDataList)
                {
                    CheckReconciliationDocumentSettleDto checkData = new CheckReconciliationDocumentSettleDto();
                    checkData.Id = item.Id;
                    checkData.CheckState = checkDto.CheckState;
                    checkData.CheckBy = checkDto.CheckBy;
                    checkData.CheckType = checkDto.CheckType;
                    checkData.IsInspectPerformance = checkDto.IsInspectPerformance;
                    checkData.CheckRemark = checkDto.CheckRemark;
                    checkData.CustomerServiceOrderPerformance = item.CustomerServiceSettlePrice;
                    checkData.CheckBelongEmpId = checkDto.CheckBelongEmpId;
                    if (item.IsOldCustomer)
                    {
                        checkData.PerformancePercent = employee.OldCustomerCommission ?? 0m;
                    }
                    else
                    {
                        checkData.PerformancePercent = item.ContentPlatFormOrderAddOrderPrice > 0 ? employee.NewCustomerCommission ?? 0m : employee.PotentialNewCustomerCommission ?? 0m;
                    }
                    checkData.CustomerServicePerformance = Math.Round(checkData.CustomerServiceOrderPerformance * checkData.PerformancePercent / 100, 2, MidpointRounding.AwayFromZero);
                    await recommandDocumentSettleService.CheckAsync(checkData);
                }
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw ex;
            }
        }


        /// <summary>
        /// 批量审核合作达人薪资数据
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        public async Task BatchCheckCooperationLiveAnchorsReconciliationDocumentsSettleAsync(BatchCheckReconciliationDocumentSettleDto checkDto)
        {
            if (checkDto.CheckType != (int)ReconciliationDocumentSettleCheckType.CooperationLiveAnchorCheck)
                throw new Exception("审核类型只能为合作达人");
            var employee = await amiyaEmployeeService.GetByIdAsync(checkDto.CheckBelongEmpId.Value);
            var checkRecordDataList = dalRecommandDocumentSettle.GetAll()
                 .Where(e => checkDto.IdList.Contains(e.Id))
                 .Select(e => new
                 {
                     Id = e.Id,
                     IsOldCustomer = e.IsOldCustomer,
                     CustomerServiceSettlePrice = e.RecolicationPrice,
                     BelongEmpId = e.BelongEmpId,
                     CreateEmpId = e.CreateEmpId,
                     ContentPlatFormOrderAddOrderPrice = e.ContentPlatFormOrderAddOrderPrice
                 }).ToList();
            if (checkRecordDataList.Count() != checkDto.IdList.Count())
                throw new Exception("编号错误");
            if (checkRecordDataList.Where(e => e.CreateEmpId != checkDto.CheckBelongEmpId).Count() > 0)
                throw new Exception("所选数据包含归属于其他客服的数据,审核失败");
            try
            {
                //定义外达人所占业绩比例（10%）
                decimal CooperationLiveAnchorPercent = 0.1M;
                unitOfWork.BeginTransaction();
                foreach (var item in checkRecordDataList)
                {
                    CheckReconciliationDocumentSettleDto checkData = new CheckReconciliationDocumentSettleDto();
                    checkData.Id = item.Id;
                    checkData.CheckState = checkDto.CheckState;
                    checkData.CheckBy = checkDto.CheckBy;
                    checkData.CheckType = checkDto.CheckType;
                    checkData.IsInspectPerformance = checkDto.IsInspectPerformance;
                    checkData.CheckRemark = checkDto.CheckRemark;
                    checkData.CustomerServiceOrderPerformance = Math.Round(item.CustomerServiceSettlePrice.Value * CooperationLiveAnchorPercent, 2, MidpointRounding.AwayFromZero);
                    checkData.CheckBelongEmpId = checkDto.CheckBelongEmpId;
                    if (item.IsOldCustomer)
                    {
                        checkData.PerformancePercent = employee.CooperateLiveanchorOldCustomerCommission;
                    }
                    else
                    {
                        checkData.PerformancePercent = employee.CooperateLiveanchorNewCustomerCommission;
                    }
                    checkData.CustomerServicePerformance = Math.Round(checkData.CustomerServiceOrderPerformance * checkData.PerformancePercent / 100, 2, MidpointRounding.AwayFromZero);
                    await recommandDocumentSettleService.CheckAsync(checkData);
                }
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw ex;
            }
        }
        /// <summary>
        /// 批量审核财务稽查数据
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        public async Task BatchCheckFinanceReconciliationDocumentsSettleAsync(BatchCheckFinanceReconciliationDocumentSettleDto checkDto)
        {
            var checkRecordDataList = dalRecommandDocumentSettle.GetAll()
                 .Where(e => checkDto.IdList.Contains(e.Id))
                 .Select(e => new
                 {
                     Id = e.Id,
                     IsOldCustomer = e.IsOldCustomer,
                     CustomerServiceSettlePrice = e.CustomerServiceSettlePrice,
                     BelongEmpId = e.BelongEmpId,
                     CreateEmpId = e.CreateEmpId,
                     ContentPlatFormOrderAddOrderPrice = e.ContentPlatFormOrderAddOrderPrice,
                     OrderFrom = e.OrderFrom,
                     LiveAnchorId = e.BelongLiveAnchorAccount
                 }).ToList();
            var haveOtherContentplatform = checkRecordDataList.Where(e => e.OrderFrom != (int)OrderFrom.ContentPlatFormOrder).Any();
            if (haveOtherContentplatform)
                throw new Exception("只能批量审核内容平台数据!");
            if (checkRecordDataList.Count() != checkDto.IdList.Count())
                throw new Exception("编号错误!");
            var financialIdList = (await amiyaEmployeeService.GetFinancialNameListAsync()).Select(e => e.Id).ToList();
            if (!financialIdList.Contains(checkDto.FinanceId))
                throw new Exception("所选员工非财务");
            if (checkRecordDataList.Any(e => e.CreateEmpId != checkDto.FinanceId))
                throw new Exception("所选数据包含上传人非财务数据或其他财务数据,审核失败!");
            var currentFinancial = await amiyaEmployeeService.GetByIdAsync(checkDto.FinanceId);
            if (checkRecordDataList.Any(e => e.LiveAnchorId == null))
                throw new Exception("所选数据包含未绑定主播数据,审核失败!");
            try
            {
                unitOfWork.BeginTransaction();
                foreach (var item in checkRecordDataList)
                {
                    var isSelfLiveAnchor = await liveAnchorService.IsBelongSelfLiveAnchorAsync(item.LiveAnchorId.Value);
                    var employee = await amiyaEmployeeService.GetByIdAsync(item.BelongEmpId.Value);
                    CheckReconciliationDocumentSettleDto checkData = new CheckReconciliationDocumentSettleDto();
                    checkData.Id = item.Id;
                    checkData.CheckState = checkDto.CheckState;
                    checkData.CheckBy = checkDto.CheckBy;
                    checkData.IsInspectPerformance = true;
                    checkData.CheckRemark = checkDto.CheckRemark;
                    checkData.CustomerServiceOrderPerformance = item.CustomerServiceSettlePrice;
                    checkData.CheckBelongEmpId = item.BelongEmpId;
                    checkData.PerformancePercent = employee.InspectionCommission.Value;
                    checkData.CustomerServicePerformance = Math.Round(checkData.CustomerServiceOrderPerformance * checkData.PerformancePercent / 100, 2, MidpointRounding.AwayFromZero);
                    #region 稽查

                    checkData.InspectEmpId = item.CreateEmpId;
                    checkData.InspectPercent = currentFinancial.AdministrativeInspectionCommission;
                    var inspectionCommission = 0m;
                    if (isSelfLiveAnchor)
                    {
                        checkData.CheckType = (int)ReconciliationDocumentSettleCheckType.SelfLiveAnchorCheck;
                        if (item.IsOldCustomer)
                        {
                            inspectionCommission = employee.OldCustomerCommission.Value;
                        }
                        else
                        {
                            inspectionCommission = item.ContentPlatFormOrderAddOrderPrice > 0 ? employee.NewCustomerCommission.Value : employee.PotentialNewCustomerCommission.Value;
                        }
                    }
                    else
                    {
                        checkData.CheckType = (int)ReconciliationDocumentSettleCheckType.CooperationLiveAnchorCheck;
                        if (item.IsOldCustomer)
                        {
                            inspectionCommission = employee.CooperateLiveanchorOldCustomerCommission;
                        }
                        else
                        {
                            inspectionCommission = employee.CooperateLiveanchorNewCustomerCommission;
                        }
                    }
                    checkData.InspectPrice = Math.Round(checkData.CustomerServiceOrderPerformance * (inspectionCommission > 5 ? 5 : inspectionCommission) / 100 * currentFinancial.AdministrativeInspectionCommission / 100, 2, MidpointRounding.AwayFromZero);

                    #endregion
                    await recommandDocumentSettleService.CheckAsync(checkData);
                }
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw ex;
            }
        }
        /// <summary>
        /// 批量审核消费追踪薪资数据
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        public async Task BatchCheckConsumptionRrackingReconciliationDocumentsSettleAsync(BatchCheckReconciliationDocumentSettleDto checkDto)
        {
            if (checkDto.CheckType != (int)ReconciliationDocumentSettleCheckType.OtherPlatformBuyAgainCheck)
                throw new Exception("审核类型只能为三方订单升单审核");
            var employee = await amiyaEmployeeService.GetByIdAsync(checkDto.CheckBelongEmpId.Value);
            if (employee.PositionId == 13)
            {
                if (checkDto.CheckBelongEmpId != 128)
                    throw new Exception("财务上传数据最终归属客服应为啊美雅");
            }
            var checkRecordDataList = dalRecommandDocumentSettle.GetAll()
                 .Where(e => checkDto.IdList.Contains(e.Id))
                 .Select(e => new
                 {
                     Id = e.Id,
                     IsOldCustomer = e.IsOldCustomer,
                     CustomerServiceSettlePrice = e.RecolicationPrice,
                     BelongEmpId = e.BelongEmpId,
                     CreateEmpId = e.CreateEmpId,
                     ContentPlatFormOrderAddOrderPrice = e.ContentPlatFormOrderAddOrderPrice,
                     OrderFrom = e.OrderFrom
                 }).ToList();
            if (checkRecordDataList.Where(e => e.OrderFrom != (int)OrderFrom.BuyAgainOrder).Count() > 0)
                throw new Exception("所选订单来源包含非消费追踪数据");
            if (checkRecordDataList.Count() != checkDto.IdList.Count())
                throw new Exception("编号错误");
            if (checkRecordDataList.Where(e => e.BelongEmpId != e.CreateEmpId).Count() > 0)
                throw new Exception("所选数据包含稽查业绩,审核失败!");
            if (checkRecordDataList.Where(e => e.BelongEmpId != checkDto.CheckBelongEmpId).Count() > 0)
                throw new Exception("所选数据包含归属于其他客服的数据,审核失败");
            try
            {
                unitOfWork.BeginTransaction();
                decimal OtherPlatformBuyAgain = 0.25m;
                foreach (var item in checkRecordDataList)
                {
                    CheckReconciliationDocumentSettleDto checkData = new CheckReconciliationDocumentSettleDto();
                    checkData.Id = item.Id;
                    checkData.CheckState = checkDto.CheckState;
                    checkData.CheckBy = checkDto.CheckBy;
                    checkData.CheckType = checkDto.CheckType;
                    checkData.IsInspectPerformance = checkDto.IsInspectPerformance;
                    checkData.CheckRemark = checkDto.CheckRemark;
                    checkData.CustomerServiceOrderPerformance = (item.CustomerServiceSettlePrice ?? 0m) * OtherPlatformBuyAgain;
                    checkData.CheckBelongEmpId = checkDto.CheckBelongEmpId;
                    checkData.PerformancePercent = employee.TmallOrderCommission;
                    checkData.CustomerServicePerformance = Math.Round((checkData.CustomerServiceOrderPerformance) * (checkData.PerformancePercent / 100), 2, MidpointRounding.AwayFromZero);
                    await recommandDocumentSettleService.CheckAsync(checkData);
                }
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw ex;
            }
        }

        /// <summary>
        /// 分页获取审核记录数据
        /// </summary>
        /// <param name="isSettle"></param>
        /// <param name="accountType"></param>
        /// <param name="chooseHospitalId">选中医院id（0查询）维多连天美之外的</param>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<RecommandDocumentSettleDto>> GetSettleListByPageAsync(DateTime? startDate, DateTime? endDate, bool? isSettle, bool? accountType, int chooseHospitalId, string keyword, int pageNum, int pageSize)
        {
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1);
            }
            var record = await recommandDocumentSettleService.GetAllAsync(startDate, endDate, isSettle, accountType, chooseHospitalId, keyword);

            FxPageInfo<RecommandDocumentSettleDto> fxPageInfo = new FxPageInfo<RecommandDocumentSettleDto>();
            var selectResult = record.OrderByDescending(x => x.CreateDate).ToList();
            List<RecommandDocumentSettleDto> recommandDocumentSettleDtos = new List<RecommandDocumentSettleDto>();
            foreach (var x in selectResult)
            {
                RecommandDocumentSettleDto recommandDocumentSettleDto = new RecommandDocumentSettleDto();
                recommandDocumentSettleDto.RecommandDocumentId = x.RecommandDocumentId;
                recommandDocumentSettleDto.CustomerServiceSettlePrice = x.CustomerServiceSettlePrice;
                recommandDocumentSettleDto.CreateDate = x.CreateDate;
                if (!string.IsNullOrEmpty(x.RecommandDocumentId) && x.RecommandDocumentId != "string")
                {
                    var reconciliationDocumentsInfo = await reconciliationDocumentsService.GetByIdAsync(x.RecommandDocumentId);
                    if (!string.IsNullOrEmpty(reconciliationDocumentsInfo.BillId))
                    {
                        var bill = await this.GetByIdAsync(reconciliationDocumentsInfo.BillId);
                        recommandDocumentSettleDto.IsCerateBill = true;
                        recommandDocumentSettleDto.BelongCompany = bill.CollectionCompanyName;
                    }
                    if (!string.IsNullOrEmpty(reconciliationDocumentsInfo.BillId2))
                    {
                        var bill = await this.GetByIdAsync(reconciliationDocumentsInfo.BillId2);
                        recommandDocumentSettleDto.IsCerateBill = true;
                        recommandDocumentSettleDto.BelongCompany2 = bill.CollectionCompanyName;
                    }
                    recommandDocumentSettleDto.HospitalName = reconciliationDocumentsInfo.HospitalName;
                    recommandDocumentSettleDto.OrderId = x.OrderId;
                    recommandDocumentSettleDto.DealInfoId = x.DealInfoId;
                    recommandDocumentSettleDto.OrderFromText = x.OrderFromText;
                    recommandDocumentSettleDto.OrderPrice = x.OrderPrice;
                    recommandDocumentSettleDto.RecolicationPrice = x.RecolicationPrice;
                    recommandDocumentSettleDto.IsOldCustomerText = x.IsOldCustomerText;
                    recommandDocumentSettleDto.IsSettle = x.IsSettle;
                    recommandDocumentSettleDto.SettleDate = x.SettleDate;
                    recommandDocumentSettleDto.ReturnBackPrice = x.ReturnBackPrice;
                    recommandDocumentSettleDto.AccountPrice = x.AccountPrice;
                    recommandDocumentSettleDto.CreateByEmpName = x.CreateByEmpName;
                    recommandDocumentSettleDto.AccountTypeText = x.AccountTypeText;
                    if (reconciliationDocumentsInfo.ReturnBackPricePercent.HasValue)
                        recommandDocumentSettleDto.InformationPrice = Math.Round(x.RecolicationPrice.Value * reconciliationDocumentsInfo.ReturnBackPricePercent.Value / 100, 2, MidpointRounding.AwayFromZero);
                    if (reconciliationDocumentsInfo.SystemUpdatePricePercent.HasValue)
                        recommandDocumentSettleDto.SystemUpdatePrice = Math.Round(x.RecolicationPrice.Value * reconciliationDocumentsInfo.SystemUpdatePricePercent.Value / 100, 2, MidpointRounding.AwayFromZero);
                }
                if (x.BelongEmpId.HasValue)
                {
                    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.BelongEmpId.Value);
                    recommandDocumentSettleDto.BelongEmpName = empInfo.Name;
                }
                if (x.CreateEmpId.HasValue)
                {
                    if (x.CreateEmpId.Value == 0)
                    {
                        recommandDocumentSettleDto.CreateEmpName = "医院创建";
                    }
                    else
                    {
                        var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CreateEmpId.Value);
                        recommandDocumentSettleDto.CreateEmpName = empInfo.Name;
                    }
                }
                if (x.BelongLiveAnchorAccount.HasValue)
                {
                    var liveAnchor = await liveAnchorService.GetByIdAsync(x.BelongLiveAnchorAccount.Value);
                    recommandDocumentSettleDto.BelongLiveAnchor = liveAnchor.Name;
                }
                switch (x.OrderFrom)
                {
                    case (int)OrderFrom.ContentPlatFormOrder:

                        var dealInfo = await contentPlatFormOrderDealInfoService.GetByIdAsync(x.DealInfoId);
                        recommandDocumentSettleDto.DealDate = dealInfo.DealDate;
                        var contentPlatFormOrderInfo = await contentPlateFormOrderService.GetByOrderIdAsync(x.OrderId);
                        recommandDocumentSettleDto.GoodsName = contentPlatFormOrderInfo.GoodsName;
                        recommandDocumentSettleDto.Phone = contentPlatFormOrderInfo.Phone;
                        break;
                    case (int)OrderFrom.BuyAgainOrder:
                        var customerHospitalConsume = await customerHospitalConsumeService.GetByConsumeIdAsync(x.DealInfoId);
                        recommandDocumentSettleDto.DealDate = customerHospitalConsume.BuyAgainTime;
                        recommandDocumentSettleDto.GoodsName = customerHospitalConsume.ItemName;
                        recommandDocumentSettleDto.Phone = customerHospitalConsume.Phone;
                        break;
                    case (int)OrderFrom.ThirdPartyOrder:
                        var tmallOrder = await orderService.GetByIdInCRMAsync(x.OrderId);
                        recommandDocumentSettleDto.DealDate = tmallOrder.WriteOffDate;
                        recommandDocumentSettleDto.GoodsName = tmallOrder.GoodsName;
                        recommandDocumentSettleDto.Phone = tmallOrder.Phone;
                        break;
                }
                recommandDocumentSettleDtos.Add(recommandDocumentSettleDto);
            }
            fxPageInfo.List = recommandDocumentSettleDtos.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            fxPageInfo.TotalCount = recommandDocumentSettleDtos.Count();
            return fxPageInfo;
        }

        /// <summary>
        /// 分页获取审核记录数据
        /// </summary>
        /// <param name="isSettle"></param>
        /// <param name="accountType"></param>
        /// <param name="chooseHospitalId">选中医院id（0查询）维多连天美之外的</param>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<RecommandDocumentSettleDto>> GetSettleListWithPageByCustomerCompensationAsync(QueryReconciliationDocumentsSettleDto query)
        {
            var record = await recommandDocumentSettleService.GetListWithPageAsync(query);

            FxPageInfo<RecommandDocumentSettleDto> fxPageInfo = new FxPageInfo<RecommandDocumentSettleDto>();
            List<RecommandDocumentSettleDto> recommandDocumentSettleDtos = new List<RecommandDocumentSettleDto>();
            //string ids = "'";
            foreach (var x in record.List)
            {
                if (x.HospitalId != 0)
                {
                    x.HospitalName = dalHospitalInfo.GetAll().Where(e => e.Id == x.HospitalId).FirstOrDefault().Name;
                }
                if (x.BelongEmpId.HasValue)
                {
                    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.BelongEmpId.Value);
                    x.BelongEmpName = empInfo.Name;
                }
                if (x.InspectBy.HasValue)
                {
                    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.InspectBy.Value);
                    x.InspectEmpName = empInfo.Name;
                }
                if (x.CreateEmpId.HasValue)
                {
                    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CreateEmpId.Value);
                    x.CreateEmpName = empInfo.Name;
                }

                if (x.CheckBelongEmpId.HasValue)
                {
                    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CheckBelongEmpId.Value);
                    x.CheckBelongEmpName = empInfo.Name;
                }
                if (x.BelongLiveAnchorAccount.HasValue)
                {
                    var liveAnchor = await liveAnchorService.GetByIdAsync(x.BelongLiveAnchorAccount.Value);
                    x.BelongLiveAnchor = liveAnchor.Name;
                }
                switch (x.OrderFrom)
                {
                    case (int)OrderFrom.ContentPlatFormOrder:

                        var dealInfo = await contentPlatFormOrderDealInfoService.GetByIdAsync(x.DealInfoId);
                        x.DealDate = dealInfo.DealDate;
                        break;
                    case (int)OrderFrom.BuyAgainOrder:
                        var customerHospitalConsume = await customerHospitalConsumeService.GetByConsumeIdAsync(x.DealInfoId);
                        x.DealDate = customerHospitalConsume.BuyAgainTime;
                        break;
                    case (int)OrderFrom.ThirdPartyOrder:
                        var tmallOrder = await orderService.GetByIdInCRMAsync(x.OrderId);
                        x.DealDate = tmallOrder.WriteOffDate;
                        break;
                }
                //ids += x.DealInfoId + "','";
            }
            fxPageInfo.List = record.List;
            fxPageInfo.TotalCount = record.TotalCount;
            return fxPageInfo;
        }

        /// <summary>
        /// 分页获取审核记录数据
        /// </summary>
        /// <param name="isSettle"></param>
        /// <param name="accountType"></param>
        /// <param name="chooseHospitalId">选中医院id（0查询）维多连天美之外的</param>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<RecommandDocumentSettleDto>> ExportSettleListByPageAsync(DateTime? startDate, DateTime? endDate, bool? isSettle, bool? accountType, int chooseHospitalId, string keyword, bool isHidePhone)
        {
            if (endDate.HasValue)
            {
                endDate = endDate.Value.Date.AddDays(1);
            }
            var record = await recommandDocumentSettleService.GetAllAsync(startDate, endDate, isSettle, accountType, chooseHospitalId, keyword);

            var selectResult = record.OrderByDescending(x => x.CreateDate).ToList();
            List<RecommandDocumentSettleDto> recommandDocumentSettleDtos = new List<RecommandDocumentSettleDto>();
            foreach (var x in selectResult)
            {
                RecommandDocumentSettleDto recommandDocumentSettleDto = new RecommandDocumentSettleDto();
                recommandDocumentSettleDto.RecommandDocumentId = x.RecommandDocumentId;
                recommandDocumentSettleDto.CustomerServiceSettlePrice = x.CustomerServiceSettlePrice;
                recommandDocumentSettleDto.CreateDate = x.CreateDate;
                if (!string.IsNullOrEmpty(x.RecommandDocumentId) && x.RecommandDocumentId != "string")
                {
                    var reconciliationDocumentsInfo = await reconciliationDocumentsService.GetByIdAsync(x.RecommandDocumentId);
                    if (!string.IsNullOrEmpty(reconciliationDocumentsInfo.BillId))
                    {
                        var bill = await this.GetByIdAsync(reconciliationDocumentsInfo.BillId);
                        recommandDocumentSettleDto.IsCerateBill = true;
                        recommandDocumentSettleDto.BelongCompany = bill.CollectionCompanyName;
                    }
                    if (!string.IsNullOrEmpty(reconciliationDocumentsInfo.BillId2))
                    {
                        var bill = await this.GetByIdAsync(reconciliationDocumentsInfo.BillId2);
                        recommandDocumentSettleDto.IsCerateBill = true;
                        recommandDocumentSettleDto.BelongCompany2 = bill.CollectionCompanyName;
                    }
                    recommandDocumentSettleDto.HospitalName = reconciliationDocumentsInfo.HospitalName;
                    recommandDocumentSettleDto.OrderId = x.OrderId;
                    recommandDocumentSettleDto.DealInfoId = x.DealInfoId;
                    recommandDocumentSettleDto.OrderFromText = x.OrderFromText;
                    recommandDocumentSettleDto.OrderPrice = x.OrderPrice;
                    recommandDocumentSettleDto.RecolicationPrice = x.RecolicationPrice;
                    recommandDocumentSettleDto.IsOldCustomerText = x.IsOldCustomerText;
                    recommandDocumentSettleDto.IsSettle = x.IsSettle;
                    recommandDocumentSettleDto.SettleDate = x.SettleDate;
                    recommandDocumentSettleDto.ReturnBackPrice = x.ReturnBackPrice;
                    recommandDocumentSettleDto.AccountPrice = x.AccountPrice;
                    recommandDocumentSettleDto.CreateByEmpName = x.CreateByEmpName;
                    recommandDocumentSettleDto.AccountTypeText = x.AccountTypeText;
                    recommandDocumentSettleDto.InformationPrice = Math.Round(x.RecolicationPrice.Value * reconciliationDocumentsInfo.ReturnBackPricePercent.Value / 100, 2, MidpointRounding.AwayFromZero);
                    recommandDocumentSettleDto.SystemUpdatePrice = Math.Round(x.RecolicationPrice.Value * reconciliationDocumentsInfo.SystemUpdatePricePercent.Value / 100, 2, MidpointRounding.AwayFromZero);
                }
                if (x.BelongEmpId.HasValue)
                {
                    var empInfo = await amiyaEmployeeService.GetByIdAsync(x.BelongEmpId.Value);
                    recommandDocumentSettleDto.BelongEmpName = empInfo.Name;
                }
                if (x.CreateEmpId.HasValue)
                {
                    if (x.CreateEmpId.Value == 0)
                    {
                        recommandDocumentSettleDto.CreateEmpName = "医院创建";
                    }
                    else
                    {
                        var empInfo = await amiyaEmployeeService.GetByIdAsync(x.CreateEmpId.Value);
                        recommandDocumentSettleDto.CreateEmpName = empInfo.Name;
                    }
                }
                if (x.BelongLiveAnchorAccount.HasValue)
                {
                    var liveAnchor = await liveAnchorService.GetByIdAsync(x.BelongLiveAnchorAccount.Value);
                    recommandDocumentSettleDto.BelongLiveAnchor = liveAnchor.Name;
                }
                switch (x.OrderFrom)
                {
                    case (int)OrderFrom.ContentPlatFormOrder:

                        var dealInfo = await contentPlatFormOrderDealInfoService.GetByIdAsync(x.DealInfoId);
                        recommandDocumentSettleDto.DealDate = dealInfo.DealDate;
                        var contentPlatFormOrderInfo = await contentPlateFormOrderService.GetByOrderIdAsync(x.OrderId);
                        recommandDocumentSettleDto.GoodsName = contentPlatFormOrderInfo.GoodsName;
                        recommandDocumentSettleDto.Phone = isHidePhone == true ? ServiceClass.GetIncompletePhone(contentPlatFormOrderInfo.Phone) : contentPlatFormOrderInfo.Phone;
                        break;
                    case (int)OrderFrom.BuyAgainOrder:
                        var customerHospitalConsume = await customerHospitalConsumeService.GetByConsumeIdAsync(x.DealInfoId);
                        recommandDocumentSettleDto.DealDate = customerHospitalConsume.BuyAgainTime;
                        recommandDocumentSettleDto.GoodsName = customerHospitalConsume.ItemName;
                        recommandDocumentSettleDto.Phone = isHidePhone == true ? ServiceClass.GetIncompletePhone(customerHospitalConsume.Phone) : customerHospitalConsume.Phone;
                        break;
                    case (int)OrderFrom.ThirdPartyOrder:
                        var tmallOrder = await orderService.GetByIdInCRMAsync(x.OrderId);
                        recommandDocumentSettleDto.DealDate = tmallOrder.WriteOffDate;
                        recommandDocumentSettleDto.GoodsName = tmallOrder.GoodsName;
                        recommandDocumentSettleDto.Phone = isHidePhone == true ? ServiceClass.GetIncompletePhone(tmallOrder.Phone) : tmallOrder.Phone;
                        break;
                }
                recommandDocumentSettleDtos.Add(recommandDocumentSettleDto);
            }
            return recommandDocumentSettleDtos;
        }

        #endregion
        #region

        /// <summary>
        /// 获取医院维度财务看板数据
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<FinancialHospitalBoardDto>> FinancialHospitalBoardDataAsync(int? hospitalId, DateTime? startDate, DateTime? endDate, int pageNum, int pageSize)
        {
            startDate = startDate == null ? DateTime.Now.Date : startDate.Value.Date;
            endDate = endDate == null ? DateTime.Now.AddDays(1).Date : endDate.Value.AddDays(1).Date;
            var bill = dalBill.GetAll().Include(e => e.HospitalInfo).Where(e => e.Valid == true && e.CreateDate > startDate && e.CreateDate < endDate);
            if (hospitalId.HasValue)
            {
                bill = bill.Where(e => e.HospitalId == hospitalId);
            }
            var data = bill.GroupBy(e => e.HospitalId).OrderByDescending(g => g.Sum(item => item.DealPrice)).Select(g => new FinancialHospitalBoardDto
            {
                HospitalName = dalHospitalInfo.GetAll().Where(e => e.Id == g.Key).FirstOrDefault().Name,
                DealPrice = g.Sum(item => item.DealPrice) ?? 0m,
                NoIncludeTaxPrice = g.Sum(item => item.NotInTaxPrice),
                InformationPrice = g.Sum(item => item.InformationPrice) ?? 0m,
                SystemUsePrice = g.Sum(item => item.SystemUpdatePrice) ?? 0m,
                ReturnBackPrice = g.Sum(item => item.ReturnBackPrice) ?? 0m,
                TotalServicePrice = g.Sum(item => item.BillPrice)
            });
            FxPageInfo<FinancialHospitalBoardDto> fxPageInfo = new FxPageInfo<FinancialHospitalBoardDto>();
            fxPageInfo.TotalCount = await data.CountAsync();
            fxPageInfo.List = data.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            return fxPageInfo;
        }


        /// <summary>
        /// 获取子公司维度财务看板数据
        /// </summary>
        /// <param name="companyId">子公司id</param>
        /// <param name="hospitalId">医院id</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<FinancialHospitalBoardDto>> FinancialCompanyBoardDataAsync(string companyId, DateTime? startDate, DateTime? endDate, int pageNum, int pageSize)
        {
            startDate = startDate == null ? DateTime.Now.Date : startDate.Value.Date;
            endDate = endDate == null ? DateTime.Now.AddDays(1).Date : endDate.Value.AddDays(1).Date;
            var bill = dalBill.GetAll().Include(e => e.HospitalInfo).Where(e => e.Valid == true && e.CreateDate > startDate && e.CreateDate < endDate);
            if (!string.IsNullOrEmpty(companyId))
            {
                bill = bill.Where(e => e.CollectionCompanyId == companyId);
            }


            var data = bill.GroupBy(e => new { e.CollectionCompanyId }).OrderByDescending(g => g.Sum(item => item.DealPrice)).Select(g => new FinancialHospitalBoardDto
            {
                CompanyName = dalCompanyBaseInfo.GetAll().Where(e => e.Id == g.Key.CollectionCompanyId).SingleOrDefault().Name,
                DealPrice = g.Sum(item => item.DealPrice) ?? 0m,
                NoIncludeTaxPrice = g.Sum(item => item.NotInTaxPrice),
                TotalServicePrice = g.Sum(item => item.BillPrice),
                InformationPrice = g.Sum(item => item.InformationPrice) ?? 0m,
                SystemUsePrice = g.Sum(item => item.SystemUpdatePrice) ?? 0m,
                ReturnBackPrice = g.Sum(item => item.ReturnBackPrice) ?? 0m,
            });
            FxPageInfo<FinancialHospitalBoardDto> fxPageInfo = new FxPageInfo<FinancialHospitalBoardDto>();
            fxPageInfo.TotalCount = await data.CountAsync();
            fxPageInfo.List = data.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            return fxPageInfo;
        }

        #endregion

        #region [枚举下拉框]

        /// <summary>
        /// 票据类型列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<BaseKeyValueDto>> GetBillTypeListAsync()
        {
            var billTypes = Enum.GetValues(typeof(BillTypeTextEnum));

            List<BaseKeyValueDto> billTypeList = new List<BaseKeyValueDto>();
            foreach (var item in billTypes)
            {
                BaseKeyValueDto baseKeyValueDto = new BaseKeyValueDto();
                baseKeyValueDto.Key = Convert.ToInt32(item).ToString();
                baseKeyValueDto.Value = ServiceClass.GetBillTypeText(Convert.ToInt32(item));
                billTypeList.Add(baseKeyValueDto);
            }
            return billTypeList;
        }
        /// <summary>
        /// 票据回款状态列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<BaseKeyValueDto>> GetBillReturnBackStateTextListAsync()
        {
            var billReturnBackStateTexts = Enum.GetValues(typeof(BillReturnBackStateTextEnum));

            List<BaseKeyValueDto> billReturnBackStateTextList = new List<BaseKeyValueDto>();
            foreach (var item in billReturnBackStateTexts)
            {
                BaseKeyValueDto baseKeyValueDto = new BaseKeyValueDto();
                baseKeyValueDto.Key = Convert.ToInt32(item).ToString();
                baseKeyValueDto.Value = ServiceClass.GetBillReturnBackStateText(Convert.ToInt32(item));
                billReturnBackStateTextList.Add(baseKeyValueDto);
            }
            return billReturnBackStateTextList;
        }










        #endregion
    }
}
