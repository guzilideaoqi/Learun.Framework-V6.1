using Learun.Util;
using System;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class DM_UserBLL : DM_UserIBLL
    {
        private DM_UserService dM_UserService = new DM_UserService();

        public IEnumerable<dm_userEntity> GetList(string queryJson)
        {
            try
            {
                return dM_UserService.GetList(queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public IEnumerable<dm_userEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_UserService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public DataTable GetPageListByDataTable(Pagination pagination, string queryJson)
        {
            try
            {
                return dM_UserService.GetPageListByDataTable(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity GetEntity(int? keyValue)
        {
            try
            {
                return dM_UserService.GetEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity GetEntityByPhone(string phone, string appid)
        {
            try
            {
                return dM_UserService.GetEntityByPhone(phone, appid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity GetEntityByCache(int id)
        {
            try
            {
                return dM_UserService.GetEntityByCache(id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity GetPersonInfo(string token) {
            try
            {
                return dM_UserService.GetPersonInfo(token);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// ͨ���������ȡʵ����Ϣ
        /// </summary>
        /// <param name="InviteCode"></param>
        /// <returns></returns>
        public dm_userEntity GetEntityByInviteCode(string InviteCode, ref dm_user_relationEntity dm_User_RelationEntity)
        {
            try
            {
                return dM_UserService.GetEntityByInviteCode(InviteCode, ref dm_User_RelationEntity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// �����û���Ϣ
        /// </summary>
        /// <param name="Phone">�ֻ���</param>
        /// <param name="RealName">��ʵ����</param>
        /// <param name="NickName">�û��ǳ�</param>
        /// <param name="identitycard">���֤��</param>
        /// <param name="userlevel">�û��ȼ� 0��ͨ�û�  1�����û�  2�߼��û�</param>
        /// <param name="province">ʡ��</param>
        /// <param name="city">����</param>
        /// <param name="down">����</param>
        /// <param name="address">��ϸ��ַ</param>
        /// <param name="wechat">΢�ź�</param>
        /// <param name="parent_id">�ϼ�id</param>
        /// <param name="parent_nickname">�ϼ��ǳ�</param>
        /// <param name="partners_id">�ϻ��˱��</param>
        /// <returns></returns>
        public bool ImportUserInfo(string AppID, string Phone, string RealName, string NickName, string identitycard, string userlevel, string province, string city, string down, string address, string wechat, string parent_id, string parent_nickname, string partners_id, string Integral)
        {
            try
            {
                return dM_UserService.ImportUserInfo(AppID, Phone, RealName, NickName, identitycard, userlevel, province, city, down, address, wechat, parent_id, parent_nickname, partners_id,Integral);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        public void DeleteEntity(int keyValue)
        {
            try
            {
                dM_UserService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public void SaveEntity(int keyValue, dm_userEntity entity)
        {
            try
            {
                dM_UserService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity Login(dm_userEntity entity)
        {
            try
            {
                return dM_UserService.Login(entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity LoginByPhone(string phone, string appid) {
            try
            {
                return dM_UserService.LoginByPhone(phone, appid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        #region �û�ע��
        /// <summary>
        /// �ֻ���+��֤��ע��
        /// </summary>
        /// <param name="dm_UserEntity"></param>
        /// <param name="VerifiCode"></param>
        /// <param name="ParentInviteCode"></param>
        /// <param name="appid"></param>
        /// <param name="SmsMessageID"></param>
        /// <returns></returns>
        public dm_userEntity Register(dm_userEntity dm_UserEntity, string VerifiCode, string ParentInviteCode, string appid, string SmsMessageID)
        {
            try
            {
                return dM_UserService.Register(dm_UserEntity, VerifiCode, ParentInviteCode, appid, SmsMessageID);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// �ֻ��ſ�ݵ�¼
        /// </summary>
        /// <param name="dm_UserEntity"></param>
        /// <param name="ParentInviteCode"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public dm_userEntity QuickLogin(dm_userEntity dm_UserEntity, string ParentInviteCode, string appid) {
            try
            {
                return dM_UserService.QuickLogin(dm_UserEntity, ParentInviteCode, appid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        public string EncodeInviteCode(int? id)
        {
            try
            {
                return dM_UserService.EncodeInviteCode(id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity DecodeInviteCode(string InviteCode)
        {
            try
            {
                return dM_UserService.DecodeInviteCode(InviteCode);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dynamic SignIn(int userid)
        {
            try
            {
                return dM_UserService.SignIn(userid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public void SetUserLevel(string userids, int user_level)
        {
            try
            {
                dM_UserService.SetUserLevel(userids, user_level);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
        /// �����Զ���������
        /// </summary>
        /// <param name="User_ID"></param>
        /// <param name="InviteCode"></param>
        public void SetInviteCode(int User_ID, string InviteCode)
        {
            try
            {
                dM_UserService.SetInviteCode(User_ID, InviteCode);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        /// <summary>
		/// �����˻����
		/// </summary>
		/// <param name="user_id">�û�id</param>
		/// <param name="updateprice">������</param>
		/// <param name="updatetype">������� 0����  1����</param>
		/// <param name="remark">������Ϣ</param>
        public void UpdateAccountPrice(int user_id, decimal updateprice, int updatetype, string remark)
        {
            try
            {
                dM_UserService.UpdateAccountPrice(user_id, updateprice, updatetype, remark);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #region ��ȡ�ƹ�ͼƬ
        public List<string> GetShareImage(int user_id, string appid)
        {
            try
            {
                return dM_UserService.GetShareImage(user_id, appid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ��ȡ�û�����
        public IEnumerable<dm_userEntity> GetParentUser(int user_id)
        {
            try
            {
                return dM_UserService.GetParentUser(user_id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        public IEnumerable<dm_userEntity> GetChildUser(int user_id)
        {
            try
            {
                return dM_UserService.GetChildUser(user_id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity GetUserByPartnersID(int partnersid)
        {
            try
            {
                return dM_UserService.GetUserByPartnersID(partnersid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ��ȡƽ̨ͳ������
        public DataTable GetStaticData1()
        {
            try
            {
                return dM_UserService.GetStaticData1();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        public DataTable GetStaticData2()
        {
            try
            {
                return dM_UserService.GetStaticData2();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        public DataTable GetStaticData3()
        {
            try
            {
                return dM_UserService.GetStaticData3();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        public DataTable GetStaticData4()
        {
            try
            {
                return dM_UserService.GetStaticData4();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ��ȡ��˿����ͳ��
        public FansStaticInfoEntity GetFansStaticInfo(int User_ID)
        {
            try
            {
                return dM_UserService.GetFansStaticInfo(User_ID);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region �ж�����ID�Ƿ����
        public bool NoExistRelationID(string relationid,int user_id)
        {
            try
            {
                return dM_UserService.NoExistRelationID(relationid,user_id);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ��ȫû����������û���Ϣ
        public void BatchGeneralInviteCode() {
            try
            {
                dM_UserService.BatchGeneralInviteCode();
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ����Ա���Ȩ��Ϣ
        public void Clear_TB_Relation_Auth(int User_ID) {
            try
            {
                dM_UserService.Clear_TB_Relation_Auth(User_ID);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ��������Token
        public string GeneralRongTokne(int User_ID, string appid)
        {
            try
            {
                return dM_UserService.GeneralRongTokne(User_ID, appid);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }

        public dm_userEntity LoginTokenVerify(string loginToken, string appid, ref string phone) {
            try
            {
                return dM_UserService.LoginTokenVerify(loginToken, appid,ref phone);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ��ȡǩ������
        public List<SignRecord> GetSignData(int User_ID, ref int sign_Count, ref int finish_sign)
        {
            try
            {
                return dM_UserService.GetSignData(User_ID, ref sign_Count, ref finish_sign);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion

        #region ����ID���ϻ�ȡ�û��б�
        public IEnumerable<dm_userEntity> GetUserListByIDS(List<string> ids)
        {
            try
            {
                return dM_UserService.GetUserListByIDS(ids);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                throw ExceptionEx.ThrowBusinessException(ex);
            }
        }
        #endregion
    }
}
