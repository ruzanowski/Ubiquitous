using System.IO;
using Caracan.Pdf.Configuration;

namespace Caracan.Pdf.Services.HtmlBuilder
{
    public interface IReportBuilder
    {
        IReportBuilder WithData(object data);
        IReportBuilder WithOptions(CaracanPdfOptions options);
        IReportBuilder AddDefaultTemplate();
        IReportBuilder AddTemplate();
        IReportBuilder AddCharts(string chartHtml);
        Stream ToStream();
    }
}