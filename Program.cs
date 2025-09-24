using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Series.Data;
using Series.Models;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var options = new DbContextOptionsBuilder<SeriesContext>()
    .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
    .Options;

using (var context = new SeriesContext(options))
{
    // Insert example series with genre + creator
    var genre = context.Genres.FirstOrDefault(g => g.Name == "Drama") ?? new Genre { Name = "Drama" };
    var creator = context.Creators.FirstOrDefault(c => c.Name == "Vince Gilligan") ?? new Creator { Name = "Vince Gilligan" };

    var series = new Series.Models.Series
    {
        Title = "Breaking Bad",
        ReleaseYear = 2008,
        Company = "AMC",
        Rating = 10
    };

    series.SeriesGenres.Add(new SeriesGenre { Series = series, Genre = genre });
    series.SeriesCreators.Add(new SeriesCreator { Series = series, Creator = creator });

    context.Series.Add(series);
    context.SaveChanges();

    // Query and print
    var seriesList = context.Series
        .Include(s => s.SeriesGenres).ThenInclude(sg => sg.Genre)
        .Include(s => s.SeriesCreators).ThenInclude(sc => sc.Creator)
        .ToList();

    foreach (var s in seriesList)
    {
        Console.WriteLine($"{s.Title} ({s.ReleaseYear}) - {s.Company} [{s.Rating}/10]");
        Console.WriteLine("Genres: " + string.Join(", ", s.SeriesGenres.Select(g => g.Genre.Name)));
        Console.WriteLine("Creators: " + string.Join(", ", s.SeriesCreators.Select(c => c.Creator.Name)));
        Console.WriteLine();
    }
}
