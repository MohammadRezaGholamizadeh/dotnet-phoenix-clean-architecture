using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Common.Exporters;

public interface IExcelWriter : TransientService
{
    Stream WriteToStream<T>(IList<T> data);
}
