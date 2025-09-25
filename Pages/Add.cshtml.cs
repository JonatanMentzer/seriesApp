using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Series.Data;
using Series.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Series.Pages
{
    public class AddModel : PageModel
    {
        private readonly SeriesContext _context;

        public AddModel(SeriesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Series Series { get; set; } = new Models.Series();

        [BindProperty]
        [Required]
        public string CreatorName { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string GenreName { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Series.Add(Series);
            _context.SaveChanges();

            var creatorNames = CreatorName.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var name in creatorNames)
            {
                var creator = _context.Creators.FirstOrDefault(c => c.Name == name);
                if (creator == null)
                {
                    creator = new Creator { Name = name };
                    _context.Creators.Add(creator);
                    _context.SaveChanges();
                }

                _context.SeriesCreators.Add(new SeriesCreator
                {
                    SeriesId = Series.SeriesId,
                    CreatorId = creator.CreatorId
                });
            }

            var genreNames = GenreName.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var name in genreNames)
            {
                var genre = _context.Genres.FirstOrDefault(g => g.Name == name);
                if (genre == null)
                {
                    genre = new Genre { Name = name };
                    _context.Genres.Add(genre);
                    _context.SaveChanges();
                }

                _context.SeriesGenres.Add(new SeriesGenre
                {
                    SeriesId = Series.SeriesId,
                    GenreId = genre.GenreId
                });
            }

            _context.SaveChanges();

            return RedirectToPage("/Index");
        }

    }
}
