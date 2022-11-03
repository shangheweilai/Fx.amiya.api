﻿using Fx.Amiya.Dto.BeautyDiaryManage;
using Fx.Amiya.IDal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Fx.Amiya.IService;
using Fx.Infrastructure;
using Fx.Amiya.DbModels.Model;
using Fx.Infrastructure.DataAccess;
using Fx.Common;
using Fx.Common.Utils;

namespace Fx.Amiya.Service
{
    public class BeautyDiaryManageService : IBeautyDiaryManageService
    {
        private readonly IOrderAppInfoService orderAppInfoService;
        private IDalBeautyDiaryManage dalBeautyDiaryManage;
        private IDalBeautyDiaryTagDetail dalBeautyDiaryTagDetail;
        private IUnitOfWork unitOfWork;
        private IDalHospitalPartakeItem dalHospitalQuotedPriceItemInfo;
        private IDalBeautyDiaryBannerImage dalBeautyDiaryBannerImage;
        private IDalOrderInfo dalOrderInfo;
        private IDalItemInfo dalItemInfo;
        public BeautyDiaryManageService(IDalBeautyDiaryManage dalBeautyDiaryManage,
            IDalBeautyDiaryTagDetail dalBeautyDiaryTagDetail,
            IUnitOfWork unitOfWork,
            IDalHospitalPartakeItem dalHospitalQuotedPriceItemInfo,
            IDalOrderInfo dalOrderInfo,
            IDalBeautyDiaryBannerImage dalBeautyDiaryBannerImage,
            IDalItemInfo dalItemInfo, IOrderAppInfoService orderAppInfoService)
        {
            this.dalBeautyDiaryManage = dalBeautyDiaryManage;
            this.dalBeautyDiaryTagDetail = dalBeautyDiaryTagDetail;
            this.unitOfWork = unitOfWork;
            this.dalHospitalQuotedPriceItemInfo = dalHospitalQuotedPriceItemInfo;
            this.dalOrderInfo = dalOrderInfo;
            this.dalBeautyDiaryBannerImage = dalBeautyDiaryBannerImage;
            this.dalItemInfo = dalItemInfo;
            this.orderAppInfoService = orderAppInfoService;
        }


