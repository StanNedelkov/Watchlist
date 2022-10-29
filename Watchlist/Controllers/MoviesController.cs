using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist.Models;
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
                ModelState.AddModelError(string.Empty, "Something went wrong!");

                return View(model);
            }
        }

        [HttpPost]

        public async Task<IActionResult> AddToCollection(int movieId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                await service.AddToCollectionAsync(movieId, userId);
            }
            catch (ArgumentNullException ae)
            {
                throw;
            }
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Watched()
        {

            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var movies = await service.WatchedMoviesAsync(userId);
                return View(movies);
            }
            catch (ArgumentNullException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);

                return View();
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int movieId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                await service.RemoveFromCollectionAsync(movieId, userId);
            }
            catch (ArgumentNullException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);
                throw;
            }
            return RedirectToAction(nameof(Watched));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var model = (await service.GetMovieToEdit(Id));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMoviesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await service.EditMovieAsync(model);

            return RedirectToAction(nameof(All));
        }
    }
}
