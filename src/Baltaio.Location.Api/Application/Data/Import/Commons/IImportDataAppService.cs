using Baltaio.Location.Api.Application.Data.Import.ImportData;

namespace Baltaio.Location.Api.Application.Data.Import.Commons
{
    public interface IImportDataAppService
    {
        Task<ImportDataOutput> Execute(Stream source);
    }
}
