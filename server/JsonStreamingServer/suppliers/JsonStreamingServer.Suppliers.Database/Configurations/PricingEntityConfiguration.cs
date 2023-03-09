using JsonStreamingServer.Suppliers.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JsonStreamingServer.Suppliers.Database.Configurations;

internal class PricingEntityConfiguration : IEntityTypeConfiguration<PricingEntity>
{
    public void Configure(EntityTypeBuilder<PricingEntity> builder)
    {
        builder.Property<int>("id")
            .IsRequired();

        builder.Property<int>("offer_id")
            .IsRequired();

        builder.Property(x => x.DayFrom)
            .HasColumnName("day_from")
            .IsRequired();

        builder.Property(x => x.DayTo)
            .HasColumnName("day_to")
            .IsRequired();

        builder.Property(x => x.AgeFrom)
            .HasColumnName("age_from")
            .IsRequired();

        builder.Property(x => x.AgeTo)
            .HasColumnName("age_to")
            .IsRequired();

        builder.Property(x => x.Value)
            .HasColumnName("value")
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasColumnName("currency")
            .IsRequired();

        builder.HasOne(x => x.OfferEntity)
            .WithMany(x => x.Pricings)
            .HasForeignKey("offer_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey("id");
        builder.ToTable("pricing");
    }
}
