using Domain.Entity;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfiguration
{
    internal class PharmacyConfiguration : IEntityTypeConfiguration<Pharmacy>
    {
        public void Configure(EntityTypeBuilder<Pharmacy> builder)
        {
            // Make the DBId the primary key and have it auto-generated
            builder.HasKey(p => p.DBId)
                .HasName("PharmacyId");

            builder.Property(p => p.DBId)
                .ValueGeneratedOnAdd();

            // For Name, make it required and have a max length of 100 characters
            builder.Property(p => p.PharmacyName)
                .IsRequired()
                .HasMaxLength(100);

            // Make the Domain Id required and not auto-generate and add an index on it
            builder.Property(p => p.Id)
                .HasColumnName("DomainGuid")
                .IsRequired()
                .ValueGeneratedNever();

            builder.HasIndex(p => p.Id)
                .IsUnique();

            // For Address, make it required and have a max length of 200 characters'
            builder.Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(200);

            // Mapping VO Location to its properties in the Pharmacy table and make index on Location
            builder.OwnsOne(p => p.Location, loc =>
            {
                loc.Property(l => l.Point)
                   .HasColumnName("Location")
                   .HasColumnType("geography");

                loc.HasIndex(l => l.Point);
            });

           

            // Mapping VO LicenseNumber to its properties in the Pharmacy table and a
            builder.OwnsOne(p => p.LicenseNumber, lic =>
            {
                lic.Property(l => l.Number).HasColumnName("LicenseNumber");
            });

            // Mapping CreatedAt to be required
            builder.Property(p => p.CeatedAt)
                .IsRequired();

            // AppUser Relationship
            builder.HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<Pharmacy>(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
