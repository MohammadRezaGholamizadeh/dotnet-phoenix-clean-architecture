using Phoenix.Domain.Entities.Colors;
using Phoenix.SharedConfiguration.Common.Contracts.Repositories;

namespace Phoenix.Application.Services.Colors.Contracts
{
    public interface ColorRepository : ScopedRepository
    {
        void Add(Color color);
    }
}
