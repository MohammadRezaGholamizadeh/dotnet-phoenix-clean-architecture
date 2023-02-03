using Phoenix.Application.Services.Colors.Contracts;
using Phoenix.Application.Services.Colors.Contracts.Dtos;

namespace Phoenix.RestApi.Controllers.Colors
{
    [ApiController]
    [Route("api/v{version:apiVersion}/colors")]
    [ApiVersion("1.0")]
    public class ColorsController : ControllerBase
    {
        private readonly ColorService _colorService;

        public ColorsController(ColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpPost(Name = "Colors")]
        public async Task<int> Add(AddColorDto dto)
        {
            return await _colorService.Add(dto);
        }
    }
}