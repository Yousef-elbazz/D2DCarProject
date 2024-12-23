using DALProject.Models;
using DALProject.Models.sss;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DALProject
{
    public class CarAppDbContext : IdentityDbContext<AppUser>
    {
        public CarAppDbContext(DbContextOptions<CarAppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Category> Categroies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ProdServCategory> prodServCategories { get; set; }
        public DbSet<PartService> partServices { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderDetial> OrderDetials { get; set; }
        public DbSet<OrdeHeader> OrdeHeaders { get; set; }


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(8, 2);
        }
        #region TPC
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        #endregion

    }
}
