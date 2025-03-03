using SkiaSharp;
using Frontend.Resources.Entities;
using PdfSharp.Pdf;

namespace Frontend.Resources.PDF_Pages
{
    public class CrearPDF_Android
    {
        private Match_Dto? Match { get; set; }
        private Club_Dto? TeamLocal { get; set; }
        private Club_Dto? TeamAway { get; set; }

        public async Task<PdfDocument> CrearPDF_A(Guid idMatch)
        {
            if (!LoadData(idMatch))
            {
                Console.WriteLine("Error al cargar los datos");
                return null;
            }

            // Solicitar permisos de escritura en tiempo de ejecución
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                Console.WriteLine("Permiso de escritura en almacenamiento no concedido.");
                return null;
            }

            // Crear un documento PDF
            var pdfDocument = new PdfDocument();

            // Construir el contenido del documento
            BuildDocument(pdfDocument);

            string fileName = "StatBoard_Android.pdf";
            //var customFilePath = FileSystem.Current.AppDataDirectory;
            string customFilePath = Path.Combine(FileSystem.CacheDirectory, "Documents");
            //string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            // Verificar y crear la carpeta si no existe
            if (!Directory.Exists(customFilePath))
            {
                Directory.CreateDirectory(customFilePath);
            }

            string filePath = Path.Combine(customFilePath, fileName);

            try
            {
                Console.WriteLine($"Guardando PDF en: {filePath}");
                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    pdfDocument.Save(stream);
                //}
                pdfDocument.Save(filePath);

                Console.WriteLine($"PDF guardado en: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el PDF: {ex.Message}");
            }

            return pdfDocument;
        }

        private void BuildDocument(PdfDocument pdfDocument)
        {
            // Primera página
            var pdfPage = pdfDocument.AddPage();
            using (var canvas = GetCanvas(pdfPage))
            {
                SummaryMatch(canvas, pdfPage);
                Footer(canvas, pdfPage, pdfDocument);
            }

            // Segunda página - Equipo Local
            pdfPage = pdfDocument.AddPage();
            using (var canvas = GetCanvas(pdfPage))
            {
                SummaryTeam(canvas, pdfPage, TeamLocal);
                Footer(canvas, pdfPage, pdfDocument);
            }

            // Tercera página - Equipo Visitante
            pdfPage = pdfDocument.AddPage();
            using (var canvas = GetCanvas(pdfPage))
            {
                SummaryTeam(canvas, pdfPage, TeamAway);
                Footer(canvas, pdfPage, pdfDocument);
            }

            // Páginas para jugadores del equipo local
            foreach (var idPlayer in TeamLocal.IdPlayers)
            {
                pdfPage = pdfDocument.AddPage();
                using (var canvas = GetCanvas(pdfPage))
                {
                    SummaryPlayer(canvas, pdfPage, idPlayer);
                    Footer(canvas, pdfPage, pdfDocument);
                }
            }

            // Páginas para jugadores del equipo visitante
            foreach (var idPlayer in TeamAway.IdPlayers)
            {
                pdfPage = pdfDocument.AddPage();
                using (var canvas = GetCanvas(pdfPage))
                {
                    SummaryPlayer(canvas, pdfPage, idPlayer);
                    Footer(canvas, pdfPage, pdfDocument);
                }
            }
        }

        private void SummaryMatch(SKCanvas canvas, PdfPage pdfPage)
        {
            float yPos = 40;
            using (var textPaint = new SKPaint { Color = SKColors.Black })
            {
                using (var skFont = new SKFont(SKTypeface.Default, 40))
                {
                    // Título
                    canvas.DrawText("STAT-BOARD", 200, yPos, SKTextAlign.Left, skFont, textPaint);
                    yPos += 50;

                    // Información del partido
                    using (var skFontSmall = new SKFont(SKTypeface.Default, 30))
                    {
                        canvas.DrawText($"Día: {Match.Date:d}", 50, yPos, SKTextAlign.Left, skFontSmall, textPaint);
                        canvas.DrawText($"Torneo: {Match.Tournament}", 300, yPos, SKTextAlign.Left, skFontSmall, textPaint);
                        yPos += 40;
                        canvas.DrawText($"Lugar: {Match.Place}", 50, yPos, SKTextAlign.Left, skFontSmall, textPaint);
                        canvas.DrawText($"Fecha: {Match.MatchWeek}", 300, yPos, SKTextAlign.Left, skFontSmall, textPaint);
                        yPos += 60;
                    }

                    // Tabla de resultados
                    DrawTable(canvas, yPos);
                }
            }
        }

