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
    [Dou.Misc.Attr.MenuDef(Id = "HistoryParticipant", Name = "參與者名單", MenuPath = "歷史專區", Action = "Index", Index = 3, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    [Dou.Misc.Attr.AutoLogger(Status = LoggerEntity.LoggerDataStatus.All, Content = Dou.Misc.Attr.AutoLoggerAttribute.LogContent.AssignContent,
        AssignContent = "KEY:{FKey}, 字串:{FText}")]
    public class HistoryParticipantController : Dou.Controllers.AGenericModelController<PARTICIPANT>
    {
        private DrawGameContextExt db = new DrawGameContextExt();
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

        public override DataManagerOptions GetDataManagerOptions()
        {
            var options = base.GetDataManagerOptions();
            options.editformWindowStyle = "modal";
            options.GetFiled("DCODE").editable = false;
            options.editable = false;
            options.addable = false;
            options.deleteable = false;
            return options;
        }

        protected override IModelEntity<PARTICIPANT> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<PARTICIPANT>(this.db);
        }


    }
}