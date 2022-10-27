using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kasirResto.Models
{
    [Table("Pesanan")]
    public partial class Pesanan
    {
        [Key]
        public int PesananId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? PesananCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PesananDate { get; set; }
        [StringLength(450)]
        [Unicode(false)]
        public string? PesananUserId { get; set; }
        public byte? PesananStatus { get; set; }
    }
}
