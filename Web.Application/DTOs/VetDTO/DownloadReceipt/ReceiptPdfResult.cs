using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO.DownloadReceipt
{
    public class ReceiptPdfResult
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = "receipt.pdf";
        public string ContentType { get; set; } = "application/pdf";
    }

}
