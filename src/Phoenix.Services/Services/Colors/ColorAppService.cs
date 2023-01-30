using Phoenix.Application.Services.Colors.Contracts;
using Phoenix.Application.Services.Colors.Contracts.Dtos;
using Phoenix.Domain.Entities.Colors;
using Phoenix.SharedConfiguration.Common.Contracts.UnitOfWorks;

namespace Phoenix.Application.Services.Colors
{
    public class ColorAppService : ColorService
    {
        private readonly ColorRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ColorAppService(
            ColorRepository colorRepository,
            UnitOfWork unitOfWork)
        {
            _repository = colorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(AddColorDto dto)
        {
            var color = new Color()
            {
                Title = dto.Title
            };
            _repository.Add(color);
            await _unitOfWork.SaveAllChangesAsync();
            return color.Id;
        }
    }
}
