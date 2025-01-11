using Fx.Amiya.DbModels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.DbModels.DBModelConfigs
{
    public class ContentPlatformOrderConfiguration : IEntityTypeConfiguration<ContentPlatformOrder>
    {
        public void Configure(EntityTypeBuilder<ContentPlatformOrder> builder)
        {
            builder.ToTable("tbl_content_platform_order");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(e => e.BelongMonth).HasColumnName("belong_month").HasColumnType("int").IsRequired();
            builder.Property(e => e.OrderType).HasColumnName("order_type").HasColumnType("int").IsRequired();
            builder.Property(e => e.ContentPlateformId).HasColumnName("content_plateform_id").HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(e => e.LiveAnchorId).HasColumnName("live_anchor_id").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.LiveAnchorWeChatNo).HasColumnName("live_anchor_we_chat_no").HasColumnType("VARCHAR(100)").IsRequired(false);
            builder.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("DATETIME").IsRequired(); 
            builder.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.SendDate).HasColumnName("send_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.SceneConsulationName).HasColumnName("scene_consulation_name").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(e => e.NetWorkConsulationName).HasColumnName("network_consulation_name").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(e => e.GoodsId).HasColumnName("goods_id").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(e => e.HospitalDepartmentId).HasColumnName("hospital_department_id").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(e => e.CustomerName).HasColumnName("customer_name").HasColumnType("VARCHAR(100)").IsRequired(false);
            builder.Property(e => e.AddOrderPrice).HasColumnName("add_order_price").HasColumnType("DECIMAL(12,2)").IsRequired();
            builder.Property(e => e.Phone).HasColumnName("phone").HasColumnType("VARCHAR(20)").IsRequired();
            builder.Property(e => e.AppointmentDate).HasColumnName("appointment_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.ConsultationEmpId).HasColumnName("consultation_emp_id").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.ConsulationType).HasColumnName("consultation_type").HasColumnType("int").IsRequired();
            builder.Property(e => e.AppointmentHospitalId).HasColumnName("appointment_hospital_id").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.OrderStatus).HasColumnName("order_status").HasColumnType("int").IsRequired();
            builder.Property(e => e.DepositAmount).HasColumnName("deposit_amount").HasColumnType("DECIMAL").IsRequired(false);
            builder.Property(e => e.DealAmount).HasColumnName("deal_amount").HasColumnType("DECIMAL").IsRequired(false);
            builder.Property(e => e.DealPerformanceType).HasColumnName("deal_performance_type").HasColumnType("int").IsRequired();
            builder.Property(e => e.DealPictureUrl).HasColumnName("deal_picture_url").HasColumnType("VARCHAR(200)").IsRequired(false);
            builder.Property(e => e.DealDate).HasColumnName("deal_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.RepeatOrderPictureUrl).HasColumnName("repeat_order_picture_url").HasColumnType("VARCHAR(200)").IsRequired(false);
            builder.Property(e => e.UnSendReason).HasColumnName("unsend_reason").HasColumnType("VARCHAR(300)").IsRequired(false);
            builder.Property(e => e.AcceptConsulting).HasColumnName("accepts_consulting").HasColumnType("VARCHAR(45)").IsRequired(false);
            builder.Property(e => e.UnDealReason).HasColumnName("undeal_reason").HasColumnType("VARCHAR(200)").IsRequired(false);
            builder.Property(e => e.UnDealPictureUrl).HasColumnName("un_deal_picture_url").HasColumnType("VARCHAR(200)").IsRequired(false);
            builder.Property(e => e.LateProjectStage).HasColumnName("late_project_stage").HasColumnType("VARCHAR(200)").IsRequired(false);
            builder.Property(e => e.ConsultingContent).HasColumnName("consulting_content").HasColumnType("VARCHAR(200)").IsRequired(false);
            builder.Property(e => e.Remark).HasColumnName("remark").HasColumnType("VARCHAR(200)").IsRequired(false);
            builder.Property(t => t.IsToHospital).HasColumnName("is_to_hospital").HasColumnType("bit").IsRequired();
            builder.Property(e => e.ToHospitalType).HasColumnName("to_hospital_type").HasColumnType("int").IsRequired();
            builder.Property(e => e.ToHospitalDate).HasColumnName("to_hospital_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.LastDealHospitalId).HasColumnName("last_deal_hospital_id").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.CheckState).HasColumnName("check_state").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.OrderSource).HasColumnName("order_source").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.CheckPrice).HasColumnName("check_price").HasColumnType("DECIMAL").IsRequired(false);
            builder.Property(e => e.CheckDate).HasColumnName("check_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.SettlePrice).HasColumnName("settle_price").HasColumnType("DECIMAL").IsRequired(false);
            builder.Property(e => e.CustomerServiceSettlePrice).HasColumnName("customer_service_settle_price").HasColumnType("DECIMAL").IsRequired(false);
            builder.Property(e => e.CheckBy).HasColumnName("check_by").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.CheckRemark).HasColumnName("check_remark").HasColumnType("VARCHAR(300)").IsRequired(false);
            builder.Property(e => e.BelongEmpId).HasColumnName("belong_emp_id").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.IsReturnBackPrice).HasColumnName("is_return_back_price").HasColumnType("bit").IsRequired();
            builder.Property(e => e.ReturnBackPrice).HasColumnName("return_back_price").HasColumnType("DECIMAL(12,2)").IsRequired(false);
            builder.Property(e => e.ReturnBackDate).HasColumnName("return_back_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.OtherContentPlatFormOrderId).HasColumnName("other_content_platform_order_id").HasColumnType("VARCHAR(50)").IsRequired(false);
            builder.Property(t => t.IsOldCustomer).HasColumnName("is_old_customer").HasColumnType("bit").IsRequired();
            builder.Property(t => t.IsAcompanying).HasColumnName("is_accompanying").HasColumnType("bit").IsRequired();
            builder.Property(e => e.CommissionRatio).HasColumnName("commission_ratio").HasColumnType("DECIMAL(5,2)").IsRequired();
            builder.Property(e => e.IsRepeatProfundityOrder).HasColumnName("is_repeat_profundity_order").HasColumnType("bit").IsRequired();
            builder.Property(e => e.IsCreateBill).HasColumnName("is_create_bill").HasColumnType("bit").IsRequired();
            builder.Property(e => e.BelongCompany).HasColumnName("belong_company").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(t => t.IsSupportOrder).HasColumnName("is_support_order").HasColumnType("bit").IsRequired();
            builder.Property(e => e.SupportEmpId).HasColumnName("support_emp_id").HasColumnType("int").IsRequired();
            builder.Property(e => e.GetCustomerType).HasColumnName("get_customer_type").HasColumnType("int").IsRequired();
            builder.Property(e => e.CustomerSource).HasColumnName("customer_source").HasColumnType("int").IsRequired();
            builder.Property(e => e.CustomerType).HasColumnName("customer_type").HasColumnType("int").IsRequired();
            builder.Property(e=>e.BelongChannel).HasColumnName("belong_channel").HasColumnType("int").IsRequired();
            builder.Property(e => e.ConsultingContent2).HasColumnName("consulting_content2").HasColumnType("varchar(2000)").IsRequired(false);
            builder.Property(e => e.IsRiBuLuoLiving).HasColumnName("is_ribuluo_living").HasColumnType("bit").IsRequired();
            builder.Property(e => e.OrderBelongCompany).HasColumnName("order_belong_company").HasColumnType("int").IsRequired();

            builder.HasOne(e => e.Contentplatform).WithMany(e => e.ContentPlatformOrderList).HasForeignKey(e=>e.ContentPlateformId);
            builder.HasOne(e => e.LiveAnchor).WithMany(e => e.ContentPlatformOrderList).HasForeignKey(e=>e.LiveAnchorId);
            builder.HasOne(e => e.AmiyaGoodsDemand).WithMany(e => e.ContentPlatformOrderList).HasForeignKey(e=>e.GoodsId);
            builder.HasOne(e => e.HospitalInfo).WithMany(e => e.ContentPlatformOrderList).HasForeignKey(e=>e.AppointmentHospitalId);
        }
    }
}
