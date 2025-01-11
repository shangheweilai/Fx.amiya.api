using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dto.AmiyaOperationsBoardService.Result
{

    /// <summary>
    /// 医美（年度）业绩趋势
    /// </summary>
    public class PerformanceYearDataDto
    {
        /// <summary>
        /// 组别
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string SortName { get; set; }
        /// <summary>
        /// 一月
        /// </summary>
        public string JanuaryPerformance{ get; set; }

        /// <summary>
        /// 二月
        /// </summary>
        public string FebruaryPerformance { get; set; }
        /// <summary>
        /// 三月
        /// </summary>
        public string MarchPerformance { get; set; }
        /// <summary>
        /// 四月
        /// </summary>
        public string AprilPerformance { get; set; }
        /// <summary>
        /// 五月
        /// </summary>
        public string MayPerformance { get; set; }
        /// <summary>
        /// 六月
        /// </summary>
        public string JunePerformance { get; set; }
        /// <summary>
        /// 七月
        /// </summary>
        public string JulyPerformance { get; set; }
        /// <summary>
        /// 八月
        /// </summary>
        public string AugustPerformance { get; set; }
        /// <summary>
        /// 九月
        /// </summary>
        public string SeptemberPerformance { get; set; }
        /// <summary>
        /// 十月
        /// </summary>
        public string OctoberPerformance { get; set; }
        /// <summary>
        /// 十一月
        /// </summary>
        public string NovemberPerformance { get; set; }
        /// <summary>
        /// 十二月
        /// </summary>
        public string DecemberPerformance { get; set; }
        /// <summary>
        /// 合计值
        /// </summary>
        public string SumPerformance { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        public string AveragePerformance { get; set; }
    }

    public class PerformanceYearDataListDto { 
    
        /// <summary>
        /// 总业绩
        /// </summary>
        public List<PerformanceYearDataDto> TotalPerformanceData { get; set; }

        /// <summary>
        /// 刀刀组业绩
        /// </summary>
        public List<PerformanceYearDataDto> DaoDaoPerformanceData { get; set; }
        /// <summary>
        /// 吉娜组业绩
        /// </summary>
        public List<PerformanceYearDataDto> JiNaPerformanceData { get; set; }
    }

    public class AssistantPerformanceYearDataListDto
    {
                /// <summary>
        /// 刀刀组业绩
        /// </summary>
        public List<PerformanceYearDataDto> DaoDaoPerformanceData { get; set; }
        /// <summary>
        /// 吉娜组业绩
        /// </summary>
        public List<PerformanceYearDataDto> JiNaPerformanceData { get; set; }
    }
}
