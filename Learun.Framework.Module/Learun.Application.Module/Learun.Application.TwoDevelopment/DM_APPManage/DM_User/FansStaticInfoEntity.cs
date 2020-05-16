using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learun.Application.TwoDevelopment.DM_APPManage
{
    public class FansStaticInfoEntity
    {
        public string Parent_WX { get; set; }
        public string Parent_NickName { get; set; }
        public string HeadPic { get; set; }

        public int? MyChildCount { get; set; }
        public int? MySonChildCount { get; set; }
        public int? MyPartnersCount { get; set; }
    }
}
