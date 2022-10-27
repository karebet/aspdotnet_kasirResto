using kasirResto.Areas.Identity.Data;
using kasirResto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace kasirResto.Data;

public class kasirRestoContext : IdentityDbContext<kasirRestoUser>
{
    public kasirRestoContext(DbContextOptions<kasirRestoContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-CID4Q10\\SQLEXPRESS;Initial Catalog=kasirResto;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }


    public virtual DbSet<KategoriMenu> KategoriMenus { get; set; } = null!;
    public virtual DbSet<Menu> Menus { get; set; } = null!;
    public virtual DbSet<Pesanan> Pesanans { get; set; } = null!;
    public virtual DbSet<PesananDetail> PesananDetails { get; set; } = null!;
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
