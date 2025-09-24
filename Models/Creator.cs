using System.ComponentModel.DataAnnotations.Schema;

namespace Series.Models
{
    [Table("creators")]
    public class Creator
    {
        [Column("creatorid")]
        public int CreatorId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<SeriesCreator> SeriesCreators { get; set; } = new List<SeriesCreator>();
    }
}