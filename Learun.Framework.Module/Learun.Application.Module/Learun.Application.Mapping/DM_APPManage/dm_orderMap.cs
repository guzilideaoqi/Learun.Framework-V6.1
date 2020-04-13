using Learun.Application.TwoDevelopment.DM_APPManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.Mapping.DM_APPManage
{
    public class dm_orderMap : EntityTypeConfiguration<dm_orderEntity>
    {
        public dm_orderMap()
        {
            #region 表、主键
            //表
            this.ToTable("DM_ORDER");
            //主键
            this.HasKey(t => t.id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
