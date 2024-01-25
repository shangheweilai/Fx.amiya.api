﻿using System;
using System.Collections.Generic;
using System.Text;
using Fx.Amiya.DbModels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fx.Amiya.DbModels.DBModelConfigs
{
    public class LiveAnchorMonthlyTargetBeforeLivingConfiguration : IEntityTypeConfiguration<LiveAnchorMonthlyTargetBeforeLiving>
    {
        public void Configure(EntityTypeBuilder<LiveAnchorMonthlyTargetBeforeLiving> builder)
        {
            builder.ToTable("tbl_liveanchor_monthly_target_before_living");
            builder.Property(t => t.Id).HasColumnName("id").HasColumnType("varchar(120)").IsRequired();
            builder.Property(t => t.Year).HasColumnName("year").HasColumnType("int").IsRequired();
            builder.Property(t => t.Month).HasColumnName("month").HasColumnType("int").IsRequired();
            builder.Property(t => t.MonthlyTargetName).HasColumnName("monthly_target_name").HasColumnType("varchar(200)").IsRequired(false);
            builder.Property(t => t.LiveAnchorId).HasColumnName("live_anchor_id").HasColumnType("int").IsRequired();

            builder.Property(t => t.ZhihuReleaseTarget).HasColumnName("zhihu_release_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeZhihuRelease).HasColumnName("cumulative_zhihu_release").HasColumnType("int").IsRequired();
            builder.Property(t => t.ZhihuReleaseCompleteRate).HasColumnName("zhihu_release_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.ZhihuFlowinvestmentTarget).HasColumnName("zhihu_flow_investment_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeZhihuFlowinvestment).HasColumnName("cumulative_zhihu_flow_investment").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.ZhihuFlowinvestmentCompleteRate).HasColumnName("zhihu_flow_investment_complete_rate").HasColumnType("decimal(12,2)").IsRequired();

            builder.Property(t => t.SinaWeiBoReleaseTarget).HasColumnName("sina_weibo_release_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeSinaWeiBoRelease).HasColumnName("cumulative_sina_weibo_release").HasColumnType("int").IsRequired();
            builder.Property(t => t.SinaWeiBoReleaseCompleteRate).HasColumnName("sina_weibo_release_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.SinaWeiBoFlowinvestmentTarget).HasColumnName("sina_weibo_flow_investment_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeSinaWeiBoFlowinvestment).HasColumnName("cumulative_sina_weibo_flow_investment").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.SinaWeiBoFlowinvestmentCompleteRate).HasColumnName("sina_weibo_flow_investment_complete_rate").HasColumnType("decimal(12,2)").IsRequired();

            builder.Property(t => t.TikTokReleaseTarget).HasColumnName("tiktok_release_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeTikTokRelease).HasColumnName("cumulative_tiktok_release").HasColumnType("int").IsRequired();
            builder.Property(t => t.TikTokReleaseCompleteRate).HasColumnName("tiktok_release_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokFlowinvestmentTarget).HasColumnName("tik_tok_flow_investment_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeTikTokFlowinvestment).HasColumnName("cumulative_tik_tok_flow_investment").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokFlowinvestmentCompleteRate).HasColumnName("tik_tok_flow_investment_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokShowcaseIncomeTarget).HasColumnName("tik_tok_showcase_income_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeTikTokShowcaseIncome).HasColumnName("cumulative_tik_tok_showcase_income").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokShowcaseIncomeCompleteRate).HasColumnName("tik_tok_showcase_income_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokCluesTarget).HasColumnName("tiktok_clues_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeTikTokClues).HasColumnName("cumulative_tiktok_clues").HasColumnType("int").IsRequired();
            builder.Property(t => t.TikTokCluesCompleteRate).HasColumnName("tiktok_clues_complete_rate").HasColumnType("decimal(12,2)").IsRequired();           
            builder.Property(t => t.TikTokIncreaseFansTarget).HasColumnName("tiktok_increase_fans_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeTikTokIncreaseFans).HasColumnName("cumulative_tiktok_increase_fans").HasColumnType("int").IsRequired();
            builder.Property(t => t.TikTokIncreaseFanseCompleteRate).HasColumnName("tiktok_increase_fans_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokIncreaseFansFeesTarget).HasColumnName("tiktok_increase_fans_fees_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeTikTokIncreaseFansFees).HasColumnName("cumulative_tiktok_increase_fans_fees").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokIncreaseFansFeesCompleteRate).HasColumnName("tiktok_increase_fans_fees_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            
            builder.Property(t => t.TikTokShowCaseFeeTarget).HasColumnName("tiktok_showcase_fee_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeTikTokShowCaseFee).HasColumnName("cumulative_tiktok_showcase_fee").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.TikTokShowCaseFeeCompleteRate).HasColumnName("tiktok_showcase_fee_complete_rate").HasColumnType("decimal(12,2)").IsRequired();


            builder.Property(t => t.XiaoHongShuReleaseTarget).HasColumnName("xiaohongshu_release_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeXiaoHongShuRelease).HasColumnName("cumulative_xiaohongshu_release").HasColumnType("int").IsRequired();
            builder.Property(t => t.XiaoHongShuReleaseCompleteRate).HasColumnName("xiaohongshu_release_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuFlowinvestmentTarget).HasColumnName("xiaohongshu_flow_investment_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeXiaoHongShuFlowinvestment).HasColumnName("cumulative_xiaohongshu_flow_investment").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuFlowinvestmentCompleteRate).HasColumnName("xiaohongshu_flow_investment_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            
            builder.Property(t => t.XiaoHongShuShowcaseIncomeTarget).HasColumnName("xiaohongshu_showcase_income_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeXiaoHongShuShowcaseIncome).HasColumnName("cumulative_xiaohongshu_showcase_income").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuShowcaseIncomeCompleteRate).HasColumnName("xiaohongshu_showcase_income_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuCluesTarget).HasColumnName("xiaohongshu_clues_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeXiaoHongShuClues).HasColumnName("cumulative_xiaohongshu_clues").HasColumnType("int").IsRequired();
            builder.Property(t => t.XiaoHongShuCluesCompleteRate).HasColumnName("xiaohongshu_clues_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuIncreaseFansTarget).HasColumnName("xiaohongshu_increase_fans_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeXiaoHongShuIncreaseFans).HasColumnName("cumulative_xiaohongshu_increase_fans").HasColumnType("int").IsRequired();
            builder.Property(t => t.XiaoHongShuIncreaseFanseCompleteRate).HasColumnName("xiaohongshu_increase_fans_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuIncreaseFansFeesTarget).HasColumnName("xiaohongshu_increase_fans_fees_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeXiaoHongShuIncreaseFansFees).HasColumnName("cumulative_xiaohongshu_increase_fans_fees").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuIncreaseFansFeesCompleteRate).HasColumnName("xiaohongshu_increase_fans_fees_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            
            builder.Property(t => t.XiaoHongShuShowCaseFeeTarget).HasColumnName("xiaohongshu_showcase_fee_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeXiaoHongShuShowCaseFee).HasColumnName("cumulative_xiaohongshu_showcase_fee").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.XiaoHongShuShowCaseFeeCompleteRate).HasColumnName("xiaohongshu_showcase_fee_complete_rate").HasColumnType("decimal(12,2)").IsRequired();

            builder.Property(t => t.VideoReleaseTarget).HasColumnName("video_release_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeVideoRelease).HasColumnName("cumulative_video_release").HasColumnType("int").IsRequired();
            builder.Property(t => t.VideoReleaseCompleteRate).HasColumnName("video_release_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoFlowinvestmentTarget).HasColumnName("video_flow_investment_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeVideoFlowinvestment).HasColumnName("cumulative_video_flow_investment").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoFlowinvestmentCompleteRate).HasColumnName("video_flow_investment_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            
            builder.Property(t => t.VideoShowcaseIncomeTarget).HasColumnName("video_showcase_income_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeVideoShowcaseIncome).HasColumnName("cumulative_video_showcase_income").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoShowcaseIncomeCompleteRate).HasColumnName("video_showcase_income_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoCluesTarget).HasColumnName("video_clues_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeVideoClues).HasColumnName("cumulative_video_clues").HasColumnType("int").IsRequired();
            builder.Property(t => t.VideoCluesCompleteRate).HasColumnName("video_clues_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoIncreaseFansTarget).HasColumnName("video_increase_fans_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeVideoIncreaseFans).HasColumnName("cumulative_video_increase_fans").HasColumnType("int").IsRequired();
            builder.Property(t => t.VideoIncreaseFanseCompleteRate).HasColumnName("video_increase_fans_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoIncreaseFansFeesTarget).HasColumnName("video_increase_fans_fees_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeVideoIncreaseFansFees).HasColumnName("cumulative_video_increase_fans_fees").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoIncreaseFansFeesCompleteRate).HasColumnName("video_increase_fans_fees_complete_rate").HasColumnType("decimal(12,2)").IsRequired();
            
            builder.Property(t => t.VideoShowCaseFeeTarget).HasColumnName("video_showcase_fee_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeVideoShowCaseFee).HasColumnName("cumulative_video_showcase_fee").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.VideoShowCaseFeeCompleteRate).HasColumnName("video_showcase_fee_complete_rate").HasColumnType("decimal(12,2)").IsRequired();

            builder.Property(t => t.ReleaseTarget).HasColumnName("release_target").HasColumnType("int").IsRequired();
            builder.Property(t => t.CumulativeRelease).HasColumnName("cumulative_release").HasColumnType("int").IsRequired();
            builder.Property(t => t.ReleaseCompleteRate).HasColumnName("release_complete_rate").HasColumnType("decimal(12,2)").IsRequired();

            builder.Property(t => t.FlowInvestmentTarget).HasColumnName("flow_investment_target").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.CumulativeFlowInvestment).HasColumnName("cumulative_flow_investment").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.FlowInvestmentCompleteRate).HasColumnName("flow_investment_complete_rate").HasColumnType("decimal(12,2)").IsRequired();


            builder.Property(t => t.CreateDate).HasColumnName("create_date").HasColumnType("datetime").IsRequired();
            builder.HasOne(e => e.LiveAnchor).WithMany(e => e.LiveAnchorMonthlyTargetBeforeLivings).HasForeignKey(e => e.LiveAnchorId);
        }
    }
}
