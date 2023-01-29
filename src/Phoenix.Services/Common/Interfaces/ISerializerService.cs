using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Common.Interfaces;

public interface ISerializerService : TransientService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}
