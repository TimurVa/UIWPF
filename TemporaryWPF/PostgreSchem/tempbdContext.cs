using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TemporaryWPF.PostgreSchem
{
    public partial class tempbdContext : DbContext
    {
        public tempbdContext()
        {
        }

        public tempbdContext(DbContextOptions<tempbdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Shapes> Shapes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=tempbd;Port=5432;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shapes>(entity =>
            {
                entity.ToTable("shapes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Figuretype).HasColumnName("figuretype");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.Leftpos).HasColumnName("leftpos");

                entity.Property(e => e.Toppos).HasColumnName("toppos");

                entity.Property(e => e.Width).HasColumnName("width");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
