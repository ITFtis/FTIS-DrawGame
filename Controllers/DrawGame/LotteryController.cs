using Dou.Misc.Attr;
using Dou.Models.DB;
using Microsoft.Ajax.Utilities;
using MongoDB.Bson;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using DouHelper;
using Antlr.Runtime.Misc;
using DouImp.Models;
using FtisHelperDrawGame.DB.Model;
using FtisHelperDrawGame.DB.Helpe;
using FtisHelperDrawGame.DB;

namespace DouImp.Controllers.DrawGame
{
    [Dou.Misc.Attr.MenuDef(Id = "Lottery", Name = "抽獎活動", MenuPath = "", Action = "Index", Index = 0, Func = Dou.Misc.Attr.FuncEnum.ALL, AllowAnonymous = true)]
    [Dou.Misc.Attr.AutoLogger(Status = LoggerEntity.LoggerDataStatus.All, Content = Dou.Misc.Attr.AutoLoggerAttribute.LogContent.AssignContent,
        AssignContent = "KEY:{FKey}, 字串:{FText}")]

    public class LotteryController : Dou.Controllers.AGenericModelController<WINNER>
    {
        /// <summary>
        /// 不經sso登入，匿名抽獎
        /// </summary>
        /// <param name="ctx"></param>
        /// 
        private DrawGameContextExt db = new DrawGameContextExt();
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            //base.OnActionExecuting(ctx);
        }

        // GET: Lottery
        public ActionResult Index(string ACTId)
        {
            if (ACTId == null)
            {
                ACTId = db.ACTIVITIES.First().ACTID;
            }

            // 取得目前可抽獎活動的獎品
            var PrizejoinAct =
                Acts.GetAllActs().Join(
                    Acts.GetAllPrizes(),
                    act => act.ACTID,
                    prize => prize.ACTID,
                    (act, prize) => new PRIZE_Front
                    {
                        PID = prize.PID,
                        ACTID = prize.ACTID,
                        ACTNAME = act.NAME,
                        NAME = prize.NAME,
                        REMARK = prize.REMARK,
                    }).ToList();

            // 依據活動進行分組
            var groupPrize = PrizejoinAct.GroupBy(x => x.ACTID).Select(g => g.FirstOrDefault()).ToList();

            //取得活動清單(透過活動時間篩選，未來需要多加判斷人員權限)
            var lastDate = DateTime.Now.AddDays(-1);
            var ActsInTimes = db.ACTIVITIES.Where(e => e.STARTTIME < DateTime.Now && e.ENDTIME > lastDate).ToList();

            // 預設待入第一筆資料，若無待入""
            string firstGroupPrizeActID = ActsInTimes.Count() > 0 ? ActsInTimes.FirstOrDefault().ACTID : "";
            var Prizes = db.PRIZES.Where(i => i.ACTID == firstGroupPrizeActID).ToList();


            //ViewBag.Activities = new SelectList(ActsInTimes, "ACTID", "ACTNAME");
            ViewBag.Activities = new SelectList(ActsInTimes, "ACTID", "NAME");
            ViewBag.Prizes = new SelectList(Prizes, "PID", "NAME");

            return View("Index", Prizes.Count);
        }

        /// <summary>
        /// 取得該活動所有的獎項
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public JsonResult GetAllPrizesByActs(string groupName)
        {
            return Json(new SelectList(db.PRIZES.Where(i => i.ACTID == groupName), "PID", "NAME"));
        }

