using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Series.Data;

namespace Series.Pages
{
    public class CreatorAveragesModel : PageModel
    {
        private readonly SeriesContext _context;

        public CreatorAveragesModel(SeriesContext context)
        {
            _context = context;
        }

        public IList<CreatorAverage> CreatorAverages { get; set; } = new List<CreatorAverage>();

        /// <summary>
        /// Orders creators by the average rating of creators' series 
        /// </summary>
        /// <param name="sortOrder">Ascending or descending sorting</param>
        public void OnGet(string sortOrder)
        {
            var query = _context.Creators
                .Include(c => c.SeriesCreators)
                    .ThenInclude(sc => sc.Series)
                .Select(c => new CreatorAverage
                {
                    CreatorName = c.Name,
                    AverageRating = c.SeriesCreators.Any()
                        ? c.SeriesCreators.Average(sc => (double)sc.Series.Rating)
                        : 0
                });

            CreatorAverages = sortOrder == "asc"
                ? query.OrderBy(c => c.AverageRating).ToList()
                : query.OrderByDescending(c => c.AverageRating).ToList();
        }

        public class CreatorAverage
        {
            public string CreatorName { get; set; } = string.Empty;
            public double AverageRating { get; set; }
        }
    }
}
