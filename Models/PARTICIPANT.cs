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
        [ColumnDef(Display = "����", EditType = EditType.Select
            , SelectItemsClassNamespace = ActIDSelectItemsClassImp.AssemblyQualifiedName
            , Filter = true, FilterAssign = FilterAssignType.Contains, Sortable = true)]
        public string ACTID { get; set; }

        [Key]
        [Column(Order = 0)]
        [ColumnDef(Display = "��ܦW��", Filter = true, FilterAssign = FilterAssignType.Contains)]
        public string Name { get; set; }


        [ColumnDef(Display = "����", EditType = EditType.Select, SelectGearingWith = "FNO,DCODE",
            SelectItemsClassNamespace = DepartmentSelectItemsClassImp.AssemblyQualifiedName,
            Filter = true, FilterAssign = FilterAssignType.Contains, Sortable = true)]
        [StringLength(20)]
        public string DCODE { get; set; }

        [ColumnDef(Display = "�����", EditType = EditType.TextList, SelectItemsClassNamespace = EmpSelectItemsClassImp.AssemblyQualifiedName, Sortable = true)]
        [StringLength(6)]
        public string FNO { get; set; }

        [ColumnDef(Display = "�q���K�y", Sortable = true)]
        [StringLength(200)]
        public string PASSPHRASE { get; set; }

        [ColumnDef(Display = "�v��(�����j��0)", DefaultValue = "1")]
        public int Weight { get; set; }

        [Required]
        [ColumnDef(Display = "�O�_�w�o��", EditType = EditType.Radio, SelectItems = "{\"true\":\"�O\",\"false\":\"�_\"}", DefaultValue = "false"
            , Sortable = true)]
        public bool ISWON { get; set; }

        [Required]
        [ColumnDef(Display = "�O�_�ŦX���", EditType = EditType.Radio, SelectItems = "{\"true\":\"�O\",\"false\":\"�_\"}", DefaultValue = "true"
            , Visible = false, Sortable = true)]
        public bool ELIGIBLE { get; set; }

    }
}
