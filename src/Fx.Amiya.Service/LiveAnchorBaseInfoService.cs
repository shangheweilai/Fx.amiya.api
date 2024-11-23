using Fx.Amiya.Dto.LiveAnchorBaseInfo;
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
    public class LiveAnchorBaseInfoService : ILiveAnchorBaseInfoService
    {
        private IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo;
        public LiveAnchorBaseInfoService(IDalLiveAnchorBaseInfo dalLiveAnchorBaseInfo)
        {
            this.dalLiveAnchorBaseInfo = dalLiveAnchorBaseInfo;
        }

        /// <summary>
        /// 获取有效的主播列表
        /// </summary>
        /// <param name="isSelfLiveAnchor">是否为自播达人主播</param>
        /// <returns></returns>
        public async Task<List<LiveAnchorBaseInfoDto>> GetValidAsync(bool? isSelfLiveAnchor)
        {
            var liveAnchorBaseInfos = from d in dalLiveAnchorBaseInfo.GetAll()
                                      where (d.Valid == true) && (!isSelfLiveAnchor.HasValue || d.IsSelfLivevAnchor == isSelfLiveAnchor.Value)
                                      select new LiveAnchorBaseInfoDto
                                      {
                                          Id = d.Id,
                                          LiveAnchorName = d.LiveAnchorName,
                                          Valid = d.Valid,
                                          IsSelfLivevAnchor = d.IsSelfLivevAnchor,
                                      };
            var resultList = await liveAnchorBaseInfos.ToListAsync();
            return resultList;
        }

        /// <summary>
        /// 获取全部主播列表
        /// </summary>
        /// <param name="isSelfLiveAnchor">是否为自播达人主播</param>
        /// <returns></returns>
        public async Task<List<LiveAnchorBaseInfoDto>> GetAllLiveAnchorAsync(bool? isSelfLiveAnchor)
        {
            var liveAnchorBaseInfos = from d in dalLiveAnchorBaseInfo.GetAll()
                                      where (!isSelfLiveAnchor.HasValue || d.IsSelfLivevAnchor == isSelfLiveAnchor.Value)
                                      select new LiveAnchorBaseInfoDto
                                      {
                                          Id = d.Id,
                                          LiveAnchorName = d.LiveAnchorName,
                                          Valid = d.Valid,
                                          IsSelfLivevAnchor = d.IsSelfLivevAnchor,
                                      };
            var resultList = await liveAnchorBaseInfos.ToListAsync();
            return resultList;
        }

        /// <summary>
        /// 获取名索医生列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<LiveAnchorBaseInfoDto>> GetMingSuoLiveAnchorAsync()
        {
            var liveAnchorBaseInfos = from d in dalLiveAnchorBaseInfo.GetAll()
                                      where (d.LiveAnchorName=="李芬"|| d.LiveAnchorName == "马瑶" || d.LiveAnchorName == "林合晟" )
                                      select new LiveAnchorBaseInfoDto
                                      {
                                          Id = d.Id,
                                          LiveAnchorName = d.LiveAnchorName,
                                          Valid = d.Valid,
                                          IsSelfLivevAnchor = d.IsSelfLivevAnchor,
                                      };
            var resultList = await liveAnchorBaseInfos.ToListAsync();
            return resultList;
        }


        public async Task<FxPageInfo<LiveAnchorBaseInfoDto>> GetListAsync(string name, bool valid, int pageNum, int pageSize)
        {

            var liveAnchorBaseInfos = from d in dalLiveAnchorBaseInfo.GetAll()
                                      where (string.IsNullOrWhiteSpace(name) || d.NickName.Contains(name) || d.LiveAnchorName.Contains(name))
                                      && (d.Valid == valid)
                                      select new LiveAnchorBaseInfoDto
                                      {
                                          Id = d.Id,
                                          LiveAnchorName = d.LiveAnchorName,
                                          ThumbPicture = d.ThumbPicture,
                                          NickName = d.NickName,
                                          IndividualitySignature = d.IndividualitySignature,
                                          Description = d.Description,
                                          ContractUrl = d.ContractUrl,
                                          VideoUrl = d.VideoUrl,
                                          DueTime = d.DueTime,
                                          DetailPicture = d.DetailPicture,
                                          IsMain = d.IsMain,
                                          Valid = d.Valid,
                                          IsSelfLivevAnchor = d.IsSelfLivevAnchor
                                      };
            FxPageInfo<LiveAnchorBaseInfoDto> liveAnchorBaseInfoPageInfo = new FxPageInfo<LiveAnchorBaseInfoDto>();
            liveAnchorBaseInfoPageInfo.TotalCount = await liveAnchorBaseInfos.CountAsync();
            liveAnchorBaseInfoPageInfo.List = await liveAnchorBaseInfos.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return liveAnchorBaseInfoPageInfo;
        }


        /// <summary>
        /// 添加主播基础信息
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddLiveAnchorBaseInfoDto addDto)
        {
            try
            {
                var resultCount = await this.GetValidListAsync();
                var result = resultCount.FirstOrDefault(e => e.NickName == addDto.NickName && e.LiveAnchorName == addDto.LiveAnchorName);
                if (result != null)
                    throw new Exception("已存在该主播基础信息");
                LiveAnchorBaseInfo liveAchor = new LiveAnchorBaseInfo();
                liveAchor.Id = Guid.NewGuid().ToString();
                liveAchor.LiveAnchorName = addDto.LiveAnchorName;
                liveAchor.ThumbPicture = addDto.ThumbPicture;
                liveAchor.NickName = addDto.NickName;
                liveAchor.IndividualitySignature = addDto.IndividualitySignature;
                liveAchor.Description = addDto.Description;
                liveAchor.DueTime = addDto.DueTime;
                liveAchor.ContractUrl = addDto.ContractUrl;
                liveAchor.VideoUrl = addDto.VideoUrl;
                liveAchor.DetailPicture = addDto.DetailPicture;
                liveAchor.IsMain = addDto.IsMain;
                liveAchor.IsSelfLivevAnchor = addDto.IsSelfLivevAnchor;
                liveAchor.Valid = true;
                await dalLiveAnchorBaseInfo.AddAsync(liveAchor, true);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message.ToString());
            }
        }



        public async Task<LiveAnchorBaseInfoDto> GetByIdAsync(string id)
        {
            var result = from d in dalLiveAnchorBaseInfo.GetAll()
                         select d;
            var x = result.FirstOrDefault(e => e.Id == id);
            if (x == null)
            {
                return new LiveAnchorBaseInfoDto();
            }

            LiveAnchorBaseInfoDto liveAnchorBaseInfoDto = new LiveAnchorBaseInfoDto();
            liveAnchorBaseInfoDto.Id = x.Id;
            liveAnchorBaseInfoDto.LiveAnchorName = x.LiveAnchorName;
            liveAnchorBaseInfoDto.ThumbPicture = x.ThumbPicture;
            liveAnchorBaseInfoDto.NickName = x.NickName;
            liveAnchorBaseInfoDto.VideoUrl = x.VideoUrl;
            liveAnchorBaseInfoDto.ContractUrl = x.ContractUrl;
            liveAnchorBaseInfoDto.DueTime = x.DueTime;
            liveAnchorBaseInfoDto.IndividualitySignature = x.IndividualitySignature;
            liveAnchorBaseInfoDto.Description = x.Description;
            liveAnchorBaseInfoDto.DetailPicture = x.DetailPicture;
            liveAnchorBaseInfoDto.IsMain = x.IsMain;
            liveAnchorBaseInfoDto.IsSelfLivevAnchor = x.IsSelfLivevAnchor;
            liveAnchorBaseInfoDto.Valid = true;
            return liveAnchorBaseInfoDto;
        }


        public async Task<LiveAnchorBaseInfoDto> GetByNameAsync(string name)
        {
            var result = from d in dalLiveAnchorBaseInfo.GetAll()
                         select d;
            var x = result.FirstOrDefault(e => e.LiveAnchorName == name);
            if (x == null)
            {
                return new LiveAnchorBaseInfoDto();
            }

            LiveAnchorBaseInfoDto liveAnchorBaseInfoDto = new LiveAnchorBaseInfoDto();
            liveAnchorBaseInfoDto.Id = x.Id;
            liveAnchorBaseInfoDto.LiveAnchorName = x.LiveAnchorName;
            liveAnchorBaseInfoDto.ThumbPicture = x.ThumbPicture;
            liveAnchorBaseInfoDto.NickName = x.NickName;
            liveAnchorBaseInfoDto.VideoUrl = x.VideoUrl;
            liveAnchorBaseInfoDto.ContractUrl = x.ContractUrl;
            liveAnchorBaseInfoDto.DueTime = x.DueTime;
            liveAnchorBaseInfoDto.IndividualitySignature = x.IndividualitySignature;
            liveAnchorBaseInfoDto.Description = x.Description;
            liveAnchorBaseInfoDto.DetailPicture = x.DetailPicture;
            liveAnchorBaseInfoDto.IsMain = x.IsMain;
            liveAnchorBaseInfoDto.IsSelfLivevAnchor = x.IsSelfLivevAnchor;
            liveAnchorBaseInfoDto.Valid = true;
            return liveAnchorBaseInfoDto;
        }


        public async Task<List<LiveAnchorBaseInfoDto>> GetByIdAndIsSelfLiveAnchorAsync(string id, bool? isSelfLiveAnchor)
        {
            var result = from d in dalLiveAnchorBaseInfo.GetAll()
                         select d;
            var x = result.Where(e => string.IsNullOrEmpty(id) || e.Id == id)
                .Where(e => !isSelfLiveAnchor.HasValue || e.IsSelfLivevAnchor == isSelfLiveAnchor.Value).ToList();
            if (x == null)
            {
                return new List<LiveAnchorBaseInfoDto>();
            }
            List<LiveAnchorBaseInfoDto> liveAnchorBaseInfoDtos = new List<LiveAnchorBaseInfoDto>();

            foreach (var k in x)
            {
                LiveAnchorBaseInfoDto liveAnchorBaseInfoDto = new LiveAnchorBaseInfoDto();
                liveAnchorBaseInfoDto.Id = k.Id;
                liveAnchorBaseInfoDto.LiveAnchorName = k.LiveAnchorName;
                liveAnchorBaseInfoDto.ThumbPicture = k.ThumbPicture;
                liveAnchorBaseInfoDto.NickName = k.NickName;
                liveAnchorBaseInfoDto.VideoUrl = k.VideoUrl;
                liveAnchorBaseInfoDto.ContractUrl = k.ContractUrl;
                liveAnchorBaseInfoDto.DueTime = k.DueTime;
                liveAnchorBaseInfoDto.IndividualitySignature = k.IndividualitySignature;
                liveAnchorBaseInfoDto.Description = k.Description;
                liveAnchorBaseInfoDto.DetailPicture = k.DetailPicture;
                liveAnchorBaseInfoDto.IsMain = k.IsMain;
                liveAnchorBaseInfoDto.IsSelfLivevAnchor = k.IsSelfLivevAnchor;
                liveAnchorBaseInfoDto.Valid = true;
                liveAnchorBaseInfoDtos.Add(liveAnchorBaseInfoDto);
            }
            return liveAnchorBaseInfoDtos;
        }

        /// <summary>
        /// 修改主播基础信息
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateLiveAnchorBaseInfoDto updateDto)
        {
            var resultCount = await this.GetValidListAsync();
            var result = resultCount.FirstOrDefault(e => e.Id != updateDto.Id && e.NickName == updateDto.NickName && e.LiveAnchorName == updateDto.LiveAnchorName);
            if (result != null)
                throw new Exception("已存在该主播基础信息");
            var liveAchor = dalLiveAnchorBaseInfo.GetAll().Where(x => x.Id == updateDto.Id).SingleOrDefault();
            liveAchor.LiveAnchorName = updateDto.LiveAnchorName;
            liveAchor.ThumbPicture = updateDto.ThumbPicture;
            liveAchor.NickName = updateDto.NickName;
            liveAchor.IndividualitySignature = updateDto.IndividualitySignature;
            liveAchor.Description = updateDto.Description;
            liveAchor.DetailPicture = updateDto.DetailPicture;
            liveAchor.IsMain = updateDto.IsMain;
            liveAchor.IsSelfLivevAnchor = updateDto.IsSelfLivevAnchor;
            liveAchor.Valid = updateDto.Valid;
            liveAchor.VideoUrl = updateDto.VideoUrl;
            liveAchor.ContractUrl = updateDto.ContractUrl;
            liveAchor.DueTime = updateDto.DueTime;
            await dalLiveAnchorBaseInfo.UpdateAsync(liveAchor, true);
        }



        /// <summary>
        /// 删除主播基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            var result = await dalLiveAnchorBaseInfo.GetAll().SingleOrDefaultAsync(e => e.Id == id);
            await dalLiveAnchorBaseInfo.DeleteAsync(result, true);
        }

        /// <summary>
        /// 获取有效的主播基础信息
        /// </summary>
        /// <returns></returns>
        private async Task<List<LiveAnchorBaseInfoDto>> GetValidListAsync()
        {
            var liveAnchorBaseInfos = from d in dalLiveAnchorBaseInfo.GetAll()
                                      where d.Valid == true
                                      select new LiveAnchorBaseInfoDto
                                      {
                                          Id = d.Id,
                                          LiveAnchorName = d.LiveAnchorName,
                                          ThumbPicture = d.ThumbPicture,
                                          NickName = d.NickName,
                                          IndividualitySignature = d.IndividualitySignature,
                                          Description = d.Description,
                                          DetailPicture = d.DetailPicture,
                                          IsMain = d.IsMain,
                                          Valid = d.Valid,
                                          DueTime = d.DueTime,
                                          ContractUrl = d.ContractUrl,
                                          VideoUrl = d.VideoUrl,
                                          IsSelfLivevAnchor = d.IsSelfLivevAnchor,
                                      };
            var resultList = await liveAnchorBaseInfos.ToListAsync();
            return resultList;
        }
        /// <summary>
        /// 获取合作达人
        /// </summary>
        /// <returns></returns>
        public async Task<List<LiveAnchorBaseInfoDto>> GetCooperateLiveAnchorAsync(bool? isValid)
        {
            return await dalLiveAnchorBaseInfo.GetAll().Where(e => e.IsSelfLivevAnchor == false && (!isValid.HasValue || e.Valid == isValid)).Select(e => new LiveAnchorBaseInfoDto
            {
                LiveAnchorName = e.LiveAnchorName,
                Id = e.Id
            }).ToListAsync();
        }
    }
}
