using Fx.Amiya.Dto.LiveAnchor;
using Fx.Amiya.IDal;
using Fx.Amiya.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Fx.Infrastructure;
using Fx.Common;
using Fx.Amiya.DbModels.Model;

namespace Fx.Amiya.Service
{
    public class LiveAnchorService : ILiveAnchorService
    {
        private IDalLiveAnchor dalLiveAnchor;
        private IDalContentplatform _contentPlateForm;
        private IEmployeeBindLiveAnchorService employeeBindLiveAnchorService;
        private IAmiyaEmployeeService _amiyaEmployeeService;
        private ILiveRequirementInfoService _liveRequirementInfoService;
        private IDalOrderAppInfo dalOrderAppInfo;
        private IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo;
        public LiveAnchorService(IDalLiveAnchor dalLiveAnchor,
            IEmployeeBindLiveAnchorService employeeBindLiveAnchorService,
            ILiveRequirementInfoService liveRequirementInfoService,
            IAmiyaEmployeeService amiyaEmployeeService,
            IDalContentplatform contentPlateForm, IDalOrderAppInfo dalOrderAppInfo, IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo)
        {
            this.dalLiveAnchor = dalLiveAnchor;
            this.employeeBindLiveAnchorService = employeeBindLiveAnchorService;
            _amiyaEmployeeService = amiyaEmployeeService;
            _liveRequirementInfoService = liveRequirementInfoService;
            _contentPlateForm = contentPlateForm;
            this.dalOrderAppInfo = dalOrderAppInfo;
            this.dalLiveAnchorBaseInfo = dalLiveAnchorBaseInfo;
        }


        /// <summary>
        /// 根据主播基础id获取有效的主播列表
        /// </summary>
        /// <param name="contentPlatFormId">内容平台id</param>
        /// <returns></returns>
        public async Task<List<LiveAnchorDto>> GetValidListByLiveAnchorBaseIdAsync(string liveAnchorBaseId)
        {
            if (string.IsNullOrEmpty(liveAnchorBaseId))
            {
                throw new Exception("主播基础id为空！");
            }

            var liveAnchors = from d in dalLiveAnchor.GetAll()
                                  //where d.Valid == true
                              where d.LiveAnchorBaseId == liveAnchorBaseId
                              select new LiveAnchorDto
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  HostAccountName = string.IsNullOrWhiteSpace(d.HostAccountName) ? "" : d.HostAccountName,
                                  ContentPlateFormId = string.IsNullOrWhiteSpace(d.ContentPlateFormId) ? "" : d.ContentPlateFormId,
                                  LiveAnchorBaseId = d.LiveAnchorBaseId,
                                  Valid = d.Valid
                              };
            var resultList = await liveAnchors.ToListAsync();
            return resultList;
        }

