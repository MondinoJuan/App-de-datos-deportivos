using PdfSharp.Drawing;
using SkiaSharp;
using PdfSharp.Pdf;
using Frontend.Resources.Entities;
using System.Text.RegularExpressions;

namespace Frontend.Resources.PDF_Pages
{
    public class CrearPDF
    {
        private Match_Dto? Match { get; set; }
        private Club_Dto? TeamLocal { get; set; }
        private Club_Dto? TeamAway { get; set; }

        public PdfDocument CrearPDF1(Guid idMatch)
        {
            if (!LoadData(idMatch))
            {
                Console.WriteLine("Error al cargar los datos");
                return null;
            }

            // Crear un documento PDF
            var pdfDocument = new PdfDocument();

            // Construir el contenido del documento
            BuildDocument(pdfDocument);

            string fileName = "Stat_Board.pdf";
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

            pdfDocument.Save(filePath);

            Console.WriteLine($"PDF guardado en: {filePath}");

            return pdfDocument;
        }

        private void BuildDocument(PdfDocument pdfDocument)
        {
            // Eliminar las imágenes marcadas
            string canchaMarkedPath = GetImagePath("cancha_marked.png");
            if (File.Exists(canchaMarkedPath))
                File.Delete(canchaMarkedPath);

            string arcoMarkedPath = GetImagePath("arco_marked.png");
            if (File.Exists(arcoMarkedPath))
                File.Delete(arcoMarkedPath);

            // Primera página
            var pdfPage = pdfDocument.AddPage();
            var gfx = XGraphics.FromPdfPage(pdfPage);
            SummaryMatch(gfx, pdfPage);
            Footer(gfx, pdfPage, pdfDocument);

            // Segunda página
            pdfPage = pdfDocument.AddPage();
            gfx = XGraphics.FromPdfPage(pdfPage);
            SummaryTeam(gfx, pdfPage, TeamLocal);
            Footer(gfx, pdfPage, pdfDocument);

            // Tercera página
            pdfPage = pdfDocument.AddPage();
            gfx = XGraphics.FromPdfPage(pdfPage);
            SummaryTeam(gfx, pdfPage, TeamAway);
            Footer(gfx, pdfPage, pdfDocument);

            // Páginas para jugadores del equipo local
            foreach (var idPlayer in TeamLocal.IdPlayers)
            {
                pdfPage = pdfDocument.AddPage();
                gfx = XGraphics.FromPdfPage(pdfPage);
                SummaryPlayer(gfx, pdfPage, idPlayer);
                Footer(gfx, pdfPage, pdfDocument);
            }

            // Páginas para jugadores del equipo visitante
            foreach (var idPlayer in TeamAway.IdPlayers)
            {
                pdfPage = pdfDocument.AddPage();
                gfx = XGraphics.FromPdfPage(pdfPage);
                SummaryPlayer(gfx, pdfPage, idPlayer);
                Footer(gfx, pdfPage, pdfDocument);
            }
        }

