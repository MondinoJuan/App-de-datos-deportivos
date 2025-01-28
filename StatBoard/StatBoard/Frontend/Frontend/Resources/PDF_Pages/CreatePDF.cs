using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Frontend.Resources.Entities;
using SkiaSharp;
using static PdfSharp.Capabilities.Features;
using PdfSharp.Drawing;

namespace Frontend.Resources.PDF_Pages;
class CreatePDF
{
    private Match_Dto? Match { get; set; }
    private Club_Dto? TeamLocal { get; set; }
    private Club_Dto? TeamAway { get; set; }

    public PdfDocument CrearPDF(Guid idMatch)
    {
        if (!LoadData(idMatch))
        {
            Console.WriteLine("Error al cargar los datos");
            return null;
        }

        // Create a new MigraDoc document
        var document = new Document();

        // Recuperar todos los objetos creados en la aplicacion para plasmar la informacion en el PDF

        BuildDocument(document);

        //Create a render for the MigraDoc document
        var pdfRenderer = new PdfDocumentRenderer();
        pdfRenderer.Document = document;

        //Layout and render document to PDF
        pdfRenderer.RenderDocument();

        string fileName = "MatchSummary.pdf";
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

        pdfRenderer.PdfDocument.Save(filePath);

        Console.WriteLine($"PDF guardado en: {filePath}");

        return pdfRenderer.PdfDocument;
    }

    private void BuildDocument(Document document)
    {
        //Eliminar las imágenes marcadas
        if (File.Exists("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha_marked.png"))
            File.Delete("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha_marked.png");
        if (File.Exists("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco_marked.png"))
            File.Delete("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco_marked.png");

        SummaryMatch(document);
        Footer(document);
        SummaryTeam(document, TeamLocal);
        Footer(document);
        SummaryTeam(document, TeamAway);
        Footer(document);
        foreach (var idPlayer in TeamLocal.IdPlayers)
        {
            SummaryPlayer(document, idPlayer);
            Footer(document);
        }
        foreach (var idPlayer in TeamAway.IdPlayers)
        {
            SummaryPlayer(document, idPlayer);
            Footer(document);
        }
    }

    public void SummaryPlayer(Document document, Guid idPlayer)
    {
        var result = Simulo_BdD.GetOnePlayer(idPlayer);
        if (!result.Success)
        {
            Console.WriteLine(result.Message);
            return;
        }

        // Título
        var section = document.AddSection();
        var titulo = section.AddParagraph($"{result.Data.Name} - {result.Data.Number}");
        titulo.Style = "Heading2";
        titulo.AddLineBreak();
        titulo.Format.SpaceAfter = 20;

        // Goles
        EndingSection(document, result.Data.Id, Ending.Goal);
        //Saves
        EndingSection(document, result.Data.Id, Ending.Save);
        //Miss
        EndingSection(document, result.Data.Id, Ending.Miss);
        //Bloqueos
        EndingSection(document, result.Data.Id, Ending.Blocked);
        //Foul
        EndingSection(document, result.Data.Id, Ending.Foul);
        //Robo
        EndingSection(document, result.Data.Id, Ending.Steal_W);
        //Perdidas
        EndingSection(document, result.Data.Id, Ending.Steal_L);

        //Contar cantidad de 2mins, rojas y azules por equipo

        var temp = CountSanctions(Match.Id, idPlayer);

        if (temp.Quantity2min.HasValue)
        {
            section.AddParagraph($"2 minutos: {temp.Quantity2min.Value}");
        }
        if (temp.Red.HasValue)
        {
            section.AddParagraph($"Rojas: {temp.Red.Value}");
        }
        if (temp.Blue.HasValue)
        {
            section.AddParagraph($"Azules: {temp.Blue.Value}");
        }
    }

