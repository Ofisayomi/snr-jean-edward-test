using Imdb.Application.Models;

namespace Imdb.Application.Interfaces;

public interface IMovieServices
{
    Task<MovieModel> SearchMovie(string title, MovieQuery query);
    Task<List<SearchResultResponse>> GetSearchResults();
}
