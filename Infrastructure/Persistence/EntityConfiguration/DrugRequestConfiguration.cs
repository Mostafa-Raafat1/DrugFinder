using Domain.Domain;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfiguration
{
    internal class DrugRequestConfiguration : IEntityTypeConfiguration<DrugRequest>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DrugRequest> builder)
        {
            // Make the DBId the primary key and have it auto-generated]
            builder.HasKey(dr => dr.DBId)
                   .HasName("DrugRequestId");
            builder.Property(dr => dr.DBId)
                   .ValueGeneratedOnAdd();

            // Make the Domain Id required and not auto-generate and add an index on it
            builder.Property(dr => dr.Id)
                   .HasColumnName("DomainGuid")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.HasIndex(dr => dr.Id)
                     .IsUnique();

            // Patient Relationship
            builder.HasOne<Patient>(D => D.Patient)
                   .WithMany(p => p.DrugRequests)
                   .HasForeignKey(dr => dr.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Adding DrugDetails as an owned entity
            builder.OwnsMany(dr => dr.DrugDetails, dd =>
            {
                dd.ToTable("DrugRequestDetails");

                dd.Property(d => d.DrugName).HasColumnName("DrugName").IsRequired();
                dd.Property(d => d.Strength).HasColumnName("Strength").IsRequired();
                dd.Property(d => d.Form).HasColumnName("Form").IsRequired();
                dd.Property(d => d.Quantity).HasColumnName("Quantity").IsRequired();
            });

            // Adding Location as an owned entity with spatial data configuration For Fast Queries
            builder.OwnsOne(p => p.Location, loc =>
            {
                loc.Property(l => l.Point)
                   .HasColumnName("Location")
                   .HasColumnType("geography");
            });

            // Adding PatientName property to DrugRequest for easier querying and display in Pharmacy's nearby requests list
            builder.Property(dr => dr.PatientName)
                   .HasColumnName("PatientName")
                   .IsRequired();


            // Adding RequestTime and Status properties
            builder.Property(dr => dr.RequestTime)
                   .HasColumnName("RequestTime")
                   .IsRequired();

            builder.Property(dr => dr.Status)
                     .HasColumnName("Status")
                     .IsRequired();

        }
    }
}