        public async Task<List<LiveAnchorDto>> GetValidListByContentPlatFormIdAndNameAsync(string contentPlatFormId, string name)
        {

            var liveAnchors = from d in dalLiveAnchor.GetAll()
                                  // d.Valid == true
                              where d.ContentPlateFormId == contentPlatFormId
                              && d.Name.Contains(name)
                              select new LiveAnchorDto
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  HostAccountName = string.IsNullOrWhiteSpace(d.HostAccountName) ? "" : d.HostAccountName,
                                  ContentPlateFormId = string.IsNullOrWhiteSpace(d.ContentPlateFormId) ? "" : d.ContentPlateFormId,
                                  LiveAnchorBaseId = d.LiveAnchorBaseId,
                                  Valid = d.Valid
                              };
            var resultList = await liveAnchors.ToListAsync();
            return resultList;
        }

        /// <summary>
        /// 根据主播基础id集合获取有效的主播列表
        /// </summary>
        /// <param name="contentPlatFormId">内容平台id</param>
        /// <returns></returns>
        public async Task<List<LiveAnchorDto>> GetValidListByLiveAnchorBaseIdAsync(List<string> liveAnchorBaseIds)
        {
            if (liveAnchorBaseIds.Count == 0)
            {
                throw new Exception("主播基础id为空！");
            }
            return (from d in dalLiveAnchor.GetAll()
                        // d.Valid == true
                    where liveAnchorBaseIds.Contains(d.LiveAnchorBaseId)
                    select new LiveAnchorDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        HostAccountName = string.IsNullOrWhiteSpace(d.HostAccountName) ? "" : d.HostAccountName,
                        ContentPlateFormId = string.IsNullOrWhiteSpace(d.ContentPlateFormId) ? "" : d.ContentPlateFormId,
                        LiveAnchorBaseId = d.LiveAnchorBaseId,
                        Valid = d.Valid
                    }).ToList();

        }

        /// <summary>
        /// 根据内容平台id获取有效的主播列表
        /// </summary>
        /// <param name="contentPlatFormId">内容平台id</param>
        /// <returns></returns>
        public async Task<List<LiveAnchorDto>> GetValidListByContentPlatFormIdAsync(string contentPlatFormId, int employeeId)
        {
            if (string.IsNullOrEmpty(contentPlatFormId))
            {
                throw new Exception("请先选择内容平台进行主播账号查询！");
            }
            List<int> liveAnchorIds = new List<int>();

            var empInfo = await _amiyaEmployeeService.GetByIdAsync(employeeId);
            if (empInfo.PositionId == 19)
            {
                var bindLiveAnchorInfo = await employeeBindLiveAnchorService.GetByEmpIdAsync(employeeId);
                foreach (var x in bindLiveAnchorInfo)
                {
                    liveAnchorIds.Add(x.LiveAnchorId);
                }
            }

            var liveAnchors = from d in dalLiveAnchor.GetAll()
                              where d.Valid == true
                               && d.ContentPlateFormId == contentPlatFormId
                              && (liveAnchorIds.Count == 0 || liveAnchorIds.Contains(d.Id))
                              select new LiveAnchorDto
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  HostAccountName = string.IsNullOrWhiteSpace(d.HostAccountName) ? "" : d.HostAccountName,
                                  ContentPlateFormId = string.IsNullOrWhiteSpace(d.ContentPlateFormId) ? "" : d.ContentPlateFormId,
                                  LiveAnchorBaseId = d.LiveAnchorBaseId,
                                  Valid = d.Valid
                              };
            var resultList = await liveAnchors.ToListAsync();
            foreach (var x in resultList)
            {
                if (!string.IsNullOrEmpty(x.ContentPlateFormId))
                {
                    var contentPlateFormInfo = _contentPlateForm.GetAll().FirstOrDefaultAsync(z => z.Id == x.ContentPlateFormId);
                    if (contentPlateFormInfo.Result != null)
                    {
                        x.ContentPlateFormName = contentPlateFormInfo.Result.ContentPlatformName;
                    }
                }
            }
            return resultList;
        }

        /// <summary>
        /// 获取有效的主播列表
        /// </summary>
        /// <param name="contentPlatFormId">内容平台id</param>
        /// <returns></returns>
        public async Task<List<LiveAnchorDto>> GetValidAsync()
        {
            var liveAnchors = from d in dalLiveAnchor.GetAll()
                              where d.Valid == true
                              select new LiveAnchorDto
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  HostAccountName = string.IsNullOrWhiteSpace(d.HostAccountName) ? "" : d.HostAccountName,
                                  ContentPlateFormId = string.IsNullOrWhiteSpace(d.ContentPlateFormId) ? "" : d.ContentPlateFormId,
                                  Valid = d.Valid
                              };
            var resultList = await liveAnchors.ToListAsync();
            foreach (var x in resultList)
            {
                if (!string.IsNullOrEmpty(x.ContentPlateFormId))
                {
                    var contentPlateFormInfo = _contentPlateForm.GetAll().FirstOrDefaultAsync(z => z.Id == x.ContentPlateFormId);
                    if (contentPlateFormInfo.Result != null)
                    {
                        x.ContentPlateFormName = contentPlateFormInfo.Result.ContentPlatformName;
                    }
                }
            }
            return resultList;
        }



        public async Task<FxPageInfo<LiveAnchorDto>> GetListAsync(string name, int employeeId, string contentPlatformId, bool valid, int pageNum, int pageSize)
        {
            List<int> liveAnchorIds = new List<int>();

            var empInfo = await _amiyaEmployeeService.GetByIdAsync(employeeId);
            if (empInfo.PositionId == 19)
            {
                var bindLiveAnchorInfo = await employeeBindLiveAnchorService.GetByEmpIdAsync(employeeId);
                foreach (var x in bindLiveAnchorInfo)
                {
                    liveAnchorIds.Add(x.LiveAnchorId);
                }
            }

            var liveAnchors = from d in dalLiveAnchor.GetAll()
                              where (string.IsNullOrWhiteSpace(name) || d.Name.Contains(name))
                              && (liveAnchorIds.Count == 0 || liveAnchorIds.Contains(d.Id))
                              && (string.IsNullOrEmpty(contentPlatformId) || d.ContentPlateFormId == contentPlatformId)
                              && (d.Valid == valid)
                              select new LiveAnchorDto
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  HostAccountName = string.IsNullOrWhiteSpace(d.HostAccountName) ? "" : d.HostAccountName,
                                  ContentPlateFormId = string.IsNullOrWhiteSpace(d.ContentPlateFormId) ? "" : d.ContentPlateFormId,
                                  LiveAnchorBaseId = d.LiveAnchorBaseId,
                                  Valid = d.Valid
                              };
            FxPageInfo<LiveAnchorDto> liveAnchorPageInfo = new FxPageInfo<LiveAnchorDto>();
            liveAnchorPageInfo.TotalCount = await liveAnchors.CountAsync();
            liveAnchorPageInfo.List = await liveAnchors.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var x in liveAnchorPageInfo.List)
            {
                if (!string.IsNullOrEmpty(x.ContentPlateFormId))
                {
                    var contentPlateFormInfo = _contentPlateForm.GetAll().FirstOrDefaultAsync(z => z.Id == x.ContentPlateFormId);
                    if (contentPlateFormInfo.Result != null)
                    {
                        x.ContentPlateFormName = contentPlateFormInfo.Result.ContentPlatformName;
                    }
                }
            }
            return liveAnchorPageInfo;
        }


        /// <summary>
        /// 添加主播账号
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddLiveAnchorDto addDto)
        {
            try
            {
                var resultCount = await this.GetValidListAsync();
                var result = resultCount.FirstOrDefault(e => e.HostAccountName == addDto.HostAccountName && e.ContentPlateFormId == addDto.ContentPlateFormId);
                if (result != null)
                    throw new Exception("已存在该主播账号");
                LiveAnchor liveAchor = new LiveAnchor();
                liveAchor.Name = addDto.Name;
                liveAchor.Valid = addDto.Valid;
                liveAchor.LiveAnchorBaseId = addDto.LiveAnchorBaseId;
                liveAchor.HostAccountName = addDto.HostAccountName;
                liveAchor.ContentPlateFormId = addDto.ContentPlateFormId;
                await dalLiveAnchor.AddAsync(liveAchor, true);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message.ToString());
            }
        }



        public async Task<LiveAnchorDto> GetByIdAsync(int id)
        {
            var result = from d in dalLiveAnchor.GetAll()
                         select d;
            var x = result.SingleOrDefault(e => e.Id == id);
            if (x == null)
            {
                return new LiveAnchorDto()
                {
                    Name = "",
                    Id = 0,
                    HostAccountName = "",
                    ContentPlateFormId = "",
                    LiveAnchorBaseId = "",
                    Valid = false
                };
            }

            LiveAnchorDto liveAnchorDto = new LiveAnchorDto();
            liveAnchorDto.Id = x.Id;
            liveAnchorDto.Name = x.Name;
            liveAnchorDto.LiveAnchorBaseId = x.LiveAnchorBaseId;
            liveAnchorDto.HostAccountName = x.HostAccountName;
            liveAnchorDto.ContentPlateFormId = x.ContentPlateFormId;
            liveAnchorDto.Valid = x.Valid;
            return liveAnchorDto;
        }


        /// <summary>
        /// 修改主播账号
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateLiveAnchorDto updateDto)
        {
            var resultCount = await this.GetValidListAsync();
            var result = resultCount.FirstOrDefault(e => e.Id != updateDto.Id && e.HostAccountName == updateDto.HostAccountName && e.ContentPlateFormId == updateDto.ContentPlateFormId);
            if (result != null)
                throw new Exception("已存在该主播账号");
            LiveAnchor liveAchor = new LiveAnchor();
            liveAchor.Id = updateDto.Id;
            liveAchor.Name = updateDto.Name;
            liveAchor.Valid = updateDto.Valid;
            liveAchor.LiveAnchorBaseId = updateDto.LiveAnchorBaseId;
            liveAchor.HostAccountName = updateDto.HostAccountName;
            liveAchor.ContentPlateFormId = updateDto.ContentPlateFormId;
            await dalLiveAnchor.UpdateAsync(liveAchor, true);
        }



        /// <summary>
        /// 删除主播账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var result = await dalLiveAnchor.GetAll().SingleOrDefaultAsync(e => e.Id == id);
            var livingRequirement = _liveRequirementInfoService.GetByLiveAnchorIdAsync(id);
            if (livingRequirement.Result.Count > 0)
            { throw new Exception("该主播已发布过直播间需求信息，请清空对应需求后进行删除！"); }
            await dalLiveAnchor.DeleteAsync(result, true);
        }

        /// <summary>
        /// 获取有效的主播账号
        /// </summary>
        /// <returns></returns>
        private async Task<List<LiveAnchorDto>> GetValidListAsync()
        {
            var liveAnchors = from d in dalLiveAnchor.GetAll()
                              where d.Valid == true
                              select new LiveAnchorDto
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  HostAccountName = string.IsNullOrWhiteSpace(d.HostAccountName) ? "" : d.HostAccountName,
                                  ContentPlateFormId = string.IsNullOrWhiteSpace(d.ContentPlateFormId) ? "" : d.ContentPlateFormId,
                                  Valid = d.Valid
                              };
            var resultList = await liveAnchors.ToListAsync();
            foreach (var x in resultList)
            {
                if (!string.IsNullOrEmpty(x.ContentPlateFormId))
                {
                    var contentPlateFormInfo = _contentPlateForm.GetAll().FirstOrDefaultAsync(z => z.Id == x.ContentPlateFormId);
                    if (contentPlateFormInfo.Result != null)
                    {
                        x.ContentPlateFormName = contentPlateFormInfo.Result.ContentPlatformName;
                    }
                }
            }
            return resultList;
        }

        public async Task<List<LiveAnchorDto>> GetLiveAnchorListByBaseInfoId(string baseInfoId)
        {
            return await dalLiveAnchor.GetAll().Where(l => l.LiveAnchorBaseId == baseInfoId && l.Valid == true).Select(l => new LiveAnchorDto
            {
                Id = l.Id
            }).ToListAsync();
        }
        /// <summary>
        /// 根据基础主播id获取主播ip(不区分有效无效)
        /// </summary>
        /// <param name="baseInfoId"></param>
        /// <returns></returns>
        public async Task<List<LiveAnchorDto>> GetAllLiveAnchorListByBaseInfoId(string baseInfoId)
        {
            return await dalLiveAnchor.GetAll().Where(l => l.LiveAnchorBaseId == baseInfoId).Select(l => new LiveAnchorDto
            {
                Id = l.Id
            }).ToListAsync();
        }

        public async Task<List<LiveAnchorDto>> GetAllLiveAnchorListByBaseInfoIds(List<string> baseInfoIds)
        {
            return await dalLiveAnchor.GetAll().Where(l =>baseInfoIds.Contains( l.LiveAnchorBaseId)).Select(l => new LiveAnchorDto
            {
                Id = l.Id
            }).ToListAsync();
        }
        /// <summary>
        /// 获取非自播达人的主播IP
        /// </summary>
        /// <param name="baseInfoId"></param>
        /// <returns></returns>
        public async Task<List<LiveAnchorDto>> GetNotSelfLiveAnchor()
        {
            return await dalLiveAnchor.GetAll().Where(l => l.LiveAnchorBaseId!= "f0a77257-c905-4719-95c4-ad2c4f33855c" && l.LiveAnchorBaseId!= "af69dcf5-f749-41ea-8b50-fe685facdd8b").Select(l => new LiveAnchorDto
            {
                Id = l.Id
            }).ToListAsync();
        }
        /// <summary>
        /// 获取需要同步视频号订单的主播信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<LiveAnchorDto>> GetWechatVideoOrderLiveAnchorIdAsync()
        {
            var list = dalOrderAppInfo.GetAll().Where(e => e.AppType == (byte)AppType.WeChatVideo).Select(e => e.BelongLiveAnchor).ToList();
            return dalLiveAnchor.GetAll().Where(e => list.Contains(e.Id)).Select(e => new LiveAnchorDto
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();

        }
        /// <summary>
        /// 根据平台id和基础主播id获取主播ip的id
        /// </summary>
        /// <returns></returns>
        public async Task<List<int>> GetLiveAnchorIdsByContentPlatformIdAndBaseId(string contentPlatformId, string baseId)
        {
            var data = dalLiveAnchor.GetAll().Where(e => e.ContentPlateFormId == contentPlatformId);
            if (!string.IsNullOrEmpty(baseId))
            {
                data = data.Where(e => e.LiveAnchorBaseId == baseId);
            }
            return await data.Select(e => e.Id).ToListAsync();
        }

        public async Task<List<LiveAnchorDto>> GetLiveAnchorListByBaseInfoIdListAsync(List<string> baseInfoId)
        {
            return await dalLiveAnchor.GetAll().Where(e => baseInfoId.Contains(e.LiveAnchorBaseId)).Select(e => new LiveAnchorDto
            {
                Id = e.Id,
                LiveAnchorBaseId = e.LiveAnchorBaseId,
            }).ToListAsync();
        }
        /// <summary>
        /// 判断主播是否归属于自播达人
        /// </summary>
        /// <param name="liveANchorId"></param>
        /// <returns></returns>
        public async Task<bool> IsBelongSelfLiveAnchorAsync(int liveAnchorId)
        {
            var baseId = await dalLiveAnchor.GetAll().Where(e => e.Id == liveAnchorId).Select(e => e.LiveAnchorBaseId).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(baseId))
            {
                return await dalLiveAnchorBaseInfo.GetAll().Where(e => e.Id == baseId).Select(e => e.IsSelfLivevAnchor).FirstOrDefaultAsync();
            }
            return false;
        }
    }
}
