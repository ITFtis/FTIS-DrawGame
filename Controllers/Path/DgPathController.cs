using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DouImp.Controllers.Path
{
    [Dou.Misc.Attr.MenuDef(Id = "DgPath", Name = "抽獎專區", Index = 3, IsOnlyPath = true)]
    public class DgPathController : Controller
    {
        // GET: DgPath
        public ActionResult Index()
        {
            return View();
        }
    }
}