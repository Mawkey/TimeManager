using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeManager.Model;

namespace TimeManager.Services
{
    public class AppDbContext : DbContext
    {
        public static string DATABASE_NAME = "TimeManager.db";
        public DbSet<DayEntry> DayEntries { get; set; }

        public List<DayEntry> GetDays(int days)
        {
            return DayEntries.OrderBy(x => x.Date).Reverse().Take(days).Reverse().Include(x => x.TimeEntries).ToList();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"FileName={DATABASE_NAME}", option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DayEntry>()
                .HasMany(day => day.TimeEntries)
                .WithOne(time => time.DayEntry)
                .IsRequired();
            modelBuilder.Entity<DayEntry>()
                .HasIndex(e => e.Date)
                .IsUnique();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
