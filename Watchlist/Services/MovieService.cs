using Microsoft.EntityFrameworkCore;
using Watchlist.Controllers;
using Watchlist.Data;
using Watchlist.Data.Models;
using Watchlist.Models;
using Watchlist.Services.Contracts;

namespace Watchlist.Services
{
    public class MovieService : IMovieService
    {
        private readonly WatchlistDbContext context;

        public MovieService(WatchlistDbContext _context)
        {
            this.context = _context;
        }

        public async Task AddMovieAsync(AddMoviesViewModel viewModel)
        {
            var movie = new Movie()
            {
                
                Title = viewModel.Title,
                Director = viewModel.Director,
                GenreId = viewModel.GenreId,
                ImageUrl = viewModel.ImageUrl,
                Rating = viewModel.Rating,

            };
            await context.AddAsync(movie);
            await context.SaveChangesAsync();

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieViewModel>> AllMoviesAsync()
        {
            var movies = await context.Movies.Include(x => x.Genre).ToListAsync();
            

            return movies.Select(x => new MovieViewModel()
            {
                Id = x.Id,
                Director = x.Director,
                Genre = x.Genre.Name,
                ImageUrl = x.ImageUrl,
                Rating = x.Rating,
                Title = x.Title,
            }); 
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            var genres = await context.Genres.ToListAsync();

            return genres;
        }
    }
}
