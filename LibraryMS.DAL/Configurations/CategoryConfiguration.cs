
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMS.DAL.Configurations
{
    public  class CategoryConfiguration:BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.ToTable("Categories");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
