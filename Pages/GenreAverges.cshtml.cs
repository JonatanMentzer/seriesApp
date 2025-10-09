using Microsoft.AspNetCore.Mvc;
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
        [BindProperty(SupportsGet = true)]
        public int CombinationSize { get; set; } = 1;
        public List<GenreCombinationResult> GenreCombinations { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public int MinOccurrences { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "desc";
        public IList<GenreCombinationResult> GenreAverages { get; set; } = new List<GenreCombinationResult>();

        /// <summary>
        /// Orders genres based on the average rating of series in that genre
        /// </summary>
        /// <param name="sortOrder">Ascending or descending sorting</param>
        public void OnGet()
        {
            var seriesList = _context.Series
                .Include(s => s.SeriesGenres)
                .ThenInclude(sg => sg.Genre)
                .ToList();

            var comboRatings = new Dictionary<string, List<double>>();

            foreach (var s in seriesList)
            {
                var genres = s.SeriesGenres.Select(g => g.Genre.Name).Distinct().ToList();
                if (genres.Count >= CombinationSize && CombinationSize > 0)
                {
                    var combos = GetCombinations(genres, CombinationSize);
                    foreach (var combo in combos)
                    {
                        var key = string.Join(",", combo.OrderBy(x => x));
                        if (!comboRatings.ContainsKey(key))
                            comboRatings[key] = new List<double>();
                        comboRatings[key].Add(s.Rating);
                    }
                }
            }

            var result = comboRatings
                .Where(kvp => kvp.Value.Count >= MinOccurrences)
                .Select(kvp => new GenreCombinationResult
                {
                    Genres = kvp.Key.Split(',').ToList(),
                    AverageRating = kvp.Value.Average()
                });

            GenreAverages = SortOrder == "asc"
                ? result.OrderBy(r => r.AverageRating).ToList()
                : result.OrderByDescending(r => r.AverageRating).ToList();
        }

        private static List<List<string>> GetCombinations(List<string> list, int length)
        {
            if (length == 1)
                return list.Select(x => new List<string> { x }).ToList();

            var result = new List<List<string>>();
            for (int i = 0; i < list.Count; i++)
            {
                var head = list[i];
                var tailCombos = GetCombinations(list.Skip(i + 1).ToList(), length - 1);
                foreach (var tail in tailCombos)
                {
                    var combo = new List<string> { head };
                    combo.AddRange(tail);
                    result.Add(combo);
                }
            }
            return result;
        }

        public class GenreCombinationResult
        {
            public List<string> Genres { get; set; } = new();
            public double AverageRating { get; set; }
            public int Count { get; set; }
        }
    }
}
