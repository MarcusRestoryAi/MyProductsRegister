using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyProductsRegister.Models;

namespace MyProductsRegister.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Skapa ref till mina modeller
        public DbSet<Products> Products { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }
    }
}