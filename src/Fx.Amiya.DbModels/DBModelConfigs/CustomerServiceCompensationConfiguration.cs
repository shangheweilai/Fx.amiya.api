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
    public class CustomerServiceCompensationConfiguration : IEntityTypeConfiguration<CustomerServiceCompensation>
    {
        public void Configure(EntityTypeBuilder<CustomerServiceCompensation> builder)
        {
            builder.ToTable("tbl_customer_service_compensation");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.OtherPrice).HasColumnName("other_price").HasColumnType("DECIMAL(12,2)").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(e => e.BelongEmpId).HasColumnName("belong_emp_id").HasColumnType("INT").IsRequired();
            builder.Property(e => e.TotalPrice).HasColumnName("total_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.OtherPrice).HasColumnName("other_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.Remark).HasColumnName("remark").HasColumnType("VARCHAR(1000)").IsRequired(false);
            builder.Property(e=>e.Salary).HasColumnName("salary").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.CustomerServicePerformance).HasColumnName("customer_service_performance").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.ToHospitalRate).HasColumnName("to_hospital_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.ToHospitalRateReword).HasColumnName("to_hospital_rate_reword").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.RepeatPurchasesRate).HasColumnName("repeat_purchases_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.RepeatPurchasesRateReword).HasColumnName("repeat_purchases_rate_reword").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.NewCustomerToHospitalReword).HasColumnName("new_customer_to_hospital_reword").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.OldCustomerToHospitalReword).HasColumnName("old_customer_to_hospital_reword").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.TargetFinishReword).HasColumnName("target_finish_reword").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.OtherChargebacks).HasColumnName("other_chargebacks").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.OldTakeNewCustomerPrice).HasColumnName("old_take_newcustomer_price").HasColumnType("decimal(12,2)").IsRequired();


            builder.Property(e => e.AddClueCompletePrice).HasColumnName("addclue_complete_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.AddWechatCompletePrice).HasColumnName("addwechat_complete_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.BeautyAddWechatPrice).HasColumnName("beauty_add_wechat_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.TakeGoodsAddWechatPrice).HasColumnName("take_goods_add_wechat_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.ConsulationCardPrice).HasColumnName("consulation_card_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.ConsulationCardAddWechatPrice).HasColumnName("consulation_card_add_wechat_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.CooperationLiveAnchorSendOrderPrice).HasColumnName("cooperation_live_anchor_send_order_price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.CooperationLiveAnchorToHospitalPrice).HasColumnName("cooperation_live_anchor_to_hospital_price").HasColumnType("decimal(12,2)").IsRequired();

            builder.Property(e => e.SpecialHospitalVisitPrice).HasColumnName("special_hospital_visit_price").HasColumnType("decimal(12,2)").IsRequired();

            builder.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.CreateBy).HasColumnName("create_by").HasColumnType("INT").IsRequired();
            builder.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.Valid).HasColumnName("valid").HasColumnType("BIT(1)").IsRequired();
            builder.Property(e => e.DeleteDate).HasColumnName("delete_date").HasColumnType("DATETIME").IsRequired(false);

            builder.Property(e => e.Verison).HasColumnName("verison").HasColumnType("VARCHAR(45)").IsRequired(false);


            builder.HasOne(t => t.CreateByEmployee).WithMany(t => t.CustomerServiceCompensationCreateByList).HasForeignKey(t => t.CreateBy);
            builder.HasOne(t => t.BelongEmployee).WithMany(t => t.CustomerServiceCompensationBelongEmpList).HasForeignKey(t => t.BelongEmpId);
        }
    }
}
