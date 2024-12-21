using BookDepositoryApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookDepositoryApi.Models
{
    public class DbBookRepository : IBookCatalog
    {
        private readonly BookContext _context;

        public DbBookRepository(BookContext context)
        {
            _context = context;
            _context.Database.EnsureCreated(); // Создаёт базу данных, если она не существует
        }

        public async Task AddBookAsync(ConcreteBook book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<IBook>> FindByTitleAsync(string titleFragment)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(titleFragment, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<IBook>> FindByAuthorAsync(string authorName)
        {
            return await _context.Books
                .Where(b => b.Author.Contains(authorName, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<IBook?> FindByISBNAsync(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<(IBook Book, List<string> KeywordsFound)>> FindByKeywordsAsync(string[] keywords)
        {
            var results = new List<(IBook, List<string>)>();

            foreach (var book in await _context.Books.ToListAsync())
            {
                var keywordsFound = keywords.Where(k => book.ContainsKeyword(k)).ToList();
                if (keywordsFound.Any())
                {
                    results.Add((book, keywordsFound));
                }
            }

            return results.OrderByDescending(r => r.Item2.Count).ToList();
        }

        public async Task UpdateBookAsync(ConcreteBook book)
        {
            _context.Books.Update(book);await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(ConcreteBook book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}