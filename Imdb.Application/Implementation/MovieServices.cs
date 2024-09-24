using System.Net;
using Imdb.Application.Interfaces;
using Imdb.Application.Models;
using Imdb.Domain.Entities;
using Imdb.Persistence.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Imdb.Application.Implementation;

public class MovieServices : IMovieServices {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<MovieServices> _logger;
    private readonly IMovieRepository _movieRepository;

    public MovieServices (HttpClient httpClient, IConfiguration config, IMovieRepository movieRepository, ILogger<MovieServices> logger) {
        _httpClient = httpClient;
        _config = config;
        _movieRepository = movieRepository;
        _logger = logger;
    }

    /// <summary>
    /// Searches a movie by title.
    /// </summary>
    /// <param name="title">The title of the movie.</param>
    /// <param name="query">The search query.</param>
    /// <returns>The movie model.</returns>
    /// <exception cref="WebException">Thrown when there is an error connecting to the remote server.</exception>
    /// <exception cref="Exception">Thrown when there is an error.</exception>
    public async Task<MovieModel> SearchMovie (string title, MovieQuery query) {
        try {
            var apiKey = _config.GetSection ("ApiKey").Value;
            var year = query.Year == 0 ? string.Empty : query.Year.ToString ();
            var plot = string.IsNullOrEmpty (query.Plot) ? "short" : query.Plot;
            var responseString = await _httpClient.GetStringAsync ($"?apiKey={apiKey}&t={title}&plot={plot}&y={year}");

            var responseModel = JsonConvert.DeserializeObject<MovieModel> (responseString);
            if (responseModel.Response) {
                var movieResult = await _movieRepository.FindSingleItem (x => x.ImdbId == responseModel.imdbID);
                if (movieResult == null) {
                    await _movieRepository.CreateItem (new Movie {
                        Keyword = title,
                            SearchResult = JsonConvert.SerializeObject (responseModel),
                            ImdbId = responseModel.imdbID
                    });
                    _logger.LogInformation ($"Movie created title=${title}, plot=${plot}, year=${year}");
                } else {
                    movieResult.DateCreated = DateTime.UtcNow;
                    movieResult.Keyword = title;
                    await _movieRepository.UpdateItem (movieResult);

                    _logger.LogInformation ($"Movie {movieResult.ImdbId} updated");
                }

            }
            return responseModel;
        } catch (WebException webEx) {
            _logger.LogError ($"Error connecting to remote server. {webEx.Message}");
            throw new WebException ($"Error connecting to remote server. {webEx.Message}");
        } catch (Exception ex) {
            _logger.LogError ($"An error occurred. {ex.Message}");
            throw new Exception ($"An error occurred. {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves the last 5 search results.
    /// </summary>
    /// <returns>A list of search result responses.</returns>
    public async Task<List<SearchResultResponse>> GetSearchResults () {
        try {
            var searchResults = _movieRepository.FindItems ().OrderByDescending (x => x.DateCreated).Take (5).ToList ();
            List<SearchResultResponse> response = new ();
            searchResults.ForEach (x => {
                response.Add (new SearchResultResponse {
                    Title = x.Keyword,
                        SearchResult = JsonConvert.DeserializeObject<MovieModel> (x.SearchResult)
                });
            });

            _logger.LogInformation ($"Retrieved last 5 search results");
            return response;
        } catch (Exception ex) {
            _logger.LogError ($"An error occurred. {ex.Message}");
            throw new Exception ($"An error occurred. {ex.Message}");
        }
    }
}