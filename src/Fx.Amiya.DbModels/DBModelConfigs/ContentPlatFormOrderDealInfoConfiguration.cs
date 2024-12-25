using System;
using System.Collections.Generic;
using System.Text;
using Fx.Amiya.DbModels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fx.Amiya.DbModels.DBModelConfigs
{
    public class ContentPlatFormOrderDealInfoConfiguration : IEntityTypeConfiguration<ContentPlatformOrderDealInfo>
    {
        public void Configure(EntityTypeBuilder<ContentPlatformOrderDealInfo> builder)
        {
            builder.ToTable("tbl_content_platform_order_deal_info");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.ContentPlatFormOrderId).HasColumnName("content_platform_order_id").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(t => t.CreateDate).HasColumnName("create_date").HasColumnType("datetime").IsRequired();
            builder.Property(t => t.IsToHospital).HasColumnName("is_to_hospital").HasColumnType("bit").IsRequired();
            builder.Property(e => e.ToHospitalType).HasColumnName("to_hospital_type").HasColumnType("int").IsRequired();
            builder.Property(e => e.ToHospitalDate).HasColumnName("to_hospital_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.LastDealHospitalId).HasColumnName("last_deal_hospital_id").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.IsDeal).HasColumnName("is_deal").HasColumnType("bit").IsRequired();
            builder.Property(t => t.DealPicture).HasColumnName("deal_picture").HasColumnType("varchar(200)").IsRequired(false);
            builder.Property(t => t.Remark).HasColumnName("remark").HasColumnType("varchar(200)").IsRequired(false);
            builder.Property(t => t.Price).HasColumnName("price").HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(e => e.DealPerformanceType).HasColumnName("deal_performance_type").HasColumnType("int").IsRequired();
            builder.Property(t => t.DealDate).HasColumnName("deal_date").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.OtherAppOrderId).HasColumnName("other_order_id").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(t => t.IsOldCustomer).HasColumnName("is_old_customer").HasColumnType("bit").IsRequired();
            builder.Property(t => t.IsAcompanying).HasColumnName("is_accompanying").HasColumnType("bit").IsRequired();
            builder.Property(e => e.CommissionRatio).HasColumnName("commission_ratio").HasColumnType("DECIMAL(5,2)").IsRequired();
            builder.Property(e => e.CheckState).HasColumnName("check_state").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.CheckPrice).HasColumnName("check_price").HasColumnType("DECIMAL").IsRequired(false);
            builder.Property(e => e.CheckDate).HasColumnName("check_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.InformationPrice).HasColumnName("information_price").HasColumnType("DECIMAL(12,2)").IsRequired(false);
            builder.Property(e => e.SystemUpdatePrice).HasColumnName("system_update_price").HasColumnType("DECIMAL(12,2)").IsRequired(false);
            builder.Property(e => e.SettlePrice).HasColumnName("settle_price").HasColumnType("DECIMAL(12,2)").IsRequired(false);
            builder.Property(e => e.CustomerServiceSettlePrice).HasColumnName("customer_service_settle_price").HasColumnType("DECIMAL(12,2)").IsRequired(false);
            builder.Property(e => e.CheckBy).HasColumnName("check_by").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.CheckRemark).HasColumnName("check_remark").HasColumnType("VARCHAR(300)").IsRequired(false);
            builder.Property(e => e.CreateBy).HasColumnName("create_by").HasColumnType("int").IsRequired();
            builder.Property(t => t.IsReturnBackPrice).HasColumnName("is_return_back_price").HasColumnType("bit").IsRequired();
            builder.Property(e => e.ReturnBackPrice).HasColumnName("return_back_price").HasColumnType("DECIMAL(12,2)").IsRequired(false);
            builder.Property(e => e.ReturnBackDate).HasColumnName("return_back_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(t => t.ReconciliationDocumentsId).HasColumnName("reconciliation_documents_id").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(t => t.IsRepeatProfundityOrder).HasColumnName("is_repeat_profundity_order").HasColumnType("bit").IsRequired();
            builder.Property(e => e.IsCreateBill).HasColumnName("is_create_bill").HasColumnType("bit").IsRequired();
            builder.Property(e => e.BelongCompany).HasColumnName("belong_company").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(e => e.ConsumptionType).HasColumnName("consumption_type").HasColumnType("int").IsRequired(false);
            builder.Property(e => e.LastDealInfoId).HasColumnName("last_deal_info_id").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(e => e.Valid).HasColumnName("valid").HasColumnType("bit").IsRequired();

            builder.HasOne(t => t.ContentPlatFormOrder).WithMany(t => t.ContentPlatformOrderDealInfoList).HasForeignKey(t => t.ContentPlatFormOrderId);
            
        }
    }
}
