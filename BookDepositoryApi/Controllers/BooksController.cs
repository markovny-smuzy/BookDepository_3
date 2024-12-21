using Microsoft.AspNetCore.Mvc;
using BookDepositoryApi.Models;

namespace BookDepositoryApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookContext _context;

    public BooksController(BookContext context)
    {
        _context = context;
    }

    // 1. Добавление книги
    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] ConcreteBook book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    // 2. Получение книги по ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound();
        return Ok(book);
    }

    // 3. Поиск книг по названию
    [HttpGet("search/title")]
    public IActionResult FindBooksByTitle([FromQuery] string title)
    {
        var books = _context.Books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!books.Any())
            return NotFound("Книги с таким названием не найдены.");
        return Ok(books);
    }

    // 4. Поиск книг по автору
    [HttpGet("search/author")]
    public IActionResult FindBooksByAuthor([FromQuery] string author)
    {
        var books = _context.Books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!books.Any())
            return NotFound("Книги этого автора не найдены.");
        return Ok(books);
    }

    // 5. Поиск книги по ISBN
    [HttpGet("search/isbn")]
    public IActionResult FindBookByISBN([FromQuery] string isbn)
    {
        var book = _context.Books.FirstOrDefault(b => b.ISBN == isbn);
        if (book == null)
            return NotFound("Книга с таким ISBN не найдена.");
        return Ok(book);
    }

    // 6. Поиск книг по ключевым словам
    [HttpGet("search/keywords")]
    public IActionResult FindBooksByKeywords([FromQuery] string keywords)
    {
        var keywordList = keywords.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(k => k.Trim()).ToList();

        var booksWithKeywords = _context.Books
            .Where(b => keywordList.Any(keyword => 
                b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Annotation.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (!booksWithKeywords.Any())
            return NotFound("Книги с такими ключевыми словами не найдены.");
        return Ok(booksWithKeywords);
    }

    // 7. Удаление книги
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
