using System.ComponentModel.DataAnnotations;

namespace Watchlist.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Director { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;
        [Required]
        [Range(0, 10.00)]
        public decimal Rating { get; set; }
        [Required]

        public int GenreId { get; set; }
        [Required]

        public string Genre { get; set; }
    }
}
