using DALProject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DALProject.Data.Configartions
{
    internal class CarConfigration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        { 

            builder.Property(c => c.Description)
            .HasColumnType("nvarchar(max)");

            builder.Property(c => c.PlateNumber)
                .IsRequired()
                .HasColumnType("nvarchar(max)"); 
            

        }
    }

}