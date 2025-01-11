using System;
using System.Collections.Generic;
using System.Text;
using Fx.Amiya.DbModels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fx.Amiya.DbModels.DBModelConfigs
{
    public class ShoppingCartRegistrationConfiguration : IEntityTypeConfiguration<ShoppingCartRegistration>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartRegistration> builder)
        {
            builder.ToTable("tbl_shopping_cart_registration");
            builder.Property(t => t.Id).HasColumnName("id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(t => t.RecordDate).HasColumnName("record_date").HasColumnType("datetime").IsRequired();
            builder.Property(t => t.ContentPlatFormId).HasColumnName("content_plat_form_id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(t => t.LiveAnchorId).HasColumnName("live_anchor_id").HasColumnType("int").IsRequired();
            builder.Property(t => t.LiveAnchorWechatNo).HasColumnName("live_anchor_wechat_no").HasColumnType("varchar(100)").IsRequired(false);
            builder.Property(t => t.CustomerNickName).HasColumnName("customer_nick_name").HasColumnType("varchar(200)").IsRequired(false);
            builder.Property(t => t.Phone).HasColumnName("phone").HasColumnType("varchar(20)").IsRequired();
            builder.Property(t => t.SubPhone).HasColumnName("sub_phone").HasColumnType("varchar(45)").IsRequired(false);
            builder.Property(t => t.Price).HasColumnName("price").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(t => t.ConsultationType).HasColumnName("consultation_type").HasColumnType("int").IsRequired();
            builder.Property(t => t.IsAddWeChat).HasColumnName("is_add_wechat").HasColumnType("bit").IsRequired();
            builder.Property(t => t.IsWriteOff).HasColumnName("is_write_off").HasColumnType("bit").IsRequired();
            builder.Property(t => t.IsConsultation).HasColumnName("is_consultation").HasColumnType("bit").IsRequired();
            builder.Property(t => t.ConsultationDate).HasColumnName("consultation_date").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.IsReturnBackPrice).HasColumnName("is_return_back_price").HasColumnType("bit").IsRequired();
            builder.Property(t => t.Remark).HasColumnName("remark").HasColumnType("varchar(300)").IsRequired(false);
            builder.Property(t => t.CreateDate).HasColumnName("create_date").HasColumnType("datetime").IsRequired();
            builder.Property(t => t.CreateBy).HasColumnName("create_by").HasColumnType("int").IsRequired();
            builder.Property(t => t.AssignEmpId).HasColumnName("assign_emp_id").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.RefundDate).HasColumnName("refund_date").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.RefundReason).HasColumnName("refund_reason").HasColumnType("varchar(500)").IsRequired(false);
            builder.Property(t => t.BadReviewDate).HasColumnName("badreview_date").HasColumnType("datetime").IsRequired(false);
            builder.Property(t => t.BadReviewContent).HasColumnName("badreview_content").HasColumnType("varchar(500)").IsRequired(false);
            builder.Property(t => t.BadReviewReason).HasColumnName("badreview_reason").HasColumnType("varchar(500)").IsRequired(false);
            builder.Property(t => t.IsReContent).HasColumnName("is_recontent").HasColumnType("bit(1)").IsRequired();
            builder.Property(t => t.ReContent).HasColumnName("recontent").HasColumnType("varchar(500)").IsRequired(false);
            builder.Property(t => t.IsBadReview).HasColumnName("is_badreview").HasColumnType("bit(1)").IsRequired();
            builder.Property(t => t.IsCreateOrder).HasColumnName("is_create_order").HasColumnType("bit(1)").IsRequired();
            builder.Property(t => t.IsSendOrder).HasColumnName("is_send_order").HasColumnType("bit(1)").IsRequired();
            builder.Property(t=>t.EmergencyLevel).HasColumnName("emergency_level").HasColumnType("int").HasDefaultValue(0);
            builder.Property(t => t.BaseLiveAnchorId).HasColumnName("base_liveanchor_id").HasColumnType("varchar(50)").IsRequired(false);
            builder.Property(t => t.Source).HasColumnName("source").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.ProductType).HasColumnName("product_type").HasColumnType("int").IsRequired();
            builder.Property(t => t.GetCustomerType).HasColumnName("get_customer_type").HasColumnType("int").IsRequired();
            builder.Property(t => t.ShoppingCartRegistrationCustomerType).HasColumnName("customer_type").HasColumnType("int").IsRequired();
            builder.Property(t => t.BelongChannel).HasColumnName("belong_channel").HasColumnType("int").IsRequired();
            builder.Property(t=>t.AddWechatPicture).HasColumnName("add_wechat_picture").HasColumnType("varchar(100)").IsRequired(false);
            builder.Property(t => t.CluePicture).HasColumnName("clue_picture").HasColumnType("varchar(100)").IsRequired(false);
            builder.Property(t => t.AddWechatEmpId).HasColumnName("add_wechat_empid").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.IsRiBuLuoLiving).HasColumnName("is_ribuluo_living").HasColumnType("bit(1)").IsRequired();
            builder.Property(t => t.IsHistoryCustomerActive).HasColumnName("is_history_customer_active").HasColumnType("bit(1)").IsRequired();
            builder.Property(t => t.ActiveEmployeeId).HasColumnName("active_emp_id").HasColumnType("int").IsRequired(false);
            builder.Property(t => t.CustomerWechatNo).HasColumnName("custoemr_wechat_no").HasColumnType("varchar(100)").IsRequired(false);
            builder.Property(t => t.FromTitle).HasColumnName("from_title").HasColumnType("varchar(400)").IsRequired(false);
            builder.Property(t => t.IsRepeateCreateOrder).HasColumnName("is_repeate_create_order").HasColumnType("bit(1)").IsRequired();
            builder.Property(t => t.BelongCompany).HasColumnName("belong_company").HasColumnType("int").IsRequired();

            builder.HasOne(e => e.Contentplatform).WithMany(e => e.ShoppingCartRegistrationList).HasForeignKey(e => e.ContentPlatFormId);
            builder.HasOne(e => e.LiveAnchor).WithMany(e => e.ShoppingCartRegistrationList).HasForeignKey(e => e.LiveAnchorId);
            builder.HasOne(e => e.AmiyaEmployee).WithMany(e => e.ShoppingCartRegistrationList).HasForeignKey(e => e.CreateBy);
        }
    }
}
