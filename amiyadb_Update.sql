

------------------------------------余建明 2024/10/15 BEGIN--------------------------------------
--小黄车登记列表新增是否为历史顾客激活，激活人选项
ALTER TABLE `amiyadb`.`tbl_shopping_cart_registration` 
ADD COLUMN `is_history_customer_active` BIT(1) NOT NULL DEFAULT b'0' AFTER `is_ribuluo_living`,
ADD COLUMN `active_emp_id` INT NULL AFTER `is_history_customer_active`;

--小黄车登记列表新增来源词条和客户微信号
ALTER TABLE `amiyadb`.`tbl_shopping_cart_registration` 
ADD COLUMN `custoemr_wechat_no` VARCHAR(100) NULL AFTER `sub_phone`,
ADD COLUMN `from_title` VARCHAR(400) NULL AFTER `active_emp_id`;
------------------------------------余建明 2024/10/17 END--------------------------------------



------------------------------------王健 2024/10/17 BEGIN--------------------------------------

--直播前月度目标添加负责人
ALTER TABLE `tbl_liveanchor_monthly_target_before_living`
	ADD COLUMN `owner_id` INT NULL AFTER `video_showcase_fee_complete_rate`;

------------------------------------王健 2024/10/17 END--------------------------------------


------------------------------------余建明 2024/10/21 BEGIN--------------------------------------
--助理提取薪资模块新增提成金额
ALTER TABLE `amiyadb`.`tbl_customer_service_check_performance` 
ADD COLUMN `performance_commission` DECIMAL(12,2) NOT NULL DEFAULT 0.00 AFTER `delete_date`,
ADD COLUMN `performance_commission_check` DECIMAL(12,2) NOT NULL DEFAULT 0.00 AFTER `performance_commission`;
------------------------------------余建明 2024/10/21 END--------------------------------------


------------------------------------余建明 2024/10/28 BEGIN--------------------------------------
--助理薪资数据加入版本号
ALTER TABLE `amiyadb`.`tbl_customer_service_compensation` 
ADD COLUMN `verison` VARCHAR(45) NULL AFTER `special_hospital_visit_price`;

--将当前助理薪资默认为1.0版本
update  amiyadb.tbl_customer_service_compensation set verison="1.0";
------------------------------------余建明 2024/10/28 END--------------------------------------

--小黄车列表新增是否重复下单（针对直播中面诊卡）
ALTER TABLE `amiyadb`.`tbl_shopping_cart_registration` 
ADD COLUMN `is_repeate_create_order` BIT(1) NOT NULL DEFAULT b'0' AFTER `from_title`;

--成交情况表加入上一条成交单id
ALTER TABLE `amiyadb`.`tbl_content_platform_order_deal_info` 
ADD COLUMN `last_deal_info_id` VARCHAR(50) NULL AFTER `consumption_type`;

--成交情况列表加入是否有效数据列
ALTER TABLE `amiyadb`.`tbl_content_platform_order_deal_info` 
ADD COLUMN `valid` BIT(1) NOT NULL AFTER `last_deal_info_id`;

update tbl_content_platform_order_deal_info set valid=true;


--粉丝见面会详情新增是否需助理跟进
ALTER TABLE `amiyadb`.`tbl_fans_meeting_details` 
ADD COLUMN `is_need_customerservice_help` BIT(1) NOT NULL AFTER `is_need_hospital_help`;
--------------------------------------------------------------------------------------------------------以上部分已更新到线上--------------------------------------
