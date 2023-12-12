using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBLib.EF_BlogDB;

public partial class BlogDBContext : DbContext
{
    public BlogDBContext()
    {
    }

    public BlogDBContext(DbContextOptions<BlogDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blog { get; set; }

    public virtual DbSet<Blogs> Blogs { get; set; }

    public virtual DbSet<Posts> Posts { get; set; }

    public virtual DbSet<Users> Users { get; set; }
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Url).HasMaxLength(50);
        });

        modelBuilder.Entity<Blogs>(entity =>
        {
            entity.HasKey(e => e.BlogId);
        });

        modelBuilder.Entity<Posts>(entity =>
        {
            entity.HasKey(e => e.PostId);

            entity.HasIndex(e => e.BlogId, "IX_Posts_BlogId");

            entity.HasOne(d => d.Blog).WithMany(p => p.Posts).HasForeignKey(d => d.BlogId);
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserID);

            entity.Property(e => e.UserID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsLogon)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LogonTime).HasPrecision(2);
            entity.Property(e => e.UpdateTime).HasPrecision(2);
            entity.Property(e => e.UpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserEmail)
                .HasMaxLength(130)
                .IsUnicode(false);
            entity.Property(e => e.UserMobile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
