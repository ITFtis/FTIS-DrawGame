using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using DouImp.Models;
using FtisHelperDrawGame.DB.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DouImp.Controllers
{
    [Dou.Misc.Attr.MenuDef(Id = "Activities", Name = "活動", MenuPath = "抽獎專區", Action = "Index", Index = 1, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    [Dou.Misc.Attr.AutoLogger(Status = LoggerEntity.LoggerDataStatus.All, Content = Dou.Misc.Attr.AutoLoggerAttribute.LogContent.AssignContent,
        AssignContent = "KEY:{FKey}, 字串:{FText}")]
    public class ActivitiesController : Dou.Controllers.AGenericModelController<ACTIVITIES>
    {
        private DrawGameContextExt db = new DrawGameContextExt();
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        //protected override IEnumerable<Activity> GetDataDBObject(IModelEntity<ACTIVITIES> dbEntity, params KeyValueParams[] paras)
        //{
        //    var iquery = base.GetDataDBObject(dbEntity, paras);
        //    if (string.IsNullOrEmpty(paras.FirstOrDefault(s => s.key == "sort").value + ""))
        //        iquery = iquery.OrderBy(s => s.GCode);
        //    return iquery;
        //}

        public virtual ActionResult GetData2()
        {

            Dou.Models.DB.IModelEntity<ACTIVITIES> act = GetModelEntity();
            var datas = act.GetAll().OrderBy(a => a.ACTID).OrderByDescending(a => a.NAME).ToList();

            //return Json(new { output = result });

            var opts = this.GetDataManagerOptions();
            opts.datas = datas;

            var jstr = JsonConvert.SerializeObject(datas, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
            Debug.WriteLine(jstr);
            return Content(jstr, "application/json");
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var options = base.GetDataManagerOptions();
            options.editformWindowStyle = "modal";
            options.editformWindowClasses = "modal-xl";
            options.editformSize.height = "fixed";
            options.editformSize.width = "auto";
            //options.GetFiled("DCode_").visibleEdit = false;
            //options.GetFiled("SELECTBG").editable = false;
            options.editformWindowStyle = "showEditformOnly";
            return options;
        }

        /// <summary>
        /// 新增活動
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="objs"></param>
        protected override void AddDBObject(IModelEntity<ACTIVITIES> dbEntity, IEnumerable<ACTIVITIES> objs)
        {
            foreach (var obj in objs)
            {
                obj.BUILDER = Dou.Context.CurrentUser<User>().Id;
                obj.UPDATERS = Dou.Context.CurrentUser<User>().Id;
                obj.UPDATETIME = DateTime.Now;
            }
            base.AddDBObject(dbEntity, objs);
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllActs();
            FtisHelperDrawGame.DB.ActIDSelectItemsClassImp.ResetActs();
        }

        /// <summary>
        /// 刪除活動
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="objs"></param>
        protected override void DeleteDBObject(IModelEntity<ACTIVITIES> dbEntity, IEnumerable<ACTIVITIES> objs)
        {
            //20230904, add by markhong 刪除該活動所有獎項
            foreach (var obj in objs)
            {
                if (obj.Prizes.Count > 0)
                {
                    var me = new Dou.Models.DB.ModelEntity<PRIZE>(this.db);
                    foreach (var pp in obj.Prizes)
                    {
                        //刪除該活動所有獎項
                        me.Delete(pp);
                    }
                }
            }

            base.DeleteDBObject(dbEntity, objs);
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllActs();
            FtisHelperDrawGame.DB.ActIDSelectItemsClassImp.ResetActs();
        }

        /// <summary>
        /// 編輯活動
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="objs"></param>
        protected override void UpdateDBObject(IModelEntity<ACTIVITIES> dbEntity, IEnumerable<ACTIVITIES> objs)
        {
            foreach (var obj in objs)
            {
                obj.UPDATERS = Dou.Context.CurrentUser<User>().Id;
                obj.UPDATETIME = DateTime.Now;
            }
            base.UpdateDBObject(dbEntity, objs);
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllActs();
            FtisHelperDrawGame.DB.ActIDSelectItemsClassImp.ResetActs();
        }

        protected override IModelEntity<ACTIVITIES> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<ACTIVITIES>(this.db);
        }
    }
}