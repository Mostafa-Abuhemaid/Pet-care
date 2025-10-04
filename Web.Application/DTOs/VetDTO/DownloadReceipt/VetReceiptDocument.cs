
 using QuestPDF.Fluent;
 using QuestPDF.Infrastructure;
 using QuestPDF.Helpers;
namespace Web.Application.DTOs.VetDTO.DownloadReceipt
{ 

    public class VetReceiptDocument : IDocument
    {
        private readonly VetBookingReceiptDTO _dto;

        public VetReceiptDocument(VetBookingReceiptDTO dto)
        {
            _dto = dto;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Text("E-Receipt").FontSize(20).Bold();

                page.Content().Column(col =>
                {
                    col.Item().Text($"Receipt Number: {_dto.ReceiptNumber}");
                    col.Item().Text($"Pet: {_dto.PetName}");
                    col.Item().Text($"Clinic: {_dto.ClinicName}");
                    col.Item().Text($"Date: {_dto.Date:dd/MM/yyyy}");
                    col.Item().Text($"Time: {_dto.Time}");
                    col.Item().Text($"Location: {_dto.Location}");
                    col.Item().Text($"Price: {_dto.Price:C}");

                    col.Item().Text("Services:").Bold();
                    foreach (var s in _dto.Services)
                        col.Item().Text($"- {s}");

                    col.Item().PaddingTop(10).Text("Instructions / Terms:").Bold();
                    col.Item().Text(_dto.InstructionsOrTerms);
                });

                page.Footer().AlignCenter().Text($"Generated at {DateTime.Now:dd/MM/yyyy HH:mm}");
            });
        }
    }

}
