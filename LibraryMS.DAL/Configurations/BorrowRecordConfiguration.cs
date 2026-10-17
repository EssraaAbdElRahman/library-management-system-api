using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMS.DAL.Configurations
{
    public class BorrowRecordConfiguration:BaseEntityConfiguration<BorrowRecord>
    {
        public override void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            base.Configure(builder);
            builder.ToTable("BorrowRecords");

            builder.Property(b => b.BookId)
                .IsRequired();

            builder.Property(b => b.MemberId)
                .IsRequired();

            builder.Property(b => b.BorrowDate)
                .IsRequired();

            builder.Property(b => b.DueDate)
                .IsRequired();

            builder.Property(b => b.ReturnDate);

            builder.Property(b => b.IsReturned)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(br => new { br.MemberId, br.IsReturned });
            builder.HasIndex(br => br.DueDate);

            builder.Property(b => b.FineAmount)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(b => b.BorrowNotes)
                .HasMaxLength(1000);

            builder.Property(b => b.ReturnNotes)
                .HasMaxLength(1000);

             builder.HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

             builder.HasOne(br => br.Member)
                .WithMany(m => m.BorrowRecords)
                .HasForeignKey(br => br.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
