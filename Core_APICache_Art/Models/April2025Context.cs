using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core_APICache_Art.Models;

public partial class April2025Context : DbContext
{
    public April2025Context()
    {
    }

    public April2025Context(DbContextOptions<April2025Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=April2025;Integrated Security=SSPI;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptNo).HasName("PK__Departme__0148CAAEE885507C");

            entity.ToTable("Department");

            entity.HasIndex(e => e.DeptName, "DeptName_Unique").IsUnique();

            entity.Property(e => e.DeptNo).ValueGeneratedNever();
            entity.Property(e => e.DeptName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
