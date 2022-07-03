using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Definitions;
using System.Text;
using Microsoft.AspNetCore.Builder;

class Program
{
    static void Main(string[] args)
    {
        var host = AppStartup();
        InitJourney(host, args);
    }

    static IHost AppStartup()
    {
        //https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage
        var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddDbContext<ToDoListDbContext>(ServiceLifetime.Singleton);
                        services.AddSingleton<ITodoListService, ToDoListService>();

                    }).Build();

        return host;
    }

    static void InitJourney(IHost host, string[] args)
    {
        if (args.Length > 0)
        {
            //https://github.com/commandlineparser/commandline
            var result = Parser.Default.ParseArguments<Verbs.CreateOptions, Verbs.ViewOptions, Verbs.UpdateOptions>(args)
            .MapResult((Verbs.CreateOptions opts) => CreateEntry(opts, host),
                       (Verbs.ViewOptions opts) => ViewEntries(opts, host),
                       (Verbs.UpdateOptions opts) => UpdateEntry(opts, host),
                       errs => HandleParseError(errs));

            Console.WriteLine(result);

        }

    }

    private static string CreateEntry(Verbs.CreateOptions opts, IHost host)
    {
        var toDoService = ActivatorUtilities.GetServiceOrCreateInstance<ITodoListService>(host.Services);

        try
        {
            var date = DateTimeParseReturn(opts.Date);
            if (date.HasValue)
            {
                var newEntry = new ToDoEntry { Title = opts.Title, DueDate = date.Value };
                toDoService.CreateEntry(newEntry);

                return "Successfully added new entry";
            }

            return string.Empty;

        }
        catch (Exception e)
        {
            return e.ToString();
        }

    }
    private static string ViewEntries(Verbs.ViewOptions opts, IHost host)
    {
        var toDoService = ActivatorUtilities.GetServiceOrCreateInstance<ITodoListService>(host.Services);

        //Filter by Title
        if (!string.IsNullOrEmpty(opts.Title))
        {
            var filteredItems = toDoService.SearchEntries(opts.Title);
            return StringBuilderFromList(filteredItems);
        }

        //Order by Date
        if (!string.IsNullOrEmpty(opts.OrderBy) && opts.OrderBy == "Y")
        {
            var orderByItems = toDoService.OrderByDateDesc();
            return StringBuilderFromList(orderByItems);

        }

        var listOfItems = toDoService.GetAllToDoEntries();

        return StringBuilderFromList(listOfItems);

    }
    private static string UpdateEntry(Verbs.UpdateOptions opts, IHost host)
    {
        var toDoService = ActivatorUtilities.GetServiceOrCreateInstance<ITodoListService>(host.Services);

        if (!string.IsNullOrEmpty(opts.Title))
        {
            var completedItem = toDoService.CompleteTask(opts.Title);
            if (completedItem == true)
                return $"Successfully updated {opts.Title} as Complete";
        }

        if (!string.IsNullOrEmpty(opts.Date))
        {
            var date = DateTimeParseReturn(opts.Date);
            List<ToDoEntry>? result = toDoService.CompleteAllTasksForDate(date.Value);
            if (!result.Any()) return string.Empty;

            Console.WriteLine("Successfully Updated Tasks:");
            return StringBuilderFromList(result);

        }

        return string.Empty;

    }

    static string StringBuilderFromList(List<ToDoEntry> listOfItems)
    {
        StringBuilder sb = new StringBuilder("");

        if (listOfItems.Any())
        {
            sb.Append("Please find all to do list entries matching criteria below:" + "\n");
            foreach (var item in listOfItems)
            {
                sb.Append("Title =" + item.Title + " Due Date=" + item.DueDate.ToString() + " Is Complete=" + item.IsComplete.ToString() + "\n");
            }
        }

        return sb.ToString();
    }


    static DateTime? DateTimeParseReturn(string dateTime)
    {
        //https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tryparse?view=net-6.0
        DateTime dateValue;
        try
        {
            dateValue = DateTime.Parse(dateTime);
            return DateTime.Parse(dateTime);
        }
        catch (FormatException)
        {
            return null;
        }
    }

    //in case of errors or --help or --version
    //not configured due to project brief
    static string HandleParseError(IEnumerable<Error> errs)
    {
        var result = -2;
        Console.WriteLine("errors {0}", errs.Count());
        if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            result = -1;
        Console.WriteLine("Exit code {0}");
        return "";
    }

}
