using Dapper;
using Learun.DataBase.Repository;
using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    /// <summary>
    /// 版 本 Learun-ADMS V6.1.6.0 力软敏捷开发框架
    /// Copyright (c) 2013-2017 上海力软信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2020-04-10 13:38
    /// 描 述：开通代理记录
    /// </summary>
    public class DM_Alipay_RecordService : RepositoryFactory
    {
        DM_UserService dM_UserService = new DM_UserService();
        DM_UserRelationService dM_UserRelationService = new DM_UserRelationService();
        DM_BaseSettingService dM_BaseSettingService = new DM_BaseSettingService();


        #region 构造函数和属性

        private string fieldSql;
        public DM_Alipay_RecordService()
        {
            fieldSql = @"
                t.id,
                t.out_trade_no,
                t.user_id,
                t.alipay_trade_no,
                t.total_amount,
                t.alipay_status,
                t.templateid,
                t.gmt_create,
                t.gmt_payment,
                t.notify_time,
                t.createtime,
                t.updatetime,
                t.seller_id,
                t.notify_id,
                t.subject
            ";
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_alipay_recordEntity> GetList(string queryJson)
        {
            try
            {
                //参考写法
                //var queryParam = queryJson.ToJObject();
                // 虚拟参数
                //var dp = new DynamicParameters(new { });
                //dp.Add("startTime", queryParam["StartTime"].ToDate(), DbType.DateTime);
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_alipay_record t ");
                return this.BaseRepository("dm_data").FindList<dm_alipay_recordEntity>(strSql.ToString());
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取列表分页数据
        /// <param name="pagination">分页参数</param>
        /// <summary>
        /// <returns></returns>
        public IEnumerable<dm_alipay_recordEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append("SELECT ");
                strSql.Append(fieldSql);
                strSql.Append(" FROM dm_alipay_record t ");
                return this.BaseRepository("dm_data").FindList<dm_alipay_recordEntity>(strSql.ToString(), pagination);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 获取实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public dm_alipay_recordEntity GetEntity(int keyValue)
        {
            try
            {
                return this.BaseRepository("dm_data").FindEntity<dm_alipay_recordEntity>(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteEntity(int keyValue)
        {
            try
            {
                this.BaseRepository("dm_data").Delete<dm_alipay_recordEntity>(t => t.id == keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(int keyValue, dm_alipay_recordEntity entity)
        {
            try
            {
                if (keyValue > 0)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository("dm_data").Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository("dm_data").Insert(entity);
                }
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }

        #endregion

        #region 支付回调中开通代理(并完成返佣)
        private List<dm_accountdetailEntity> dm_AccountdetailEntities = new List<dm_accountdetailEntity>();
        private List<dm_userEntity> calculateComissionEntities = new List<dm_userEntity>();
        /// <summary>
        /// 支付回调中开通代理(并完成返佣)
        /// </summary>
        /// <param name="dm_Alipay_RecordEntity"></param>
        public void OpenAgent(dm_alipay_recordEntity dm_Alipay_RecordEntity)
        {
            IRepository db = null;
            try
            {

                /*
                 * 根据外部交易单号获取订单记录
                 * 更改用户状态并完成返利
                 * 1、一级代理返利(代理等级为高级才可得到)
                 * 2、二级代理返利(代理等级为高级才可得到)
                 * 3、一级合伙人(不校验代理等级,可能为后台开通)
                 * 4、二级合伙人(不校验代理等级,可能为后台开通)
                 */
                dm_alipay_recordEntity dm_Alipay_RecordEntity_old = this.BaseRepository("dm_data").FindEntity<dm_alipay_recordEntity>(t => t.out_trade_no == dm_Alipay_RecordEntity.out_trade_no);

                if (dm_Alipay_RecordEntity_old.templateid == 99)//余额充值
                {
                    dm_userEntity dm_UserEntity = dM_UserService.GetEntity(dm_Alipay_RecordEntity_old.user_id);
                    if (dm_UserEntity.IsEmpty())
                        throw new Exception("用户信息异常!");
                    ///修改账户余额
                    dm_UserEntity.accountprice += dm_Alipay_RecordEntity_old.total_amount;//充值成功后的账户余额
                    dm_UserEntity.Modify(dm_UserEntity.id);

                    dm_accountdetailEntity dm_AccountdetailEntity = GeneralAccountDetail(dm_Alipay_RecordEntity_old.user_id, 21, "余额充值", "账户充值", (decimal)dm_Alipay_RecordEntity_old.total_amount, dm_UserEntity.accountprice);

                    db = BaseRepository("dm_data").BeginTrans();
                    db.Update(dm_UserEntity);
                    db.Insert(dm_AccountdetailEntity);
                    db.Commit();
                }
                else
                {//代理开通
                    dm_AccountdetailEntities.Clear();
                    calculateComissionEntities.Clear();

                    decimal one_agent_commission = 0, two_agent_commission = 0, one_partners_commission = 0, two_partners_commission = 0;

                    dm_userEntity currentUser = null, one_User = null, two_User = null, one_partners = null, two_partners = null;

                    ///如果老的记录是已支付状态则不需要再执行修改和返利
                    if (dm_Alipay_RecordEntity_old.alipay_status.ToUpper() == "TRADE_SUCCESS")
                        return;

                    if (dm_Alipay_RecordEntity_old.IsEmpty())
                        throw new Exception("根据外部交易单号未能查询到支付记录,当前传入外部交易单号" + dm_Alipay_RecordEntity.out_trade_no);
                    dm_Alipay_RecordEntity.Modify(dm_Alipay_RecordEntity_old.id);//更改交易信息


                    #region 获取上下级关系
                    IEnumerable<dm_user_relationEntity> userRelationList = dM_UserRelationService.GetParentRelation(dm_Alipay_RecordEntity_old.user_id);
                    #endregion

                    #region 获取上下级关系的用户信息
                    IEnumerable<dm_userEntity> userList = dM_UserService.GetParentUser(dm_Alipay_RecordEntity_old.user_id);
                    #endregion

                    #region 更改当前用户等级
                    currentUser = userList.Where(t => t.id == dm_Alipay_RecordEntity_old.user_id).FirstOrDefault();
                    if (currentUser.IsEmpty())
                        throw new Exception("用户信息异常!");
                    //初级代理
                    if (dm_Alipay_RecordEntity_old.templateid == 1)
                    {
                        currentUser.userlevel = 1;
                    }
                    //高级代理
                    else if (dm_Alipay_RecordEntity_old.templateid == 2)
                    {
                        currentUser.userlevel = 2;
                    }
                    //升级代理
                    else if (dm_Alipay_RecordEntity.templateid == 3)
                    {
                        currentUser.userlevel = 2;
                    }
                    calculateComissionEntities.Add(currentUser);
                    #endregion

                    #region 增加开通代理消息记录
                    dm_messagerecordEntity dm_MessagerecordEntity = new dm_messagerecordEntity();
                    dm_MessagerecordEntity.user_id = dm_Alipay_RecordEntity_old.user_id;
                    dm_MessagerecordEntity.messagetitle = "开通代理";
                    dm_MessagerecordEntity.messagecontent = "代理开通成功，发展下级可享受永久提成!";
                    dm_MessagerecordEntity.messagetype = 1;
                    dm_MessagerecordEntity.createtime = DateTime.Now;
                    dm_MessagerecordEntity.isread = 0;
                    #endregion

                    dm_basesettingEntity dm_BasesettingEntity = dM_BaseSettingService.GetEntityByCache(currentUser.appid);

                    #region 更改一级账户余额及明细
                    dm_user_relationEntity dm_User_RelationEntity_one = userRelationList.Where(t => t.user_id == dm_Alipay_RecordEntity_old.user_id).FirstOrDefault();
                    one_User = userList.Where(t => t.id == dm_User_RelationEntity_one.parent_id).FirstOrDefault();
                    if (!one_User.IsEmpty())
                    {
                        if (one_User.userlevel == 2)
                        {//高级代理才能享受代理提成
                            one_agent_commission = ConvertComission(dm_BasesettingEntity.openagent_one * dm_Alipay_RecordEntity_old.total_amount);
                            if (one_agent_commission > 0)
                            {
                                one_User = CalculateComission(one_User.id, one_agent_commission, one_User.accountprice);
                                dm_AccountdetailEntities.Add(GeneralAccountDetail(one_User.id, 6, "下级开通代理", "您的下级《" + currentUser.nickname + "》开通代理成功,奖励已到账,继续努力哟!", one_agent_commission, one_User.accountprice));
                            }

                            #region 更改二级账户余额及明细
                            dm_user_relationEntity dm_User_RelationEntity_two = userRelationList.Where(t => t.user_id == one_User.id).FirstOrDefault();
                            two_User = userList.Where(t => t.id == dm_User_RelationEntity_two.parent_id).FirstOrDefault();
                            if (!two_User.IsEmpty())
                            {
                                two_agent_commission = ConvertComission(dm_BasesettingEntity.openagent_two * dm_Alipay_RecordEntity_old.total_amount);
                                if (two_agent_commission > 0)
                                {
                                    two_User = CalculateComission(two_User.id, two_agent_commission, two_User.accountprice);
                                    dm_AccountdetailEntities.Add(GeneralAccountDetail(two_User.id, 7, "二级开通代理", "您的二级《" + currentUser.nickname + "》开通代理成功,奖励已到账,继续努力哟!", two_agent_commission, two_User.accountprice));
                                }
                            }
                            #endregion

                        }
                    }
                    #endregion

                    #region 获取当前用户所属合伙人(一级合伙人)
                    one_partners = dM_UserService.GetUserByPartnersID(dm_User_RelationEntity_one.partners_id);
                    if (!one_partners.IsEmpty())
                    {
                        one_partners_commission = ConvertComission(dm_BasesettingEntity.openagent_one_partners * dm_Alipay_RecordEntity_old.total_amount);
                        if (one_partners_commission > 0)
                        {
                            one_partners = CalculateComission(one_partners.id, one_partners_commission, one_partners.accountprice);
                            dm_AccountdetailEntities.Add(GeneralAccountDetail(one_partners.id, 8, "团队成员开通代理", "您的团队成员《" + currentUser.nickname + "》开通代理成功,奖励已到账,继续努力哟!", one_partners_commission, one_partners.accountprice));
                        }

                        #region 二级合伙人
                        dm_user_relationEntity dm_User_RelationEntity_one_partners = dM_UserRelationService.GetEntityByUserID(one_partners.id);
                        two_partners = dM_UserService.GetEntityByCache(dm_User_RelationEntity_one_partners.parent_id);
                        if (!two_partners.IsEmpty())
                        {
                            if (two_partners.partnersstatus == 1)
                            {//二级用户为合伙人时才进行返利
                                two_partners_commission = ConvertComission(dm_BasesettingEntity.openagent_two_partners * dm_Alipay_RecordEntity_old.total_amount);
                                if (two_partners_commission > 0)
                                {
                                    two_partners = CalculateComission(two_partners.id, two_partners_commission, two_partners.accountprice);
                                    dm_AccountdetailEntities.Add(GeneralAccountDetail(two_partners.id, 9, "下级团队成员开通代理", "您的下级团队成员《" + currentUser.nickname + "》开通代理成功,奖励已到账,继续努力哟!", two_partners_commission, two_partners.accountprice));
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion


                    if (calculateComissionEntities.Count > 0)
                    {
                        //必须加上这个变量,用于清除当前返利账户的余额
                        foreach (var item in calculateComissionEntities)
                        {
                            item.Modify(item.id);
                        }
                        db = BaseRepository("dm_data").BeginTrans();
                        db.Update(dm_Alipay_RecordEntity);//修改原有的支付宝支付记录
                        db.Insert(dm_MessagerecordEntity);//增加消息记录
                        db.Insert(dm_AccountdetailEntities);//增加账户余额明细
                        db.Update(calculateComissionEntities);//批量修改用户信息
                        db.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Rollback();
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowServiceException(ex);
                }
            }
        }
        private decimal ConvertComission(decimal? comissionamount)
        {
            return Math.Round(Convert.ToDecimal(comissionamount / 100m), 2);
        }

        private dm_accountdetailEntity GeneralAccountDetail(int? user_id, int type, string title, string remark, decimal billdetailCommission, decimal? currentaccountprice)
        {
            return new dm_accountdetailEntity
            {
                createtime = DateTime.Now,
                remark = remark,
                stepvalue = billdetailCommission,
                currentvalue = currentaccountprice,
                title = title,
                type = type,
                user_id = user_id
            };
        }

        private dm_userEntity CalculateComission(int? user_id, decimal? commission, decimal? currentaccount)
        {
            dm_userEntity calculateComissionEntity = calculateComissionEntities.Where((dm_userEntity t) => t.id == user_id).FirstOrDefault();
            if (calculateComissionEntity.IsEmpty())
            {
                calculateComissionEntity = new dm_userEntity
                {
                    id = user_id,
                    accountprice = currentaccount + commission
                };
                calculateComissionEntities.Add(calculateComissionEntity);
            }
            else
            {
                dm_userEntity dm_userEntity = calculateComissionEntity;
                dm_userEntity.accountprice += commission;
            }
            return calculateComissionEntity;
        }
        #endregion
    }
}
