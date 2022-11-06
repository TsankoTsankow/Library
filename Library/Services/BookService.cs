using Library.Contracts;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;

        public BookService(LibraryDbContext _context)
        {
            context = _context;
        }

        public async Task AddBookAsync(AddBookViewModel model)
        {
            var book = new Book()
            {

                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                Author = model.Author,
                Title = model.Title,
                Description = model.Description,
            };

            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
        }

        public async Task AddBookToCollectionAsyc(int bookId, string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user Id");
            }

            var book = await context.Books.FirstOrDefaultAsync(m => m.Id == bookId);

            if (book == null)
            {
                throw new ArgumentException("Invalid movie Id");
            }

            if (user.ApplicationUsersBooks.Any(aub => aub.BookId == bookId))
            {
                return;
            }

            var curBook = new ApplicationUserBook()
            {
                BookId = book.Id,
                ApplicationUserId = user.Id,
                Book = book,
                ApplicationUser = user
            };

            user.ApplicationUsersBooks.Add(curBook);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookViewModel>> GetAllBooksAsync()
        {
            return await context.Books
                .Select(b => new BookViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Category = b.Category.Name,
                    ImageUrl = b.ImageUrl,
                    Rating = b.Rating,

                }).ToListAsync();

        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<MineBooksViewModel>> GetUsersBooksAsync(string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .ThenInclude(au => au.Book)
                .ThenInclude(au => au.Category)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Ivalid User ID");
            }

            return user.ApplicationUsersBooks.Select(b => new MineBooksViewModel()
            {
                Id = b.BookId,
                Author = b.Book.Author,
                Title = b.Book.Title,
                ImageUrl = b.Book.ImageUrl,
                Description = b.Book.Description,
                Category = b.Book.Category.Name
            });
        }

        public async Task RemoveBookFromCollectionAsync(int bookId, string userId)
        {
            var user = await context.Users
               .Where(u => u.Id == userId)
               .Include(u => u.ApplicationUsersBooks)
               .ThenInclude(au => au.Book)
               .ThenInclude(au => au.Category)
               .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Ivalid User ID");
            }

            var book = user.ApplicationUsersBooks.FirstOrDefault(m => m.BookId == bookId);

            if (book != null)
            {
                user.ApplicationUsersBooks.Remove(book);
                await context.SaveChangesAsync();
            }
        }
    }
}
