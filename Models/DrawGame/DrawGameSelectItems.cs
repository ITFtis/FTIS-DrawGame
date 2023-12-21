using Dou.Misc.Attr;
using FtisHelperDrawGame.DB.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FtisHelperDrawGame.DB
{
    /// <summary>
    /// 所有活動條列項目
    /// </summary>
    public class ActIDSelectItemsClassImp : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "FtisHelperDrawGame.DB.ActIDSelectItemsClassImp, DouImp";

        protected static IEnumerable<ACTIVITIES> _acts;
        protected static new IEnumerable<ACTIVITIES> ACTS
        {
            get
            {
                _acts = DouHelper.Misc.GetCache<IEnumerable<ACTIVITIES>>(2 * 60 * 1000, AssemblyQualifiedName);
                if (_acts == null)
                {
                    _acts = Helpe.Acts.GetAllActs();
                    DouHelper.Misc.AddCache(_acts, AssemblyQualifiedName);
                }
                return _acts;
            }
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return ACTS.Select(s => new KeyValuePair<string, object>(s.ACTID, "{\"v\":\"" + s.NAME + "\"}"));
        }
        public static void ResetActs()
        {
            DouHelper.Misc.ClearCache(AssemblyQualifiedName);
        }
    }
    /// <summary>
    /// 所有獎項條列項目
    /// </summary>
    public class PrizesSelectItemsClassImp : SelectItemsClass
    {
        public const string AssemblyQualifiedName = "FtisHelperDrawGame.DB.PrizesSelectItemsClassImp, DouImp";

        protected static IEnumerable<PRIZE> _Prizes;
        internal static new IEnumerable<PRIZE> Prizes
        {
            get
            {
                
                _Prizes = DouHelper.Misc.GetCache<IEnumerable<PRIZE>>(1 * 60 * 1000, AssemblyQualifiedName);
                if (_Prizes == null)
                {
                    _Prizes = Helpe.Acts.GetAllPrizes();
                    DouHelper.Misc.AddCache(_Prizes, AssemblyQualifiedName);
                }
                return _Prizes;
            }
        }
        public override IEnumerable<KeyValuePair<string, object>> GetSelectItems()
        {
            return Prizes.Select(s => new KeyValuePair<string, object>(s.PID.ToString(), "{\"v\":\"" + s.NAME + "\",\"ACTID\":\"" + s.ACTID + "\"}"));
        }

        public static void ResetPrizes()
        {
            DouHelper.Misc.ClearCache(AssemblyQualifiedName);
        }
    }
}
