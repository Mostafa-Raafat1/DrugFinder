using Domain.Domain;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfiguration
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Patient> builder)
        {
            // Make the DBId the primary key and have it auto-generated
            builder.HasKey(p => p.DBId)
                   .HasName("PatientId");

            builder.Property(p => p.DBId)
                   .ValueGeneratedOnAdd();

            // For FName and SName, make them required and have a max length of 100 characters
            builder.Property(p => p.FName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.SName)
                     .IsRequired()
                     .HasMaxLength(100);

            // Make the Domain Id required and not auto-generate and add an index on it
            builder.Property(p => p.Id)
                   .HasColumnName("DomainGuid")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.HasIndex(p => p.Id)
                   .IsUnique();

            // Mapping VO Location to its properties in the Patient table and add Index on Location
            builder.OwnsOne(p => p.Location, loc =>
            {
                loc.Property(l => l.Point)
                   .HasColumnName("Location")
                   .HasColumnType("geography");

                loc.HasIndex(l => l.Point);
            });

            // Mapping CreatedAt to be required
            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            // Set up the one-to-one relationship between Patient and AppUser
            builder.HasOne<AppUser>()
                   .WithOne()
                   .HasForeignKey<Patient>(p => p.AppUserId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
