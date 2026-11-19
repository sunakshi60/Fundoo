using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions<FundooContext> options): base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; } = null!;

        public DbSet<NoteEntity> Notes { get; set; } = null!;

        public DbSet<LabelEntity> Labels { get; set; } = null!;

        public DbSet<NoteLabelEntity> NoteLabels { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Label configuration
            modelBuilder.Entity<LabelEntity>(entity =>
            {
                entity.HasKey(l => l.LabelId);

                entity.Property(l => l.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(l => l.NormalizedName)
                    .IsRequired()
                    .HasMaxLength(100);

                // Prevent duplicate label names for the same user
                entity.HasIndex(l => new
                {
                    l.UserId,
                    l.NormalizedName
                })
                .IsUnique();

                // One user can own multiple labels
                entity.HasOne(l => l.User)
                    .WithMany()
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // NoteLabel configuration
            modelBuilder.Entity<NoteLabelEntity>(entity =>
            {
                // Composite primary key
                entity.HasKey(nl => new
                {
                    nl.NoteId,
                    nl.LabelId
                });

                // One note can have multiple label associations
                entity.HasOne(nl => nl.Note)
                    .WithMany(n => n.NoteLabels)
                    .HasForeignKey(nl => nl.NoteId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.GoogleId)
                .IsUnique();

                // One label can be associated with multiple notes
                entity.HasOne(nl => nl.Label)
                    .WithMany(l => l.NoteLabels)
                    .HasForeignKey(nl => nl.LabelId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

