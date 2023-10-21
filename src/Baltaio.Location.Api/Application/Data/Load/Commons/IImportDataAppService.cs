using Baltaio.Location.Api.Application.Data.Load.LoadData;

namespace Baltaio.Location.Api.Application.Data.Load.Commons
{
    public interface IImportDataAppService
    {
        Task<ImportDataOutput> Execute(Stream source);
    }
}
