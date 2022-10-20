using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Watchlist.Models;
using Watchlist.Services;
using Watchlist.Services.Contracts;

namespace Watchlist.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieService service;
        public MoviesController(IMovieService _service)
        {
            this.service = _service;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var movies = await service.AllMoviesAsync();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddMoviesViewModel()
            {
                Genres = await service.GetGenresAsync()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddMoviesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await service.AddMovieAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ups");

                return View(model);
            }
        }
    }
}
