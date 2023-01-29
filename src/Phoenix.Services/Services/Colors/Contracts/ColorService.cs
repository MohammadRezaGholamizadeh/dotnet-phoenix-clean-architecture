

using Phoenix.Application.Services.Colors.Contracts.Dtos;
using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Services.Colors.Contracts
{
    public interface ColorService : ScopedService
    {
        Task<int> Add(AddColorDto dto);
    }
}
