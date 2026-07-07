using Amazon.S3.Model.Internal.MarshallTransformations;
using Notification.Infrastructure.Documents.Interface;
using QRCoder;
using QuestPDF.Infrastructure;
using SkiaSharp;
using System.Drawing;

namespace Notification.Infrastructure.Documents
{
    public class QrGenerator:IQrGenerator
    {
        public byte[] GenerateValidationQrCode(string ticketCode)
        {
            string validationUrl = $"https://app.tudiscoteca.com/validar/{ticketCode}";

            using var qrGenerator = new QRCodeGenerator();

            using var qrCodeData = qrGenerator.CreateQrCode(validationUrl, QRCodeGenerator.ECCLevel.H);

            using var qrCode = new PngByteQRCode(qrCodeData);

            // 🎨 2. COLOR PERSONALIZADO
            var colorQr = new byte[] { 20, 20, 20, 255 }; // Casi negro
            var colorFondo = new byte[] { 255, 255, 255, 255 }; // Blanco puro

            var qrImageBytes = qrCode.GetGraphic(10, colorQr, colorFondo, drawQuietZones: false);

            // 🚀 3. INCRUSTAR EL LOGO
            // Cargamos el QR recién generado
            using var qrBitMap = SKBitmap.Decode(qrImageBytes);
            using var canvas = new SKCanvas(qrBitMap);

            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "logo-cliente.png");

            if (File.Exists(logoPath))
            {
                using var logoBitMap = SKBitmap.Decode(logoPath);
                int logoWidth = logoBitMap.Width / 4;

                using var resizedLog = new SKBitmap(logoWidth, logoWidth);
                logoBitMap.ScalePixels(resizedLog, new SKSamplingOptions(SKCubicResampler.Mitchell));

                float x = (qrBitMap.Width - logoWidth) / 2f;
                float y = (qrBitMap.Height - logoWidth) / 2f;

                canvas.DrawBitmap(resizedLog, x, y, new SKSamplingOptions());
            }

            using var image = SKImage.FromBitmap(qrBitMap);

            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return data.ToArray();
        }
    }
}
