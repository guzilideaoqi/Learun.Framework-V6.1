using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.Base.SystemModule.Log
{
    public class LogEntity
    {
        [Column("F_LOGID")]
        public string F_LogId
        {
            get;
            set;
        }
        [Column("F_CATEGORYID")]
        public int? F_CategoryId
        {
            get;
            set;
        }
        [Column("F_SOURCEOBJECTID")]
        public string F_SourceObjectId
        {
            get;
            set;
        }
        [Column("F_SOURCECONTENTJSON")]
        public string F_SourceContentJson
        {
            get;
            set;
        }
        [Column("F_OPERATETIME")]
        public DateTime? F_OperateTime
        {
            get;
            set;
        }
        [Column("F_OPERATEUSERID")]
        public string F_OperateUserId
        {
            get;
            set;
        }
        [Column("F_OPERATEACCOUNT")]
        public string F_OperateAccount
        {
            get;
            set;
        }
        [Column("F_OPERATETYPEID")]
        public string F_OperateTypeId
        {
            get;
            set;
        }
        [Column("F_OPERATETYPE")]
        public string F_OperateType
        {
            get;
            set;
        }
        [Column("F_MODULE")]
        public string F_Module
        {
            get;
            set;
        }
        [Column("F_IPADDRESS")]
        public string F_IPAddress
        {
            get;
            set;
        }
        [Column("F_IPADDRESSNAME")]
        public string F_IPAddressName
        {
            get;
            set;
        }
        [Column("F_HOST")]
        public string F_Host
        {
            get;
            set;
        }
        [Column("F_BROWSER")]
        public string F_Browser
        {
            get;
            set;
        }
        [Column("F_EXECUTERESULT")]
        public int? F_ExecuteResult
        {
            get;
            set;
        }
        [Column("F_EXECUTERESULTJSON")]
        public string F_ExecuteResultJson
        {
            get;
            set;
        }
        [Column("F_DESCRIPTION")]
        public string F_Description
        {
            get;
            set;
        }
        [Column("F_DELETEMARK")]
        public int? F_DeleteMark
        {
            get;
            set;
        }
        [Column("F_ENABLEDMARK")]
        public int? F_EnabledMark
        {
            get;
            set;
        }
    }
}
