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
            //var pdfPage = pdfDocument.AddPage();
            //var gfx = XGraphics.FromPdfPage(pdfPage);

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
            if (File.Exists("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha_marked.png"))
                File.Delete("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha_marked.png");
            if (File.Exists("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco_marked.png"))
                File.Delete("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco_marked.png");

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

        //private void SummaryMatch(XGraphics gfx, PdfPage pdfPage)
        //{
        //    // Título
        //    var font = new XFont("Verdana", 20);
        //    double yPosition = 0;
        //    gfx.DrawString("STAT-BOARD", font, XBrushes.Black, new XRect(0, yPosition, pdfPage.Width, 50), XStringFormats.Center);
        //    yPosition += 60; // Ajustar la posición Y para el siguiente elemento

        //    // Información del partido
        //    var infoFont = new XFont("Verdana", 12);
        //    gfx.DrawString($"Día: {Match.Date:d}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
        //    yPosition += 20;
        //    gfx.DrawString($"Tournament: {Match.Tournament}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
        //    yPosition += 20;
        //    gfx.DrawString($"Lugar: {Match.Place}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
        //    yPosition += 20;
        //    gfx.DrawString($"Fecha: {Match.MatchWeek}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
        //    yPosition += 30;

        //    // Tabla de resultados
        //    var tableFont = new XFont("Verdana", 12);
        //    gfx.DrawString($"{TeamLocal.Name} {Match.GoalsTeamA} - {Match.GoalsTeamB} {TeamAway.Name}", tableFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
        //}

        public void SummaryMatch(XGraphics gfx, PdfPage pdfPage)
        {
            XFont titleFont = new XFont("Arial", 18);
            XFont normalFont = new XFont("Arial", 12);
            XFont boldFont = new XFont("Arial", 12);

            double yPos = 40;
            gfx.DrawString("STAT-BOARD", titleFont, XBrushes.Black, new XPoint(200, yPos));
            yPos += 30;

            // Información del partido
            gfx.DrawString("Día: ", boldFont, XBrushes.Black, new XPoint(50, yPos));
            gfx.DrawString(Match.Date.ToString("d"), normalFont, XBrushes.Black, new XPoint(100, yPos));
            gfx.DrawString("Tournament: ", boldFont, XBrushes.Black, new XPoint(200, yPos));
            gfx.DrawString(Match.Tournament, normalFont, XBrushes.Black, new XPoint(280, yPos));
            yPos += 20;
            gfx.DrawString("Lugar: ", boldFont, XBrushes.Black, new XPoint(50, yPos));
            gfx.DrawString(Match.Place, normalFont, XBrushes.Black, new XPoint(100, yPos));
            gfx.DrawString("Fecha: ", boldFont, XBrushes.Black, new XPoint(200, yPos));
            gfx.DrawString(Match.MatchWeek.ToString(), normalFont, XBrushes.Black, new XPoint(280, yPos));
            yPos += 30;

            // Tabla de resultados
            gfx.DrawString(TeamLocal.Name, boldFont, XBrushes.Black, new XPoint(100, yPos));
            gfx.DrawString(Match.GoalsTeamA.ToString(), boldFont, XBrushes.Black, new XPoint(250, yPos));
            gfx.DrawString(Match.GoalsTeamB.ToString(), boldFont, XBrushes.Black, new XPoint(300, yPos));
            gfx.DrawString(TeamAway.Name, boldFont, XBrushes.Black, new XPoint(400, yPos));
            yPos += 30;

            // Encabezado de la tabla de jugadores
            gfx.DrawString("N°", boldFont, XBrushes.Black, new XPoint(50, yPos));
            gfx.DrawString("Nombre", boldFont, XBrushes.Black, new XPoint(80, yPos));
            gfx.DrawString("Goles", boldFont, XBrushes.Black, new XPoint(200, yPos));
            gfx.DrawString("Goles", boldFont, XBrushes.Black, new XPoint(300, yPos));
            gfx.DrawString("N°", boldFont, XBrushes.Black, new XPoint(350, yPos));
            gfx.DrawString("Nombre", boldFont, XBrushes.Black, new XPoint(380, yPos));
            yPos += 20;

            // Datos de jugadores
            for (var i = 0; i < CreatedVariablesTypes.QuantityOfPlayersPerClub; i++)
            {
                if (i < TeamLocal.IdPlayers.Count)
                {
                    var resultL = Simulo_BdD.GetOnePlayer(TeamLocal.IdPlayers[i]);
                    if (resultL.Success)
                    {
                        var playerL = resultL.Data;
                        gfx.DrawString(playerL.Number.ToString(), normalFont, XBrushes.Black, new XPoint(50, yPos));
                        gfx.DrawString(playerL.Name, normalFont, XBrushes.Black, new XPoint(80, yPos));
                        gfx.DrawString(CountEndings(Match.Id, playerL.Id, Ending.Goal).QuantityEnding.ToString(), normalFont, XBrushes.Black, new XPoint(200, yPos));
                    }
                }
                if (i < TeamAway.IdPlayers.Count)
                {
                    var resultA = Simulo_BdD.GetOnePlayer(TeamAway.IdPlayers[i]);
                    if (resultA.Success)
                    {
                        var playerA = resultA.Data;
                        gfx.DrawString(CountEndings(Match.Id, playerA.Id, Ending.Goal).QuantityEnding.ToString(), normalFont, XBrushes.Black, new XPoint(300, yPos));
                        gfx.DrawString(playerA.Number.ToString(), normalFont, XBrushes.Black, new XPoint(350, yPos));
                        gfx.DrawString(playerA.Name, normalFont, XBrushes.Black, new XPoint(380, yPos));
                    }
                }
                yPos += 20;
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
            gfx.DrawString($"{result.Data.Name} - {result.Data.Number}", font, XBrushes.Black, new XRect(0, yPosition, pdfPage.Width, 50), XStringFormats.Center);
            yPosition += 60; // Ajustar la posición Y para el siguiente elemento

            // Goles
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Goal, yPosition);
            // Saves
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Save, yPosition);
            // Miss
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Miss, yPosition);
            // Bloqueos
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Blocked, yPosition);
            // Foul
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Foul, yPosition);
            // Robo
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Steal_W, yPosition);
            // Perdidas
            yPosition = EndingSection(gfx, pdfPage, result.Data.Id, Ending.Steal_L, yPosition);

            // Contar cantidad de 2mins, rojas y azules por equipo
            var temp = CountSanctions(Match.Id, idPlayer);

            var infoFont = new XFont("Verdana", 12);
            if (temp.Quantity2min.HasValue)
            {
                gfx.DrawString($"2 minutos: {temp.Quantity2min.Value}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
            if (temp.Red.HasValue)
            {
                gfx.DrawString($"Rojas: {temp.Red.Value}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
            if (temp.Blue.HasValue)
            {
                gfx.DrawString($"Azules: {temp.Blue.Value}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
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
            // Saves
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Save, yPosition);
            // Miss
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Miss, yPosition);
            // Bloqueos
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Blocked, yPosition);
            // Foul
            yPosition = EndingSection(gfx, pdfPage, team, Ending.Foul, yPosition);
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

            var infoFont = new XFont("Verdana", 12);
            gfx.DrawString($"{end}: {temp.QuantityEnding}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
            yPosition += 20;

            // Generar imágenes con marcas
            string absolutePathCancha = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha.png";
            string canchaWithMarksPath = GenerateMarkedImage(absolutePathCancha, marcasCampo, new SKColor(255, 0, 0)); // Rojo fuerte

            string absolutePathArco = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco.png";
            string arcoWithMarksPath = null;
            if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
            {
                arcoWithMarksPath = GenerateMarkedImage(absolutePathArco, marcasArco, new SKColor(0, 0, 255)); // Azul fuerte
            }

            try
            {
                // Insertar imágenes generadas al documento
                var markedCanchaImage = XImage.FromFile(canchaWithMarksPath);
                gfx.DrawImage(markedCanchaImage, 50, yPosition, 200, 150);
                yPosition += 160; // Ajustar la posición Y para el siguiente elemento
                Console.WriteLine($"Imagen de cancha añadida al documento: {canchaWithMarksPath}");

                if (arcoWithMarksPath != null)
                {
                    var markedArcoImage = XImage.FromFile(arcoWithMarksPath);
                    gfx.DrawImage(markedArcoImage, 50, yPosition, 200, 150);
                    yPosition += 160; // Ajustar la posición Y para el siguiente elemento
                    Console.WriteLine($"Imagen de arco añadida al documento: {arcoWithMarksPath}");
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

            var infoFont = new XFont("Verdana", 12);
            gfx.DrawString($"{end}: {cantidadEndingsTeam}", infoFont, XBrushes.Black, new XRect(50, yPosition, pdfPage.Width, 20), XStringFormats.TopLeft);
            yPosition += 20;

            // Generar imágenes con marcas
            string absolutePathCancha = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha.png";
            string canchaWithMarksPath = GenerateMarkedImage(absolutePathCancha, marcasCampo, new SKColor(255, 0, 0)); // Rojo fuerte

            try
            {
                // Insertar imágenes generadas al documento
                var markedCanchaImage = XImage.FromFile(canchaWithMarksPath);
                gfx.DrawImage(markedCanchaImage, 50, yPosition, 200, 150);
                yPosition += 160; // Ajustar la posición Y para el siguiente elemento
                Console.WriteLine($"Imagen de cancha añadida al documento: {canchaWithMarksPath}");

                if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
                {
                    string absolutePathArco = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco.png";
                    string arcoWithMarksPath = GenerateMarkedImage(absolutePathArco, marcasArco, new SKColor(0, 0, 255)); // Azul fuerte
                    var markedArcoImage = XImage.FromFile(arcoWithMarksPath);
                    gfx.DrawImage(markedArcoImage, 50, yPosition, 200, 150);
                    yPosition += 160; // Ajustar la posición Y para el siguiente elemento
                    Console.WriteLine($"Imagen de arco añadida al documento: {arcoWithMarksPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir imágenes al documento: {ex.Message}");
            }

            return yPosition;
        }

        private string GenerateMarkedImage(string imagePath, List<Coordenates> marks, SKColor color)
        {
            var outputPath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + "_marked.png");

            using var imageStream = File.OpenRead(imagePath);
            using var bitmap = SKBitmap.Decode(imageStream);
            using var canvas = new SKCanvas(bitmap);

            foreach (var mark in marks)
            {
                int pointSize = 20;
                canvas.DrawCircle((float)mark.X, (float)mark.Y, pointSize / 2, new SKPaint { Color = color, IsAntialias = true });
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
    }
}
