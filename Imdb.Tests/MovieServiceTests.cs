using System.Text;
using Castle.Core.Logging;
using Imdb.Application.Implementation;
using Imdb.Application.Interfaces;
using Imdb.Application.Models;
using Imdb.Persistence.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;


namespace Imdb.Tests;

public class MovieServicesTests
{
    private HttpClient _httpClient;
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ILogger<MovieServices>> _loggerMock;
    private readonly MovieServices _movieServices;

    public MovieServicesTests()
    {
        var mockedSection = new Mock<IConfigurationSection>();
        mockedSection.Setup(x => x.Value).Returns("92d335bc");

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(x => x.GetSection("ApiKey")).Returns(mockedSection.Object);

        var httpClientFactoryMock = new HttpClient();
        httpClientFactoryMock.BaseAddress = new Uri("https://www.omdbapi.com/");

        _movieRepositoryMock = new Mock<IMovieRepository>();
        _loggerMock = new Mock<ILogger<MovieServices>>();
        _movieServices = new MovieServices(httpClientFactoryMock, configuration.Object, _movieRepositoryMock.Object, _loggerMock.Object);
    }

    [TestCase("Inception", null, null)]
    [TestCase("Inception", null, 2010)]
    [TestCase("Inception", "short", 2010)]
    [TestCase("Inception", "full", 2010)]
    public async Task SearchMovie_Should_Return_Valid_Response(string title, string? Plot, int? Year)
    {
        var query = new MovieQuery{
            Plot = Plot,
            Year = Year
        };
        
        var result = await _movieServices.SearchMovie(title, query);

        Assert.NotNull(result);
    }

    [Test]
    public async Task Get_Last5_Movie(){
        var result = await _movieServices.GetSearchResults();

        Assert.NotNull(result);
    }
}