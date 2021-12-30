using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Day3_Lab.Models
{
    public partial class userDBContext : DbContext
    {
        public userDBContext()
            : base("name=userBDContext")
        {
        }

        public virtual DbSet<catalog> catalogs { get; set; }
        public virtual DbSet<news> news { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<user_data> user_data { get; set; }
        public virtual DbSet<skill> skills { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<catalog>()
                .Property(e => e.cat_name)
                .IsUnicode(false);

            modelBuilder.Entity<news>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<news>()
                .Property(e => e.brief)
                .IsUnicode(false);

            modelBuilder.Entity<news>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<news>()
                .Property(e => e.photo)
                .IsUnicode(false);

            modelBuilder.Entity<user_data>()
                .HasOptional(e => e.skill)
                .WithRequired(e => e.user_data);

            modelBuilder.Entity<skill>()
                .Property(e => e.skill_name)
                .IsUnicode(false);
        }
    }
}
