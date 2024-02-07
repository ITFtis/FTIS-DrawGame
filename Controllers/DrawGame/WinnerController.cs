using Dou.Controllers;
using Dou.Misc;
using Dou.Models.DB;
using FtisDrawGame.Models;
using FtisHelperDrawGame.DB.Model;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FtisDrawGame.Controllers
{    
    [Dou.Misc.Attr.MenuDef(Id = "Winner", Name = "得獎者清單", MenuPath = "抽獎專區", Action = "Index", Index = 4, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = false)]
    [Dou.Misc.Attr.AutoLogger(Status = LoggerEntity.LoggerDataStatus.All, Content = Dou.Misc.Attr.AutoLoggerAttribute.LogContent.AssignContent,
        AssignContent = "KEY:{FKey}, 字串:{FText}")]
    public class WinnerController : Dou.Controllers.AGenericModelController<WINNER>
    {
        // GET: Country
        public ActionResult Index()
        {
            return View();
        }

        protected override IEnumerable<WINNER> GetDataDBObject(IModelEntity<WINNER> dbEntity, params KeyValueParams[] paras)
        {
            var DCode = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "DCODE");
            var Fno = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "FNO");
            var ACTID = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "ACTID");
            var PRIZE = Dou.Misc.HelperUtilities.GetFilterParaValue(paras, "PRIZE");

            //20230814, add by markhong 初始沒有篩選條件時不顯示資料
            if (DCode == null && Fno == null && ACTID == null && PRIZE == null)
                return base.GetDataDBObject(dbEntity, paras).Take(0);
            return base.GetDataDBObject(dbEntity, paras);
        }

        public override DataManagerOptions GetDataManagerOptions()
        {
            var options = base.GetDataManagerOptions();
            options.editformWindowStyle = "modal";
            options.GetFiled("DCODE").editable = false;

            return options;
        }

        protected override void DeleteDBObject(IModelEntity<WINNER> dbEntity, IEnumerable<WINNER> objs)
        {
            //刪除該活動得獎者，1.如果該獎項[PRIZE]的[ISSPEC]=0，2.那該活動參加者[PARTICIPANT]的[ISWON]要修正為0/false
            //1.
            bool _ISSPEC = false;
            foreach (var obj in objs)
            {
                if (obj.ACTID != null && obj.PRIZE != null)
                {
                    using (var cxt = FtisHelperDrawGame.DB.Helper.CreateFtisDrawGameModelContext())
                    {
                        _ISSPEC = cxt.PRIZE.Where(s=>s.ACTID==obj.ACTID && s.PID.ToString() == obj.PRIZE).First().ISSPEC;
                        if (!_ISSPEC)
                            //2.
                            using (var cxt2 = FtisHelperDrawGame.DB.Helper.CreateFtisDrawGameModelContext())
                            {
                                var pp = cxt.PARTICIPANT.Where(s => s.ACTID == obj.ACTID && s.FNO == obj.FNO).ToList();
                                if (pp.Count > 0) 
                                    pp[0].ISWON = false;
                                var douPP = new Dou.Models.DB.ModelEntity<PARTICIPANT>(cxt2);
                                douPP.Update(pp);
                            }
                    }
                }
            }
            base.DeleteDBObject(dbEntity, objs);
        }

        protected override IModelEntity<WINNER> GetModelEntity()
        {
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetPrize();
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllPrizes();
            FtisHelperDrawGame.DB.PrizesSelectItemsClassImp.ResetPrizes();
            return new Dou.Models.DB.ModelEntity<WINNER>(FtisHelperDrawGame.DB.Helper.CreateFtisDrawGameModelContext());
        }
    }
}