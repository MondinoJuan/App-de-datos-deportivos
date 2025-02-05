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

            var path = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha.png";
            var greenTranslucentPaint = new SKPaint
            {
                Color = new SKColor(180, 200, 0, 50), // Verde translúcido (RGBA)
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            var exampleCoordinates = new List<Coordenates>
            {
                new Coordenates { X = 50, Y = 100 },
                new Coordenates { X = 55, Y = 100 },
                new Coordenates { X = 60, Y = 100 },
                new Coordenates { X = 53, Y = 100 },
                new Coordenates { X = 150, Y = 200 },
                new Coordenates { X = 250, Y = 300 }
            };

            var imageOutputPath = Path.Combine(FileSystem.AppDataDirectory, "cancha_marcada.png");
            crearImagenMarcada(path, exampleCoordinates, greenTranslucentPaint, imageOutputPath);

            // Cargar la imagen marcada
            var imagePath = "cancha_marcada.png";
            var image = XImage.FromFile(imagePath);

            // Dibujar la imagen en la página PDF
            var gfx = XGraphics.FromPdfPage(pdfPage);
            //gfx.DrawImage(image, 0, 0, pdfPage.Width, pdfPage.Height);
            gfx.DrawImage(image, 0, 0, 150, 200);

            // Dibujar un punto en las coordenadas (x, y)
            //var brush = XBrushes.Red;
            //int x = 100; // Coordenada X
            //int y = 150; // Coordenada Y
            //int pointSize = 10; // Tamaño del punto
            //gfx.DrawEllipse(brush, x - pointSize / 2, y - pointSize / 2, pointSize, pointSize);

            // Guardar el PDF
            pdfDocument.Save(filePath);

            return pdfDocument;
        }

        public async void crearImagenMarcada(string imagePath, List<Coordenates> marks, SKPaint color, string outputPath)
        {
            // Cargar la imagen de la cancha
            using var imageStream = await FileSystem.OpenAppPackageFileAsync(imagePath);
            using var bitmap = SKBitmap.Decode(imageStream);

            // Crear un canvas para dibujar sobre la imagen
            using var canvas = new SKCanvas(bitmap);

            foreach (var mark in marks)
            {
                int pointSize = 20;
                canvas.DrawCircle((float)mark.X, (float)mark.Y, pointSize / 2, color);
            }

            // Guardar la imagen con las marcas
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            data.SaveTo(fileStream);
        }
    }
}
