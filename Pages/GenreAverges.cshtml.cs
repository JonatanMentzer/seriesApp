using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Series.Data;

namespace Series.Pages
{
    public class GenreAveragesModel : PageModel
    {
        private readonly SeriesContext _context;

        public GenreAveragesModel(SeriesContext context)
        {
            _context = context;
        }

        public IList<GenreAverage> GenreAverages { get; set; } = new List<GenreAverage>();

        public void OnGet(string sortOrder)
        {
            var query = _context.Genres
                .Include(g => g.SeriesGenres)
                    .ThenInclude(sg => sg.Series)
                .Select(g => new GenreAverage
                {
                    GenreName = g.Name,
                    AverageRating = g.SeriesGenres.Any()
                        ? g.SeriesGenres.Average(sg => (double)sg.Series.Rating)
                        : 0
                });

            GenreAverages = sortOrder == "asc"
                ? query.OrderBy(g => g.AverageRating).ToList()
                : query.OrderByDescending(g => g.AverageRating).ToList();
        }

        public class GenreAverage
        {
            public string GenreName { get; set; } = string.Empty;
            public double AverageRating { get; set; }
        }
    }
}
