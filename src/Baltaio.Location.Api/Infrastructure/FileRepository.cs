using Baltaio.Location.Api.Application.Data.Load.Commons;
using Baltaio.Location.Api.Domain;
using SpreadsheetLight;

namespace Baltaio.Location.Api.Infrastructure
{
    public class FileRepository : IFileRepository
    {
        public List<City> GetCities(Stream source)
        {
            List<City> cities = new();

            SLDocument document = new SLDocument(source);
            bool worksheetsExist = document.SelectWorksheet("MUNICIPIOS");
            if (!worksheetsExist)
                return cities;

            const int firstDataRow = 2;
            for (int i = firstDataRow; document.HasCellValue($"A{i}"); i++)
            {
                cities.Add(
                    new City
                    (
                        document.GetCellValueAsInt32($"A{i}"),
                        document.GetCellValueAsString($"B{i}"),
                        document.GetCellValueAsString($"C{i}")
                    )
                );
            }

            return cities;
        }

        public List<State> GetStates(Stream source)
        {
            List<State> states = new();

            SLDocument document = new SLDocument(source);
            bool worksheetsExist = document.SelectWorksheet("ESTADOS");
            if (!worksheetsExist)
                return states;

            const int firstDataRow = 2;
            for (int i = firstDataRow; document.HasCellValue($"A{i}"); i++)
            {
                states.Add(
                    new State
                    (
                        document.GetCellValueAsInt32($"A{i}"),
                        document.GetCellValueAsString($"B{i}"),
                        document.GetCellValueAsString($"C{i}")
                    )
                );
            }

            return states;
        }
    }
}
