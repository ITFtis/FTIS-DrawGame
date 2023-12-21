using Dou.Misc.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.UI.WebControls;

namespace FtisHelperDrawGame.DB.Model
{
    [Table("PARTICIPANT")]
    public partial class PARTICIPANT
    {
        [Key]
        [Column(Order = 1)]
        [ColumnDef(Display = "活動", EditType = EditType.Select
            , SelectItemsClassNamespace = ActIDSelectItemsClassImp.AssemblyQualifiedName
            , Filter = true, FilterAssign = FilterAssignType.Contains, Sortable = true)]
        public string ACTID { get; set; }

        [Key]
        [Column(Order = 0)]
        [ColumnDef(Display = "顯示名稱", Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string Name { get; set; }


        [ColumnDef(Display = "部門", EditType = EditType.Select, SelectGearingWith = "FNO,DCODE",
            SelectItemsClassNamespace = DepartmentSelectItemsClassImp.AssemblyQualifiedName,
            Filter = true, FilterAssign = FilterAssignType.Contains, Sortable = true)]
        [StringLength(20)]
        public string DCODE { get; set; }

        [ColumnDef(Display = "抽獎者", EditType = EditType.TextList, SelectItemsClassNamespace = EmpSelectItemsClassImp.AssemblyQualifiedName, Sortable = true)]
        [StringLength(6)]
        public string FNO { get; set; }

        [ColumnDef(Display = "通關密語", Sortable = true)]
        [StringLength(200)]
        public string PASSPHRASE { get; set; }

        [ColumnDef(Display = "權重(必須大於0)", DefaultValue = "1")]
        public int Weight { get; set; }

        [Required]
        [ColumnDef(Display = "是否已得獎", EditType = EditType.Radio, SelectItems = "{\"true\":\"是\",\"false\":\"否\"}", DefaultValue = "false"
            , Sortable = true)]
        public bool ISWON { get; set; }

        [Required]
        [ColumnDef(Display = "是否符合資格", EditType = EditType.Radio, SelectItems = "{\"true\":\"是\",\"false\":\"否\"}", DefaultValue = "true"
            , Visible = false, Sortable = true)]
        public bool ELIGIBLE { get; set; }

    }
}
