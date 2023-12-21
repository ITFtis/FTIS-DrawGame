using Dou.Misc.Attr;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.UI.WebControls;

namespace FtisHelperDrawGame.DB.Model
{
    [Table("ACTIVITIES")]
    public partial class ACTIVITIES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnDef(Display = "活動編號", Sortable = true, VisibleEdit = false)]
        public string ACTID { get; set; }

        [Required]
        [ColumnDef(Display = "活動名稱", Sortable = true)]
        [StringLength(50)]
        public string NAME { get; set; }

        [ColumnDef(Display = "開始時間", EditType = EditType.Date, ColSize = 3, Sortable = true)]
        public DateTime STARTTIME { get; set; }

        [ColumnDef(Display = "結束時間", EditType = EditType.Date, ColSize = 3, Sortable = true)]
        public DateTime ENDTIME { get; set; }

        [ColumnDef(Display = "備註", ColSize = 3, Sortable = true)]
        [StringLength(50)]
        public string REMARK { get; set; }

        [ColumnDef(Display = "下拉式選單底色", DefaultValue = "#000000", ColSize = 3, Visible = false)]
        [StringLength(7)]
        public string SELECTBG { get; set; }

        [ColumnDef(Display = "獎項/名單/數量/人數文字顏色", DefaultValue = "#000000", ColSize = 3, Visible = false)]
        [StringLength(7)]
        public string DISPLAYFONTCOLOR { get; set; }

        [ColumnDef(Display = "抽獎/清除按鈕文字顏色", DefaultValue = "#000000", ColSize = 3, Visible = false)]
        [StringLength(7)]
        public string BUTTONFONTCOLOR { get; set; }

        [ColumnDef(Display = "背景圖檔", EditType = EditType.Image,
            ImageMaxWidth = 1920, ImageMaxHeight = 1080, ColSize = 3, Sortable = true)]
        public string BACKGROUND { get; set; }

        [ColumnDef(Display = "建立者", Sortable = true, EditType = EditType.Select,
            SelectItemsClassNamespace = EmpSelectItemsClassImp.AssemblyQualifiedName, VisibleEdit = false)]
        [StringLength(6)]
        public string BUILDER { get; set; }

        [ColumnDef(Display = "更新者", Sortable = true, EditType = EditType.Select,
            SelectItemsClassNamespace = EmpSelectItemsClassImp.AssemblyQualifiedName, VisibleEdit = false)]
        [StringLength(6)]
        public string UPDATERS { get; set; }

        [ColumnDef(Display = "更新時間", Sortable = true, VisibleEdit = false)]
        public DateTime? UPDATETIME { get; set; }

        public virtual ICollection<PRIZE> Prizes
        {
            get; set;
        }

        [NotMapped]
        public virtual ICollection<PARTICIPANT> Participants
        {
            get; set;
        }
    }
}
