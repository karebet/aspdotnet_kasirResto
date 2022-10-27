using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kasirResto.Models
{
    [Table("PesananDetail")]
    public partial class PesananDetail
    {
        [Key]
        [Column("PesananDID")]
        public int PesananDid { get; set; }
        [Column("PesananDPID")]
        public int? PesananDpid { get; set; }
        [Column("PesananMenuID")]
        public int? PesananMenuId { get; set; }
        public int? PesananPrice { get; set; }
        public int? PesananStatus { get; set; }
        public int? PesananQty { get; set; }
    }
}
