using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Definitions
{
    public class Verbs
    {
        [Verb("Create", HelpText = "Create a new todo entry")]
        public class CreateOptions
        {
            [Option('t', "Title", Required = false, HelpText = "Title for Entry")]
            public string Title { get; set; }

            [Option('d', "Date", Required = false, HelpText = "Date for Entry")]
            public string Date { get; set; }
        }

        [Verb("Read", HelpText = "Read to do entries")]
        public class ViewOptions
        {
            [Option('a', "All", Required = false, HelpText = "Read all todo entries")]
            public string All { get; set; }

            [Option('d', "fordate", Required = false, HelpText = "Read all todo entries for a given day")]
            public string Date { get; set; }
        }

        [Verb("Update", HelpText = "Mark enty(ies) as complete")]
        public class UpdateOptions
        {
            [Option('u', "complete", Required = false, HelpText = "Mark an entry as complete")]
            public string Title { get; set; }

            [Option('u', "update --all --date", Required = false, HelpText = "Mark all entries for day as complete")]
            public string Date { get; set; }
        }
    }

}
