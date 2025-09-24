using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Series.Models
{
    [Table("series")]
    public class Series
    {
        [Column("seriesid")]
        public int SeriesId { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("releaseyear")]
        public int ReleaseYear { get; set; }

        [Column("company")]
        public string Company { get; set; } = string.Empty;

        [Column("rating")]
        public int Rating { get; set; }

        public ICollection<SeriesGenre> SeriesGenres { get; set; } = new List<SeriesGenre>();
        public ICollection<SeriesCreator> SeriesCreators { get; set; } = new List<SeriesCreator>();
    }
}