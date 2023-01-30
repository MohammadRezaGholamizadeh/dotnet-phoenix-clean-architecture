using Phoenix.Application.Services.Colors.Contracts;
using Phoenix.Application.Services.Colors.Contracts.Dtos;
using Phoenix.TestTools.Infrastructure;
using System.Threading.Tasks;
using Xunit;
using Phoenix.Domain.Entities.Colors;
using System.Linq;
using FluentAssertions;
using Phoenix.TestTools.Tools.Buliders;

namespace Phoenix.IntegrationTests.ServiceTests.Colors
{
    public class ColorServiceTests : SpecTestSut<ColorService>
    {
        [Fact]
        public async Task Add_add_color_Successfully()
        {
            var dto = new Builder<AddColorDto>()
                          .With(_ => _.Title, "dummy")
                          .Build();

            var colorId = await Sut.Add(dto);

            var expected = Context.Set<Color>().ToList();
            expected.Single().Title.Should().Be(dto.Title);

        }
    }
}
