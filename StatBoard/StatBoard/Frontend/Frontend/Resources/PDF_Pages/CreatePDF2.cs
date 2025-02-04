using PdfSharp.Drawing;
using SkiaSharp;
using PdfSharp.Pdf;

namespace Frontend.Resources.PDF_Pages
{
    public class CreatePDF2
    {
        public PdfDocument CrearPDF2()
        {
            string fileName = "CrearPDF2_partido.pdf";
            string filePath;

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            }
            else
            {
                filePath = Path.Combine(FileSystem.CacheDirectory, fileName); // Fallback
            }

            // Crear un documento PDF
            var pdfDocument = new PdfDocument();
            var pdfPage = pdfDocument.AddPage();

            crearImagenMarcada();

            // Cargar la imagen marcada
            var imagePath = "cancha_marcada.png";
            var image = XImage.FromFile(imagePath);

            // Dibujar la imagen en la página PDF
            var gfx = XGraphics.FromPdfPage(pdfPage);
            gfx.DrawImage(image, 0, 0, pdfPage.Width, pdfPage.Height);

            // Dibujar un punto en las coordenadas (x, y)
            var brush = XBrushes.Red;
            int x = 100; // Coordenada X
            int y = 150; // Coordenada Y
            int pointSize = 10; // Tamaño del punto
            gfx.DrawEllipse(brush, x - pointSize / 2, y - pointSize / 2, pointSize, pointSize);

            // Guardar el PDF
            pdfDocument.Save(filePath);

            return pdfDocument;
        }

        public async void crearImagenMarcada()
        {
            // Cargar la imagen de la cancha
            using var imageStream = await FileSystem.OpenAppPackageFileAsync("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha.png");
            using var bitmap = SKBitmap.Decode(imageStream);

            // Crear un canvas para dibujar sobre la imagen
            using var canvas = new SKCanvas(bitmap);

            // Definir un pincel para dibujar los puntos
            var paint = new SKPaint
            {
                Color = SKColors.Green,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            // Dibujar un punto en las coordenadas (x, y)
            int x = 100; // Coordenada X
            int y = 150; // Coordenada Y
            int pointSize = 10; // Tamaño del punto
            canvas.DrawCircle(x, y, pointSize / 2, paint);

            // Guardar la imagen con las marcas
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var fileStream = new FileStream("cancha_marcada.png", FileMode.Create, FileAccess.Write);
            data.SaveTo(fileStream);
        }
    }
}
