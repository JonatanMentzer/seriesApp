using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Series.Models
{
     [Table("seriesgenres")]
    public class SeriesGenre
    {
        [Column("seriesid")]
        public int SeriesId { get; set; }

        [Column("genreid")]
        public int GenreId { get; set; }

        public Series Series { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}