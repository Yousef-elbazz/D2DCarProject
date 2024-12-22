using DALProject.Models;
using DALProject.Models.sss;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            //var admin = new IdentityRole("admin");
            //admin.NormalizedName = "admin";

            //var customer = new IdentityRole("customer");
            //customer.NormalizedName = "customer";

            //var technincian = new IdentityRole("technincian");
            //customer.NormalizedName = "technincian";

            //modelBuilder.Entity<IdentityRole>().HasData(admin,technincian, customer);

            



        }


        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
         public DbSet<Category> Categroies { get; set; }
         public DbSet<Service> Services { get; set; }
        public DbSet<Color> Colors{ get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ProdServCategory> prodServCategories { get; set; }
        public DbSet<PartService> partServices { get; set; }
        public DbSet<ShoppingCart> shoppingCarts{ get; set; }
        public DbSet<OrderDetial> OrderDetials{ get; set; }
        public DbSet<OrdeHeader> OrdeHeaders{ get; set; }
        


        #region TPC
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        #endregion

    }
}
