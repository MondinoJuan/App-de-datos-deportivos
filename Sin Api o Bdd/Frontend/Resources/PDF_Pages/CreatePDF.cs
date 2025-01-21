using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Frontend.Resources.Entities;
using PdfSharp.Drawing;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Graphics;
using SkiaSharp;

namespace Frontend.Resources.PDF_Pages;
class CreatePDF
{
    private Match_Dto? Match { get; set; }
    private Club_Dto? TeamLocal { get; set; }
    private Club_Dto? TeamAway { get; set; }

    public PdfDocument CrearPDF(Guid idMatch)       //Actualizar el Match y Club asi se guardan los datos una vez que se pone finalizar en MatchView
    {
        if (!LoadData(idMatch))
        {
            Console.WriteLine("Error al cargar los datos");
            return null;
        }

        // Create a new MigraDoc document
        var document = new Document();

        // Recuperar todos los objetos creados en la aplicacion para plasmar la informacion en el PDF

        BuildDocumnet(document);


        //Create a render for the MigraDoc document
        var pdfRenderer = new PdfDocumentRenderer();
        pdfRenderer.Document = document;

        //Layout and render document to PDF
        pdfRenderer.RenderDocument();

        return pdfRenderer.PdfDocument;
    }

    private void BuildDocumnet(Document document)
    {
        SummaryMatch(document);
        //Footer
        SummaryTeam(document, TeamLocal);
        //Footer
        SummaryTeam(document, TeamAway);
        //Footer
    }

    public void SummaryTeam(Document document, Club_Dto team)
    {
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
    }

    private void EndingSection(Document document, Club_Dto team, Ending end)
    {
        var cantidadEndingsTeam = 0;
        var marcasCampo = new List<Coordenates>();
        var marcasArco = new List<Coordenates>();

        foreach (var idPlayer in team.IdPlayers)
        {
            var temp = CountEndings(Match.Id, idPlayer, end);

            cantidadEndingsTeam += temp.quantityEnding;

            if (temp.CooField != null)
                marcasCampo.AddRange(temp.CooField);

            if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
            {
                if (temp.CooGoal != null)
                    marcasArco.AddRange(temp.CooGoal);
            }
            
        }

        var section = document.AddSection();
        section.AddParagraph($"{end}: {cantidadEndingsTeam}");

        // Generar imágenes con marcas
        string canchaWithMarksPath = GenerateMarkedImage("Resources/Images/cancha.png", marcasCampo, new SKColor(255, 0, 0, 128)); // Rojo semitransparente
        string arcoWithMarksPath = null;
        if (end == Ending.Goal || end == Ending.Miss || end == Ending.Save)
        {
            arcoWithMarksPath = GenerateMarkedImage("Resources/Images/arco.png", marcasArco, new SKColor(0, 0, 255, 128)); // Azul semitransparente
        }

        try
        {
            // Insertar imágenes generadas al documento
            var markedCanchaImage = section.AddImage(canchaWithMarksPath);
            markedCanchaImage.Width = "10cm";
            markedCanchaImage.Height = "7cm";

            if (arcoWithMarksPath == null)
            {
                var markedArcoImage = section.AddImage(arcoWithMarksPath);
                markedArcoImage.Width = "5cm";
                markedArcoImage.Height = "5cm";
            }
            
        }
        finally
        {
            // Eliminar las imágenes marcadas
            if (File.Exists(canchaWithMarksPath))
                File.Delete(canchaWithMarksPath);
            if (arcoWithMarksPath == null)
            {
                if (File.Exists(arcoWithMarksPath))
                    File.Delete(arcoWithMarksPath);
            }
        }
    }

