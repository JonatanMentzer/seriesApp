using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Series.Models
{
    [Table("seriescreators")]
    public class SeriesCreator
    {
        [Column("seriesid")]
        public int SeriesId { get; set; }

        [Column("creatorid")]
        public int CreatorId { get; set; }

        public Series Series { get; set; } = null!;
        public Creator Creator { get; set; } = null!;
    }
}