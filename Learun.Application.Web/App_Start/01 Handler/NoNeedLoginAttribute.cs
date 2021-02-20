using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Learun.Application.Web.App_Start._01_Handler
{
    public class NoNeedLoginAttribute : Attribute
    {
        public NoNeedLoginAttribute() { }
    }
}