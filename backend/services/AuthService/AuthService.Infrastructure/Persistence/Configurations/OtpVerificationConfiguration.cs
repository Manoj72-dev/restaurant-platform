using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class OtpVerificationConfiguration : IEntityTypeConfiguration<OtpVerification>
    {
        public void Configure(EntityTypeBuilder<OtpVerification> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.PhoneNumber).HasMaxLength(10).IsRequired();
            builder.Property(o => o.OtpCodeHash).IsRequired();
        }
    }
}
