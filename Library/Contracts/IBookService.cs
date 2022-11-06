using Library.Data.Models;
using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllBooksAsync();

        Task<IEnumerable<MineBooksViewModel>> GetUsersBooksAsync(string userId);

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task AddBookAsync(AddBookViewModel model);

        Task AddBookToCollectionAsyc(int bookId, string userId);


        Task RemoveBookFromCollectionAsync(int bookId, string userId);
    }
}
