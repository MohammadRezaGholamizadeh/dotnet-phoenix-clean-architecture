using Phoenix.Application.Common.Interfaces;
namespace Phoenix.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}
