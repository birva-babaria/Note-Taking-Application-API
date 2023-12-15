using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NoteAPPLibrary;

public partial class NoteAppContext : IdentityDbContext
{
    public NoteAppContext()
    {
    }

    public NoteAppContext(DbContextOptions<NoteAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Trash> Trashes { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
   //     => optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=NoteAPP;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Content)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.NoteId).HasColumnName("noteID");
            entity.Property(e => e.Posteddate)
                .HasColumnType("datetime")
                .HasColumnName("posteddate");

            entity.HasOne(d => d.Note).WithMany(p => p.Comments)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comments_Notes");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Notes");

            entity.ToTable("notes");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Createddate)
                .HasColumnType("datetime")
                .HasColumnName("createddate");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Lastmodifieddate)
                .HasColumnType("datetime")
                .HasColumnName("lastmodifieddate");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.Trashstatus).HasColumnName("trashstatus");
        });

        modelBuilder.Entity<Trash>(entity =>
        {
            entity.ToTable("trash");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Deleteddate)
                .HasColumnType("datetime")
                .HasColumnName("deleteddate");
            entity.Property(e => e.NoteId).HasColumnName("noteID");

            entity.HasOne(d => d.Note).WithMany(p => p.Trashes)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_trash_Notes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
