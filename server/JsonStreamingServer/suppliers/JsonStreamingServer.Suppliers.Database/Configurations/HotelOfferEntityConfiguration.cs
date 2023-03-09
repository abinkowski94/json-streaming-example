using JsonStreamingServer.Suppliers.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JsonStreamingServer.Suppliers.Database.Configurations;

internal class HotelOfferEntityConfiguration : IEntityTypeConfiguration<HotelOfferEntity>
{
    public void Configure(EntityTypeBuilder<HotelOfferEntity> builder)
    {
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.ShortDescription)
            .HasColumnName("short_description")
            .IsRequired(false);

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired(false);

        builder.HasKey(x => x.Id);
        builder.ToTable("hotel_offer");
    }
}
