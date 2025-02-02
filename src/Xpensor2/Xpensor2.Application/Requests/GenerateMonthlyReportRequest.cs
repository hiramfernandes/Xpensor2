namespace Xpensor2.Application.Requests
{
    public class GenerateMonthlyReportRequest
    {
        public required string UserName { get; set; }
        public string? UserId { get; set; }
        public required int ReportMonth { get; set; }
        public required int ReportYear { get; set; }
    }
}
