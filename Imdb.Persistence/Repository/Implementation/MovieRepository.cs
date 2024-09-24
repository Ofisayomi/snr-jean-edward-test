using Imdb.Domain.Entities;
using Imdb.Persistence.DatabaseContext;
using Imdb.Persistence.Repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace Imdb.Persistence.Repository.Implementation;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(MovieDbContext dbContext, ILogger<Repository<Movie>> logger) : base(dbContext, logger)
    {
    }

}
