using System;
using System.Collections.Generic;
using ClinicDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicInfrastructure;

public partial class ClinicContext : DbContext
{
    public ClinicContext()
    {
    }

    public ClinicContext(DbContextOptions<ClinicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentProcedure> AppointmentProcedures { get; set; }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<PatientCard> PatientCards { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=User-PC\\SQLEXPRESS; Database=Clinic; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(e => e.EndTime).HasColumnName("End_time");
            entity.Property(e => e.PatientId).HasColumnName("Patient_id");
            entity.Property(e => e.StartTime).HasColumnName("Start_time");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Patient_cards");
        });

        modelBuilder.Entity<AppointmentProcedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Appointmnet_procedures");

            entity.ToTable("Appointment_procedures");

            entity.Property(e => e.AppointmentId).HasColumnName("Appointment_id");
            entity.Property(e => e.ProceduresId).HasColumnName("Procedures_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentProcedures)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointmnet_procedures_Appointments");

            entity.HasOne(d => d.Procedures).WithMany(p => p.AppointmentProcedures)
                .HasForeignKey(d => d.ProceduresId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointmnet_procedures_Procedures");

            entity.HasOne(d => d.User).WithMany(p => p.AppointmentProcedures)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointmnet_procedures_Users");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.ToTable("Clinic");

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Phone_number");
            entity.Property(e => e.Specialization).HasMaxLength(50);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.ToTable("Discount");

            entity.Property(e => e.DiscountPercent).HasColumnName("Discount_percent");
            entity.Property(e => e.SocialGroup)
                .HasMaxLength(50)
                .HasColumnName("Social_group");
        });

        modelBuilder.Entity<PatientCard>(entity =>
        {
            entity.ToTable("Patient_cards");

            entity.Property(e => e.AddInfo).HasColumnName("Add_info");
            entity.Property(e => e.Allergy).HasMaxLength(50);
            entity.Property(e => e.ChronicDisease)
                .HasMaxLength(50)
                .HasColumnName("Chronic_disease");
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_birth");
            entity.Property(e => e.DiscountId).HasColumnName("Discount_id");
            entity.Property(e => e.Diseases).HasMaxLength(50);
            entity.Property(e => e.FatherName)
                .HasMaxLength(50)
                .HasColumnName("Father_name");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("First_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("Last_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Phone_number");

            entity.HasOne(d => d.Discount).WithMany(p => p.PatientCards)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_cards_Discount");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.Property(e => e.ClinicId).HasColumnName("Clinic_id");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Clinic).WithMany(p => p.Procedures)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Procedures_Clinic");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.ClinicId).HasColumnName("Clinic_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Phone_number");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Users)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Clinic");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
