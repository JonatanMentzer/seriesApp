using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Series.Data;
using Series.Models;

namespace Series.Pages
{
    public class ListSeriesModel : PageModel
    {
        private readonly SeriesContext _context;

        public ListSeriesModel(SeriesContext context)
        {
            _context = context;
        }

        public IList<Series.Models.Series> SeriesList { get; set; } = new List<Series.Models.Series>();

        /// <summary>
        /// Orders series based on their rating.
        /// </summary>
        /// <param name="sortOrder">Ascending or descending sorting</param>
        public void OnGet(string sortOrder)
        {
            var query = _context.Series
                .Include(s => s.SeriesGenres).ThenInclude(sg => sg.Genre)
                .Include(s => s.SeriesCreators).ThenInclude(sc => sc.Creator)
                .AsQueryable();

            query = sortOrder switch
            {
                "asc" => query.OrderBy(s => s.Rating),
                "desc" => query.OrderByDescending(s => s.Rating),
                _ => query
            };

            SeriesList = query.ToList();
        }
    }
}
