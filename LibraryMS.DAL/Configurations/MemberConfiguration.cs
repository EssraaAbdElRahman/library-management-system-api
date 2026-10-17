using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LibraryMS.DAL.Configurations
{
    public class MemberConfiguration:BaseEntityConfiguration<Member>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);
            builder.ToTable("Members");

            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(m => m.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);
            builder.Property(m => m.Email)
                .HasMaxLength(100);
            builder.HasIndex(m => m.PhoneNumber).IsUnique();

            builder.Property(m => m.MembershipDate)
            .IsRequired();

            builder.Property(m => m.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
