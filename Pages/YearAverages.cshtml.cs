using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Series.Data;

namespace Series.Pages
{
    public class YearAveragesModel : PageModel
    {
        private readonly SeriesContext _context;

        public YearAveragesModel(SeriesContext context)
        {
            _context = context;
        }

        public IList<YearAverage> YearAverages { get; set; } = new List<YearAverage>();

        public void OnGet(string sortOrder)
        {
            var query = _context.Series
                .GroupBy(s => s.ReleaseYear)
                .Select(g => new YearAverage
                {
                    ReleaseYear = g.Key,
                    AverageRating = g.Average(s => (double)s.Rating)
                });

            YearAverages = sortOrder == "asc"
                ? query.OrderBy(y => y.AverageRating).ToList()
                : query.OrderByDescending(y => y.AverageRating).ToList();
        }

        public class YearAverage
        {
            public int ReleaseYear { get; set; }
            public double AverageRating { get; set; }
        }
    }
}
