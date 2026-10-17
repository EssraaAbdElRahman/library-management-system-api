using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LibraryMS.DAL.Configurations
{
    public class AuthorConfiguration:BaseEntityConfiguration<Author>
    {
        public override void Configure(EntityTypeBuilder<Author> builder)
        {
            base.Configure(builder);
            builder.ToTable("Authors");

            builder.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(150);
            builder.Property(x => x.Bio)
              .HasMaxLength(1000);


        }
    }
}
