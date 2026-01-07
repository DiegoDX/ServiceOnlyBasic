using Core.Entities;
using Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class CountryConfiguration: IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Code)
                 //.HasConversion(
                 //    v => v.ToString(),
                 //    v => (Core.ValueObjects.CountryCode)Enum.Parse(typeof(Core.ValueObjects.CountryCode), v))
                .HasConversion(
                code => code.Value,                 // to database
                value => new CountryCode(value))    // from database
                .IsRequired();
            builder.Property(c => c.Description)
                .HasMaxLength(500);
            builder.Property(c => c.Population)
                .HasColumnType("bigint");
            builder.Property(c => c.AreaInSquareKm)
                .HasColumnType("decimal(18,2)");
            builder.Property(c => c.CreatedAt)
           .IsRequired()
           .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.UpdatedAt)
                .IsRequired(false);

            builder.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(c => c.Code)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0") // Unique only for non-deleted records
                .HasDatabaseName("IX_Countries_Code");

            builder.HasIndex(c => c.Name)
                .HasDatabaseName("IX_Countries_Name");

            builder.HasIndex(c => c.IsDeleted)
                .HasDatabaseName("IX_Countries_IsDeleted");

            // Relationships
            builder.HasMany(c => c.Cities)
                .WithOne(city => city.Country)
                .HasForeignKey(city => city.CountryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Query Filter (Global filter for soft delete)
            builder.HasQueryFilter(c => !c.IsDeleted);

        }
    }
}
