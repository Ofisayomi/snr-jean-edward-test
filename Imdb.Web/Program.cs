using Imdb.Application.Implementation;
using Imdb.Application.Interfaces;
using Imdb.Persistence.DatabaseContext;
using Imdb.Persistence.Repository.Implementation;
using Imdb.Persistence.Repository.Interfaces;
using Imdb.Web.Endpoints;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<MovieDbContext>(context=>{
    context.UseInMemoryDatabase("MovieSearch");
});

builder.Services.AddTransient<IMovieRepository, MovieRepository>();

builder.Services.AddHttpClient<IMovieServices, MovieServices>(client=>{
    client.BaseAddress = new Uri(builder.Configuration["BaseAddress"]);
});

builder.Services.AddCors (options => {
    options.AddDefaultPolicy (cors =>
        cors
        .WithOrigins (builder.Configuration["FrontendOrigin"])
        .WithHeaders ("Content-Type", "Authorization")
        .AllowAnyMethod ()
        .AllowCredentials ()
    );
});

var app = builder.Build();

app.MapImdbEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.Run();

