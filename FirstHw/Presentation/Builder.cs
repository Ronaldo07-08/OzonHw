using Application;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Domain.Exceptions;
using Presentation.Interfaces;
using Presentation.Output;

namespace Presentation
{
    public static class Builder
    {
        public static void ConfigureServices(IServiceCollection services, string mode)
        {
            object? parsedMode;
            Enum.TryParse(typeof(OutputMode), mode, true, out parsedMode);
            switch (parsedMode)
            {
                case OutputMode.console:
                    services.AddSingleton<IReportWriter, WriteInConsole>();
                    break;
                case OutputMode.file:
                    services.AddSingleton<IReportWriter, WriteInFile>();
                    break;
                default:
                    throw new WrongInputException("Неверный режим вывода данных");
            }
            services.AddSingleton<IRepository, CSVRepository>();
            services.AddSingleton<Report>();
        }
    }
}
