using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace ToDoList.Data
{
    public class ToDoListDbContext : DbContext
    {
        public DbSet<ToDoEntry> ToDoEntry { get; set; }

        public string DbPath { get; }

        public ToDoListDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "ToDoList.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoEntry>().HasData(
                new ToDoEntry
                {
                    Id = 1,
                    Title = "Set Up ToDo Application",
                    DueDate = DateTime.Now,
                    IsComplete = true
                },
                new ToDoEntry
                {
                    Id = 2,
                    Title = "Add a first entry",
                    DueDate = DateTime.Now,
                    IsComplete = true
                });
        }
    }

}
