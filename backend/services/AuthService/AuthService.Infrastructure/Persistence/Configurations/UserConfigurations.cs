using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).HasMaxLength(100);
            builder.Property(u => u.PhoneNumber).HasMaxLength(10).IsRequired();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
            builder.Property(u => u.Email).HasMaxLength(150);
            builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        }
    }
}
