using Library.Contracts;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService bookService;

        public BooksController(IBookService _bookService)
        {
            this.bookService = _bookService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await bookService.GetAllBooksAsync();

            return View(model);
        }

        public async Task<IActionResult> Mine()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await bookService.GetUsersBooksAsync(userId);

            return View("Mine", model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddBookViewModel()
            {
                Categories = await bookService.GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await bookService.AddBookAsync(model);

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went terribly wrong...");

                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int bookId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await bookService.AddBookToCollectionAsyc(bookId, userId);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await bookService.RemoveBookFromCollectionAsync(bookId, userId);

            return RedirectToAction(nameof(Mine));
        }
    }
}
