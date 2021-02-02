using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Learun.Application.Web.Controllers
{
    public class DLM_PageController : Controller
    {
        // GET: DLM_Page
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CloseAccount()
        {
            return View();
        }

        public ActionResult CopyInviteCode(string InviteCode)
        {
            ViewBag.InviteCode = InviteCode;
            return View();
        }

        public ActionResult ActivityPage(string token, string appid, string platfotm, string version)
        {
            ViewBag.Token = token;
            ViewBag.AppID = appid;
            ViewBag.Platform = platfotm;
            ViewBag.Version = version;
            return View();
        }
    }
}