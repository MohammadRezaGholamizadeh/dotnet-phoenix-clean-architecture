using FluentAssertions;
using Phoenix.Application.Services.Colors.Contracts;
using Phoenix.Application.Services.Colors.Contracts.Dtos;
using Phoenix.Domain.Entities.Colors;
using Phoenix.TestTools.Infrastructure;
using Phoenix.TestTools.Tools.Buliders;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Phoenix.UnitTests.ServiceTests.Colors
{
    public class ColorServiceTests : UnitTestSut<ColorService>
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
