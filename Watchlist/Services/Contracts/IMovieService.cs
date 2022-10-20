using Watchlist.Controllers;
using Watchlist.Data.Models;
using Watchlist.Models;

namespace Watchlist.Services.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieViewModel>> AllMoviesAsync();

        Task<IEnumerable<Genre>> GetGenresAsync();

        Task AddMovieAsync(AddMoviesViewModel viewModel);
    }
}
