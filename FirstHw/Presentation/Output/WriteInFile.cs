using Domain.Models;
using Presentation.Interfaces;

namespace Presentation.Output
{
    public class WriteInFile : IReportWriter
    {
        public void ShowReport(List<ClientReport> clientReports)
        {
            var fileName = $"report_{Guid.NewGuid()}.txt";
            var outPath = Path.Combine(AppContext.BaseDirectory, fileName);
            var outStrings = new List<string>();

            foreach (var clientReport in clientReports)
            {
                outStrings.Add(OutputFormatter.ReportFormat(clientReport));
            }

            File.WriteAllLines(outPath, outStrings);

            Console.WriteLine($"Отчет создан в файле - {fileName}. В Папке - {AppContext.BaseDirectory}");
        }
    }
}
