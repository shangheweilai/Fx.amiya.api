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
    public class FansMeetingDetailsConfiguration : IEntityTypeConfiguration<FansMeetingDetails>
    {
        public void Configure(EntityTypeBuilder<FansMeetingDetails> builder)
        {
            builder.ToTable("tbl_fans_meeting_details");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.FansMeetingId).HasColumnName("fans_meeting_id").HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(e => e.OrderId).HasColumnName("order_id").HasColumnType("VARCHAR(50)").IsRequired(false);
            builder.Property(e => e.AppointmentDate).HasColumnName("appointment_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.AppointmentDetailsDate).HasColumnName("appointment_details_date").HasColumnType("VARCHAR(45)").IsRequired(false);
            builder.Property(e => e.CustomerName).HasColumnName("customer_name").HasColumnType("VARCHAR(100)").IsRequired(false);
            builder.Property(e => e.Phone).HasColumnName("phone").HasColumnType("VARCHAR(30)").IsRequired(false);
            builder.Property(e => e.CustomerQuantity).HasColumnName("customer_quantity").HasColumnType("INT").IsRequired();
            builder.Property(e => e.IsOldCustomer).HasColumnName("is_old_customer").HasColumnType("BIT(1)").IsRequired();
            builder.Property(e => e.AmiyaConsulationId).HasColumnName("amiya_consulation_id").HasColumnType("INT").IsRequired();
            builder.Property(e => e.HospitalConsulationName).HasColumnName("hospital_consulation_name").HasColumnType("VARCHAR(30)").IsRequired(false);
            builder.Property(e => e.City).HasColumnName("city").HasColumnType("VARCHAR(45)").IsRequired(false);
            builder.Property(e => e.TravelInformation).HasColumnName("travel_information").HasColumnType("VARCHAR(500)").IsRequired(false);
            builder.Property(e => e.IsNeedDriver).HasColumnName("is_need_driver").HasColumnType("BIT(1)").IsRequired();
            builder.Property(e => e.HotelPlan).HasColumnName("hotel_plan").HasColumnType("VARCHAR(500)").IsRequired(false);
            builder.Property(e => e.PlanConsumption).HasColumnName("plan_consumption").HasColumnType("decimal(12,2)").IsRequired();
            builder.Property(e => e.Remark).HasColumnName("remark").HasColumnType("VARCHAR(500)").IsRequired(false);
            builder.Property(e => e.CustomerPictureUrl).HasColumnName("customer_picture_url").HasColumnType("VARCHAR(300)").IsRequired(false);
            builder.Property(e => e.IsDeal).HasColumnName("is_deal").HasColumnType("bit").IsRequired(true);
            builder.Property(e => e.IsToHospital).HasColumnName("is_to_hospital").HasColumnType("bit").IsRequired(true);
            builder.Property(e=>e.CumulativeDealPrice).HasColumnName("cumulative_deal_price").HasColumnType("decimal(20,6)").IsRequired(true);

            builder.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.Valid).HasColumnName("valid").HasColumnType("BIT(1)").IsRequired();
            builder.Property(e => e.DeleteDate).HasColumnName("delete_date").HasColumnType("DATETIME").IsRequired(false);

            builder.Property(e => e.UnDealReason).HasColumnName("un_deal_reason").HasColumnType("VARCHAR(500)").IsRequired(false);
            builder.Property(e => e.FollowUpContent).HasColumnName("follow_up_content").HasColumnType("VARCHAR(500)").IsRequired(false);
            builder.Property(e => e.FansMeetingProject).HasColumnName("fans_meeting_project").HasColumnType("VARCHAR(500)").IsRequired(false);
            builder.Property(e => e.NextAppointmentDate).HasColumnName("next_appointment_date").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.IsNeedHospitalHelp).HasColumnName("is_need_hospital_help").HasColumnType("BIT(1)").IsRequired();
            builder.Property(e => e.IsNeedCustomerServiceHelp).HasColumnName("is_need_customerservice_help").HasColumnType("BIT(1)").IsRequired();
            builder.Property(e => e.HospitalMemberCardId).HasColumnName("hospital_member_card_id").HasColumnType("VARCHAR(100)").IsRequired(false);

            builder.HasOne(t => t.FansMeetingInfo).WithMany(t => t.FansMeetingDetailsList).HasForeignKey(t => t.FansMeetingId);
            builder.HasOne(t => t.AmiyaEmployeeInfo).WithMany(t => t.FansMeetingDetailsList).HasForeignKey(t => t.AmiyaConsulationId);
        }
    }
}
