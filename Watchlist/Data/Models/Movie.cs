using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watchlist.Data.Models
{
    public class Movie
    {

        public Movie()
        {
            this.UsersMovies = new HashSet<UserMovie>();
        }
        [Key]
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
        [Range(0,10.00)]
        public decimal Rating { get; set; }
        [Required]
       
        public int GenreId { get; set; }
        [Required]
        [ForeignKey(nameof(GenreId))]

        public Genre Genre { get; set; }

        public ICollection<UserMovie> UsersMovies { get; set; }
    }
}
