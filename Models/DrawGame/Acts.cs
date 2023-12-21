using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DouImp.Models;
using FtisHelperDrawGame.DB.Model;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml.ConditionalFormatting;

namespace FtisHelperDrawGame.DB.Helpe
{
    public class Acts
    {
        static object lockGetAllActs = new object();
        static object lockGetAllPrizes = new object();
        static object lockGetAllParticipants = new object();
        static object lockGetPrize = new object();



        /// <summary>
        /// 取所有活動資料
        /// </summary>
        /// <param name="cachetimer">資料快取時間(毫秒),預設30分</param>
        /// <returns>IEnumerable<ACTIVITIES></returns>
        public static IEnumerable<ACTIVITIES> GetAllActs(int cachetimer = 300000)
        {
            DrawGameContextExt db = new DrawGameContextExt();

            string key = "FtisHelperDrawGame.DB.Model.GetAllActs";
            var allActs = DouHelper.Misc.GetCache<List<ACTIVITIES>>(cachetimer, key);
            lock (lockGetAllActs)
            {
                if (allActs == null)
                    DouHelper.Misc.AddCache(db.ACTIVITIES.ToList(), key);
            }
            return DouHelper.Misc.GetCache<List<ACTIVITIES>>(cachetimer, key);
        }

        public static void ResetGetAllActs()
        {
            string key = "FtisHelperDrawGame.DB.Model.GetAllActs";
            DouHelper.Misc.ClearCache(key);
        }

        /// <summary>
        /// 取所有獎項
        /// </summary>
        /// <param name="cachetimer">資料快取時間(毫秒),預設30分</param>
        /// <returns>IEnumerable<PRIZE></returns>
        public static IEnumerable<PRIZE> GetAllPrizes(int cachetimer = 300000)
        {
            string key = "FtisHelperDrawGame.DB.Model.GetAllPrizes";
            var allPrizes = DouHelper.Misc.GetCache<IEnumerable<PRIZE>>(cachetimer, key);

            DrawGameContextExt db = new DrawGameContextExt();
            lock (lockGetAllPrizes)
            {
                if (allPrizes == null)
                {
                    allPrizes = db.PRIZES.ToList();
                    DouHelper.Misc.AddCache(allPrizes, key);
                }
            }
            return allPrizes;
        }

        public static void ResetGetAllPrizes()
        {
            string key = "FtisHelperDrawGame.DB.Model.GetAllPrizes";
            DouHelper.Misc.ClearCache(key);
        }


        /// <summary>
        /// 取特定獎項
        /// </summary>
        /// <param name="cachetimer">資料快取時間(毫秒),預設30分</param>
        /// <returns>IEnumerable<PRIZE></returns>
        public static IEnumerable<PRIZE> GetPrize(string PID, int cachetimer = 300000)
        {
            string key = "FtisHelperDrawGame.DB.Model.GetActPrizes";
            var Prize = DouHelper.Misc.GetCache<IEnumerable<PRIZE>>(cachetimer, key);
            DrawGameContextExt db = new DrawGameContextExt();
            lock (lockGetPrize)
            {
                if (Prize == null)
                {
                    Prize = db.PRIZES.ToArray();
                    DouHelper.Misc.AddCache(Prize, key);

                }
            }
            return Prize.Where(x => x.PID.ToString() == PID);
        }

        public static void ResetGetPrize()
        {
            string key = "FtisHelperDrawGame.DB.Model.GetPrize";
            DouHelper.Misc.ClearCache(key);
        }

        /// <summary>
        /// 取所有參與者
        /// </summary>
        /// <param name="cachetimer">資料快取時間(毫秒),預設30分</param>
        /// <returns>IEnumerable<PARTICIPANT></returns>
        public static IEnumerable<PARTICIPANT> GetAllParticipants(int cachetimer = 300000)
        {
            string key = "FtisHelperDrawGame.DB.Model.GetAllParticipants";
            var allParticipants = DouHelper.Misc.GetCache<IEnumerable<PARTICIPANT>>(cachetimer, key);
            DrawGameContextExt db = new DrawGameContextExt();
            lock (lockGetAllParticipants)
            {
                if (allParticipants == null)
                {
                    allParticipants = db.PARTICIPANTS.ToArray();
                    DouHelper.Misc.AddCache(allParticipants, key);
                }
            }
            return allParticipants;
        }

        public static void ResetGetAllParticipants()
        {
            string key = "FtisHelperDrawGame.DB.Model.GetAllParticipants";
            DouHelper.Misc.ClearCache(key);
        }

        public static void ResetGetAllWinners()
        {
            string key = "FtisHelperDrawGame.DB.Model.GetAllwinners";
            DouHelper.Misc.ClearCache(key);
        }

        /// <summary>
        /// 依Fno取Employee
        /// </summary>
        /// <param name="Fno">員工編號</param>
        /// <param name="cachetimer">資料快取時間(毫秒),預設30分</param>
        /// <returns>Employee</returns>
        //public static F22cmmEmpData GetEmployee(string Fno, int cachetimer = Helper.shortcacheduration)
        //{
        //    if (string.IsNullOrEmpty(Fno))
        //        return null;
        //    return GetAllEmployee(cachetimer).FirstOrDefault(m => m.Fno == Fno);
        //}
    }
}
