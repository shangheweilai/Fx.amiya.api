using Fx.Amiya.Background.Api.Vo.LiveAnchorBaseInfo;
using Fx.Amiya.Dto.LiveAnchorBaseInfo;
using Fx.Amiya.IService;
using Fx.Common;
using Fx.Infrastructure;
using Fx.Open.Infrastructure.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.Background.Api.Controllers
{

    /// <summary>
    /// 主播基础信息
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LiveAnchorBaseInfoController : ControllerBase
    {
        private ILiveAnchorBaseInfoService liveAnchorBaseInfoService;
        private IHttpContextAccessor httpContextAccessor;
        public LiveAnchorBaseInfoController(IHttpContextAccessor httpContextAccessor, ILiveAnchorBaseInfoService liveAnchorBaseInfoService)
        {
            this.liveAnchorBaseInfoService = liveAnchorBaseInfoService;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///  获取有效的主播基础信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("valid")]
        public async Task<ResultData<List<LiveAnchorBaseInfoIdAndNameVo>>> GetValidListAsync(bool? isSelfLiveAnchor)
        {
            var liveAnchorBaseInfos = from d in await liveAnchorBaseInfoService.GetValidAsync(isSelfLiveAnchor)
                              select new LiveAnchorBaseInfoIdAndNameVo
                              {
                                  Id = d.Id,
                                  Name =d.LiveAnchorName,
                              };
            return ResultData<List<LiveAnchorBaseInfoIdAndNameVo>>.Success().AddData("liveAnchorBaseInfos", liveAnchorBaseInfos.ToList());
        }

        /// <summary>
        ///  获取有效的主播基础信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("total")]
        public async Task<ResultData<List<LiveAnchorBaseInfoIdAndNameVo>>> GetTotalListAsync(bool? isSelfLiveAnchor)
        {
            var liveAnchorBaseInfos = from d in await liveAnchorBaseInfoService.GetAllLiveAnchorAsync(isSelfLiveAnchor)
                                      select new LiveAnchorBaseInfoIdAndNameVo
                                      {
                                          Id = d.Id,
                                          Name = d.LiveAnchorName,
                                      };
            return ResultData<List<LiveAnchorBaseInfoIdAndNameVo>>.Success().AddData("liveAnchorBaseInfos", liveAnchorBaseInfos.ToList());
        }

        /// <summary>
        /// 获取主播基础信息列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valid"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ResultData<FxPageInfo<LiveAnchorBaseInfoVo>>> GetListAsync(string name,bool valid, int pageNum, int pageSize)
        {
            var employee = httpContextAccessor.HttpContext.User as FxAmiyaEmployeeIdentity;
            int employeeId = Convert.ToInt32(employee.Id);
            var q = await liveAnchorBaseInfoService.GetListAsync(name, valid, pageNum, pageSize);
            var liveAnchorBaseInfos = from d in q.List
                                      select new LiveAnchorBaseInfoVo
                                      {
                                          Id = d.Id,
                                          LiveAnchorName = d.LiveAnchorName,
                                          ThumbPicture = d.ThumbPicture,
                                          NickName = d.NickName,
                                          ContractUrl = d.ContractUrl,
                                          VideoUrl = d.VideoUrl,
                                          DueTime = d.DueTime,
                                          IndividualitySignature = d.IndividualitySignature,
                                          Description = d.Description,
                                          DetailPicture = d.DetailPicture,
                                          IsMain = d.IsMain,
                                          Valid = d.Valid,
                                          IsSelfLivevAnchor = d.IsSelfLivevAnchor,
                              };
            FxPageInfo<LiveAnchorBaseInfoVo> liveAnchorBaseInfoPageInfo = new FxPageInfo<LiveAnchorBaseInfoVo>();
            liveAnchorBaseInfoPageInfo.TotalCount = q.TotalCount;
            liveAnchorBaseInfoPageInfo.List = liveAnchorBaseInfos;
            return ResultData<FxPageInfo<LiveAnchorBaseInfoVo>>.Success().AddData("liveAnchorBaseInfos", liveAnchorBaseInfoPageInfo);
        }




        /// <summary>
        /// 添加主播基础信息
        /// </summary>
        /// <param name="addVo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultData> AddAsync(AddLiveAnchorBaseInfoVo addVo)
        {
            AddLiveAnchorBaseInfoDto addDto = new AddLiveAnchorBaseInfoDto();
            addDto.LiveAnchorName = addVo.LiveAnchorName;
            addDto.ThumbPicture = addVo.ThumbPicture;
            addDto.ContractUrl = addVo.ContractUrl;
            addDto.VideoUrl = addVo.VideoUrl;
            addDto.DueTime = addVo.DueTime;
            addDto.NickName = addVo.NickName;
            addDto.IsSelfLivevAnchor = addVo.IsSelfLivevAnchor;
            addDto.IndividualitySignature = addVo.IndividualitySignature;
            addDto.Description = addVo.Description;
            addDto.DetailPicture = addVo.DetailPicture;
            addDto.IsMain = addVo.IsMain;
            await liveAnchorBaseInfoService.AddAsync(addDto);
            return ResultData.Success();
        }


        /// <summary>
        /// 根据编号获取主播基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byId/{id}")]
        public async Task<ResultData<LiveAnchorBaseInfoVo>> GetByIdAsync(string id)
        {

            var result = await liveAnchorBaseInfoService.GetByIdAsync(id);
            LiveAnchorBaseInfoVo liveAnchorBaseInfoVo = new LiveAnchorBaseInfoVo();
            liveAnchorBaseInfoVo.Id = result.Id;
            liveAnchorBaseInfoVo.LiveAnchorName = result.LiveAnchorName;
            liveAnchorBaseInfoVo.ThumbPicture = result.ThumbPicture;
            liveAnchorBaseInfoVo.DueTime = result.DueTime;
            liveAnchorBaseInfoVo.ContractUrl = result.ContractUrl;
            liveAnchorBaseInfoVo.VideoUrl = result.VideoUrl;
            liveAnchorBaseInfoVo.NickName = result.NickName;
            liveAnchorBaseInfoVo.IndividualitySignature = result.IndividualitySignature;
            liveAnchorBaseInfoVo.Description = result.Description;
            liveAnchorBaseInfoVo.DetailPicture = result.DetailPicture;
            liveAnchorBaseInfoVo.IsMain = result.IsMain;
            liveAnchorBaseInfoVo.IsSelfLivevAnchor = result.IsSelfLivevAnchor;
            liveAnchorBaseInfoVo.Valid = true;
            return ResultData<LiveAnchorBaseInfoVo>.Success().AddData("liveAnchorBaseInfo", liveAnchorBaseInfoVo);
        }

        /// <summary>
        /// 修改主播基础信息
        /// </summary>
        /// <param name="updateVo"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultData> UpdateAsync(UpdateLiveAnchorBaseInfoVo updateVo)
        {
            UpdateLiveAnchorBaseInfoDto updateDto = new UpdateLiveAnchorBaseInfoDto();
            updateDto.Id = updateVo.Id;
            updateDto.LiveAnchorName = updateVo.LiveAnchorName;
            updateDto.ThumbPicture = updateVo.ThumbPicture;
            updateDto.NickName = updateVo.NickName;
            updateDto.IndividualitySignature = updateVo.IndividualitySignature;
            updateDto.Description = updateVo.Description;
            updateDto.DetailPicture = updateVo.DetailPicture;
            updateDto.IsMain = updateVo.IsMain;
            updateDto.ContractUrl = updateVo.ContractUrl;
            updateDto.VideoUrl = updateVo.VideoUrl;
            updateDto.DueTime = updateVo.DueTime;
            updateDto.Valid = updateVo.Valid;
            updateDto.IsSelfLivevAnchor = updateVo.IsSelfLivevAnchor;
            await liveAnchorBaseInfoService.UpdateAsync(updateDto);
            return ResultData.Success();
        }



        /// <summary>
        /// 删除主播基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ResultData> DeleteAsync([FromRoute] string id)
        {
            await liveAnchorBaseInfoService.DeleteAsync(id);
            return ResultData.Success();
        }
    }
}
