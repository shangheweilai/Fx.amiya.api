﻿using System;
using System.Collections.Generic;
using System.Text;
using Fx.Amiya.DbModels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fx.Amiya.DbModels.DBModelConfigs
{
    public class BeforeLivingTikTokDailyTragetConfiguration : IEntityTypeConfiguration<BeforeLivingTikTokDailyTarget>
    {
        public void Configure(EntityTypeBuilder<BeforeLivingTikTokDailyTarget> builder)
        {
            builder.ToTable("tbl_beforeliving_tiktok_daily_target");
            builder.Property(t => t.Id).HasColumnName("id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(t => t.LiveAnchorMonthlyTargetId).HasColumnName("live_anchor_monthly_target_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.OperationEmpId).HasColumnName("operation_empId").HasColumnType("datetime").IsRequired();
            builder.Property(e => e.FlowInvestmentNum).HasColumnName("flow_investment_num").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.SendNum).HasColumnName("send_num").HasColumnType("int").IsRequired();
            builder.Property(e => e.TikTokShowcaseIncome).HasColumnName("tiktok_showcase_income").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.RecordDate).HasColumnName("record_date").HasColumnType("datetime").IsRequired();
            builder.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("datetime").IsRequired();
            builder.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("datetime").IsRequired(false);
            builder.Property(e => e.Valid).HasColumnName("valid").HasColumnType("bit").IsRequired();
            builder.Property(e => e.DeleteDate).HasColumnName("delete_date").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.TikTokClues).HasColumnName("tiktok_clues").HasColumnType("int").IsRequired();
            builder.Property(t => t.TikTokIncreaseFans).HasColumnName("tiktok_increase_fans").HasColumnType("int").IsRequired();
            builder.Property(t => t.TikTokIncreaseFansFees).HasColumnName("tiktok_increase_fans_fees").HasColumnType("decimal(12,2)").IsRequired();
            
            builder.Property(t => t.TikTokShowCaseFee).HasColumnName("tiktok_showcase_fee").HasColumnType("decimal(12,2)").IsRequired();
            builder.HasOne(e => e.LiveAnchorMonthlyTargetBeforeLiving).WithMany(e => e.beforeLivingTikTokDailyTragets).HasForeignKey(e => e.LiveAnchorMonthlyTargetId);
            builder.HasOne(e => e.AmiyaEmployee).WithMany(e => e.beforeLivingTikTokDailyTragets).HasForeignKey(e => e.OperationEmpId);
        }
    }
}
