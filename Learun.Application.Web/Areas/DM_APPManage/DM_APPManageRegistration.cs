using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.DM_APPManage
{
    public class DM_APPManageRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DM_APPManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DM_APPManage_default",
                "DM_APPManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}