    public void Footer(Document document)
    {
        var section = document.LastSection ?? document.AddSection();
        var footer = section.Footers.Primary.AddParagraph();
        footer.AddText("Página ");
        footer.AddPageField(); // Campo para el número de página actual
        footer.AddText(" de ");
        footer.AddNumPagesField(); // Campo para el número total de páginas
        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.Format.Font.Size = 9;
        footer.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Gray;
        //section.AddPageBreak();
    }

    public void SummaryTeam(Document document, Club_Dto team)
    {
        var minutes2 = 0;
        var reds = 0;
        var blues = 0;

        // Título
        var section = document.AddSection();
        var titulo = section.AddParagraph(team.Name);
        titulo.Style = "Heading2";
        titulo.AddLineBreak();
        titulo.Format.SpaceAfter = 20;

        // Goles
        EndingSection(document, team, Ending.Goal);
        //Saves
        EndingSection(document, team, Ending.Save);
        //Miss
        EndingSection(document, team, Ending.Miss);
        //Bloqueos
        EndingSection(document, team, Ending.Blocked);
        //Foul
        EndingSection(document, team, Ending.Foul);
        //Robo
        EndingSection(document, team, Ending.Steal_W);
        //Perdidas
        EndingSection(document, team, Ending.Steal_L);

        //Contar cantidad de 2mins, rojas y azules por equipo
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

        if (minutes2 > 0)
        {
            section.AddParagraph($"2 minutos: {minutes2}");
        }
        if (reds > 0)
        {
            section.AddParagraph($"Rojas: {reds}");
        }
        if (blues > 0)
        {
            section.AddParagraph($"Azules: {blues}");
        }
    }

    private void EndingSection(Document document, Guid idPlayer, Ending end)
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

        var section = document.LastSection ?? document.AddSection();
        section.AddParagraph($"{end}: {temp.QuantityEnding}");

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
            var markedCanchaImage = section.AddImage(canchaWithMarksPath);
            markedCanchaImage.Width = "10cm";
            markedCanchaImage.Height = "7cm";
            Console.WriteLine($"Imagen de cancha añadida al documento: {canchaWithMarksPath}");

