using System.Threading.Tasks;
using Caracan.Pdf.Configuration;
using Caracan.Pdf.Extensions;
using Caracan.Pdf.Models;
using Caracan.Pdf.Services.IPdfGenerator;
using U.EventBus.Abstractions;

namespace U.ReportService.Handlers
{
    public class GenerateProductReportEventHandler : IIntegrationEventHandler<GenerateProductReportEvent>
    {
        private readonly ICaracanPdfGenerator _pdfGenerator;

        public GenerateProductReportEventHandler(ICaracanPdfGenerator pdfGenerator)
        {
            _pdfGenerator = pdfGenerator;
        }

        public async Task Handle(GenerateProductReportEvent @event)
        {
            //todo PdfOptions Factory
            var pdfOptions = new CaracanPdfOptions
            {
                Format = new Format
                {
                    Type = FormatType.A4
                },
                Landscape = false,
                MarginOptions = new MarginOptions
                {
                    Bottom = "40px",
                    Left = "40px",
                    Right = "40px",
                    Top = "40px"
                },
                FooterTemplate = "Caracan.Ubiquitous",
                HeaderTemplate = "Caracan.Ubiquitous",
                
            };

            var generatedReport = await _pdfGenerator.CreatePdfAsync(@event, pdfOptions);
            generatedReport.ToPdf();
        }
    }
}