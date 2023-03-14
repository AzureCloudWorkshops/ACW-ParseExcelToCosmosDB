using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingFunctions.ParseExcel
{
    public class ParseFile
    {
        public static List<Movie> ParseDataFile(MemoryStream fileStreamInMemory)
        {
            fileStreamInMemory.Position = 0;

            var sampleData = new List<Movie>();

            string columnRef;

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileStreamInMemory, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var worksheetPart = workbookPart.WorksheetParts.First();
                var sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                var sst = sstpart.SharedStringTable;
                var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                foreach (Row r in sheetData.Elements<Row>())
                {
                    if (r.RowIndex == 1)
                    {
                        //skip columns row
                        continue;
                    }

                    var nextMovie = new Movie();

                    foreach (Cell c in r.Elements<Cell>())
                    {
                        columnRef = c?.CellReference?.Value?.Substring(0, 1) ?? "";

                        string dataString = c?.CellValue?.Text ?? string.Empty;

                        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                        {
                            var isIndex = int.TryParse(c?.CellValue?.Text, out int ssid);
                            if (!isIndex) continue;
                            dataString = sst.ChildElements[ssid].InnerText;
                        }

                        switch (columnRef)
                        {
                            case "A":
                                //id
                                nextMovie.id = dataString;
                                break;
                            case "B":
                                //MovieId
                                var isId = int.TryParse(dataString, out int id);
                                if (isId && id > 0)
                                {
                                    nextMovie.MovieId = id;
                                }
                                break;
                            case "C":
                                //Title
                                nextMovie.Title = dataString;
                                break;
                            case "D":
                                //Rating
                                nextMovie.Rating = dataString;
                                break;
                            case "E":
                                //Review
                                nextMovie.Review = dataString;
                                break;
                            case "F":
                                //year
                                var isYear = int.TryParse(dataString, out int year);
                                if (isYear && year > 0)
                                {
                                    nextMovie.Year = year;
                                }
                                break;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(nextMovie.id) && nextMovie.MovieId > 0)
                    {
                        sampleData.Add(nextMovie);
                    }
                }
            }

            return sampleData;
        }
    }
}