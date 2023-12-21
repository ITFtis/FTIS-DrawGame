using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DouImp.Controllers.Path
{
    [Dou.Misc.Attr.MenuDef(Id = "HistoryPath", Name = "歷史專區", Index = 4, IsOnlyPath = true)]
    public class HistoryPathController : Controller
    {
        // GET: DgPath
        public ActionResult Index()
        {
            return View();
        }
    }
}