using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfiguration
{
    internal class PharmacyResponseConfiguration : IEntityTypeConfiguration<PharmacyResponse>
    {
        public void Configure(EntityTypeBuilder<PharmacyResponse> builder)
        {
            // Configure the DB primary key and auto increment
            builder.HasKey(pr => pr.DBId)
                   .HasName("PharmacyResponseId");

            builder.Property(pr => pr.DBId)
                   .ValueGeneratedOnAdd();

            // Configure the Domain Id as required and not auto-generated and add an index on it
            builder.Property(pr => pr.Id)
                   .HasColumnName("DomainGuid")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.HasIndex(pr => pr.Id)
                   .IsUnique();

            // Adding Drug Request Relation
            builder.HasOne<DrugRequest>(PhR => PhR.DrugRequest)
                   .WithMany(d => d.PharmacyResponses)
                   .HasForeignKey(pr => pr.DrugRequestId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Adding Pharmacy Relation
            builder.HasOne<Pharmacy>(PhR => PhR.Pharmacy)
                   .WithMany(ph => ph.PharmacyResponses)
                   .HasForeignKey(pr => pr.PharmacyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Configure the Price as an owned entity
            builder.OwnsOne(pr => pr.Price, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("PriceAmount")
                     .IsRequired();
                money.Property(m => m.Currency)
                     .HasColumnName("PriceCurrency")
                     .IsRequired()
                     .HasMaxLength(3);
            });

            // Configure the ResponseItems as an owned collection
            builder.OwnsMany(pr => pr.ResponseItems, pr =>
            {
                pr.ToTable("PharmacyResponseItems");

                pr.Property(r => r.DrugName).HasColumnName("DrugName").IsRequired();
                pr.Property(r => r.Available).HasColumnName("Available").IsRequired();
                pr.Property(r => r.Price).HasColumnName("Price");
            });

            // Configure the AvailabilityStatus as an enum integer
            builder.Property(pr => pr.AvailabilityStatus)
                   .IsRequired();

            // Configure the ResponseTime as required
            builder.Property(pr => pr.ResponseTime)
                   .IsRequired();
        }
    }
}
