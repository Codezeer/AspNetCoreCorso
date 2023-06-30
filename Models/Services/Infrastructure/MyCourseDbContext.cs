using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MyCourse.Models.Entities;

namespace MyCourse.Models.Services.Infrastructure
{
    public partial class MyCourseDbContext : DbContext
    {
        public MyCourseDbContext()
        {
        }

        public MyCourseDbContext(DbContextOptions<MyCourseDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=Data/MyCourse.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");//superfluo se la tabella si chiama come l'entity
                entity.HasKey(course =>course.Id); //superfluo se la propietà si chiama id o CoursesId

                entity.OwnsOne(course => course.CurrentPrice, builder =>{
                    builder.Property(money => money.Currency)
                    .HasConversion<string>()
                    .HasColumnName("CurrentPrice_Currency");//superfluo perchp le nostr colonne seguono già le convenzioni dei nomi
                    builder.Property(money => money.Amount).HasColumnName("CurrentPrice_Amount");//superfluo perchp le nostr colonne seguono già le convenzioni dei nomi
                });

                 entity.OwnsOne(course => course.FullPrice, builder =>{
                    builder.Property(money => money.Currency)
                    .HasConversion<string>();
                });
                
                entity.HasMany(course => course.Lessons)
                .WithOne(lesson => lesson.Course)
                .HasForeignKey(lesson => lesson.CourseId);// superflua se la proripetà si chiama CourseId

                #region Mapping autogenerato
                /*
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasColumnType("TEXT (100)");

                entity.Property(e => e.CurrentPriceAmount)
                    .IsRequired()
                    .HasColumnName("CurrentPrice_Amount")
                    .HasColumnType("NUMERIC")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CurrentPriceCurrency)
                    .IsRequired()
                    .HasColumnName("CurrentPrice_Currency")
                    .HasColumnType("TEXT (3)")
                    .HasDefaultValueSql("'EUR'");

                entity.Property(e => e.Description).HasColumnType("TEXT (10000)");

                entity.Property(e => e.Email).HasColumnType("TEXT (100)");

                entity.Property(e => e.FullPriceAmount)
                    .IsRequired()
                    .HasColumnName("FullPrice_Amount")
                    .HasColumnType("NUMERIC")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.FullPriceCurrency)
                    .IsRequired()
                    .HasColumnName("FullPrice_Currency")
                    .HasColumnType("TEXT (3)")
                    .HasDefaultValueSql("'EUR'");

                entity.Property(e => e.ImagePath).HasColumnType("TEXT (100)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("TEXT (100)");
                */
                #endregion
                
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                #region 
                /*
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("TEXT (10000)");

                entity.Property(e => e.Duration)
                    .IsRequired()
                    .HasColumnType("TEXT (8)")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("TEXT (100)");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.CourseId);
                */
                #endregion
                
            });
        }
    }
}
