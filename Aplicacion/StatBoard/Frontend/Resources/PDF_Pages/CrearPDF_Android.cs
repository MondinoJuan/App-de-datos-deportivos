using SkiaSharp;
using Frontend.Resources.Entities;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.PlatformConfiguration;

#if ANDROID
using Android.Content;
using Android.Provider;
using Android.OS;
using System.IO;
using AndroidX.Core.Content;
using AndroidX.Core.App;
#endif

namespace Frontend.Resources.PDF_Pages
{
    public class CrearPDF_Android
    {
        private Match_Dto? Match { get; set; }
        private Club_Dto? TeamLocal { get; set; }
        private Club_Dto? TeamAway { get; set; }


        public async Task<bool> CrearPDF_A(Guid idMatch)
        {
            if (!LoadData(idMatch))
            {
                Console.WriteLine("Error al cargar los datos");
                return false;
            }

            // Solicitar permisos de escritura en tiempo de ejecución
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                Console.WriteLine("Permiso de escritura en almacenamiento no concedido.");
                return false;
            }

            string fileName = "StatBoard_Android.pdf";

            try
            {
#if ANDROID

        string externalStorage = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments)!.AbsolutePath;
        var path4 = Path.Combine(externalStorage, fileName);
        Console.WriteLine($"PDF guardado temporalmente en: {path4}");

        // Asegurar que el directorio exista
        if (!Directory.Exists(externalStorage))
        {
            Directory.CreateDirectory(externalStorage);
        }

        // Crear el documento PDF con SkiaSharp
        using (var stream = File.Open(path4, FileMode.Create, FileAccess.Write))
        using (var document = SKDocument.CreatePdf(stream))
        {
            BuildDocument(document);
            document.Close();
        }

        // Mover el archivo a la carpeta pública "Downloads" usando MediaStore
        var context = Android.App.Application.Context;
        var values = new ContentValues();
        values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
        values.Put(MediaStore.IMediaColumns.MimeType, "application/pdf");
        values.Put(MediaStore.IMediaColumns.RelativePath, Android.OS.Environment.DirectoryDownloads);

        var contentResolver = context.ContentResolver;
        var uri = contentResolver.Insert(MediaStore.Downloads.ExternalContentUri, values);


        if (uri != null)
        {
            using (var outputStream = contentResolver.OpenOutputStream(uri))
            using (var inputStream = File.OpenRead(path4))
            {
                await inputStream.CopyToAsync(outputStream);
            }

            Console.WriteLine($"PDF guardado en Downloads: {uri}");

            // Eliminar el archivo temporal después de moverlo
            File.Delete(path4);
            Console.WriteLine("Archivo temporal eliminado.");

            // Intent para abrir el PDF después de guardarlo
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "application/pdf");
            intent.SetFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);

