using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kasirResto.Models
{
    [Table("Menu")]
    public partial class Menu
    {
        [Key]
        public int MenuId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? MenuNama { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? MenuImage { get; set; }
        public int? MenuKategoriId { get; set; }
        public int? MenuPrice { get; set; }
        public byte? MenuStatus { get; set; }
    }
}
