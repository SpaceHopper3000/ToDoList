using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Definitions;

class Program
{
    static void Main(string[] args)
    {
        var host = AppStartup();
        InitJourney(host, args);
    }

    static IHost AppStartup()
    {
        // Initiated the denpendency injection container 
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
        if (args.Length == 0)
        {
            var testString = @"Create --Title ""Test Event"" --Date ""22/01/22""";
            //var testString = @"Read --All";
            var testArgs = testString.Split();

            //https://github.com/commandlineparser/commandline

            var result = Parser.Default.ParseArguments<Verbs.CreateOptions, Verbs.ViewOptions, Verbs.UpdateOptions>(testArgs)
            .MapResult((Verbs.CreateOptions opts) => CreateEntry(opts, host),
                       (Verbs.ViewOptions opts) => ViewEntries(opts, host),
                       (Verbs.UpdateOptions opts) => UpdateEntry(opts, host),
                       errs => HandleParseError(errs));

            Console.WriteLine(result);

        }
        
        //var test = await toDoService.GetAllToDoEntries();

        //var search = await toDoService.SearchEntries("Test");

        //var orderBy = await toDoService.OrderByDateDesc();

        //var date = DateTime.Now;
        //var completeDates = await toDoService.CompleteAllTasksForDate(date);

    }

    private static string CreateEntry(Verbs.CreateOptions opts, IHost host)
    {
        var toDoService = ActivatorUtilities.GetServiceOrCreateInstance<ITodoListService>(host.Services);

        try
        {
            DateTime.TryParse(opts.Date, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out var parsedTime);
            var newEntry = new ToDoEntry { Title = opts.Title, DueDate = parsedTime };
            toDoService.CreateEntry(newEntry);

            return "Successfully added new entry";
        }
        catch (Exception e)
        {
            return e.ToString();
        }

    }
    private static string ViewEntries(Verbs.ViewOptions opts, IHost host)
    {
        return "";

    }
    private static string UpdateEntry(Verbs.UpdateOptions opts, IHost host)
    {
        return "";
    }

    //in case of errors or --help or --version
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
