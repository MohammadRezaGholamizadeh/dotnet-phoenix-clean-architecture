using Phoenix.SharedConfiguration.Common;
using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Common.FileStorage;

public interface IFileStorageService : TransientService
{
    public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

    public void Remove(string? path);
}