        private void DrawTable(SKCanvas canvas, float yPos)
        {
            float xStart = 50;
            float[] columnWidths = { 200, 50, 50, 200 };
            float tableWidth = columnWidths.Sum();
            float rowHeight = 40;

            using (var linePaint = new SKPaint { Color = SKColors.Black, StrokeWidth = 3 })
            {
                // Línea superior de la tabla
                canvas.DrawLine(xStart, yPos, xStart + tableWidth, yPos, linePaint);

                // Encabezado de la tabla
                using (var textPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true })
                {
                    using (var typefaceBold = SKTypeface.FromFamilyName(null, SKFontStyle.Bold))
                    {
                        using (var textFont = new SKFont(typefaceBold, 30))
                        {
                            canvas.DrawText(TeamLocal.Name.ToUpper(), xStart + 10, yPos + 30, SKTextAlign.Left, textFont, textPaint);
                            canvas.DrawText(Match.GoalsTeamA.ToString(), xStart + columnWidths[0] + 20, yPos + 30, SKTextAlign.Left, textFont, textPaint);
                            canvas.DrawText(Match.GoalsTeamB.ToString(), xStart + columnWidths[0] + columnWidths[1] + 20, yPos + 30, SKTextAlign.Left, textFont, textPaint);
                            canvas.DrawText(TeamAway.Name.ToUpper(), xStart + columnWidths[0] + columnWidths[1] + columnWidths[2] + 20, yPos + 30, SKTextAlign.Left, textFont, textPaint);

                            yPos += rowHeight;
                            canvas.DrawLine(xStart, yPos, xStart + tableWidth, yPos, linePaint); // Línea bajo encabezado
                        }
                    }
                }
            }
        }

        private SKCanvas GetCanvas(PdfPage pdfPage)
        {
            var skBitmap = new SKBitmap((int)pdfPage.Width.Point, (int)pdfPage.Height.Point);
            return new SKCanvas(skBitmap);
        }

        private void Footer(SKCanvas canvas, PdfPage pdfPage, PdfDocument pdfDocument)
        {
            int pageIndex = pdfDocument.Pages.Cast<PdfPage>().ToList().IndexOf(pdfPage) + 1;
            int totalPages = pdfDocument.PageCount;

            using (var textPaint = new SKPaint { Color = SKColors.Gray, IsAntialias = true })
            {
                using (var textFont = new SKFont(SKTypeface.Default, 20))
                {
                    float xPos = (float)pdfPage.Width.Point / 2;
                    float yPos = (float)pdfPage.Height.Point - 30;

                    canvas.DrawText($"Página {pageIndex} de {totalPages}", xPos, yPos, SKTextAlign.Center, textFont, textPaint);
                }
            }
        }

        private void SummaryPlayer(SKCanvas canvas, PdfPage pdfPage, Guid idPlayer)
        {
            var result = Simulo_BdD.GetOnePlayer(idPlayer);
            if (!result.Success)
            {
                Console.WriteLine(result.Message);
                return;
            }

            float yPosition = 40;
            using (var textPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true })
            {
                using (var typefaceBold = SKTypeface.FromFamilyName(null, SKFontStyle.Bold))
                {
                    using (var textFont = new SKFont(typefaceBold, 35))
                    {
                        // Crear SKTextBlob para el texto
                        using var textBlob = SKTextBlob.Create($"{result.Data.Name} - {result.Data.Number}", textFont);

                        // Convertir XUnit a float
                        float xPos = (float)pdfPage.Width.Point / 2;
                        float yPos = yPosition;

                        // Dibujar el texto
                        canvas.DrawText($"{result.Data.Name} - {result.Data.Number}", xPos, yPos, SKTextAlign.Left, textFont, textPaint);
                        yPosition += 60;

                        // Secciones de estadísticas
                        yPosition = EndingSection(canvas, pdfPage, result.Data.Id, Ending.Goal, yPosition);
                        yPosition = EndingSection(canvas, pdfPage, result.Data.Id, Ending.Blocked, yPosition);
                        yPosition = EndingSection(canvas, pdfPage, result.Data.Id, Ending.Save, yPosition);
                        yPosition = EndingSection(canvas, pdfPage, result.Data.Id, Ending.Foul, yPosition);
                        yPosition = EndingSection(canvas, pdfPage, result.Data.Id, Ending.Miss, yPosition);
                        yPosition = EndingSection(canvas, pdfPage, result.Data.Id, Ending.Steal_W, yPosition);
                        yPosition = EndingSection(canvas, pdfPage, result.Data.Id, Ending.Steal_L, yPosition);

                        // Contar 2 minutos, rojas y azules
                        var stats = Functions.GetActionCountForPlayer(idPlayer, Ending.Foul);

                        using (var statsFont = new SKFont(SKTypeface.Default, 30))
                        {
                            float textX = 50;

                            if (stats.Quantity2min.HasValue)
                            {
                                canvas.DrawText($"2 minutos: {stats.Quantity2min.Value}", textX, yPosition, SKTextAlign.Left, statsFont, textPaint);
                                yPosition += 40;
                            }
                            if (stats.Red.HasValue)
                            {
                                canvas.DrawText($"Rojas: {stats.Red.Value}", textX, yPosition, SKTextAlign.Left, statsFont, textPaint);
                                yPosition += 40;
                            }
                            if (stats.Blue.HasValue)
                            {
                                canvas.DrawText($"Azules: {stats.Blue.Value}", textX, yPosition, SKTextAlign.Left, statsFont, textPaint);
                                yPosition += 40;
                            }
                        }
                    }
                }
            }
        }

        private float EndingSection(SKCanvas canvas, PdfPage pdfPage, Guid idPlayer, Ending end, float yPosition)
        {
            var temp = Functions.GetActionCountForPlayer(idPlayer, end);

            using (var textPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true })
            {
                using (var textFont = new SKFont(SKTypeface.Default, 30))
                {
                    canvas.DrawText($"{end}: {temp.QuantityEnding}", 50, yPosition, SKTextAlign.Left, textFont, textPaint);
                }
            }

            yPosition += 40;

            // Obtener imágenes con marcas
            string canchaWithMarksPath = GenerateMarkedImage(GetImagePath("cancha.png"), temp.CooField, new SKColor(255, 0, 0, 180)); // Cambiado aquí
            string arcoWithMarksPath = null;

            if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
            {
                arcoWithMarksPath = GenerateMarkedImage(GetImagePath("arco.png"), temp.CooGoal, new SKColor(0, 0, 255, 180)); // Cambiado aquí
            }

            try
            {
                if (!string.IsNullOrEmpty(canchaWithMarksPath))
                {
                    using var canchaImage = LoadSkImage(canchaWithMarksPath);
                    canvas.DrawBitmap(canchaImage, 50, yPosition);
                    yPosition += 120;
                }

                if (!string.IsNullOrEmpty(arcoWithMarksPath))
                {
                    using var arcoImage = LoadSkImage(arcoWithMarksPath);
                    canvas.DrawBitmap(arcoImage, 220, yPosition - 120);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir imágenes: {ex.Message}");
            }

            return yPosition + 20;
        }


        private float EndingSection(SKCanvas canvas, PdfPage pdfPage, Club_Dto team, Ending end, float yPosition)
        {
            int totalEndings = 0;
            List<Coordenates> marcasCampo = new List<Coordenates>();
            List<Coordenates> marcasArco = new List<Coordenates>();

            foreach (var idPlayer in team.IdPlayers)
            {
                var temp = Functions.GetActionCountForPlayer(idPlayer, end);
                totalEndings += temp.QuantityEnding;

                if (temp.CooField != null)
                    marcasCampo.AddRange(temp.CooField);
                if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
                    if (temp.CooGoal != null)
                        marcasArco.AddRange(temp.CooGoal);
            }

            using (var textPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true })
            {
                using (var textFont = new SKFont(SKTypeface.Default, 30))
                {
                    canvas.DrawText($"{end}: {totalEndings}", 50, yPosition, SKTextAlign.Left, textFont, textPaint);
                }
            }
            yPosition += 40;

            string canchaWithMarksPath = GenerateMarkedImage(GetImagePath("cancha.png"), marcasCampo, new SKColor(255, 0, 0, 180)); // Cambiado aquí
            string arcoWithMarksPath = GenerateMarkedImage(GetImagePath("arco.png"), marcasArco, new SKColor(0, 0, 255, 180)); // Cambiado aquí

            try
            {
                if (!string.IsNullOrEmpty(canchaWithMarksPath))
                {
                    using var canchaImage = LoadSkImage(canchaWithMarksPath);
                    canvas.DrawBitmap(canchaImage, 50, yPosition);
                    yPosition += 120;
                }

                if (!string.IsNullOrEmpty(arcoWithMarksPath))
                {
                    using var arcoImage = LoadSkImage(arcoWithMarksPath);
                    canvas.DrawBitmap(arcoImage, 220, yPosition - 120);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir imágenes: {ex.Message}");
            }

            return yPosition + 20;
        }

        private void SummaryTeam(SKCanvas canvas, PdfPage pdfPage, Club_Dto team)
        {
            float yPosition = 40;

            using var typeface = SKTypeface.FromFamilyName(null, SKFontStyle.Bold);
            using var font = new SKFont(typeface, 40);

            using var titlePaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true
            };

            using var textBlob = SKTextBlob.Create("STAT-BOARD", font);
            canvas.DrawText(textBlob, 200, yPosition, titlePaint);

            using var teamNameBlob = SKTextBlob.Create(team.Name, font);
            float xPos = (float)pdfPage.Width.Point / 2;
            canvas.DrawText(teamNameBlob, xPos, yPosition, titlePaint);
            yPosition += 60;

            yPosition = EndingSection(canvas, pdfPage, team, Ending.Goal, yPosition);
            yPosition = EndingSection(canvas, pdfPage, team, Ending.Blocked, yPosition);
            yPosition = EndingSection(canvas, pdfPage, team, Ending.Save, yPosition);
            yPosition = EndingSection(canvas, pdfPage, team, Ending.Foul, yPosition);
            yPosition = EndingSection(canvas, pdfPage, team, Ending.Miss, yPosition);
            yPosition = EndingSection(canvas, pdfPage, team, Ending.Steal_W, yPosition);
            yPosition = EndingSection(canvas, pdfPage, team, Ending.Steal_L, yPosition);

            int minutes2 = 0, reds = 0, blues = 0;

            foreach (var idPlayer in team.IdPlayers)
            {
                var temp = Functions.GetActionCountForPlayer(idPlayer, Ending.Foul);
                if (temp.Quantity2min.HasValue) minutes2 += temp.Quantity2min.Value;
                if (temp.Red.HasValue) reds += temp.Red.Value;
                if (temp.Blue.HasValue) blues += temp.Blue.Value;
            }

            using (var infoPaint = new SKPaint { Color = SKColors.Black, IsAntialias = true })
            {
                using (var infoFont = new SKFont(SKTypeface.Default, 30))
                {
                    if (minutes2 > 0)
                    {
                        canvas.DrawText($"2 minutos: {minutes2}", 50, yPosition, SKTextAlign.Left, infoFont, infoPaint);
                        yPosition += 40;
                    }
                    if (reds > 0)
                    {
                        canvas.DrawText($"Rojas: {reds}", 50, yPosition, SKTextAlign.Left, infoFont, infoPaint);
                        yPosition += 40;
                    }
                    if (blues > 0)
                    {
                        canvas.DrawText($"Azules: {blues}", 50, yPosition, SKTextAlign.Left, infoFont, infoPaint);
                        yPosition += 40;
                    }
                }
            }
        }

        private SKBitmap LoadSkImage(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return SKBitmap.Decode(stream);
        }

        private string GenerateMarkedImage(string imageName, List<Coordenates> marks, SKColor color)
        {
            string outputPath = Path.Combine(FileSystem.AppDataDirectory, $"{Path.GetFileNameWithoutExtension(imageName)}_marked.png");

            try
            {
                string imagePath = GetImagePath(imageName); // Cambiado aquí
                if (!File.Exists(imagePath))
                {
                    Console.WriteLine($"Imagen no encontrada: {imagePath}");
                    return null;
                }

                using var imageStream = File.OpenRead(imagePath);
                using var bitmap = SKBitmap.Decode(imageStream);
                using var canvas = new SKCanvas(bitmap);

                using (var paint = new SKPaint { Color = color, IsAntialias = true, Style = SKPaintStyle.Fill })
                {
                    foreach (var mark in marks)
                    {
                        canvas.DrawCircle(mark.X, mark.Y, 10, paint); // Dibuja el punto en la imagen
                    }

                    using var image = SKImage.FromBitmap(bitmap);
                    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                    using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

                    data.SaveTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar imagen con marcas: {ex.Message}");
                return null;
            }

            return outputPath;
        }

        private bool LoadData(Guid idMatch)
        {
            var result = Simulo_BdD.GetOneMatch(idMatch);
            if (result.Success)
            {
                Match = result.Data;
            }
            else
            {
                Console.WriteLine($"Error al recuperar el partido con ID = {idMatch}");
                return false;
            }

            var resultLocal = Simulo_BdD.GetOneClub(Match.IdTeamLocal);
            if (resultLocal.Success)
            {
                TeamLocal = resultLocal.Data;
            }
            else
            {
                Console.WriteLine($"Error al recuperar el equipo local con ID = {Match.IdTeamLocal}");
                return false;
            }

            var resultAway = Simulo_BdD.GetOneClub(Match.IdTeamAway);
            if (resultAway.Success)
            {
                TeamAway = resultAway.Data;
            }
            else
            {
                Console.WriteLine($"Error al recuperar el equipo visitante con ID = {Match.IdTeamAway}");
                return false;
            }

            return true;
        }

        private string GetImagePath(string imageName)
        {
            string imagePath;

            // Acceder a los recursos incrustados en la aplicación
            var assembly = typeof(CrearPDF_Android).Assembly;
            var resourcePath = $"Frontend.Resources.Images.{imageName}";
            using var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                Console.WriteLine($"Imagen no encontrada: {resourcePath}");
                return null;
            }

            // Guardar el recurso en un archivo temporal
            var tempPath = Path.Combine(FileSystem.CacheDirectory, imageName);
            using var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            imagePath = tempPath;

            return imagePath;
        }
    }
}
