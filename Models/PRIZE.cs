using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace FtisHelperDrawGame.DB.Model
{
    [Table("PRIZE")]
    public partial class PRIZE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Display = "獎項編號", Sortable = true, Visible = false, VisibleEdit = false)]
        public int PID { get; set; }

        [Key]
        [Column(Order = 1)]
        [ColumnDef(Display = "活動編號", Sortable = true)]
        public string ACTID { get; set; }

        [ColumnDef(Display = "獎項名稱", Sortable = true)]
        [StringLength(20)]
        public string NAME { get; set; }

        [Required]
        [ColumnDef(Display = "數量", DefaultValue = "0", Sortable = true)]
        public int COUNTS { get; set; }

        [Required]
        [ColumnDef(Display = "已中獎是否重複抽獎", EditType = EditType.Radio, SelectItems = "{\"true\":\"是\",\"false\":\"否\"}", DefaultValue = "false"
            , Visible = false, Sortable = true)]
        public bool ISSPEC { get; set; }

        [ColumnDef(Display = "備註", Sortable = true)]
        [StringLength(50)]
        public string REMARK { get; set; }
    }
}
