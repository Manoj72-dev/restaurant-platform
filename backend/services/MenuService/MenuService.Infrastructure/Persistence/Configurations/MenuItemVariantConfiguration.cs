using MenuService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuService.Infrastructure.Persistence.Configurations;

public class MenuItemVariantConfiguration : IEntityTypeConfiguration<MenuItemVariant>
{
    public void Configure(EntityTypeBuilder<MenuItemVariant> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Name).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Price).HasColumnType("decimal(10,2)");

        builder.HasOne(v => v.MenuItem)
               .WithMany(m => m.Variants)
               .HasForeignKey(v => v.MenuItemId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}