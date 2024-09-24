using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imdb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Imdb.Persistence.DatabaseContext;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

    public DbSet<Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Movie>().HasIndex(x => x.ImdbId);
    }
}