        private void SummaryMatch(XGraphics gfx, PdfPage pdfPage)
        {
            XFont titleFont = new XFont("Times New Roman", 18);
            XFont normalFont = new XFont("Arial", 12);
            XFont boldFont = new XFont("Arial", 12);
            XFont subTitleFont = new XFont("Times New Roman", 14);

            double yPos = 40;
            gfx.DrawString("STAT-BOARD", titleFont, XBrushes.Black, new XPoint(200, yPos));
            yPos += 30;

            // Información del partido
            gfx.DrawString("Día: ", boldFont, XBrushes.Black, new XPoint(50, yPos));
            gfx.DrawString(Match.Date.ToString("d"), normalFont, XBrushes.DarkGray, new XPoint(100, yPos));
            gfx.DrawString("Torneo: ", boldFont, XBrushes.Black, new XPoint(200, yPos));
            gfx.DrawString(Match.Tournament, normalFont, XBrushes.DarkGray, new XPoint(280, yPos));
            yPos += 20;
            gfx.DrawString("Lugar: ", boldFont, XBrushes.Black, new XPoint(50, yPos));
            gfx.DrawString(Match.Place, normalFont, XBrushes.DarkGray, new XPoint(100, yPos));
            gfx.DrawString("Fecha: ", boldFont, XBrushes.Black, new XPoint(200, yPos));
            gfx.DrawString(Match.MatchWeek.ToString(), normalFont, XBrushes.DarkGray, new XPoint(280, yPos));
            yPos += 30;

            // ---------------- TABLA DE RESULTADOS ----------------
            double xStart = 50;
            double[] columnWidths = { 150, 50, 50, 150 }; // Ancho de cada columna
            double tableWidth = columnWidths.Sum();
            double rowHeight = 20;

            // Línea superior de la tabla
            gfx.DrawLine(XPens.Black, xStart, yPos, xStart + tableWidth, yPos);

            // Encabezado de la tabla
            gfx.DrawString(TeamLocal.Name.ToUpper(), subTitleFont, XBrushes.Black, new XPoint(xStart + 10, yPos + 15));
            gfx.DrawString(Match.GoalsTeamA.ToString(), subTitleFont, XBrushes.Black, 
                new XPoint(xStart + columnWidths[0] + 10, yPos + 15));
            gfx.DrawString(Match.GoalsTeamB.ToString(), subTitleFont, XBrushes.Black, 
                new XPoint(xStart + columnWidths[0] + columnWidths[1] + 10, yPos + 15));
            gfx.DrawString(TeamAway.Name.ToUpper(), subTitleFont, XBrushes.Black, 
                new XPoint(xStart + columnWidths[0] + columnWidths[1] + columnWidths[2] + 10, yPos + 15));

            yPos += rowHeight;
            gfx.DrawLine(XPens.Black, xStart, yPos, xStart + tableWidth, yPos); // Línea bajo encabezado

            // Dibujar líneas verticales para dividir las columnas
            double xColumn = xStart;
            foreach (var width in columnWidths)
            {
                gfx.DrawLine(XPens.Black, xColumn, yPos - rowHeight, xColumn, yPos + ((CreatedVariablesTypes.QuantityOfPlayersPerClub+1) * rowHeight));
                xColumn += width;
            }
            gfx.DrawLine(XPens.Black, xColumn, yPos - rowHeight, xColumn, yPos + ((CreatedVariablesTypes.QuantityOfPlayersPerClub + 1) * rowHeight)); // Última línea vertical

            // ---------------- ENCABEZADO DE LA TABLA DE JUGADORES ----------------
            gfx.DrawString("N°", subTitleFont, XBrushes.Black, new XPoint(xStart + 10, yPos + 15));
            gfx.DrawString("Nombre", subTitleFont, XBrushes.Black, new XPoint(xStart + 30, yPos + 15));
            gfx.DrawString("Goles", subTitleFont, XBrushes.Black, new XPoint(xStart + columnWidths[0] + 10, yPos + 15));
            gfx.DrawString("Goles", subTitleFont, XBrushes.Black, new XPoint(xStart + columnWidths[0] + columnWidths[1] + 10, yPos + 15));
            gfx.DrawString("N°", subTitleFont, XBrushes.Black, new XPoint(xStart + columnWidths[0] + columnWidths[1] + columnWidths[2] + 10, yPos + 15));
            gfx.DrawString("Nombre", subTitleFont, XBrushes.Black, new XPoint(xStart + columnWidths[0] + columnWidths[1] + columnWidths[2] + 30, yPos + 15));

            yPos += rowHeight;
            gfx.DrawLine(XPens.Black, xStart, yPos, xStart + tableWidth, yPos); // Línea después del encabezado

            // ---------------- DATOS DE LOS JUGADORES ----------------
            for (var i = 0; i < CreatedVariablesTypes.QuantityOfPlayersPerClub; i++)
            {
                double currentY = yPos + (i * rowHeight);

                if (i < TeamLocal.IdPlayers.Count)
                {
                    var resultL = Simulo_BdD.GetOnePlayer(TeamLocal.IdPlayers[i]);
                    if (resultL.Success)
                    {
                        var playerL = resultL.Data;
                        gfx.DrawString(playerL.Number.ToString(), normalFont, XBrushes.DarkGray, new XPoint(xStart + 10, currentY + 15));
                        gfx.DrawString(playerL.Name, normalFont, XBrushes.DarkGray, new XPoint(xStart + 30, currentY + 15));
                        gfx.DrawString(CountEndings(Match.Id, playerL.Id, Ending.Goal).QuantityEnding.ToString(), normalFont, 
                            XBrushes.DarkGray, new XPoint(xStart + columnWidths[0] + 10, currentY + 15));
                    }
                }

                if (i < TeamAway.IdPlayers.Count)
                {
                    var resultA = Simulo_BdD.GetOnePlayer(TeamAway.IdPlayers[i]);
                    if (resultA.Success)
                    {
                        var playerA = resultA.Data;
                        gfx.DrawString(CountEndings(Match.Id, playerA.Id, Ending.Goal).QuantityEnding.ToString(), normalFont, XBrushes.DarkGray, 
                            new XPoint(xStart + columnWidths[0] + columnWidths[1] + 10, currentY + 15));
                        gfx.DrawString(playerA.Number.ToString(), normalFont, XBrushes.DarkGray, 
                            new XPoint(xStart + columnWidths[0] + columnWidths[1] + columnWidths[2] + 10, currentY + 15));
                        gfx.DrawString(playerA.Name, normalFont, XBrushes.DarkGray, 
                            new XPoint(xStart + columnWidths[0] + columnWidths[1] + columnWidths[2] + 30, currentY + 15));
                    }
                }

                // Línea horizontal después de cada fila
                gfx.DrawLine(XPens.Black, xStart, currentY + rowHeight, xStart + tableWidth, currentY + rowHeight);
            }
        }

