using Phoenix.Application.Services.Colors.Contracts;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.Domain.Entities.Colors;

namespace Phoenix.Persistance.EF.Repositories.Colors
{
    public class EFColorRepository : ColorRepository
    {
        private readonly EFDataContext _context;

        public EFColorRepository(EFDataContext context)
        {
            _context = context;
        }

        public void Add(Color color)
        {
            _context.Set<Color>().Add(color);
        }
    }
}