        /// <summary>
        /// 抽獎
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public ActionResult StartLottery(lotteryParameter objs)
        {

            //UI獎項編
            int intPRIZE = int.Parse(objs.PRIZE);
            //UI抽獎人數
            int newDraws = objs.numDraws;
            //該活動參加者清單(已排除已中獎)
            var participants = GetParticipants(objs.ACTID, objs.PRIZE);
            //依UI獎項編取該獎項資料
            var prize = db.PRIZES.FirstOrDefault(p => p.PID == intPRIZE);
            //依UI獎項編取獎項數量
            var PrizeCounts = db.PRIZES.Where(c => c.PID == intPRIZE).FirstOrDefault().COUNTS;
            //依UI活動編與獎項編取該獎項已中獎數量
            var CountsByPrize = db.WINNERS.Where(c => c.ACTID == objs.ACTID && c.PRIZE == objs.PRIZE).Count();
            //防呆
            if (CountsByPrize >= PrizeCounts || PrizeCounts == 0)
                return Json(new { success = false, message = "此獎項已全數抽出" }, JsonRequestBehavior.AllowGet);
            if (participants.Count < newDraws || participants.Count == 0)
                return Json(new { success = false, message = "此抽獎活動參加者人數有誤，請確認" }, JsonRequestBehavior.AllowGet);
            if (newDraws + CountsByPrize > PrizeCounts)
                //newDraws = PrizeCounts - CountsByPrize;
                return Json(new { success = false, message = "抽獎人數已超過該獎項餘數，請重新輸入" }, JsonRequestBehavior.AllowGet);
            if (prize == null)
                return Json(new { success = false, message = "無獎項可提供抽獎" }, JsonRequestBehavior.AllowGet);

            var winners = new List<WINNER>();
            var winners_front = new List<WINNER_Front>();

            // 加入權重
            participants.Where(e => e.Weight > 1).ToList().ForEach(e =>
            {
                for (int index = 1; index < e.Weight; index++)
                {
                    participants.Add(e);
                }
            });

            // 抽出多個人選
            for (int i = 0; i < newDraws; i++)
            {
                if (participants.Count == 0)
                    break;

                // 亂數抽獎
                int randomIndex = new Random().Next(0, participants.Count);
                var winnerls = participants[randomIndex];
                participants = participants.Where(e => e.Name != winnerls.Name).ToList();  //去除同名

                //紀錄贏家拿到的獎品
                var winner = new WINNER
                {
                    Name = winnerls.Name,
                    ACTID = objs.ACTID,
                    FNO = winnerls.FNO,
                    DCODE = winnerls.DCODE,
                    PRIZE = objs.PRIZE,
                    UPDATETIME = DateTime.Now,
                };
                winners.Add(winner);

                // 整理給前端的資料
                var winner_front = new WINNER_Front
                {
                    FNAME = winnerls.Name,
                    DNICKNAME = "",
                    PRIZENAME = PrizeNameByPID(intPRIZE),
                };
                winners_front.Add(winner_front);

                // 更新中獎人                                
                var part = db.PARTICIPANTS.Where(e => e.ACTID == objs.ACTID && e.Name == winnerls.Name).FirstOrDefault();
                if (part == null) //找不到該對象就拋出，應該不會有此狀況
                    return Json(new { success = false, desc = "查無此人，" + winnerls.Name }, JsonRequestBehavior.AllowGet);

                //特別獎，所有符合資個的人都可以抽獎，但是單一特別獎裡面不能重複抽到相同的人。                                         
                var getPrizeEligible = Acts.GetPrize(objs.PRIZE).Where(x => x.PID == intPRIZE).FirstOrDefault().ISSPEC;
                part.ISWON = getPrizeEligible ? winnerls.ISWON : true; //更新參加人員狀態，若為特別獎則不予理會
            }

            db.WINNERS.AddRange(winners);
            db.SaveChanges();
            FtisHelperDrawGame.DB.Helpe.Acts.ResetGetAllParticipants();
            return Json(new { success = true, winners = winners_front }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 切換背景圖檔
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public ActionResult ChangeBackground(string selectedValue)
        {
            // 在此處根據選擇的值從資料庫中獲取相應的背景圖片URL
            var bg = GetBackgroundFromDatabase(selectedValue);

            // 將背景圖片URL傳遞到前端
            return Json(new List<string> { bg[0], bg[1], bg[2], bg[3] });
        }

        /// <summary>
        /// 取得該活動獎項與抽獎者的剩餘數量
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public ActionResult getPrizeAndParticipantCount(lotteryParameter objs)
        {
            if (objs.PRIZE == "null")
                return Json(new List<string> { string.Format("剩餘獎項數量：{0}", "0"), string.Format("抽獎人數：{0}", "0") });
            // 從資料庫中取該獎項剩餘數量
            var PCs = string.Format("剩餘獎項數量：{0}", LastPrizeCounts(objs.ACTID, int.Parse(objs.PRIZE)));
            // 從資料庫中取該獎項剩餘抽獎人數
            var PPs = string.Format("參與抽獎人數：{0}", GetParticipants(objs.ACTID, objs.PRIZE).Count().ToString());
            // 將獎項與抽獎者數量傳遞到前端
            return Json(new List<string> { PCs, PPs });
        }

        /// <summary>
        /// 計算該活動獎項剩餘數量
        /// </summary>
        /// <param name="actid">活動編號</param>
        /// <param name="pid">獎項編號</param>
        /// <returns></returns>
        private int LastPrizeCounts(string actid, int pid)
        {
            var PrizeCounts = db.PRIZES.Where(c => c.ACTID == actid && c.PID == pid).FirstOrDefault().COUNTS;
            var CountsByPrize = db.WINNERS.Where(c => c.ACTID == actid && c.PRIZE == pid.ToString()).Count();
            return PrizeCounts - CountsByPrize <= 0 ? 0 : PrizeCounts - CountsByPrize;
        }

        /// <summary>
        /// 取得該活動的背景圖檔
        /// </summary>
        /// <param name="ACTId">活動編</param>
        /// <returns></returns>
        private List<string> GetBackgroundFromDatabase(string ACTId)
        {
            List<string> ss = new List<string>();
            string BgImageUrl = string.Empty;
            string SelectBgColor = string.Empty;
            string DisplayFontColor = string.Empty;
            string BtnFontColor = string.Empty;


            var background = db.ACTIVITIES.Where(p => p.ACTID == ACTId).FirstOrDefault();
            if (background != null)
            {
                BgImageUrl = background.BACKGROUND != null ? background.BACKGROUND.ToString() : string.Empty;
                SelectBgColor = background.SELECTBG;
                DisplayFontColor = background.DISPLAYFONTCOLOR;
                BtnFontColor = background.BUTTONFONTCOLOR;
            }
            ss.Add(BgImageUrl);
            //20231004, add by markhong 新增下拉示選單底色
            ss.Add(SelectBgColor);
            //20231004, add by markhong 新增得獎抽獎/按鈕文字顏色
            ss.Add(DisplayFontColor);
            ss.Add(BtnFontColor);
            return ss;

        }

        /// <summary>
        /// 取得該活動背景圖檔字串
        /// </summary>
        /// <param name="ACTId">活動編</param>
        /// <returns></returns>
        private string GetSelectBackgroundFromDatabase(string ACTId)
        {
            string background = string.Empty;
            background = db.ACTIVITIES.Where(p => p.ACTID == ACTId).FirstOrDefault().SELECTBG.ToString();
            return background;
        }

        /// <summary>
        /// 依員工編回傳員工姓名
        /// </summary>
        /// <param name="Fno"></param>
        /// <returns></returns>
        private string NameByFno(string Fno)
        {
            using (var db = Helper.CreateFtisT8ModelContext())
            {
                var ss = db.F22cmmEmpData.Where(p => p.Fno == Fno).ToArray();
                if (ss.Count() == 0)
                    return Fno;
                return db.F22cmmEmpData.Where(p => p.Fno == Fno).FirstOrDefault().Name.ToString();
            }
        }

        /// <summary>
        /// 依部門編回傳部門簡稱
        /// </summary>
        /// <param name="DCode"></param>
        /// <returns></returns>
        private string DnickNameByDCode(string DCode)
        {
            using (var db = Helper.CreateFtisT8ModelContext())
            {
                if (DCode == null)
                    return "";
                return db.F22cmmDep.Where(p => p.DCode == DCode).FirstOrDefault().Dnickname.ToString();
            }
        }

        /// <summary>
        /// 依員工編回傳部門編
        /// </summary>
        /// <param name="Fno"></param>
        /// <returns></returns>
        private string DCodeByFno(string Fno)
        {
            using (var db = Helper.CreateFtisT8ModelContext())
            {
                var ss = db.F22cmmEmpData.Where(p => p.Fno == Fno).ToArray();
                if (ss.Count() == 0)
                    return "";
                return db.F22cmmEmpData.Where(p => p.Fno == Fno).FirstOrDefault().DCode.ToString();
            }
        }

        /// <summary>
        /// 依獎項編回傳獎項名稱
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        private string PrizeNameByPID(int PID)
        {
            return db.PRIZES.Where(p => p.PID == PID).FirstOrDefault().NAME.ToString();
        }

        /// <summary>
        /// 清除資料庫該活動得獎者名單
        /// </summary>
        /// <param name="objs">UI參數</param>
        /// <returns></returns>
        public ActionResult ClearWinners(lotteryParameter objs)
        {
            var Parts = db.PARTICIPANTS.Where(p => p.ACTID == objs.ACTID);
            foreach (PARTICIPANT item in Parts)
                item.ISWON = false;
            var winners = db.WINNERS.Where(p => p.ACTID == objs.ACTID).ToList();
            db.WINNERS.RemoveRange(winners);
            db.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得該活動該獎項之參加抽獎者名單(已排除已中獎)
        /// </summary>
        /// <param name="activityId">活動編</param>
        /// <param name="PID">獎項編</param>
        /// <returns></returns>
        private List<PARTICIPANT> GetParticipants(string activityId, string PID)
        {
            //確認獎項是否為重複抽獎
            var getPrizeEligible = Acts.GetPrize(PID).Where(x => x.PID.ToString() == PID).FirstOrDefault().ISSPEC;
            //可重複抽獎獎項
            if (getPrizeEligible)
            {
                //要排除有中獎過的人
                //1.有資格的抽獎者
                var getEligible = Acts.GetAllParticipants().Where(x => x.ACTID == activityId && x.ELIGIBLE == true).ToList();
                //2.排除已中獎者
                var s1 = (from a in getEligible
                          join b in db.WINNERS on new { a.ACTID, a.FNO } equals new { b.ACTID, b.FNO }
                          where b.PRIZE == PID
                          select a.FNO);
                var getIsNotWon = (from a in getEligible
                                   where !s1.Contains(a.FNO)
                                   select a
                                   ).ToList();
                //return Acts.GetAllParticipants().Where(x => x.ACTID == activityId && x.ELIGIBLE == true).ToList();
                return getIsNotWon;
            }
            //不能重複抽獎(一般)獎項
            else
            {
                var allParts = Acts.GetAllParticipants().Where(x => x.ACTID == activityId && x.ELIGIBLE == true && x.ISWON == false).ToList();
                Acts.ResetGetAllParticipants();
                return allParts;
            }
        }

        protected override IModelEntity<WINNER> GetModelEntity()
        {
            return new Dou.Models.DB.ModelEntity<WINNER>(this.db);
        }
    }

    public class lotteryParameter
    {
        public int numDraws { get; set; }
        public string ACTID { get; set; }
        public string PRIZE { get; set; }
    }

    public class PRIZE_Front
    {
        public int PID { get; set; }
        public string ACTID { get; set; }
        public string ACTNAME { get; set; }
        public string NAME { get; set; }
        public string REMARK { get; set; }
    }

    public class WINNER_Front
    {
        public string FNAME { get; set; }
        public string DNICKNAME { get; set; }
        public string PRIZENAME { get; set; }
    }

}