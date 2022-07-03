using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class ToDoListService : ITodoListService
    {
        private readonly ToDoListDbContext _context;

        public ToDoListService(ToDoListDbContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
        }

        //CREATE

        public bool CreateEntry(ToDoEntry newEntry)
        {
            try
            {
                _context.Add(newEntry);
                _context.SaveChanges();
                _context.ChangeTracker.Clear();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        //READ

        public List<ToDoEntry> GetAllToDoEntries()
        {
            return _context.ToDoEntry.AsNoTracking().ToList();
        }

        public List<ToDoEntry> SearchEntries(string searchTerm)
        {
            return _context.ToDoEntry.AsNoTracking().Where(x => x.Title.ToLower() == searchTerm.ToLower()).ToList();
        }

        public List<ToDoEntry> OrderByDateDesc()
        {
            return _context.ToDoEntry.AsNoTracking().OrderByDescending(x => x.DueDate).ToList();
        }


        //UPDATE

        public bool CompleteTask(string taskName)
        {
            var item = _context.ToDoEntry.AsNoTracking().FirstOrDefault(x => x.Title == taskName);

            if (item == null) return false;

            item.IsComplete = true;
            _context.ToDoEntry.Update(item);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            return true;
        }

        public List<ToDoEntry>? CompleteAllTasksForDate(DateTime dateTime)
        {
            var itemsToUpdate = _context.ToDoEntry.AsNoTracking().Where(x => x.DueDate.Date == dateTime.Date && !x.IsComplete).ToList();

            if (!itemsToUpdate.Any()) return null;

            var updatedList = new List<ToDoEntry>();

            foreach (var item in itemsToUpdate)
            {
                item.IsComplete = true;
                _context.ToDoEntry.Update(item);
                _context.SaveChanges();
                updatedList.Add(item);
                _context.ChangeTracker.Clear();
            }

            return updatedList;
        }

        //DELETE
    }
}
