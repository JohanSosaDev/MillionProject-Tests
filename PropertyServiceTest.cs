using Moq;
using PropertyApi.Models;
using PropertyApi.Repositories;
using PropertyApi.Services;

public class PropertyServiceTests
{
    [Fact]
    public async Task Search_ByName_FiltersCorrectly()
    {
        var sample = new List<Property> {
            new() { Name = "Casa Bonita", Address = "A", Price = 100, IdOwner = "1" },
            new() { Name = "Depto Centro", Address = "B", Price = 200, IdOwner = "2" },
        };

        var repo = new Mock<IPropertyRepository>();
        repo.Setup(r => r.SearchAsync("Casa", null, null, null, 1, 12, default))
            .ReturnsAsync(((IReadOnlyList<Property>)sample.Where(x => x.Name.Contains("Casa")).ToList(), 1L));

        var svc = new PropertyService(repo.Object);
        var (items, total) = await svc.SearchAsync("Casa", null, null, null, 1, 12, default);

        Assert.Single(items);
        Assert.Equal(1L, total);
        Assert.Equal("Casa Bonita", items.First().Name);
    }
}

