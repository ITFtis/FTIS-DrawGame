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
        [ColumnDef(Display = "�����s��", Sortable = true, Visible = false, VisibleEdit = false)]
        public int PID { get; set; }

        [Key]
        [Column(Order = 1)]
        [ColumnDef(Display = "���ʽs��", Sortable = true)]
        public string ACTID { get; set; }

        [ColumnDef(Display = "�����W��", Sortable = true)]
        [StringLength(20)]
        public string NAME { get; set; }

        [Required]
        [ColumnDef(Display = "�ƶq", DefaultValue = "0", Sortable = true)]
        public int COUNTS { get; set; }

        [Required]
        [ColumnDef(Display = "�w�����O�_���Ʃ��", EditType = EditType.Radio, SelectItems = "{\"true\":\"�O\",\"false\":\"�_\"}", DefaultValue = "false"
            , Visible = false, Sortable = true)]
        public bool ISSPEC { get; set; }

        [ColumnDef(Display = "�Ƶ�", Sortable = true)]
        [StringLength(50)]
        public string REMARK { get; set; }
    }
}
