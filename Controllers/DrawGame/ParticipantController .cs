using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using DouHelper;
using FtisDrawGame.Models;
using FtisHelperDrawGame.DB;
using FtisHelperDrawGame.DB.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FtisDrawGame.Controllers
{    
    [Dou.Misc.Attr.MenuDef(Id = "Participant", Name = "參與者名單", MenuPath = "抽獎專區", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    [Dou.Misc.Attr.AutoLogger(Status = LoggerEntity.LoggerDataStatus.All, Content = Dou.Misc.Attr.AutoLoggerAttribute.LogContent.AssignContent,
        AssignContent = "KEY:{FKey}, 字串:{FText}")]
    public class ParticipantController : Dou.Controllers.AGenericModelController<PARTICIPANT>
    {
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable<PARTICIPANT> GetDataDBObject(IModelEntity<PARTICIPANT> dbEntity, params KeyValueParams[] paras)
        {
            var DCode = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "DCODE");
            var Fno = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "FNO");
            var ACTID = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "ACTID");

            //20230814, add by markhong 初始沒有篩選條件時不顯示資料
            if (DCode == null && Fno == null && ACTID == null)
                 return base.GetDataDBObject(dbEntity, paras).Take(0);
            return base.GetDataDBObject(dbEntity, paras);
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
            options.GetFiled("DCODE").editable = false;
            return options;
        }

        //[HttpPost]
        //public virtual ActionResult RefreshPage(PARTICIPANT objs)
        //{
        //    var f = objs;

        //    System.Data.Entity.DbContext _dbContext = new FtisDrawGameModelContext();
        //    Dou.Models.DB.IModelEntity<PARTICIPANT> model = new Dou.Models.DB.ModelEntity<PARTICIPANT>(_dbContext);

        //    //var datas = model.GetAll(a => a.ACTID == f.ACTID
        //    //                    && a.NAME == f.NAME
        //    //                    && a.REMARK == f.REMARK
        //    //                    ).ToList();
        //    var datas = model.GetAll().Where(a=>a.ACTID == f.ACTID).ToList();

        //    var opts = Dou.Misc.DataManagerScriptHelper.GetDataManagerOptions<PARTICIPANT>();

        //    //全部欄位排序
        //    foreach (var field in opts.fields)
        //        field.sortable = true;

        //    opts.datas = datas;

        //    var jstr = JsonConvert.SerializeObject(opts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        //    jstr = jstr.Replace(DataManagerScriptHelper.JavaScriptFunctionStringStart, "(").Replace(DataManagerScriptHelper.JavaScriptFunctionStringEnd, ")");
        //    return Content(jstr, "application/json");
        //}
        protected override IModelEntity<PARTICIPANT> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<PARTICIPANT>(FtisHelperDrawGame.DB.Helper.CreateFtisDrawGameModelContext());
        }


    }
}