using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using DouHelper;
using DouImp.Models;
using FtisHelperDrawGame.DB;
using FtisHelperDrawGame.DB.Helpe;
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
    [Dou.Misc.Attr.MenuDef(Id = "Participant", Name = "參與者名單", MenuPath = "抽獎專區", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    [Dou.Misc.Attr.AutoLogger(Status = LoggerEntity.LoggerDataStatus.All, Content = Dou.Misc.Attr.AutoLoggerAttribute.LogContent.AssignContent,
        AssignContent = "KEY:{FKey}, 字串:{FText}")]
    public class ParticipantController : Dou.Controllers.AGenericModelController<PARTICIPANT>
    {
        private DrawGameContextExt db = new DrawGameContextExt();
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        protected override void AddDBObject(IModelEntity<PARTICIPANT> dbEntity, IEnumerable<PARTICIPANT> objs)
        {
            foreach (var obj in objs)
            {
                if (obj.Name == null)
                {
                    var departNickName = Department.GetAllDepartment().Where(e => e.DCode == obj.DCODE && e.IsUsed == "Y").FirstOrDefault();
                    var userName = Employee.GetAllEmployee().Where(e => e.Fno == e.Fno && e.Quit == false).FirstOrDefault();

                    if (departNickName != null && userName != null)
                        obj.Name = departNickName + "_" + userName;
                    else
                        throw new Exception("查無此員工資料");
                }
            }
            base.AddDBObject(dbEntity , objs);
        }

        protected override IEnumerable<PARTICIPANT> GetDataDBObject(IModelEntity<PARTICIPANT> dbEntity, params KeyValueParams[] paras)
        {
            return base.GetDataDBObject(dbEntity, paras);
        }

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
            return new Dou.Models.DB.ModelEntity<PARTICIPANT>(this.db);
        }


    }
}