using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LibraryMS.DAL.Configurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
          where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(450);

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.UpdatedBy)
                .HasMaxLength(450);

            builder.Property(x => x.DeletedAt);

            builder.Property(x => x.DeletedBy)
                .HasMaxLength(450);

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}