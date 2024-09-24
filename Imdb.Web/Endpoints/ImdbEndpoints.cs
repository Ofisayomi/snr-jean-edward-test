using Imdb.Application.Interfaces;
using Imdb.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Imdb.Web.Endpoints;

public static class ImdbEndpoints
{
    public static WebApplication MapImdbEndpoints (this WebApplication app)
    {
        app.MapGet ("/api/v1/movies/{title}", async ([FromQuery]int? year, [FromQuery]string? plot, string title,IMovieServices movieServices) =>
        {
            var movieQuery = new MovieQuery{
                Year = year, 
                Plot = plot
            };

            var movies = await movieServices.SearchMovie(title, movieQuery);
            return Results.Ok (movies);
        })
        .WithName("SearchMovies")
        .WithOpenApi();

        
        app.MapGet ("/api/v1/movies/results", async (IMovieServices movieServices) =>
        {
            var movies = await movieServices.GetSearchResults();
            return Results.Ok (movies);
        })
        .WithName("GetLast5SearchResults")
        .WithOpenApi();

        return app;
    }
}
