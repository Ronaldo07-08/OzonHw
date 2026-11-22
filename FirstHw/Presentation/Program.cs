using Application;
using Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Input;
using Presentation.Interfaces;
using Presentation.Output;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Неверный формат ввода");
            Console.WriteLine("arg 1 - <Формат вывода> arg 2 - <Id клиента> arg 3 - <Id клиента> ...");
            Console.WriteLine($"Доступные форматы: {string.Join(", ", Enum.GetNames(typeof(OutputMode)))}");
            return;
        }

        try
        {
            var services = new ServiceCollection();
            var mode = args[0];

            Builder.ConfigureServices(services, mode);

            using var provider = services.BuildServiceProvider();
            var report = provider.GetRequiredService<Report>();
            var writer = provider.GetRequiredService<IReportWriter>();

            var clientIds = Parser.ParseClientIds(args.Skip(1).ToArray());
            var clientReports = report.GetReport(clientIds);

            writer.ShowReport(clientReports);
        }
        catch (ClientNotFoundException exception)
        {
            Console.WriteLine($"Клиентов с id [{string.Join(", ", exception.ClientIds)}] нет в базе.");
        }
        catch (WrongInputException exception)
        {
            Console.WriteLine(exception.Message);
        }
        catch (BadRepositoryException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}