        /// <summary>
        /// 获取日记列表（分页）
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<BeautyDiaryManageDto>> GetListWithPageAsync(string keyword, int pageNum, int pageSize, bool? isReleased)
        {
            try
            {
                if (pageSize > 100)
                    throw new Exception("每次查询不能超过100条");

                var beautyDiaryManage = from d in dalBeautyDiaryManage.GetAll()
                                        where (keyword == null || d.CoverTitle.Contains(keyword) || d.DetailsTitle.Contains(keyword))
                                        && (isReleased == null || d.ReleaseState == isReleased)
                                        select new BeautyDiaryManageDto
                                        {
                                            Id = d.Id,
                                            CoverTitle = d.CoverTitle,
                                            DetailsTitle = d.DetailsTitle,
                                            ReleaseState = d.ReleaseState,
                                            CreateDate = d.CreateDate,
                                            Views = d.Views,
                                            GivingLikes = d.GivingLikes,
                                            ThumbPictureUrl = d.ThumbPictureUrl,
                                            VideoUrl = d.VideoUrl,
                                            DetailsDescription = d.DetailsDescription
                                        };
                FxPageInfo<BeautyDiaryManageDto> beautyDiaryManagePageInfo = new FxPageInfo<BeautyDiaryManageDto>();
                beautyDiaryManagePageInfo.TotalCount = await beautyDiaryManage.CountAsync();
                beautyDiaryManagePageInfo.List = await beautyDiaryManage.OrderByDescending(z => z.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                return beautyDiaryManagePageInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 添加日记
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task AddAsync(AddBeautyDiaryManageDto addDto)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var beautyDiaryManageCount = await dalBeautyDiaryManage.GetAll().CountAsync(e => e.CoverTitle == addDto.CoverTitle || e.DetailsTitle == addDto.DetailsTitle);
                if (beautyDiaryManageCount > 0)
                    throw new Exception("添加失败，已存在该日记标题名称");

                BeautyDiaryManage beautyDiaryManageInfo = new BeautyDiaryManage();
                beautyDiaryManageInfo.Id = Guid.NewGuid().ToString();
                beautyDiaryManageInfo.CoverTitle = addDto.CoverTitle;
                beautyDiaryManageInfo.DetailsTitle = addDto.DetailsTitle;
                beautyDiaryManageInfo.ReleaseState = addDto.ReleaseState;
                beautyDiaryManageInfo.CreateDate = DateTime.Now;
                beautyDiaryManageInfo.Views = addDto.Views;
                beautyDiaryManageInfo.GivingLikes = addDto.GivingLikes;
                beautyDiaryManageInfo.ThumbPictureUrl = addDto.ThumbPictureUrl;
                beautyDiaryManageInfo.VideoUrl = addDto.VideoUrl;
                beautyDiaryManageInfo.DetailsDescription = addDto.DetailsDescription;
                await dalBeautyDiaryManage.AddAsync(beautyDiaryManageInfo, true);

                List<BeautyDiaryTagDetail> beautyDiaryManageTagDetailList = new List<BeautyDiaryTagDetail>();
                foreach (var item in addDto.TagIds)
                {
                    BeautyDiaryTagDetail beautyDiaryManageTagDetail = new BeautyDiaryTagDetail();
                    beautyDiaryManageTagDetail.Id = Guid.NewGuid().ToString();
                    beautyDiaryManageTagDetail.BeautyDiaryId = beautyDiaryManageInfo.Id;
                    beautyDiaryManageTagDetail.TagId = item;
                    beautyDiaryManageTagDetailList.Add(beautyDiaryManageTagDetail);
                }
                // (banner未加，todo;)
                await dalBeautyDiaryTagDetail.AddCollectionAsync(beautyDiaryManageTagDetailList, true);


                List<BeautyDiaryBannerImage> beautyDiaryManageBannerImageList = new List<BeautyDiaryBannerImage>();
                foreach (var item in addDto.BeautyDiaryBannerImage)
                {
                    BeautyDiaryBannerImage beautyDiaryManageTagDetail = new BeautyDiaryBannerImage();
                    beautyDiaryManageTagDetail.Id = Guid.NewGuid().ToString();
                    beautyDiaryManageTagDetail.BeautyDiaryId = beautyDiaryManageInfo.Id;
                    beautyDiaryManageTagDetail.PicUrl = item.PicUrl;
                    beautyDiaryManageTagDetail.DisplayIndex = item.DisplayIndex;
                    beautyDiaryManageTagDetail.CreateDate = DateTime.Now;
                    beautyDiaryManageTagDetail.UpdateDate = DateTime.Now;
                    beautyDiaryManageBannerImageList.Add(beautyDiaryManageTagDetail);
                }
                // (banner未加，todo;)
                await dalBeautyDiaryBannerImage.AddCollectionAsync(beautyDiaryManageBannerImageList, true);

                unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw ex;
            }
        }



        /// <summary>
        /// 根据日记编号获取日记信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BeautyDiaryManageDetailDto> GetDetailByIdAsync(string id)
        {
            try
            {
                var beautyDiaryManageInfo = from d in dalBeautyDiaryManage.GetAll()
                                            where d.Id == id
                                            select new BeautyDiaryManageDetailDto
                                            {
                                                Id = d.Id,
                                                CoverTitle = d.CoverTitle,
                                                DetailsTitle = d.DetailsTitle,
                                                ReleaseState = d.ReleaseState,
                                                CreateDate = d.CreateDate,
                                                Views = d.Views,
                                                GivingLikes = d.GivingLikes,
                                                ThumbPictureUrl = d.ThumbPictureUrl,
                                                VideoUrl = d.VideoUrl,
                                                DetailsDescription = d.DetailsDescription,
                                                BeautyDiaryTagList = (from t in d.BeautyDiaryTagDetailList
                                                                      where t.BeautyDiaryTagInfo.Valid
                                                                      select new BeautyDiaryTagNameDto
                                                                      {
                                                                          Id = t.TagId,
                                                                          Name = t.BeautyDiaryTagInfo.Name
                                                                      }).ToList(),


                                                BeautyDiaryBannerImageList = (from z in d.BannerImages
                                                                              select new BeautyDiaryBannerImageDto
                                                                              {
                                                                                  Id = z.Id,
                                                                                  PicUrl = z.PicUrl,
                                                                                  DisplayIndex = z.DisplayIndex
                                                                              }).ToList(),
                                            };

                if (await beautyDiaryManageInfo.CountAsync() == 0)
                    throw new Exception("日记编号错误");

                return await beautyDiaryManageInfo.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<BeautyDiaryManageDto> GetBaseByIdAsync(string id)
        {
            try
            {
                var beautyDiaryManage = await dalBeautyDiaryManage.GetAll()
                    //.Include(e => e.BeautyDiaryTagDetailList)
                    //.Include(e => e.BannerImages)
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (beautyDiaryManage == null)
                {
                    return null;
                }
                BeautyDiaryManageDto beautyDiaryManageInfoDto = new BeautyDiaryManageDto();
                beautyDiaryManageInfoDto.Id = beautyDiaryManage.Id;
                beautyDiaryManageInfoDto.CoverTitle = beautyDiaryManage.CoverTitle;
                beautyDiaryManageInfoDto.DetailsTitle = beautyDiaryManage.DetailsTitle;
                beautyDiaryManageInfoDto.ReleaseState = beautyDiaryManage.ReleaseState;
                beautyDiaryManageInfoDto.CreateDate = beautyDiaryManage.CreateDate;
                beautyDiaryManageInfoDto.Views = beautyDiaryManage.Views;
                beautyDiaryManageInfoDto.GivingLikes = beautyDiaryManage.GivingLikes;
                beautyDiaryManageInfoDto.ThumbPictureUrl = beautyDiaryManage.ThumbPictureUrl;
                beautyDiaryManageInfoDto.VideoUrl = beautyDiaryManage.VideoUrl;
                beautyDiaryManageInfoDto.DetailsDescription = beautyDiaryManage.DetailsDescription;
                return beautyDiaryManageInfoDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BeautyDiaryManageDto> GetBaseByNameAsync(string Name)
        {
            try
            {
                var beautyDiaryManage = await dalBeautyDiaryManage.GetAll()
                    //.Include(e => e.CreateByAmiyaEmployee)
                    //.Include(e => e.UpdateByAmiyaEmployee)
                    //.Include(e => e.CooperativeHospitalCity)
                    .SingleOrDefaultAsync(e => e.CoverTitle == Name);

                if (beautyDiaryManage == null)
                {
                    return null;
                }
                BeautyDiaryManageDto beautyDiaryManageInfoDto = new BeautyDiaryManageDto();
                beautyDiaryManageInfoDto.Id = beautyDiaryManage.Id;
                beautyDiaryManageInfoDto.CoverTitle = beautyDiaryManage.CoverTitle;
                beautyDiaryManageInfoDto.DetailsTitle = beautyDiaryManage.DetailsTitle;
                beautyDiaryManageInfoDto.ReleaseState = beautyDiaryManage.ReleaseState;
                beautyDiaryManageInfoDto.CreateDate = beautyDiaryManage.CreateDate;
                beautyDiaryManageInfoDto.Views = beautyDiaryManage.Views;
                beautyDiaryManageInfoDto.GivingLikes = beautyDiaryManage.GivingLikes;
                beautyDiaryManageInfoDto.ThumbPictureUrl = beautyDiaryManage.ThumbPictureUrl;
                beautyDiaryManageInfoDto.VideoUrl = beautyDiaryManage.VideoUrl;
                beautyDiaryManageInfoDto.DetailsDescription = beautyDiaryManage.DetailsDescription;
                return beautyDiaryManageInfoDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改日记信息
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateBeautyDiaryManageDto updateDto)
        {
            try
            {
                unitOfWork.BeginTransaction();

                var beautyDiary = await dalBeautyDiaryManage.GetAll().SingleOrDefaultAsync(e => e.Id == updateDto.Id);

                if (updateDto == null)
                    throw new Exception("日记编号错误");

                beautyDiary.CoverTitle = updateDto.CoverTitle;
                beautyDiary.DetailsTitle = updateDto.DetailsTitle;
                beautyDiary.ReleaseState = updateDto.ReleaseState;
                beautyDiary.Views = updateDto.Views;
                beautyDiary.GivingLikes = updateDto.GivingLikes;
                beautyDiary.ThumbPictureUrl = updateDto.ThumbPictureUrl;
                beautyDiary.VideoUrl = updateDto.VideoUrl;
                beautyDiary.DetailsDescription = updateDto.DetailsDescription;
                await dalBeautyDiaryManage.UpdateAsync(beautyDiary, true);

                var tagDetail = await dalBeautyDiaryTagDetail.GetAll().Where(e => e.BeautyDiaryId == updateDto.Id).ToListAsync();

                foreach (var item in updateDto.TagIds)
                {
                    if (!tagDetail.Exists(e => e.TagId == item))
                    {
                        BeautyDiaryTagDetail beautyDiaryManageTagDetail = new BeautyDiaryTagDetail();
                        beautyDiaryManageTagDetail.BeautyDiaryId = updateDto.Id;
                        beautyDiaryManageTagDetail.TagId = item;
                        beautyDiaryManageTagDetail.Id = Guid.NewGuid().ToString();
                        await dalBeautyDiaryTagDetail.AddAsync(beautyDiaryManageTagDetail, true);
                    }
                }


                foreach (var item in tagDetail)
                {
                    if (Array.IndexOf(updateDto.TagIds, item.TagId) == -1)
                    {
                        await dalBeautyDiaryTagDetail.DeleteAsync(item, true);
                    }
                }
                var BannerDetail = await dalBeautyDiaryBannerImage.GetAll().Where(e => e.BeautyDiaryId == updateDto.Id).ToListAsync();

                foreach (var item in BannerDetail)
                {
                    await dalBeautyDiaryBannerImage.DeleteAsync(item, true);
                }
                foreach (var item in updateDto.BeautyDiaryBannerImage)
                {
                    BeautyDiaryBannerImage beautyDiaryBannerImage = new BeautyDiaryBannerImage();
                    beautyDiaryBannerImage.Id = Guid.NewGuid().ToString();
                    beautyDiaryBannerImage.BeautyDiaryId = updateDto.Id;
                    beautyDiaryBannerImage.PicUrl = item.PicUrl;
                    beautyDiaryBannerImage.DisplayIndex = item.DisplayIndex;
                    beautyDiaryBannerImage.CreateDate = DateTime.Now;
                    beautyDiaryBannerImage.UpdateDate = DateTime.Now;
                    await dalBeautyDiaryBannerImage.AddAsync(beautyDiaryBannerImage, true);
                }


                unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw ex;
            }
        }

        public async Task GivingLikesAsync(string id)
        {
            try
            {
                var beautyDiaryManage = await dalBeautyDiaryManage.GetAll()
                    .SingleOrDefaultAsync(e => e.Id == id);
                beautyDiaryManage.GivingLikes += 1;
                await dalBeautyDiaryManage.UpdateAsync(beautyDiaryManage, true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddViewsAsync(string id)
        {
            try
            {
                var beautyDiaryManage = await dalBeautyDiaryManage.GetAll()
                    .SingleOrDefaultAsync(e => e.Id == id);
                beautyDiaryManage.Views += 1;
                await dalBeautyDiaryManage.UpdateAsync(beautyDiaryManage, true);

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
                unitOfWork.BeginTransaction();
                //删除美丽日记信息
                var beautyDiaryManage = await dalBeautyDiaryManage.GetAll()
                    .SingleOrDefaultAsync(e => e.Id == id);

                if (beautyDiaryManage == null)
                    throw new Exception("日记编号错误");
                //删除美丽日记标签
                var tagDetail = await dalBeautyDiaryTagDetail.GetAll().Where(e => e.BeautyDiaryId == id).ToListAsync();

                foreach (var item in tagDetail)
                {
                    await dalBeautyDiaryTagDetail.DeleteAsync(item, true);
                }
                //删除美丽日记轮播图
                var BannerDetail = await dalBeautyDiaryBannerImage.GetAll().Where(e => e.BeautyDiaryId == id).ToListAsync();

                foreach (var item in BannerDetail)
                {
                    await dalBeautyDiaryBannerImage.DeleteAsync(item, true);
                }

                await dalBeautyDiaryManage.DeleteAsync(beautyDiaryManage, true);



                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.RollBack();
                throw ex;
            }
        }




        /// <summary>
        /// 获取日记列表（小程序）
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<BeautyDiaryManageSimpleDto>> GetSimpleListAsync(string keyword, int pageNum, int pageSize)
        {
            try
            {
                var beautyDiaryManage = from d in dalBeautyDiaryManage.GetAll()
                                        where d.ReleaseState && (keyword == null || d.CoverTitle.Contains(keyword))
                                        select new BeautyDiaryManageSimpleDto
                                        {
                                            Id = d.Id,
                                            CoverTitle = d.CoverTitle,
                                            ReleaseState = d.ReleaseState,
                                            CreateDate = d.CreateDate,
                                            Views = d.Views,
                                            GivingLikes = d.GivingLikes,
                                            ThumbPictureUrl = d.ThumbPictureUrl,
                                        };
                FxPageInfo<BeautyDiaryManageSimpleDto> beautyDiaryManagePageInfo = new FxPageInfo<BeautyDiaryManageSimpleDto>();
                beautyDiaryManagePageInfo.TotalCount = await beautyDiaryManage.CountAsync();
                beautyDiaryManagePageInfo.List = await beautyDiaryManage.OrderByDescending(z => z.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                return beautyDiaryManagePageInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateReleaseStateAsync(string id, bool releaseState)
        {
            var beautyDiaryManage = from d in dalBeautyDiaryManage.GetAll()
                                    where d.Id == id
                                    select d;
            var result = beautyDiaryManage.FirstOrDefault();
            result.ReleaseState = releaseState;
            await dalBeautyDiaryManage.UpdateAsync(result, true);
        }
        /// <summary>
        /// 从微信公众号获取日记列表（分页）
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<FxPageInfo<BeautyDiaryManageSimpleDto>> GetSimpleListFromWechatAsync(string keyword, int pageNum, int pageSize)
        {
            try
            {
                /*var beautyDiaryManage = from d in dalBeautyDiaryManage.GetAll()
                                        where d.ReleaseState && (keyword == null || d.CoverTitle.Contains(keyword))
                                        select new BeautyDiaryManageSimpleDto
                                        {
                                            Id = d.Id,
                                            CoverTitle = d.CoverTitle,
                                            ReleaseState = d.ReleaseState,
                                            CreateDate = d.CreateDate,
                                            Views = d.Views,
                                            GivingLikes = d.GivingLikes,
                                            ThumbPictureUrl = d.ThumbPictureUrl,
                                        };
                FxPageInfo<BeautyDiaryManageSimpleDto> beautyDiaryManagePageInfo = new FxPageInfo<BeautyDiaryManageSimpleDto>();
                beautyDiaryManagePageInfo.TotalCount = await beautyDiaryManage.CountAsync();
                beautyDiaryManagePageInfo.List = await beautyDiaryManage.OrderByDescending(z => z.CreateDate).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
                return beautyDiaryManagePageInfo;*/
                var appInfo=await orderAppInfoService.GetBeautyDiaryAppInfo();
                var requestUrl = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appInfo.AppKey}&secret={appInfo.AppSecret}";
                var res = await HttpUtil.HTTPJsonGetAsync(requestUrl);
                return null;
               /* if (res.Contains("errorcode"))
                    _logger.LogInformation(res);

                var order = JsonConvert.DeserializeObject<WeiFenXiaoOrderResult>(res);*/
            }
            catch (Exception ex)
            {
                throw new Exception("获取美丽日记失败");
            }
        }





        #region  已注释

        ///// <summary>
        ///// 获取日记详细列表（分页，小程序）
        ///// </summary>
        ///// <param name="pageNum"></param>
        ///// <param name="pageSize"></param>
        ///// <returns></returns>
        //public async Task<FxPageInfo<WxBeautyDiaryManageDto>> GetListWithPageOfWxAsync(int pageNum, int pageSize, string city, int itemInfoId)
        //{
        //    try
        //    {
        //        var beautyDiaryManage = from h in dalBeautyDiaryManage.GetAll()
        //                       .Include(e => e.RecommendBeautyDiaryManageList)
        //                          join d in dalHospitalQuotedPriceItemInfo.GetAll() on h.Id equals d.HospitalId
        //                          where h.Valid
        //                          && (city == null || h.Address.Contains(city))
        //                          && d.ItemId == itemInfoId
        //                          select new WxBeautyDiaryManageDto
        //                          {
        //                              Id = h.Id,
        //                              Name = h.Name,
        //                              Address = h.Address,
        //                              Longitude = h.Longitude,
        //                              Latitude = h.Latitude,
        //                              Phone = h.Phone,
        //                              ThumbPicUrl = h.ThumbPicUrl,
        //                              IsRecommend = GetRecommendHospital(h.RecommendBeautyDiaryManageList) == null ? false : true,
        //                              RecommendIndex = GetRecommendHospital(h.RecommendBeautyDiaryManageList),
        //                              DocterList = (from d in h.DocterList
        //                                            select new DocterDto
        //                                            {
        //                                                Id = d.Id,
        //                                                Name = d.Name,
        //                                                PicUrl = d.PicUrl,
        //                                                Position = d.Position,
        //                                                WorkYearNumer = DateTime.Now.Year - d.ObtainEmploymentYear,
        //                                                Description = d.Description,
        //                                            }).ToList(),

        //                              ScaleTagList = (from s in h.BeautyDiaryTagDetailList
        //                                              where s.TagInfo.Valid && s.TagInfo.Type == 0
        //                                              select new BeautyDiaryTagNameDto
        //                                              {
        //                                                  Id = s.TagId,
        //                                                  Name = s.TagInfo.Name
        //                                              }).ToList(),

        //                              FacilityTagList = (from s in h.BeautyDiaryTagDetailList
        //                                                 where s.TagInfo.Valid && s.TagInfo.Type == 1
        //                                                 select new BeautyDiaryTagNameDto
        //                                                 {
        //                                                     Id = s.TagId,
        //                                                     Name = s.TagInfo.Name
        //                                                 }).ToList()
        //                          };


        //        // beautyDiaryManagePageInfo.List = await beautyDiaryManage.Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
        //        var q = await beautyDiaryManage.ToListAsync();

        //        List<WxBeautyDiaryManageDto> beautyDiaryManageList = new List<WxBeautyDiaryManageDto>();
        //        beautyDiaryManageList.AddRange(q.Where(e => e.IsRecommend).OrderBy(e => e.RecommendIndex));
        //        beautyDiaryManageList.AddRange(q.Where(e => e.IsRecommend == false));

        //        FxPageInfo<WxBeautyDiaryManageDto> beautyDiaryManagePageInfo = new FxPageInfo<WxBeautyDiaryManageDto>();
        //        beautyDiaryManagePageInfo.TotalCount = await beautyDiaryManage.CountAsync();
        //        beautyDiaryManagePageInfo.List = beautyDiaryManageList.Skip((pageNum - 1) * pageSize).Take(pageSize);
        //        return beautyDiaryManagePageInfo;
        //        //HACK 排序
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 根据标签，名称，城市等筛选获取日记列表（分页）
        ///// </summary>
        ///// <param name="pageNum"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="city"></param>
        ///// <param name="beautyDiaryManageName"></param>
        ///// <param name="tag"></param>
        ///// <returns></returns>
        //public async Task<FxPageInfo<WxBeautyDiaryManageDto>> GetListHosPitalAsync(int pageNum, int pageSize, string city, string beautyDiaryManageName, List<string> tags)
        //{
        //    try
        //    {
        //        var beautyDiaryManage = from h in dalBeautyDiaryManage.GetAll()
        //                       .Include(e => e.RecommendBeautyDiaryManageList)
        //                          where h.Valid
        //                          && (city == null || h.Address.Contains(city))
        //                          && (beautyDiaryManageName == null || h.Name.Contains(beautyDiaryManageName))
        //                          select new WxBeautyDiaryManageDto
        //                          {
        //                              Id = h.Id,
        //                              Name = h.Name,
        //                              Address = h.Address,
        //                              Longitude = h.Longitude,
        //                              Latitude = h.Latitude,
        //                              Phone = h.Phone,
        //                              ThumbPicUrl = h.ThumbPicUrl,
        //                              IsRecommend = GetRecommendHospital(h.RecommendBeautyDiaryManageList) == null ? false : true,
        //                              RecommendIndex = GetRecommendHospital(h.RecommendBeautyDiaryManageList),
        //                              DocterList = (from d in h.DocterList
        //                                            select new DocterDto
        //                                            {
        //                                                Id = d.Id,
        //                                                Name = d.Name,
        //                                                PicUrl = d.PicUrl,
        //                                                Position = d.Position,
        //                                                WorkYearNumer = DateTime.Now.Year - d.ObtainEmploymentYear,
        //                                                Description = d.Description,
        //                                            }).ToList(),

        //                              ScaleTagList = (from s in h.BeautyDiaryTagDetailList
        //                                              where s.TagInfo.Valid && s.TagInfo.Type == 0
        //                                              select new BeautyDiaryTagNameDto
        //                                              {
        //                                                  Id = s.TagId,
        //                                                  Name = s.TagInfo.Name
        //                                              }).ToList(),

        //                              FacilityTagList = (from s in h.BeautyDiaryTagDetailList
        //                                                 where s.TagInfo.Valid && s.TagInfo.Type == 1
        //                                                 select new BeautyDiaryTagNameDto
        //                                                 {
        //                                                     Id = s.TagId,
        //                                                     Name = s.TagInfo.Name
        //                                                 }).ToList()
        //                          };
        //        var q = await beautyDiaryManage.ToListAsync();

        //        List<WxBeautyDiaryManageDto> beautyDiaryManageListResult = new List<WxBeautyDiaryManageDto>();
        //        List<WxBeautyDiaryManageDto> beautyDiaryManageList = new List<WxBeautyDiaryManageDto>();
        //        beautyDiaryManageList.AddRange(q.Where(e => e.IsRecommend).OrderBy(e => e.RecommendIndex));
        //        beautyDiaryManageList.AddRange(q.Where(e => e.IsRecommend == false));
        //        if (tags.Count > 0)
        //        {
        //            foreach (var x in beautyDiaryManageList)
        //            {
        //                bool IsAdd = false;
        //                for (int a = 0; a < tags.Count; a++)
        //                {
        //                    if (x.ScaleTagList.Exists(k => k.Id == Convert.ToInt16(tags[a])) || x.FacilityTagList.Exists(o => o.Id == Convert.ToInt16(tags[a])))
        //                    {
        //                        IsAdd = true;
        //                    }
        //                    else
        //                    {
        //                        IsAdd = false;
        //                        break;
        //                    }

        //                }
        //                if (IsAdd == true)
        //                {
        //                    WxBeautyDiaryManageDto beautyDiaryManageInfoResultDto = new WxBeautyDiaryManageDto();
        //                    beautyDiaryManageInfoResultDto.Id = x.Id;
        //                    beautyDiaryManageInfoResultDto.Name = x.Name;
        //                    beautyDiaryManageInfoResultDto.Address = x.Address;
        //                    beautyDiaryManageInfoResultDto.Longitude = x.Longitude;
        //                    beautyDiaryManageInfoResultDto.Latitude = x.Latitude;
        //                    beautyDiaryManageInfoResultDto.Phone = x.Phone;
        //                    beautyDiaryManageInfoResultDto.ThumbPicUrl = x.ThumbPicUrl;
        //                    beautyDiaryManageInfoResultDto.IsRecommend = x.IsRecommend;
        //                    beautyDiaryManageInfoResultDto.RecommendIndex = x.RecommendIndex;
        //                    beautyDiaryManageInfoResultDto.DocterList = x.DocterList;
        //                    beautyDiaryManageInfoResultDto.ScaleTagList = x.ScaleTagList;
        //                    beautyDiaryManageInfoResultDto.FacilityTagList = x.FacilityTagList;
        //                    beautyDiaryManageListResult.Add(beautyDiaryManageInfoResultDto);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            beautyDiaryManageListResult = beautyDiaryManageList;
        //        }

        //        FxPageInfo<WxBeautyDiaryManageDto> beautyDiaryManagePageInfo = new FxPageInfo<WxBeautyDiaryManageDto>();
        //        beautyDiaryManagePageInfo.TotalCount = beautyDiaryManageListResult.Count();
        //        beautyDiaryManagePageInfo.List = beautyDiaryManageListResult.Skip((pageNum - 1) * pageSize).Take(pageSize);
        //        return beautyDiaryManagePageInfo;
        //        //HACK 排序
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 根据商品编号获取参与项目的日记名称列表
        ///// </summary>
        ///// <param name="goodsId"></param>
        ///// <returns></returns>
        //public async Task<List<HospitalNameDto>> GetPartakeItemHospitalNameListAsync(string goodsId, string name)
        //{
        //    var itemInfo = await dalItemInfo.GetAll().SingleOrDefaultAsync(e => e.OtherAppItemId == goodsId);
        //    if (itemInfo == null)
        //        return new List<HospitalNameDto>();
        //    var beautyDiaryManage = from d in dalHospitalQuotedPriceItemInfo.GetAll()
        //                      where d.ItemId == itemInfo.Id
        //                      && (string.IsNullOrWhiteSpace(name) || d.BeautyDiaryManage.Name.Contains(name))
        //                      select new HospitalNameDto
        //                      {
        //                          Id = d.HospitalId,
        //                          Name = d.BeautyDiaryManage.Name
        //                      };

        //    return await beautyDiaryManage.ToListAsync();
        //}

        #endregion
    }
}
