using signa.Interfaces.Services;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using signa.Entities;

namespace signa.Services;

public class DownloadsService : IDownloadsService
{
    private readonly ITournamentsService tournamentsService;

    public DownloadsService(ITournamentsService tournamentsService)
    {
        this.tournamentsService = tournamentsService;
    }

    public async Task<byte[]> DownloadTournamentPlayers(Guid tournamentId)
    {
        var tournament = await tournamentsService.GetTournament(tournamentId);
        var users = tournament.Teams.SelectMany(x => x.Members).ToList();
        return GenerateDocx(users);
    }

    private static byte[] GenerateDocx(List<UserEntity> users)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var wordDocument = WordprocessingDocument.Create(
                       memoryStream,
                       DocumentFormat.OpenXml.WordprocessingDocumentType.Document,
                       true))
            {
                // Основная часть документа
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                // Заголовок документа
                var headerParagraph = new Paragraph(new Run(new Text("Список участников")));
                headerParagraph.ParagraphProperties = new ParagraphProperties
                {
                    Justification = new Justification { Val = JustificationValues.Center },
                    SpacingBetweenLines = new SpacingBetweenLines { After = "200" }
                };
                body.AppendChild(headerParagraph);

                // Таблица
                var table = new Table();

                // Настройка таблицы
                var tableProperties = new TableProperties(
                    new TableWidth { Type = TableWidthUnitValues.Pct, Width = "5000" }, // Ширина таблицы: 50%
                    new Justification { Val = JustificationValues.Center },  // Центрирование таблицы
                    new TableBorders( // Границы таблицы
                        new TopBorder { Val = BorderValues.Single, Size = 4 },
                        new BottomBorder { Val = BorderValues.Single, Size = 4 },
                        new LeftBorder { Val = BorderValues.Single, Size = 4 },
                        new RightBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                    )
                );
                table.AppendChild(tableProperties);

                // Ширина колонок
                var tableGrid = new TableGrid(
                    new GridColumn { Width = "500" }, // Номер участника
                    new GridColumn { Width = "3000" }, // ФИО
                    new GridColumn { Width = "1000" }  // Номер группы
                );
                table.AppendChild(tableGrid);

                // Заголовок таблицы
                var headerRow = new TableRow();
                headerRow.Append(
                    CreateTableCell(" № ", true),
                    CreateTableCell("ФИО", true),
                    CreateTableCell("Номер группы", true)
                );
                table.AppendChild(headerRow);

                // Данные пользователей
                var number = 1;
                foreach (var user in users)
                {
                    var row = new TableRow();
                    row.Append(
                        CreateTableCell($"{number}"),
                        CreateTableCell($"{user.LastName} {user.FirstName} {user.Patronymic}"),
                        CreateTableCell(user.GroupNumber)
                    );
                    table.AppendChild(row);
                    number++;
                }

                body.AppendChild(table);
            }

            return memoryStream.ToArray();
        }
    }

    private static TableCell CreateTableCell(string text, bool isHeader = false)
    {
        // Создаем параграф для ячейки
        const int leftIndent = 150;
        var paragraph = new Paragraph(new Run(new Text(text)));
        paragraph.ParagraphProperties = new ParagraphProperties
        {
            Indentation = new Indentation { Left = leftIndent.ToString() }
        };

        // Если это заголовок, добавляем центрирование и выделение жирным
        if (isHeader)
        {
            paragraph.ParagraphProperties = new ParagraphProperties
            {
                Justification = new Justification { Val = JustificationValues.Center }
            };

            var runProperties = new RunProperties(new Bold());
            ((Run)paragraph.Elements<Run>().First()).RunProperties = runProperties;

            // Добавляем серый фон для заголовков
            return new TableCell(paragraph)
            {
                TableCellProperties = new TableCellProperties(
                    new Shading
                    {
                        Val = ShadingPatternValues.Clear,
                        Fill = "D9D9D9" // Светло-серый фон
                    }
                )
            };
        }

        // Если это не заголовок, возвращаем ячейку без центрирования
        return new TableCell(paragraph);
    }
}
