using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Series.Models
{
    [Table("genres")]
    public class Genre
    {
        [Column("genreid")]
        public int GenreId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<SeriesGenre> SeriesGenres { get; set; } = new List<SeriesGenre>();
    }
}