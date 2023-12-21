using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using DouHelper;
using DouImp.Models;
using FtisHelperDrawGame.DB;
using FtisHelperDrawGame.DB.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DouImp.Controllers
{
    //[Dou.Misc.Attr.MenuDef(Id = "Prize", Name = "獎項清單", MenuPath = "抽獎專區", Action = "Index", Index = 2, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    public class PrizeController : Dou.Controllers.AGenericModelController<PRIZE>
    {
        private DrawGameContextExt db = new DrawGameContextExt();
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult RefreshPage(PRIZE objs)
        {
            var f = objs;

            Dou.Models.DB.IModelEntity<PRIZE> model = new Dou.Models.DB.ModelEntity<PRIZE>(this.db);

            //var datas = model.GetAll(a => a.ACTID == f.ACTID
            //                    && a.NAME == f.NAME
            //                    && a.REMARK == f.REMARK
            //                    ).ToList();
            var datas = model.GetAll().Where(a => a.ACTID == f.ACTID).ToList();

            var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<PRIZE>();

            //全部欄位排序
            foreach (var field in opts.fields)
                field.sortable = true;

            opts.datas = datas;

            var jstr = JsonConvert.SerializeObject(datas, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            return Content(jstr, "application/json");
        }

        //protected override IEnumerable<Activity> GetDataDBObject(IModelEntity<ACTIVITIES> dbEntity, params KeyValueParams[] paras)
        //{
        //    var iquery = base.GetDataDBObject(dbEntity, paras);
        //    if (string.IsNullOrEmpty(paras.FirstOrDefault(s => s.key == "sort").value + ""))
        //        iquery = iquery.OrderBy(s => s.GCode);
        //    return iquery;
        //}
        public override DataManagerOptions GetDataManagerOptions()
        {
            var options = base.GetDataManagerOptions();
            options.editformWindowStyle = "modal";
            return options;
        }

        protected override void AddDBObject(IModelEntity<PRIZE> dbEntity, IEnumerable<PRIZE> objs)
        {
            base.AddDBObject(dbEntity, objs);
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetPrize();
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllPrizes();
            FtisHelperDrawGame.DB.PrizesSelectItemsClassImp.ResetPrizes();
        }

        protected override void UpdateDBObject(IModelEntity<PRIZE> dbEntity, IEnumerable<PRIZE> objs)
        {
            base.UpdateDBObject(dbEntity, objs);
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetPrize();
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllPrizes();
            FtisHelperDrawGame.DB.PrizesSelectItemsClassImp.ResetPrizes();
        }

        protected override void DeleteDBObject(IModelEntity<PRIZE> dbEntity, IEnumerable<PRIZE> objs)
        {
            base.DeleteDBObject(dbEntity, objs);
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetPrize();
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllPrizes();
            FtisHelperDrawGame.DB.PrizesSelectItemsClassImp.ResetPrizes();
        }

        protected override IModelEntity<PRIZE> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<PRIZE>(this.db);
        }
    }
}