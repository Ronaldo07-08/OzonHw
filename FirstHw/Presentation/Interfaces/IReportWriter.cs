using Domain.Models;


namespace Presentation.Interfaces
{
    public interface IReportWriter
    {
        public void ShowReport(List<ClientReport> clientReports);
    }
}
