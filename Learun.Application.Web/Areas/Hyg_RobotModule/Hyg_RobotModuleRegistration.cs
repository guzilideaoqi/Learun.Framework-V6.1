using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Areas.Hyg_RobotModule
{
    public class Hyg_RobotModuleRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Hyg_RobotModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Hyg_RobotModule_default",
                "Hyg_RobotModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}