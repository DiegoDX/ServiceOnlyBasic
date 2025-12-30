using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class CityConfiguration: IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Description)
                .HasMaxLength(500);
            builder.Property(c => c.Population)
                .HasColumnType("bigint");
            builder.Property(c => c.AreaInSquareKm)
                .HasColumnType("decimal(18,2)");
            builder.Property(c => c.IsCapital)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Property(c => c.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.UpdatedAt)
                .IsRequired(false);
            builder.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(c => c.CountryId)
           .HasDatabaseName("IX_Cities_CountryId");

            builder.HasIndex(c => c.Name)
                .HasDatabaseName("IX_Cities_Name");

            builder.HasIndex(c => c.IsCapital)
                .HasDatabaseName("IX_Cities_IsCapital");

            builder.HasIndex(c => new { c.Name, c.CountryId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0") // Unique only for non-deleted records
                .HasDatabaseName("IX_Cities_Name_CountryId");

            builder.HasIndex(c => c.IsDeleted)
                .HasDatabaseName("IX_Cities_IsDeleted");

            // Composite index for country + capital (business rule: one capital per country)
            builder.HasIndex(c => new { c.CountryId, c.IsCapital })
                .HasFilter("[IsCapital] = 1 AND [IsDeleted] = 0")
                .HasDatabaseName("IX_Cities_CountryId_IsCapital");

            // Relationships
            builder.HasOne(c => c.Country)
                .WithMany(country => country.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Query Filter (Global filter for soft delete)
            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
