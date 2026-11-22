using Domain.Models;
using Presentation.Interfaces;

namespace Presentation.Output
{
    public class WriteInConsole : IReportWriter
    {
        public void ShowReport(List<ClientReport> clientReports)
        {
            foreach (var client in clientReports)
            {
                Console.WriteLine(OutputFormatter.ReportFormat(client));
            }
        }
    }
}
