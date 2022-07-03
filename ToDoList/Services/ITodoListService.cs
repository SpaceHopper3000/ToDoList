using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Services
{
    public interface ITodoListService
    {
        bool CreateEntry(ToDoEntry newEntry);
        List<ToDoEntry> GetAllToDoEntries();
        List<ToDoEntry> SearchEntries(string searchTerm);
        List<ToDoEntry> OrderByDateDesc();
        bool CompleteAllTasksForDate(DateTime dateTime);
        bool CompleteTask(string taskName);

    }
}
