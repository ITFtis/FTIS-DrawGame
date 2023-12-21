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
        [ColumnDef(Display = "���ʽs��", Sortable = true, VisibleEdit = false)]
        public string ACTID { get; set; }

        [Required]
        [ColumnDef(Display = "���ʦW��", Sortable = true)]
        [StringLength(50)]
        public string NAME { get; set; }

        [ColumnDef(Display = "�}�l�ɶ�", EditType = EditType.Date, ColSize = 3, Sortable = true)]
        public DateTime STARTTIME { get; set; }

        [ColumnDef(Display = "�����ɶ�", EditType = EditType.Date, ColSize = 3, Sortable = true)]
        public DateTime ENDTIME { get; set; }

        [ColumnDef(Display = "�Ƶ�", ColSize = 3, Sortable = true)]
        [StringLength(50)]
        public string REMARK { get; set; }

        [ColumnDef(Display = "�U�Ԧ���橳��", DefaultValue = "#000000", ColSize = 3, Visible = false)]
        [StringLength(7)]
        public string SELECTBG { get; set; }

        [ColumnDef(Display = "����/�W��/�ƶq/�H�Ƥ�r�C��", DefaultValue = "#000000", ColSize = 3, Visible = false)]
        [StringLength(7)]
        public string DISPLAYFONTCOLOR { get; set; }

        [ColumnDef(Display = "���/�M�����s��r�C��", DefaultValue = "#000000", ColSize = 3, Visible = false)]
        [StringLength(7)]
        public string BUTTONFONTCOLOR { get; set; }

        [ColumnDef(Display = "�I������", EditType = EditType.Image,
            ImageMaxWidth = 1920, ImageMaxHeight = 1080, ColSize = 3, Sortable = true)]
        public string BACKGROUND { get; set; }

        [ColumnDef(Display = "�إߪ�", Sortable = true, EditType = EditType.Select,
            SelectItemsClassNamespace = EmpSelectItemsClassImp.AssemblyQualifiedName, VisibleEdit = false)]
        [StringLength(6)]
        public string BUILDER { get; set; }

        [ColumnDef(Display = "��s��", Sortable = true, EditType = EditType.Select,
            SelectItemsClassNamespace = EmpSelectItemsClassImp.AssemblyQualifiedName, VisibleEdit = false)]
        [StringLength(6)]
        public string UPDATERS { get; set; }

        [ColumnDef(Display = "��s�ɶ�", Sortable = true, VisibleEdit = false)]
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
