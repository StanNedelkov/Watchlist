using Microsoft.EntityFrameworkCore;
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
        }

        public async Task AddToCollectionAsync(int movieId, string userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new ArgumentNullException("User not found!");
            }

            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == movieId);
            if (movie == null)
            {
                throw new ArgumentNullException("Movie not found!");
            }

            user.UsersMovies.Add(new UserMovie()
            {
                MovieId = movieId,
                UserId = userId,
                Movie = movie,
                User = user
                
            });

            
            await context.SaveChangesAsync();
           
        }

        public async Task<IEnumerable<MovieViewModel>> AllMoviesAsync()
        {
            var movies = 
                await context
                .Movies
                .Include(x => x.Genre)
                .ToListAsync();
            

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
            =>   await context
                .Genres
                .ToListAsync();

        public async Task RemoveFromCollectionAsync(int movieId, string userId)
        {
            var user = await context.Users.Where(x => x.Id == userId)
               .Include(x => x.UsersMovies)
               .ThenInclude(x => x.Movie)
               .ThenInclude(x => x.Genre)
               .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }
            var movie = user.UsersMovies.FirstOrDefault(x => x.MovieId == movieId);

            if (movie != null)
            {
                user.UsersMovies.Remove(movie);
                await context.SaveChangesAsync();
            }
           
           
        }

        public async Task<IEnumerable<MovieViewModel>> WatchedMoviesAsync(string userId)
        {
           // var movies = await context.Movies.Include(x => x.Genre).Include(x => x.UsersMovies).ToListAsync();
            var users = await context.Users.Where(x => x.Id == userId)
                .Include(x => x.UsersMovies)
                .ThenInclude(x => x.Movie)
                .ThenInclude(x => x.Genre)
                .FirstOrDefaultAsync();
            if (users == null)
            {
                throw new ArgumentNullException("User not found");
            }

            var movies = users.UsersMovies
                .Select(x => new MovieViewModel() 
                {
                    Id = x.MovieId,
                    Title = x.Movie.Title,
                    Director = x.Movie.Director,
                    GenreId = x.Movie.Genre.Id,
                    Genre = x.Movie.Genre.Name,
                    ImageUrl = x.Movie.ImageUrl,
                    Rating = x.Movie.Rating,
                });
            return movies;
           
        }
    }
}