        private void Footer(XGraphics gfx, PdfPage pdfPage, PdfDocument pdfDocument)
        {
            var footerFont = new XFont("Verdana", 9);
            int pageIndex = pdfDocument.Pages.Cast<PdfPage>().ToList().IndexOf(pdfPage);
            gfx.DrawString($"Página {pageIndex + 1} de {pdfPage.Owner.PageCount}", footerFont, XBrushes.Gray,
                new XRect(0, pdfPage.Height - 30, pdfPage.Width, 20), XStringFormats.Center);
        }

        private void SummaryPlayer(XGraphics gfx, PdfPage pdfPage, Guid idPlayer)
        {
            var result = Simulo_BdD.GetOnePlayer(idPlayer);
            if (!result.Success)
            {
                Console.WriteLine(result.Message);
                return;
            }

            // Título
            var font = new XFont("Verdana", 16);
            double yPosition = 0;
            gfx.DrawString($"{result.Data.Name} - {result.Data.Number}", font, XBrushes.Black, new XRect(0, yPosition, pdfPage.Width, 50), 
                XStringFormats.Center);
            yPosition += 60; // Ajustar la posición Y para el siguiente elemento

            // Goles
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Goal, yPosition);
            // Bloqueos
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Blocked, yPosition);
            // Saves
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Save, yPosition);
            // Foul
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Foul, yPosition);
            // Miss
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Miss, yPosition);
            // Robo
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Steal_W, yPosition);
            // Perdidas
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Steal_L, yPosition);

            // Contar cantidad de 2mins, rojas y azules por equipo
            var temp = CountSanctions(Match.Id, idPlayer);

            var infoFont = new XFont("Verdana", 12);
            if (temp.Quantity2min.HasValue)
            {
                gfx.DrawString($"2 minutos: {temp.Quantity2min.Value}", infoFont, XBrushes.Black, 
                    new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
            if (temp.Red.HasValue)
            {
                gfx.DrawString($"Rojas: {temp.Red.Value}", infoFont, XBrushes.Black, 
                    new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
            if (temp.Blue.HasValue)
            {
                gfx.DrawString($"Azules: {temp.Blue.Value}", infoFont, XBrushes.Black, 
                    new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
        }

        public void SummaryTeam(XGraphics gfx, PdfPage pdfPage, Club_Dto team)
        {
            var minutes2 = 0;
            var reds = 0;
            var blues = 0;

            // Título
            var font = new XFont("Verdana", 16);
            double yPosition = 0;
            gfx.DrawString(team.Name, font, XBrushes.Black, new XRect(0, yPosition, pdfPage.Width, 50), XStringFormats.Center);
            yPosition += 60; // Ajustar la posición Y para el siguiente elemento

            // Goles
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Goal, yPosition);
            // Bloqueos
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Blocked, yPosition);
            // Saves
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Save, yPosition);
            // Foul
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Foul, yPosition);
            // Miss
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Miss, yPosition);
            // Robo
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Steal_W, yPosition);
            // Perdidas
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Steal_L, yPosition);

            // Contar cantidad de 2mins, rojas y azules por equipo
            foreach (var idPlayer in team.IdPlayers)
            {
                var temp = CountSanctions(Match.Id, idPlayer);
                if (temp.Quantity2min.HasValue)
                {
                    minutes2 += temp.Quantity2min.Value;
                }
                if (temp.Red.HasValue)
                {
                    reds += temp.Red.Value;
                }
                if (temp.Blue.HasValue)
                {
                    blues += temp.Blue.Value;
                }
            }

            var infoFont = new XFont("Verdana", 12);
            if (minutes2 > 0)
            {
                gfx.DrawString($"2 minutos: {minutes2}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
            if (reds > 0)
            {
                gfx.DrawString($"Rojas: {reds}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
            if (blues > 0)
            {
                gfx.DrawString($"Azules: {blues}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
        }

        private double EndingSection(XGraphics gfx, PdfPage pdfPage, Guid idPlayer, Ending end, double yPosition)
        {
            var marcasCampo = new List<Coordenates>();
            var marcasArco = new List<Coordenates>();
            // Contar cantidad de endings por jugador y generar imágenes con marcas
            var temp = CountEndings(Match.Id, idPlayer, end);
            if (temp.CooField != null)
                marcasCampo.AddRange(temp.CooField);

            if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
            {
                if (temp.CooGoal != null)
                    marcasArco.AddRange(temp.CooGoal);
            }

            // Generar imágenes con marcas
            //string absolutePathCancha = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha.png";
            string absolutePathCancha = GetImagePath("cancha.png");
            string canchaWithMarksPath = GenerateMarkedImage(absolutePathCancha, marcasCampo, new SKColor(255, 0, 0, 150)); // Rojo fuerte

            //string absolutePathArco = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco.png";
            string absolutePathArco = GetImagePath("arco.png");
            string arcoWithMarksPath = null;
            if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
            {
                arcoWithMarksPath = GenerateMarkedImage(absolutePathArco, marcasArco, new SKColor(0, 0, 255, 150)); // Azul fuerte
            }

            try
            {
                // Insertar imágenes generadas al documento
                if (arcoWithMarksPath != null)
                {
                    var infoFont = new XFont("Verdana", 12);
                    gfx.DrawString($"{end}: {temp.QuantityEnding}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                    yPosition += 20;

                    var markedCanchaImage = XImage.FromFile(canchaWithMarksPath);
                    gfx.DrawImage(markedCanchaImage, 50, yPosition, 150, 120);
                    Console.WriteLine($"Imagen de cancha añadida al documento: {canchaWithMarksPath}");

                    var markedArcoImage = XImage.FromFile(arcoWithMarksPath);
                    gfx.DrawImage(markedArcoImage, 10 + 200, yPosition, 150, 100);
                    Console.WriteLine($"Imagen de arco añadida al documento: {arcoWithMarksPath}");
                    
                    yPosition -= 20;
                }
                else
                {
                    var infoFont = new XFont("Verdana", 12);
                    gfx.DrawString($"{end}: {temp.QuantityEnding}", infoFont, XBrushes.Black, new XRect(370, yPosition, pdfPage.Width, 20), 
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    var markedCanchaImage = XImage.FromFile(canchaWithMarksPath);
                    gfx.DrawImage(markedCanchaImage, 10 + 360, yPosition, 150, 120);
                    Console.WriteLine($"Imagen de cancha añadida al documento: {canchaWithMarksPath}");

                    yPosition += 120; // Ajustar la posición Y para el siguiente elemento
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir imágenes al documento: {ex.Message}");
            }

            return yPosition;
        }

        private double EndingSection(XGraphics gfx, PdfPage pdfPage, Club_Dto team, Ending end, double yPosition)
        {
            var cantidadEndingsTeam = 0;
            var marcasCampo = new List<Coordenates>();
            var marcasArco = new List<Coordenates>();

            foreach (var idPlayer in team.IdPlayers)
            {
                var temp = CountEndings(Match.Id, idPlayer, end);

                cantidadEndingsTeam += temp.QuantityEnding;

                if (temp.CooField != null)
                    marcasCampo.AddRange(temp.CooField);

                if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
                {
                    if (temp.CooGoal != null)
                        marcasArco.AddRange(temp.CooGoal);
                }
            }

            // Generar imágenes con marcas
            //string absolutePathCancha = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha.png";
            string absolutePathCancha = GetImagePath("cancha.png");
            string canchaWithMarksPath = GenerateMarkedImage(absolutePathCancha, marcasCampo, new SKColor(255, 0, 0, 150)); // Rojo fuerte

            try
            {
                // Insertar imágenes generadas al documento
                if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
                {
                    var infoFont = new XFont("Verdana", 12);
                    gfx.DrawString($"{end}: {cantidadEndingsTeam}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), 
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    var markedCanchaImage = XImage.FromFile(canchaWithMarksPath);
                    gfx.DrawImage(markedCanchaImage, 50, yPosition, 150, 120);
                    Console.WriteLine($"Imagen de cancha añadida al documento: {canchaWithMarksPath}");

                    //string absolutePathArco = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco.png";
                    string absolutePathArco = GetImagePath("arco.png");
                    string arcoWithMarksPath = GenerateMarkedImage(absolutePathArco, marcasArco, new SKColor(0, 0, 255, 150)); // Azul fuerte
                    var markedArcoImage = XImage.FromFile(arcoWithMarksPath);
                    gfx.DrawImage(markedArcoImage, 10 + 200, yPosition, 150, 100);
                    Console.WriteLine($"Imagen de arco añadida al documento: {arcoWithMarksPath}");

                    yPosition -= 20;
                }
                else
                {
                    var infoFont = new XFont("Verdana", 12);
                    gfx.DrawString($"{end}: {cantidadEndingsTeam}", infoFont, XBrushes.Black, new XRect(370, yPosition, pdfPage.Width, 20), 
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    var markedCanchaImage = XImage.FromFile(canchaWithMarksPath);
                    gfx.DrawImage(markedCanchaImage, 10 + 360, yPosition, 150, 120);
                    Console.WriteLine($"Imagen de cancha añadida al documento: {canchaWithMarksPath}");

                    yPosition += 120; // Ajustar la posición Y para el siguiente elemento
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir imágenes al documento: {ex.Message}");
            }

            return yPosition;
        }

        private string GenerateMarkedImage(string imageName, List<Coordenates> marks, SKColor color)
        {
            string outputPath;
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "App", "StatBoard", "StatBoard", "Frontend", "Frontend", "Resources", "Images", Path.GetFileNameWithoutExtension(imageName) + "_marked.png");
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                outputPath = Path.Combine(FileSystem.AppDataDirectory, "Images", Path.GetFileNameWithoutExtension(imageName) + "_marked.png");
            }
            else
            {
                outputPath = Path.Combine(FileSystem.CacheDirectory, "Images", Path.GetFileNameWithoutExtension(imageName) + "_marked.png"); // Fallback
            }

            using var imageStream = File.OpenRead(imageName);
            using var bitmap = SKBitmap.Decode(imageStream);
            using var canvas = new SKCanvas(bitmap);

            foreach (var mark in marks)
            {
                int pointSize = 20;
                canvas.DrawCircle(mark.X, mark.Y, pointSize / 2, new SKPaint { Color = color, IsAntialias = true, Style = SKPaintStyle.Fill });
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            data.SaveTo(fileStream);

            return outputPath;
        }

        private Coordenadas CountEndings(Guid idMatch, Guid idPlayer, Ending ending)
        {
            var count = 0;

            var contabilizo = new Coordenadas
            {
                CooField = new List<Coordenates>(),
                CooGoal = new List<Coordenates>()
            };

            var result = Simulo_BdD.GetAllPlayerMatches();
            if (!result.Success)
            {
                Console.WriteLine(result.Message);
                return new Coordenadas
                {
                    Success = false
                };
            }

            var allPlayersMatches = result.Data;
            var playerMatch = allPlayersMatches.FirstOrDefault(pm => pm.IdMatch == idMatch && pm.IdPlayer == idPlayer);

            foreach (var idPA in playerMatch.IdActions)
            {
                var result1 = Simulo_BdD.GetOneAction(idPA);
                if (!result1.Success)
                {
                    Console.WriteLine(result1.Message);
                    return new Coordenadas
                    {
                        Success = false
                    };
                }
                else
                {
                    if (result1.Data.Ending == ending)
                    {
                        contabilizo.CooField.Add(new Coordenates
                        {
                            X = result1.Data.ActionPositionX,
                            Y = result1.Data.ActionPositionY
                        });
                        if (ending == Ending.Goal || ending == Ending.Miss || ending == Ending.Save)
                            contabilizo.CooGoal.Add(new Coordenates
                            {
                                X = result1.Data.DefinitionPlaceX,
                                Y = result1.Data.DefinitionPlaceY
                            });
                        count++;
                    }
                }
            }
            contabilizo.Success = true;
            contabilizo.QuantityEnding = count;
            return contabilizo;
        }

        private Coordenadas CountSanctions(Guid idMatch, Guid idPlayer)
        {
            var rojas = 0;
            var azules = 0;
            var twoMinutes = 0;

            var contabilizo = new Coordenadas();

            var result = Simulo_BdD.GetAllPlayerMatches();
            if (!result.Success)
            {
                Console.WriteLine(result.Message);
                return new Coordenadas
                {
                    Success = false
                };
            }

            var allPlayersMatches = result.Data;
            var playerMatch = allPlayersMatches.FirstOrDefault(pm => pm.IdMatch == idMatch && pm.IdPlayer == idPlayer);

            foreach (var idPA in playerMatch.IdActions)
            {
                var result1 = Simulo_BdD.GetOneAction(idPA);
                if (!result1.Success)
                {
                    Console.WriteLine(result1.Message);
                    return new Coordenadas
                    {
                        Success = false
                    };
                }
                else
                {
                    if (result1.Data.Ending == Ending.Foul)
                    {
                        if (result1.Data.Sanction == Sanction.Blue)
                        {
                            azules++;
                        }
                        else if (result1.Data.Sanction == Sanction.Red)
                        {
                            rojas++;
                        }
                        else if (result1.Data.Sanction == Sanction.Two_Minutes)
                        {
                            twoMinutes++;
                        }
                    }
                }
            }
            contabilizo.Success = true;
            contabilizo.Quantity2min = twoMinutes;
            contabilizo.Red = rojas;
            contabilizo.Blue = azules;
            return contabilizo;
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
                Console.WriteLine($"Error al recuperar el partido con id = {idMatch}");
                return false;
            }

            var resultA = Simulo_BdD.GetOneClub(Match.IdTeamLocal);
            if (resultA.Success)
            {
                TeamLocal = resultA.Data;
            }
            else
            {
                Console.WriteLine($"Error al recuperar el club con id = {Match.IdTeamLocal}");
                return false;
            }

            var resultB = Simulo_BdD.GetOneClub(Match.IdTeamAway);
            if (resultB.Success)
            {
                TeamAway = resultB.Data;
            }
            else
            {
                Console.WriteLine($"Error al recuperar el club con id = {Match.IdTeamAway}");
                return false;
            }

            return true;
        }

        private string GetImagePath(string imageName)
        {
            string imagePath;
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                    "App", "StatBoard", "StatBoard", "Frontend", "Frontend", "Resources", "Images", imageName);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                imagePath = Path.Combine(FileSystem.AppDataDirectory, "Images", imageName);
            }
            else
            {
                imagePath = Path.Combine(FileSystem.CacheDirectory, "Images", imageName); // Fallback
            }
            return imagePath;
        }
    }
}
