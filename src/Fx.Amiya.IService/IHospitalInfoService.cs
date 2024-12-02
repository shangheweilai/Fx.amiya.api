using Fx.Amiya.Dto;
using Fx.Amiya.Dto.HospitalContract.Input;
using Fx.Amiya.Dto.HospitalContract.Result;
using Fx.Amiya.Dto.HospitalEnvironmentPicture;
using Fx.Amiya.Dto.HospitalInfo;
using Fx.Amiya.Dto.HospitalProject.Input;
using Fx.Amiya.Dto.HospitalProject.Result;
using Fx.Common;
using Fx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.IService
{
    public interface IHospitalInfoService
    {


        /// <summary>
        /// 获取医院列表（分页）
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<FxPageInfo<HospitalInfoDto>> GetListWithPageAsync(string keyword, int? cityId, int pageNum, int pageSize,bool? valid);

        /// <summary>
        /// 获取即将到期的医院列表数据
        /// </summary>
        /// <returns></returns>
        Task<FxPageInfo<HospitalInfoDto>> GetAboutToExpiredListAsync();
        /// <summary>
        /// 获取医院待审核数据
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="CheckState"></param>
        /// <param name="submitState"></param>
        /// <returns></returns>
        Task<FxPageInfo<HospitalCheckInfoDto>> GetCheckListWithPageAsync(string keyword, int pageNum, int pageSize, int CheckState, int submitState);

        /// <summary>
        /// 啊美雅全国供应链派单指南
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<HospitalInfoDto>> GetAmiyaTotalSendHospitalInstructionsAsync(QueryAmiyaTotalSendHospitalInstructionsDto query);

        /// <summary>
        /// 获取医院名称列表
        /// </summary>
        /// <returns></returns>
        Task<List<HospitalNameDto>> GetHospitalNameListAsync(bool? valid,string name);
        /// <summary>
        /// 获取医院简称列表
        /// </summary>
        /// <param name="valid"></param>
        /// <returns></returns>
        Task<List<HospitalNameDto>> GetHospitalSimpleNameListAsync(bool? valid);
        /// <summary>
        /// 小程序获取医院名称列表
        /// </summary>
        /// <returns></returns>
        Task<FxPageInfo<WxHospitalInfoDto>> GetWxHospitalNameListAsync(int pageNum, int pageSize, string city, string hospitalName, List<string> tags);

        /// <summary>
        /// 审核医院资料
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        Task HospitalInfoCheckAsync(HospitalInfoCheckInfoDto updateDto);

        /// <summary>
        /// 获取资料审核通过的医院医院名称列表
        /// </summary>
        /// <param name="valid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<HospitalNameDto>> GetCheckPassedHospitalNameListAsync(bool? valid, string name);
        /// <summary>
        /// 添加医院
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task AddAsync(AddHospitalInfoDto addDto, int employeeId);


        /// <summary>
        /// 根据医院编号获取医院详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HospitalInfoDetailDto> GetByIdAsync(int id);

        /// <summary>
        /// 根据医院编号获取医院基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HospitalInfoDto> GetBaseByIdAsync(int id);

        /// <summary>
        /// 根据医院名字获取医院基础信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        Task<HospitalInfoDto> GetBaseByNameAsync(string Name);

        /// <summary>
        /// 修改医院信息
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task UpdateAsync(UpdateHospitalInfoDto updateDto, int employeeId);

        /// <summary>
        /// 修改医院合同信息
        /// </summary>
        /// <param name="contractUrl"></param>
        /// <param name="hospitalId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task UpdateContractUrlAsync(string contractUrl, int hospitalId, int employeeId);

        /// <summary>
        /// 医院端编辑信息
        /// </summary>
        /// <param name="updateDto"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task HospitalUpdateAsync(HospitalUpdateHospitalInfoDto updateDto);

        /// <summary>
        /// 删除医院
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);



        /// <summary>
        /// 获取医院列表（小程序）
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<List<HospitalInfoSimpleDto>> GetSimpleListAsync(string keyword, string city);

        /// <summary>
        /// 根据项目id获取医院详细列表（分页，小程序）
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<FxPageInfo<WxHospitalInfoDto>> GetListWithPageOfWxAsync(int pageNum, int pageSize, string city,int itemInfoId);

        /// <summary>
        /// 根据标签，名称，城市等筛选获取医院列表（分页）
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="city"></param>
        /// <param name="hospitalName"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<FxPageInfo<WxHospitalInfoDto>> GetListHosPitalAsync(int pageNum, int pageSize, string city,string hospitalName, List<string> tags);
        /// <summary>
        /// 根据商品编号获取参与项目的医院名称列表
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="name">医院名称，null：全部</param>
        /// <returns></returns>
        Task<List<HospitalNameDto>> GetPartakeItemHospitalNameListAsync(string goodsId,string name);
        /// <summary>
        /// 获取预约叫车预约医院名称列表
        /// </summary>
        /// <returns></returns>
        Task<List<HospitalNameDto>> GetWxAppointCarHospitalNameList();
        /// <summary>
        /// 根据归属公司获取医院名称列表
        /// </summary>
        /// <returns></returns>
        Task<List<HospitalNameDto>> GetHospitalNameListByCompany(string companyId);
        /// <summary>
        /// 派单顺序列表
        /// </summary>
        /// <returns></returns>
        Task<List<BaseKeyValueDto<int>>> GetSendOrderListAsync();
        /// <summary>
        /// 年费和保证金缴纳状态列表
        /// </summary>
        /// <returns></returns>
        Task<List<BaseKeyValueDto<int>>> GetYearServiceStatusAsync();
        /// <summary>
        /// 根据时间获取医院派单数
        /// </summary>
        /// <returns></returns>
        Task<List<ActiveHospitalInfoDto>> GetActiveHospitalListByTimeAsync(QueryActiveHospitalDto query);

        #region 【新的医院合同接口】

        /// <summary>
        /// 新的医院合同修改接口
        /// </summary>
        /// <param name="contractUrl"></param>
        /// <param name="hospitalId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task UpdateContractAsync(UpdateHospitalContractDto updateDto);
        /// <summary>
        /// 新的医院合同添加接口
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task AddHospitalContractAsync(AddHospitalContractDto addDto);
        /// <summary>
        /// 新的删除医院合同接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteHospitalContractAsync(string id);
        /// <summary>
        /// 根据医院id获取合同列表
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        Task<List<HospitalContractInfoDto>> GetHospitalContractListAsync(int hospitalId);
        /// <summary>
        /// 根据id获取合同详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HospitalContractInfoDto> GetHospitalContractByIdAsync(string id);

        #endregion


        #region 【新的医院项目ppt接口】

        /// <summary>
        /// 新的医院项目ppt修改接口
        /// </summary>
        /// <param name="contractUrl"></param>
        /// <param name="hospitalId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task UpdateProjectAsync(UpdateHospitalProjectDto updateDto);
        /// <summary>
        /// 新的医院项目ppt添加接口
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task AddHospitalProjectAsync(AddHospitalProjectDto addDto);
        /// <summary>
        /// 新的删除医院项目ppt接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteHospitalProjectAsync(string id);
        /// <summary>
        /// 根据医院id获取项目ppt列表
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        Task<List<HospitalProjectInfoDto>> GetHospitalProjectListAsync(int hospitalId);
        /// <summary>
        /// 根据id获取项目ppt详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<HospitalProjectInfoDto> GetHospitalProjectByIdAsync(string id);

        #endregion


    }
}
