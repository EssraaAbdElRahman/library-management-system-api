
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMS.DAL.Configurations
{
    public class BookConfiguration:BaseEntityConfiguration<Book>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);

            builder.ToTable("Books");

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.PublishedYear)
                .IsRequired();

            builder.Property(b => b.TotalCopies)
                .IsRequired();

            builder.Property(b => b.AvailableCopies)
                .IsRequired();

            builder.Property(b => b.CategoryId)
                .IsRequired();

            builder.HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity(j => j.ToTable("BookAuthors"));

            builder.HasIndex(b => b.ISBN)
                .IsUnique();
        }
    }
}
