using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class FoodJournalContext : DbContext
    {
        public FoodJournalContext(DbContextOptions<FoodJournalContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Dose>? Doses { get; set; }
        public DbSet<JournalEntry>? JournalEntries { get; set; }
        public DbSet<JournalEntryDose>? JournalEntryDoses { get; set; }
        public DbSet<JournalEntryNutritionalInfo>? JournalEntryNutritionalInfos { get; set; }
        public DbSet<JournalEntryTag>? JournalEntryTags { get; set; }
        public DbSet<NutritionalInfo>? NutritionalInfos { get; set; }
        public DbSet<Password>? Passwords { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<UserJournalEntry>? UserJournalEntries { get; set; }
        public DbSet<UserPassword>? UserPasswords { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                       .SelectMany(t => t.GetProperties())
                                                       .Where(p => p.ClrType == typeof(decimal)))
            {
                property.SetColumnType("decimal(18, 2)");
            }
        }
    }
}