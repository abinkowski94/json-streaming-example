using JsonStreamingServer.Suppliers.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JsonStreamingServer.Suppliers.Database.Configurations;

internal class ImageEntityConfiguration : IEntityTypeConfiguration<ImageEntity>
{
    public void Configure(EntityTypeBuilder<ImageEntity> builder)
    {
        builder.Property<int>("id")
            .IsRequired();

        builder.Property<int>("offer_id")
            .IsRequired();

        builder.Property(x => x.Url)
            .HasColumnName("url")
            .IsRequired();

        builder.Property(x => x.Caption)
            .HasColumnName("caption")
            .IsRequired(false);

        builder.HasOne(x => x.OfferEntity)
            .WithMany(x => x.Images)
            .HasForeignKey("offer_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey("id");
        builder.ToTable("image");
    }
}
