using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kasirResto.Models
{
    [Table("KategoriMenu")]
    public partial class KategoriMenu
    {
        [Key]
        public int KategoriId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? KategoriNama { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? KategoriImage { get; set; }
        public byte? KategoriStatus { get; set; }
    }
}
