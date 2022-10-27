using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kasirResto.Models
{
    public class PesananDetailMenu
    {
        public int PesananDid { get; set; }
        public int? PesananDpid { get; set; }
        public int? PesananMenuId { get; set; }
        public int? PesananPrice { get; set; }
        public int? PesananStatus { get; set; }
        public int MenuId { get; set; }
        public string? MenuNama { get; set; }
        public string? MenuImage { get; set; }
        public int? MenuKategoriId { get; set; }
        public int? MenuPrice { get; set; }
        public byte? MenuStatus { get; set; }
    }
}
