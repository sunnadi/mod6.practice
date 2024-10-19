using System;
using System.Collections.Generic;
using System.IO;

public interface IReportBuilder
{
    void SetHeader(string header);
    void SetContent(string content);
    void SetFooter(string footer);
    void AddSection(string sectionName, string sectionContent);
    void SetStyle(ReportStyle style);
    Report GetReport();
}

public class ReportStyle
{
    public string BackgroundColor { get; set; }
    public string FontColor { get; set; }
    public int FontSize { get; set; }

    public ReportStyle(string bgColor, string fontColor, int fontSize)
    {
        BackgroundColor = bgColor;
        FontColor = fontColor;
        FontSize = fontSize;
    }
}

public class Report
{
    public string Header { get; private set; }
    public string Content { get; private set; }
    public string Footer { get; private set; }
    public List<string> Sections { get; private set; } = new List<string>();
    public ReportStyle Style { get; private set; }

    public void SetHeader(string header) => Header = header;
    public void SetContent(string content) => Content = content;
    public void SetFooter(string footer) => Footer = footer;
    public void AddSection(string section) => Sections.Add(section);
    public void SetStyle(ReportStyle style) => Style = style;

    public void Export(string format)
    {
        string reportContent = "";

        switch (format.ToLower())
        {
            case "text":
                reportContent += "Текстовый отчет:\n";
                reportContent += $"Заголовок: {Header}\n";
                reportContent += $"Содержание: {Content}\n";
                foreach (var section in Sections)
                {
                    reportContent += $"Раздел: {section}\n";
                }
                reportContent += $"Подвал: {Footer}\n";
                File.WriteAllText("report.txt", reportContent);
                Console.WriteLine("Текстовый отчет сохранен в report.txt");
                break;
            case "html":
                reportContent += "<html>\n";
                reportContent += $"<h1>{Header}</h1>\n";
                reportContent += $"<p>{Content}</p>\n";
                foreach (var section in Sections)
                {
                    reportContent += $"<h2>{section}</h2>\n";
                }
                reportContent += $"<footer>{Footer}</footer>\n";
                reportContent += "</html>";
                File.WriteAllText("report.html", reportContent);
                Console.WriteLine("HTML отчет сохранен в report.html");
                break;
            case "pdf":
                
                reportContent += "PDF отчет создан (симуляция)\n";
                File.WriteAllText("report.pdf", reportContent);
                Console.WriteLine("PDF отчет сохранен в report.pdf");
                break;
            default:
                Console.WriteLine("Неизвестный формат");
                break;
        }
    }
}

public class TextReportBuilder : IReportBuilder
{
    private Report _report = new Report();

    public void SetHeader(string header) => _report.SetHeader(header);
    public void SetContent(string content) => _report.SetContent(content);
    public void SetFooter(string footer) => _report.SetFooter(footer);
    public void AddSection(string sectionName, string sectionContent) => _report.AddSection($"{sectionName}: {sectionContent}");
    public void SetStyle(ReportStyle style) => _report.SetStyle(style);
    public Report GetReport() => _report;
}

public class HtmlReportBuilder : IReportBuilder
{
    private Report _report = new Report();

    public void SetHeader(string header) => _report.SetHeader(header);
    public void SetContent(string content) => _report.SetContent(content);
    public void SetFooter(string footer) => _report.SetFooter(footer);
    public void AddSection(string sectionName, string sectionContent) => _report.AddSection($"<section><h2>{sectionName}</h2><p>{sectionContent}</p></section>");
    public void SetStyle(ReportStyle style) => _report.SetStyle(style);
    public Report GetReport() => _report;
}

public class PdfReportBuilder : IReportBuilder
{
    private Report _report = new Report();

    public void SetHeader(string header) => _report.SetHeader(header);
    public void SetContent(string content) => _report.SetContent(content);
    public void SetFooter(string footer) => _report.SetFooter(footer);
    public void AddSection(string sectionName, string sectionContent) => _report.AddSection($"{sectionName}: {sectionContent}");
    public void SetStyle(ReportStyle style) => _report.SetStyle(style);
    public Report GetReport() => _report;
}

public class ReportDirector
{
    public void ConstructReport(IReportBuilder builder, ReportStyle style)
    {
        builder.SetStyle(style);
        builder.SetHeader("Отчет о продажах за 2024 год");
        builder.SetContent("Общее содержание отчета о продажах.");
        builder.AddSection("Продажи", "Данные о продажах за январь.");
        builder.AddSection("Маркетинг", "Анализ маркетинговых затрат.");
        builder.SetFooter("Конец отчета.");
    }
}


class Program
{
    static void Main(string[] args)
    {
        var style = new ReportStyle("white", "black", 12);

        ReportDirector director = new ReportDirector();

        IReportBuilder textBuilder = new TextReportBuilder();
        director.ConstructReport(textBuilder, style);
        Report textReport = textBuilder.GetReport();
        textReport.Export("text");

        IReportBuilder htmlBuilder = new HtmlReportBuilder();
        director.ConstructReport(htmlBuilder, style);
        Report htmlReport = htmlBuilder.GetReport();
        htmlReport.Export("html");

        IReportBuilder pdfBuilder = new PdfReportBuilder();
        director.ConstructReport(pdfBuilder, style);
        Report pdfReport = pdfBuilder.GetReport();
        pdfReport.Export("pdf");
    }
}
