﻿using Fx.Amiya.DbModels.Model;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using Fx.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fx.Infrastructure.DataAccess;
using jos_sdk_net.Util;
using Fx.Amiya.Dto.WareHouse.WareHouseInfo;
using Fx.Amiya.Dto.WareHouse.InventoryList;
using Fx.Amiya.Dto.WareHouse.OutWareHouse;
using Fx.Amiya.Dto.WareHouse.InWareHouse;
using Fx.Amiya.Dto.CustomerTagInfo;

namespace Fx.Amiya.Service
{
    public class CustomerTagInfoService : ICustomerTagInfoService
    {
        private IDalCustomerTagInfo dalCustomerTagInfoService;
        private IInventoryListService inventoryListService;
        private IUnitOfWork unitOfWork;
        private IAmiyaOutWareHouseService amiyaOutWareHouseService;
        private IAmiyaInWareHouseService amiyaInWareHouseService;

        public CustomerTagInfoService(IDalCustomerTagInfo dalCustomerTagInfoService,
            IInventoryListService inventoryListService,
            IAmiyaInWareHouseService inWareHouseService,
            IAmiyaOutWareHouseService amiyaOutWareHouseService,
            IUnitOfWork unitofWork)
        {
            this.dalCustomerTagInfoService = dalCustomerTagInfoService;
            this.inventoryListService = inventoryListService;
            this.amiyaOutWareHouseService = amiyaOutWareHouseService;
            this.amiyaInWareHouseService = inWareHouseService;
            this.unitOfWork = unitofWork;
        }



        public async Task<FxPageInfo<CustomerTagInfoDto>> GetListWithPageAsync(string keyword,  int pageNum, int pageSize)
        {
            try
            {
                var customerTagInfoService = from d in dalCustomerTagInfoService.GetAll()
                                             where (keyword == null || d.TagName.Contains(keyword))
                                             && (d.Valid == true)
                                             select new CustomerTagInfoDto
                                             {
                                                 Id = d.Id,
                                                 TagName = d.TagName,
                                                 CreateDate = d.CreateDate,
                                                 UpdateDate = d.UpdateDate,
                                                 DeleteDate = d.DeleteDate,
                                                 Valid = d.Valid,
                                             };
                FxPageInfo<CustomerTagInfoDto> customerTagInfoServicePageInfo = new FxPageInfo<CustomerTagInfoDto>();
                customerTagInfoServicePageInfo.TotalCount = await customerTagInfoService.CountAsync();
                customerTagInfoServicePageInfo.List = await customerTagInfoService.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                return customerTagInfoServicePageInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task AddAsync(AddCustomerTagInfoDto addDto)
        {
            try
            {
                CustomerTagInfo customerTagInfoService = new CustomerTagInfo();
                customerTagInfoService.Id = Guid.NewGuid().ToString();
                customerTagInfoService.TagName = addDto.TagName;
                customerTagInfoService.Valid = true;
                customerTagInfoService.CreateDate = DateTime.Now;

                await dalCustomerTagInfoService.AddAsync(customerTagInfoService, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerTagInfoDto> GetByIdAsync(string id)
        {
            try
            {
                var customerTagInfoService = await dalCustomerTagInfoService.GetAll().SingleOrDefaultAsync(e => e.Id == id);
                if (customerTagInfoService == null)
                {
                    return new CustomerTagInfoDto();
                }

                CustomerTagInfoDto customerTagInfoServiceDto = new CustomerTagInfoDto();
                customerTagInfoServiceDto.Id = customerTagInfoService.Id;
                customerTagInfoServiceDto.TagName = customerTagInfoService.TagName;
                customerTagInfoServiceDto.Valid = customerTagInfoService.Valid;
                customerTagInfoServiceDto.CreateDate = customerTagInfoService.CreateDate;
                customerTagInfoServiceDto.UpdateDate = customerTagInfoService.UpdateDate;
                customerTagInfoServiceDto.DeleteDate = customerTagInfoService.DeleteDate;


                return customerTagInfoServiceDto;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task UpdateAsync(UpdateCustomerTagInfoDto updateDto)
        {
            try
            {
                var customerTagInfoService = await dalCustomerTagInfoService.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);
                if (customerTagInfoService == null)
                    throw new Exception("标签编号错误！");

                customerTagInfoService.TagName = updateDto.TagName;
                customerTagInfoService.UpdateDate = updateDto.UpdateDate;
                customerTagInfoService.Valid = updateDto.Valid;
                if (updateDto.Valid == false)
                {
                    customerTagInfoService.DeleteDate = DateTime.Now;
                }
                await dalCustomerTagInfoService.UpdateAsync(customerTagInfoService, true);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                var customerTagInfoService = await dalCustomerTagInfoService.GetAll().SingleOrDefaultAsync(e => e.Id == id);

                if (customerTagInfoService == null)
                    throw new Exception("标签编号错误");

                await dalCustomerTagInfoService.DeleteAsync(customerTagInfoService, true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
