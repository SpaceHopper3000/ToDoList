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

        [Verb("Read", HelpText = "Read to do entries, if no option is specified, all records will be returned")]
        public class ViewOptions
        {
            [Option('f', "Filter", Required = false, HelpText = "Filter items by Title")]
            public string Title { get; set; }

            [Option('o', "OrderBy", Required = false, HelpText = "Order items by Date - Expects --OrderBy 'Y'")]
            public string OrderBy { get; set; }

        }

        [Verb("Update", HelpText = "Mark enty(ies) as complete")]
        public class UpdateOptions
        {
            [Option('t', "Title", Required = false, HelpText = "Mark an entry as complete by defining the title")]
            public string Title { get; set; }

            [Option('d', "ForDate", Required = false, HelpText = "Mark all entries for a given day as complete")]
            public string Date { get; set; }
        }
    }

}