            if (arcoWithMarksPath != null)
            {
                var markedArcoImage = section.AddImage(arcoWithMarksPath);
                markedArcoImage.Width = "7cm";
                markedArcoImage.Height = "7cm";
                Console.WriteLine($"Imagen de arco añadida al documento: {arcoWithMarksPath}");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al añadir imágenes al documento: {ex.Message}");
        }
    }

    private void EndingSection(Document document, Club_Dto team, Ending end)
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

        var section = document.LastSection ?? document.AddSection();
        section.AddParagraph($"{end}: {cantidadEndingsTeam}");

        // Generar imágenes con marcas
        string absolutePathCancha = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\cancha.png";
        string canchaWithMarksPath = GenerateMarkedImage(absolutePathCancha, marcasCampo, new SKColor(255, 0, 0)); // Rojo semitransparente

        string absolutePathArco = "C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\arco.png";
        string arcoWithMarksPath = null;
        if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
        {
            arcoWithMarksPath = GenerateMarkedImage(absolutePathArco, marcasArco, new SKColor(0, 0, 255)); // Azul semitransparente
        }

        // Insertar imágenes generadas al documento
        var markedCanchaImage = section.AddImage(canchaWithMarksPath);
        markedCanchaImage.Width = "10cm";
        markedCanchaImage.Height = "7cm";

        if (arcoWithMarksPath != null)
        {
            var markedArcoImage = section.AddImage(arcoWithMarksPath);
            markedArcoImage.Width = "7cm";
            markedArcoImage.Height = "7cm";
        }
    }

    private string GenerateMarkedImage(string imagePath, List<Coordenates> marks, SKColor color)
    {
        string markedImagePath = Path.Combine("C:\\Users\\Pc\\Desktop\\App\\StatBoard\\StatBoard\\Frontend\\Frontend\\Resources\\Images\\", $"{Path.GetFileNameWithoutExtension(imagePath)}_marked.png");

        if (!File.Exists(imagePath))
        {
            Console.WriteLine($"Error: La imagen no existe en la ruta especificada: {imagePath}");
            return null;
        }

        try
        {
            using (var stream = File.OpenRead(imagePath))
            using (var bitmap = SKBitmap.Decode(stream))
            {
                if (bitmap == null)
                {
                    Console.WriteLine($"Error: No se pudo cargar la imagen desde la ruta especificada: {imagePath}");
                    return null;
                }

                using (var surface = SKSurface.Create(new SKImageInfo(bitmap.Width, bitmap.Height)))
                {
                    var canvas = surface.Canvas;

                    // Dibuja la imagen base
                    canvas.DrawBitmap(bitmap, 0, 0);

                    // Dibuja las marcas
                    foreach (var mark in marks)
                    {
                        if (mark.X.HasValue && mark.Y.HasValue)
                        {
                            //double scaledX = (mark.X.Value / 100.0) * bitmap.Width;
                            //double scaledY = (mark.Y.Value / 100.0) * bitmap.Height;

                            double scaledX = mark.X.Value / 100.0;
                            double scaledY = mark.Y.Value / 100.0;

                            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                            Console.WriteLine($"mark.X: {mark.X}, mark.Y: {mark.Y}, scaledX: {scaledX}, scaledY: {scaledY}");

                            using (var paint = new SKPaint
                            {
                                Color = color,
                                IsAntialias = true,
                                Style = SKPaintStyle.Fill
                            })
                            {
                                canvas.DrawCircle((float)scaledX, (float)scaledY, 5, paint);
                            }
                        }
                    }

                    // Guarda la imagen marcada
                    using (var image = surface.Snapshot())
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    using (var outputStream = File.OpenWrite(markedImagePath))
                    {
                        data.SaveTo(outputStream);
                    }
                }
            }

            Console.WriteLine($"Imagen marcada guardada en: {markedImagePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar la imagen marcada: {ex.Message}");
        }

        return markedImagePath;
    }

    public void SummaryMatch(Document document)
    {
        //Titulo
        var section = document.AddSection();
        var titulo = section.AddParagraph("STAT-BOARD");
        titulo.Style = "Heading1";
        titulo.AddLineBreak();
        titulo.Format.SpaceAfter = 20;

        //Informacion del partido
        var infoPartido = section.AddParagraph();
        infoPartido.AddFormattedText("Día: ", TextFormat.Bold);
        infoPartido.AddText(Match.Date.ToString("d"));
        infoPartido.AddSpace(10);
        infoPartido.AddFormattedText("Tournament: ", TextFormat.Bold);
        infoPartido.AddText(Match.Tournament);
        infoPartido.AddSpace(10);
        infoPartido.AddFormattedText("Lugar: ", TextFormat.Bold);
        infoPartido.AddText(Match.Place);
        infoPartido.AddSpace(10);
        infoPartido.AddFormattedText("Fecha: ", TextFormat.Bold);
        infoPartido.AddText(Match.MatchWeek.ToString());

        //Tabla
        //Encabezado
        var tabla = section.AddTable();
        tabla.Borders.Width = 0.75;
        var pageWidth = document.DefaultPageSetup.PageWidth - document.DefaultPageSetup.LeftMargin - document.DefaultPageSetup.RightMargin;
        tabla.AddColumn(Unit.FromPoint(pageWidth * 0.4));
        tabla.AddColumn(Unit.FromPoint(pageWidth * 0.1));
        tabla.AddColumn(Unit.FromPoint(pageWidth * 0.1));
        tabla.AddColumn(Unit.FromPoint(pageWidth * 0.4));
        var row = tabla.AddRow();
        row.Cells[0].AddParagraph(TeamLocal.Name);
        row.Cells[1].AddParagraph(Match.GoalsTeamA.ToString());
        row.Cells[2].AddParagraph(Match.GoalsTeamB.ToString());
        row.Cells[3].AddParagraph(TeamAway.Name);
        //Subtitulos
        var subTabla = section.AddTable();
        subTabla.Borders.Width = 0.75;
        subTabla.AddColumn(Unit.FromPoint(pageWidth * 0.1));
        subTabla.AddColumn(Unit.FromPoint(pageWidth * 0.3));
        subTabla.AddColumn(Unit.FromPoint(pageWidth * 0.1));
        subTabla.AddColumn(Unit.FromPoint(pageWidth * 0.1));
        subTabla.AddColumn(Unit.FromPoint(pageWidth * 0.1));
        subTabla.AddColumn(Unit.FromPoint(pageWidth * 0.3));
        var subTitlesRow = subTabla.AddRow();
        subTitlesRow.Cells[0].AddParagraph("N°");
        subTitlesRow.Cells[1].AddParagraph("Nombre");
        subTitlesRow.Cells[2].AddParagraph("Goles");
        subTitlesRow.Cells[3].AddParagraph("Goles");
        subTitlesRow.Cells[4].AddParagraph("N°");
        subTitlesRow.Cells[5].AddParagraph("Nombre");
        //Jugadores
        for (var i = 0; i < CreatedVariablesTypes.QuantityOfPlayersPerClub; i++)
        {
            var playerRow = subTabla.AddRow();

            // Equipo local
            if (TeamLocal.IdPlayers.Count != null && i < TeamLocal.IdPlayers.Count)
            {
                var resultL = Simulo_BdD.GetOnePlayer(TeamLocal.IdPlayers[i]);
                if (resultL.Success)
                {
                    var playerL = resultL.Data;
                    playerRow.Cells[0].AddParagraph(playerL.Number.ToString());
                    playerRow.Cells[1].AddParagraph(playerL.Name);
                    var cantGolesL = CountEndings(Match.Id, playerL.Id, Ending.Goal).QuantityEnding;
                    playerRow.Cells[2].AddParagraph(cantGolesL.ToString());
                }
                else
                {
                    playerRow.Cells[0].AddParagraph("Error al cargar el jugador");
                    playerRow.Cells[1].AddParagraph("Error al cargar el jugador");
                    playerRow.Cells[2].AddParagraph("Error al cargar el jugador");
                }
            }
            else
            {
                playerRow.Cells[0].AddParagraph("-");
                playerRow.Cells[1].AddParagraph("-");
                playerRow.Cells[2].AddParagraph("-");
            }

            // Equipo visitante
            if (TeamAway.IdPlayers.Count != null && i < TeamAway.IdPlayers.Count)
            {
                var resultA = Simulo_BdD.GetOnePlayer(TeamAway.IdPlayers[i]);
                if (resultA.Success)
                {
                    var playerA = resultA.Data;
                    var cantGolesA = CountEndings(Match.Id, playerA.Id, Ending.Goal).QuantityEnding;

                    playerRow.Cells[3].AddParagraph(cantGolesA.ToString());
                    playerRow.Cells[4].AddParagraph(playerA.Number.ToString());
                    playerRow.Cells[5].AddParagraph(playerA.Name);
                }
                else
                {
                    playerRow.Cells[3].AddParagraph("Error al cargar el jugador");
                    playerRow.Cells[4].AddParagraph("Error al cargar el jugador");
                    playerRow.Cells[5].AddParagraph("Error al cargar el jugador");
                }
            }
            else
            {
                playerRow.Cells[3].AddParagraph("-");
                playerRow.Cells[4].AddParagraph("-");
                playerRow.Cells[5].AddParagraph("-");
            }
        }
    }

    public Coordenadas CountEndings(Guid idMatch, Guid idPlayer, Ending ending)
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

    public Coordenadas CountSanctions(Guid idMatch, Guid idPlayer)
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

    public bool LoadData(Guid idMatch)
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