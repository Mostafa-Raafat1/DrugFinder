using Domain.Domain;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfiguration
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Adding Pk and auto-generated DBId
            builder.HasKey(n => n.DBId)
                   .HasName("NotificationId");
            builder.Property(n => n.DBId)
                   .ValueGeneratedOnAdd();
            // Adding the Domain Id as required and not auto-generated and adding index on it
            builder.Property(n => n.Id)
                   .HasColumnName("DomainGuid")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.HasIndex(builder => builder.Id)
                   .IsUnique();

            // Message is required and has a max length of 500 characters
            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(500);
            // CreatedAt is required and defaults to current UTC time
            builder.Property(n => n.CreatedAt)
                   .IsRequired();
            // IsRead is required and defaults to false
            builder.Property(n => n.IsRead)
                   .IsRequired();
            // Patient Relationship
            builder.HasOne<Patient>(n => n.Patient)
                   .WithMany(p => p.Notifications)
                   .HasForeignKey(n => n.PatientId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
            // Pharmacy Relationship
            builder.HasOne<Pharmacy>(n => n.Pharmacy)
                   .WithMany(p => p.Notifications)
                   .HasForeignKey(n => n.PharmacyId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