            var packageManager = context.PackageManager;
            var activities = packageManager.QueryIntentActivities(intent, 0);
            if (activities.Count > 0)
            {
                context.StartActivity(intent);
            }
            else
            {
                Console.WriteLine("No hay aplicaciones disponibles para abrir el PDF.");
            }
        }
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el PDF: {ex.Message}");
                return false;
            }

            return true;
        }

        private void BuildDocument(SKDocument pdfDocument)
        {
            int totalPages = CalculateTotalPages();
            int currentPage = 0;

            if (pdfDocument == null)
            {
                Console.WriteLine("Error al crear el documento PDF");
                return;
            }

            try
            {
                // Primera página (Resumen del partido)
                currentPage++;
                using (var canvas = pdfDocument.BeginPage(595, 842)) // A4
                {
                    SummaryMatch(canvas);
                    Footer(canvas, pdfDocument, currentPage, totalPages); // Pasa currentPage y totalPages
                    pdfDocument.EndPage();
                }

                // Segunda página (Equipo Local)
                currentPage++;
                using (var canvas = pdfDocument.BeginPage(595, 842))
                {
                    SummaryTeam(canvas, TeamLocal);
                    Footer(canvas, pdfDocument, currentPage, totalPages);
                    pdfDocument.EndPage();
                }

                // Tercera página (Equipo Visitante)
                currentPage++;
                using (var canvas = pdfDocument.BeginPage(595, 842))
                {
                    SummaryTeam(canvas, TeamAway);
                    Footer(canvas, pdfDocument, currentPage, totalPages);
                    pdfDocument.EndPage();
                }

                // Páginas de jugadores locales
                foreach (var idPlayer in TeamLocal.IdPlayers)
                {
                    currentPage++;
                    using (var canvas = pdfDocument.BeginPage(595, 842))
                    {
                        SummaryPlayer(canvas, idPlayer);
                        Footer(canvas, pdfDocument, currentPage, totalPages);
                        pdfDocument.EndPage();
                    }
                }

                // Páginas de jugadores visitantes
                foreach (var idPlayer in TeamAway.IdPlayers)
                {
                    currentPage++;
                    using (var canvas = pdfDocument.BeginPage(595, 842))
                    {
                        SummaryPlayer(canvas, idPlayer);
                        Footer(canvas, pdfDocument, currentPage, totalPages);
                        pdfDocument.EndPage();
                    }
                }
            }
            catch
            {
                pdfDocument.Close();
                Console.WriteLine("Error al crear el documento PDF");
            }
        }

        private int CalculateTotalPages()
        {
            int total = 3; // Páginas fijas: Resumen, Equipo Local, Equipo Visitante
            total += TeamLocal.IdPlayers.Count;
            total += TeamAway.IdPlayers.Count;
            return total;
        }

        private void SummaryMatch(SKCanvas canvas)
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

        private void Footer(SKCanvas canvas, SKDocument pdfDocument, int current, int total)
        {
            using (var textPaint = new SKPaint { Color = SKColors.Gray, IsAntialias = true })
            {
                using (var textFont = new SKFont(SKTypeface.Default, 20))
                {
                    float xPos = 595 / 2; // Centrar en el ancho de la página (A4)
                    float yPos = 842 - 30; // Posicionar en la parte inferior de la página (A4)

                    canvas.DrawText($"Página {current} de {total}", xPos, yPos, SKTextAlign.Center, textFont, textPaint);
                }
            }
        }

        private async void SummaryPlayer(SKCanvas canvas, Guid idPlayer)
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
                        // Dibujar el nombre y número del jugador
                        string playerInfo = $"{result.Data.Name} - {result.Data.Number}";
                        float xPos = 595 / 2; // Centro de la página (A4: 595x842 puntos)
                        canvas.DrawText(playerInfo, xPos, yPosition, SKTextAlign.Center, textFont, textPaint);
                        yPosition += 60;

                        // Secciones de estadísticas
                        yPosition = await EndingSection(canvas, result.Data.Id, Ending.Goal, yPosition);
                        yPosition = await EndingSection(canvas, result.Data.Id, Ending.Blocked, yPosition);
                        yPosition = await EndingSection(canvas, result.Data.Id, Ending.Save, yPosition);
                        yPosition = await EndingSection(canvas, result.Data.Id, Ending.Foul, yPosition);
                        yPosition = await EndingSection(canvas, result.Data.Id, Ending.Miss, yPosition);
                        yPosition = await EndingSection(canvas, result.Data.Id, Ending.Steal_W, yPosition);
                        yPosition = await EndingSection(canvas, result.Data.Id, Ending.Steal_L, yPosition);

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

        private async Task<float> EndingSection(SKCanvas canvas, Guid idPlayer, Ending end, float yPosition)
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

            // Obtener imágenes con marcas (si es necesario)
            string canchaWithMarksPath = await GenerateMarkedImage("cancha.png", temp.CooField, new SKColor(255, 0, 0, 180));
            string arcoWithMarksPath = null;

            if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
            {
                arcoWithMarksPath = await GenerateMarkedImage("arco.png", temp.CooGoal, new SKColor(0, 0, 255, 180));
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

        private async Task<float> EndingSection(SKCanvas canvas, Club_Dto team, Ending end, float yPosition)
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

            // Generar imágenes con marcas
            string canchaWithMarksPath = await GenerateMarkedImage("cancha.png", marcasCampo, new SKColor(255, 0, 0, 180));
            string arcoWithMarksPath = await GenerateMarkedImage("arco.png", marcasArco, new SKColor(0, 0, 255, 180));

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

        private async void SummaryTeam(SKCanvas canvas, Club_Dto team)
        {
            float yPosition = 40;

            using var typeface = SKTypeface.FromFamilyName(null, SKFontStyle.Bold);
            using var font = new SKFont(typeface, 40);

            using var titlePaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true
            };

            // Dibujar el título "STAT-BOARD"
            //canvas.DrawText("STAT-BOARD", 200, yPosition, SKTextAlign.Left, font, titlePaint);

            // Dibujar el nombre del equipo
            float xPos = 595 / 2; // Centro de la página (A4: 595x842 puntos)
            canvas.DrawText(team.Name, xPos, yPosition, SKTextAlign.Center, font, titlePaint);
            yPosition += 60;

            // Dibujar las secciones de estadísticas
            yPosition = await EndingSection(canvas, team, Ending.Goal, yPosition);
            yPosition = await EndingSection(canvas, team, Ending.Blocked, yPosition);
            yPosition = await EndingSection(canvas, team, Ending.Save, yPosition);
            yPosition = await EndingSection(canvas, team, Ending.Foul, yPosition);
            yPosition = await EndingSection(canvas, team, Ending.Miss, yPosition);
            yPosition = await EndingSection(canvas, team, Ending.Steal_W, yPosition);
            yPosition = await EndingSection(canvas, team, Ending.Steal_L, yPosition);

            // Contar 2 minutos, rojas y azules
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

        private async Task<string> GenerateMarkedImage(string imageName, List<Coordenates> marks, SKColor color)
        {
            string outputPath = Path.Combine(FileSystem.AppDataDirectory, $"{Path.GetFileNameWithoutExtension(imageName)}_marked.png");

            try
            {
                string imagePath = await SaveImageToInternalStorage1(imageName);

                if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                {
                    Console.WriteLine($"Imagen no encontrada o ruta inválida: {imagePath}");
                    return null;
                }

                using var imageStream = File.OpenRead(imagePath);

                // Verificar que el stream tiene datos antes de decodificar
                if (imageStream.Length == 0)
                {
                    Console.WriteLine("El archivo de imagen está vacío.");
                    return null;
                }

                using var bitmap = SKBitmap.Decode(imageStream);
                if (bitmap == null)
                {
                    Console.WriteLine("Error al decodificar la imagen. Formato incompatible o archivo corrupto.");
                    return null;
                }

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
            //var assembly = typeof(CrearPDF_Android).Assembly;
            //var resourcePath = $"StatBoard.Frontend.Resources.Images.{imageName}";
            //using var stream = assembly.GetManifestResourceStream(resourcePath);

            // Depuración: Listar todos los recursos incrustados
            //var resourceNames = assembly.GetManifestResourceNames();
            //foreach (var name in resourceNames)
            //{
            //    Console.WriteLine($"Recurso encontrado: {name}");
            //}

            //if (stream == null)
            //{
            //    Console.WriteLine($"Imagen no encontrada: {resourcePath}");
            //    return null;
            //}

            // Guardar el recurso en un archivo temporal
            //var tempPath = Path.Combine(FileSystem.CacheDirectory, imageName);
            //using var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
            //stream.CopyTo(fileStream);
            //imagePath = tempPath;

            //------------------------------------------------- PRUEBA -------------------------------------------------
            // Prueba 1
            //var prueba = Path.Combine(FileSystem.AppDataDirectory, imageName);
            //return prueba;

            // PRUEBA 2
            var prueba2 = FileSystem.OpenAppPackageFileAsync(imageName).ToString();
            return prueba2;
            //------------------------------------------------- PRUEBA -------------------------------------------------


            //return imagePath;
        }

        private async Task<string> SaveImageToInternalStorage(string imageName)
        {
            try
            {
                var resourcePath = $"Resources/Images/{imageName}";

                using Stream stream = await FileSystem.OpenAppPackageFileAsync(resourcePath);
                var destinationPath = Path.Combine(FileSystem.CacheDirectory, imageName);

                using var fileStream = File.Create(destinationPath);
                await stream.CopyToAsync(fileStream);
                return destinationPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando la imagen: {ex.Message}");
                return null;
            }
        }

        private async Task<string> SaveImageToInternalStorage1(string imageName)
        {
            try
            {
                // Asegúrate de que la ruta sea correcta
                var resourcePath = $"Resources/Images/{imageName}";

                using Stream stream = await FileSystem.OpenAppPackageFileAsync(resourcePath);
                if (stream == null)
                {
                    Console.WriteLine($"No se pudo abrir la imagen: {imageName}");
                    return null;
                }

                var destinationPath = Path.Combine(FileSystem.CacheDirectory, imageName);

                // Eliminar el archivo si ya existe para evitar bloqueos
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }

                using var fileStream = File.Create(destinationPath);
                await stream.CopyToAsync(fileStream);

                Console.WriteLine($"Imagen guardada en: {destinationPath}");
                return destinationPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando la imagen: {ex.Message}");
                return null;
            }
        }
    }
}
