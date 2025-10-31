using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Model1s;

public partial class HrmsContext : DbContext
{
    public HrmsContext()
    {
    }

    public HrmsContext(DbContextOptions<HrmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAimatching> EmployeeAimatchings { get; set; }

    public virtual DbSet<EmployeeMatchingOutput> EmployeeMatchingOutputs { get; set; }

    public virtual DbSet<RecruitmentPlan> RecruitmentPlans { get; set; }

    public virtual DbSet<RecruitmentRequest> RecruitmentRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC07410CB39C");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.TableName).HasMaxLength(100);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AuditLogs__UserI__5FB337D6");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07903B1464");

            entity.HasIndex(e => e.Name, "UQ__Departme__737584F626B0EABD").IsUnique();

            entity.HasIndex(e => e.Code, "UQ__Departme__A25C5AA7E0F3AA84").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.HeadOfDepartment).WithMany(p => p.Departments)
                .HasForeignKey(d => d.HeadOfDepartmentId)
                .HasConstraintName("FK__Departmen__HeadO__4222D4EF");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC076F613F0D");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmergencyContactName).HasMaxLength(100);
            entity.Property(e => e.EmergencyContactPhone).HasMaxLength(20);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Employees__Depar__46E78A0C");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.EmployeeIdNavigation)
                .HasForeignKey<Employee>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Id__45F365D3");

            entity.HasOne(d => d.Manager).WithMany(p => p.EmployeeManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Employees__Manag__47DBAE45");
        });

        modelBuilder.Entity<EmployeeAimatching>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC074ACFA412");

            entity.ToTable("EmployeeAIMatchings");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.EmployeeAimatchings)
                .HasForeignKey(d => d.CreatedById)
                .HasConstraintName("FK__EmployeeA__Creat__5812160E");
        });

        modelBuilder.Entity<EmployeeMatchingOutput>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07782629CA");

            entity.Property(e => e.EmployeeAimatchingId).HasColumnName("EmployeeAIMatchingId");

            entity.HasOne(d => d.EmployeeAimatching).WithMany(p => p.EmployeeMatchingOutputs)
                .HasForeignKey(d => d.EmployeeAimatchingId)
                .HasConstraintName("FK__EmployeeM__Emplo__5BE2A6F2");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeMatchingOutputs)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__EmployeeM__Emplo__5AEE82B9");
        });

        modelBuilder.Entity<RecruitmentPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recruitm__3214EC0786D6C9E2");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Experience).HasMaxLength(255);
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.SalaryRangeMax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SalaryRangeMin).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Skills).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Draft");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.TrackingProgressStatus).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<RecruitmentRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recruitm__3214EC076766FA14");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Experience).HasMaxLength(255);
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.Skills).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Draft");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.TrackingProgressStatus).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Department).WithMany(p => p.RecruitmentRequests)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Recruitme__Depar__4CA06362");

            entity.HasMany(d => d.RecruitmentPlans).WithMany(p => p.RecruitmentRequests)
                .UsingEntity<Dictionary<string, object>>(
                    "RecruitmentRequestPlan",
                    r => r.HasOne<RecruitmentPlan>().WithMany()
                        .HasForeignKey("RecruitmentPlanId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Recruitme__Recru__5441852A"),
                    l => l.HasOne<RecruitmentRequest>().WithMany()
                        .HasForeignKey("RecruitmentRequestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Recruitme__Recru__534D60F1"),
                    j =>
                    {
                        j.HasKey("RecruitmentRequestId", "RecruitmentPlanId").HasName("PK__Recruitm__A65326163194A8B7");
                        j.ToTable("RecruitmentRequestPlans");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07320BB593");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E42D2E633A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345BFE59D3").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