    private string GenerateMarkedImage(string imagePath, List<Coordenates> marks, SKColor color)
    {
        string markedImagePath = Path.Combine(FileSystem.Current.CacheDirectory, $"{Path.GetFileNameWithoutExtension(imagePath)}_marked.png");

        using (var stream = File.OpenRead(imagePath))
        using (var bitmap = SKBitmap.Decode(stream))
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
                    float scaledX = (float)((mark.X.Value / 100.0) * bitmap.Width);
                    float scaledY = (float)((mark.Y.Value / 100.0) * bitmap.Height);

                    using (var paint = new SKPaint
                    {
                        Color = color,
                        IsAntialias = true,
                        Style = SKPaintStyle.Fill
                    })
                    {
                        canvas.DrawCircle(scaledX, scaledY, 5, paint);
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
        var section2 = document.AddSection();
        var infoPartido = section2.AddParagraph();
        infoPartido.AddFormattedText("Día: ", TextFormat.Bold);
        infoPartido.AddText(Match.Date.ToString());
        infoPartido.AddTab();
        infoPartido.AddFormattedText("Tournament: ", TextFormat.Bold);
        infoPartido.AddText(Match.Tournament);
        infoPartido.AddTab();
        infoPartido.AddFormattedText("Lugar: ", TextFormat.Bold);
        infoPartido.AddText(Match.Place);
        infoPartido.AddTab();
        infoPartido.AddFormattedText("Fecha: ", TextFormat.Bold);
        infoPartido.AddText(Match.MatchWeek.ToString());

        //Tabla
        //Encabezado
        var section3 = document.AddSection();
        var tabla = section3.AddTable();
        tabla.Borders.Width = 0.75;
        tabla.AddColumn("40%");
        tabla.AddColumn("10%");
        tabla.AddColumn("10%");
        tabla.AddColumn("40%");
        var row = tabla.AddRow();
        row.Cells[0].AddParagraph(TeamLocal.Name);
        row.Cells[1].AddParagraph(Match.GoalsTeamA.ToString());
        row.Cells[2].AddParagraph(Match.GoalsTeamB.ToString());
        row.Cells[3].AddParagraph(TeamAway.Name);
        //Subtitulos
        var subTabla = section3.AddTable();
        subTabla.Borders.Width = 0.75;
        subTabla.AddColumn("10%");
        subTabla.AddColumn("30%");
        subTabla.AddColumn("10%");
        subTabla.AddColumn("10%");
        subTabla.AddColumn("10%");
        subTabla.AddColumn("30%");
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
            if (i < TeamLocal.IdPlayers.Count)
            {
                var resultL = Simulo_BdD.GetOnePlayer(TeamLocal.IdPlayers[i]);
                if (resultL.Success)
                {
                    var playerL = resultL.Data;
                    playerRow.Cells[0].AddParagraph(playerL.Number.ToString());
                    playerRow.Cells[1].AddParagraph(playerL.Name);
                    var cantGolesL = CountEndings(Match.Id, playerL.Id, Ending.Goal).quantityEnding;
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
                playerRow.Cells[3].AddParagraph("-");
                playerRow.Cells[4].AddParagraph("-");
                playerRow.Cells[5].AddParagraph("-");
            }

            // Equipo visitante
            if (i < TeamAway.IdPlayers.Count)
            {
                var resultA = Simulo_BdD.GetOnePlayer(TeamAway.IdPlayers[i]);
                if (resultA.Success)
                {
                    var playerA = resultA.Data;
                    var cantGolesA = CountEndings(Match.Id, playerA.Id, Ending.Goal).quantityEnding;
                    
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
                if (result1.Data.Ending == ending)
                {
                    contabilizo.CooField.Add(new Coordenates
                    {
                        X = result1.Data.ActionPositionX,
                        Y = result1.Data.ActionPositionY
                    });
                    if(ending == Ending.Goal || ending == Ending.Miss || ending == Ending.Save)
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
        contabilizo.quantityEnding = count;
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
            TeamLocal = resultB.Data;
        }
        else
        {
            Console.WriteLine($"Error al recuperar el club con id = {Match.IdTeamAway}");
            return false;
        }

        return true;
    }
}
