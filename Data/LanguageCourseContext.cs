using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LanguageCourses.Models;

#nullable disable

namespace LanguageCourses.Data
{
    public partial class LanguageCourseContext : DbContext
    {
        public LanguageCourseContext()
        {
        }

        public LanguageCourseContext(DbContextOptions<LanguageCourseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Listener> Listeners { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-JBPJ5SS\\SQLEXPRESS;Database=LanguageCourse;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseId).HasColumnName("courseId");

                entity.Property(e => e.Cost)
                    .HasColumnType("money")
                    .HasColumnName("cost");

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.FreePlaces).HasColumnName("freePlaces");

                entity.Property(e => e.GroupSize).HasColumnName("groupSize");

                entity.Property(e => e.IntensityOfClasses)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("intensityOfClasses");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.NumberOfHours).HasColumnName("numberOfHours");

                entity.Property(e => e.TeacherId).HasColumnName("teacherId");

                entity.Property(e => e.TrainingProgram)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("trainingProgram");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__Courses__teacher__3F466844");
            });

            modelBuilder.Entity<Listener>(entity =>
            {
                entity.Property(e => e.ListenerId).HasColumnName("listenerId");

                entity.Property(e => e.Address)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("middleName");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.PassportData)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("passportData");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("surname");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).HasColumnName("paymentId");

                entity.Property(e => e.CourseId).HasColumnName("courseId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.ListenerId).HasColumnName("listenerId");

                entity.Property(e => e.NameOfCourses)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("nameOfCourses");

                entity.Property(e => e.Sum)
                    .HasColumnType("money")
                    .HasColumnName("sum");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Payments__course__3E52440B");

                entity.HasOne(d => d.Listener)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ListenerId)
                    .HasConstraintName("FK__Payments__listen__3D5E1FD2");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.Property(e => e.TeacherId).HasColumnName("teacherId");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("middleName");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.SurName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("surName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
