using QuestPDF.Infrastructure;
using WebAPI.Application.DTOs;
using QuestPDF.Fluent;

namespace WebAPI.Application.Services
{
    public class WeekComplantReport : IDocument
    {

            private readonly List<DailyComplaintReportDto> _complaints;

            public WeekComplantReport(List<DailyComplaintReportDto> complaints)
            {
                _complaints = complaints;
            }

            public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

            public void Compose(IDocumentContainer container)
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header().Text("Weekly Complaints Report")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Column(col =>
                    {
                        foreach (var c in _complaints)
                        {
                            col.Item().Border(1).Padding(10).Column(inner =>
                            {
                                inner.Item().Text($"Complaint #{c.Id}").Bold();
                                inner.Item().Text($"Type: {c.Type}");
                                inner.Item().Text($"Location: {c.Location}");
                                inner.Item().Text($"Status: {c.Status}");
                                inner.Item().Text($"User: {c.UserName}");
                                inner.Item().Text($"Date: {c.Date}");
                                if (c.ImagePaths != null && c.ImagePaths.Any())
                                {
                                    inner.Item().PaddingTop(5).Text("Images:").Bold();

                                    inner.Item().Grid(grid =>
                                    {
                                        grid.Columns(3);

                                        foreach (var img in c.ImagePaths)
                                        {
                                            var fullPath = GetFullImagePath(img);

                                            if (File.Exists(fullPath))
                                            {
                                                grid.Item()
                                               .Border(1)
                                            .Padding(5)
                                           .Height(120)
                                           .AlignMiddle()
                                          .AlignCenter()
                                         .Image(fullPath)
                                          .FitArea();

                                            }
                                        }
                                    });
                                }
                                inner.Item().PaddingTop(5).Text("History:").Bold();

                                foreach (var h in c.History)
                                {
                                    inner.Item().Text(
                                        $"- {h.ActionDate:g} | {h.ActionType} → {h.NewValue}"
                                    ).FontSize(10);
                                }
                            });

                            col.Item().PaddingBottom(10);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generated at ");
                            x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        });
                });
            }
            private string GetFullImagePath(string relativePath)
            {
                return Path.Combine(
                    Directory.GetCurrentDirectory(),
                    relativePath.Replace("\\", Path.DirectorySeparatorChar.ToString())
                );
            }

        }

    



}

