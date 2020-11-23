using Learun.Util;
using System.Collections.Generic;
using System.Data;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public interface DM_UserIBLL
    {
        IEnumerable<dm_userEntity> GetList(string queryJson);

        IEnumerable<dm_userEntity> GetPageList(Pagination pagination, string queryJson);
        DataTable GetPageListByDataTable(Pagination pagination, string queryJson);

        dm_userEntity GetEntity(int? keyValue);

        dm_userEntity GetEntityByPhone(string phone, string appid);

        dm_userEntity GetEntityByCache(int id);

        dm_userEntity GetEntityByInviteCode(string InviteCode, ref dm_user_relationEntity dm_User_RelationEntity);

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
        bool ImportUserInfo(string AppID, string Phone, string RealName, string NickName, string identitycard, string userlevel, string province, string city, string down, string address, string wechat, string parent_id, string parent_nickname, string partners_id);

        void DeleteEntity(int keyValue);

        void SaveEntity(int keyValue, dm_userEntity entity);

        dm_userEntity Login(dm_userEntity entity);

        dm_userEntity Register(dm_userEntity dm_UserEntity, string VerifiCode, string ParentInviteCode, string appid, string SmsMessageID);

        string EncodeInviteCode(int? id);

        dm_userEntity DecodeInviteCode(string InviteCode);

        dynamic SignIn(int userid);
        /// <summary>
        /// �����˻����
        /// </summary>
        /// <param name="user_id">�û�id</param>
        /// <param name="updateprice">������</param>
        /// <param name="updatetype">������� 0����  1����</param>
        /// <param name="remark">������Ϣ</param>
        void UpdateAccountPrice(int user_id, decimal updateprice, int updatetype, string remark);
        /// <summary>
        /// �����û��ȼ�
        /// </summary>
        /// <param name="userids"></param>
        /// <param name="user_level"></param>
        void SetUserLevel(string userids, int user_level);

        /// <summary>
        /// �����Զ���������
        /// </summary>
        /// <param name="User_ID"></param>
        /// <param name="InviteCode"></param>
        void SetInviteCode(int User_ID, string InviteCode);
        #region ��ȡ�ƹ�ͼƬ
        List<string> GetShareImage(int user_id, string appid);
        #endregion

        #region ��ȡ�û�����
        IEnumerable<dm_userEntity> GetParentUser(int user_id);
        IEnumerable<dm_userEntity> GetChildUser(int user_id);

        dm_userEntity GetUserByPartnersID(int partnersid);
        #endregion

        #region ��ȡƽ̨ͳ������
        DataTable GetStaticData1();
        DataTable GetStaticData2();
        DataTable GetStaticData3();
        DataTable GetStaticData4();
        #endregion

        #region ��ȡ��˿����ͳ��
        FansStaticInfoEntity GetFansStaticInfo(int User_ID);
        #endregion

        #region �ж�����ID�Ƿ����
        bool NoExistRelationID(string relationid);
        #endregion

        #region ��ȫû����������û���Ϣ
        void BatchGeneralInviteCode();
        #endregion

        #region ��������Token
        string GeneralRongTokne(int User_ID, string appid);
        #endregion

        #region ��ȡǩ������
        List<SignRecord> GetSignData(int User_ID, ref int sign_Count, ref int finish_sign);
        #endregion

        #region ����ID���ϻ�ȡ�û��б�
        IEnumerable<dm_userEntity> GetUserListByIDS(List<string> ids);
        #endregion
    }
}
