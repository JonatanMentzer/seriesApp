using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Series.Data;

namespace Series.Pages
{
    public class CompanyAveragesModel : PageModel
    {
        private readonly SeriesContext _context;

        public CompanyAveragesModel(SeriesContext context)
        {
            _context = context;
        }

        public IList<CompanyAverage> CompanyAverages { get; set; } = new List<CompanyAverage>();

        public void OnGet(string sortOrder)
        {
            var query = _context.Series
                .GroupBy(s => s.Company)
                .Select(g => new CompanyAverage
                {
                    Company = g.Key,
                    AverageRating = g.Average(s => (double)s.Rating)
                });

            CompanyAverages = sortOrder == "asc"
                ? query.OrderBy(c => c.AverageRating).ToList()
                : query.OrderByDescending(c => c.AverageRating).ToList();
        }

        public class CompanyAverage
        {
            public string Company { get; set; } = string.Empty;
            public double AverageRating { get; set; }
        }
    }
}
