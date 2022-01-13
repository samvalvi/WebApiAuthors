using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Models;
using WebApiAutores.Data;

namespace WebApiAuthors.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : Controller
    {
        private readonly AppDBContext _db;

        public BookController(AppDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("/get-books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksASync()
        {
            var books = await _db.Books.AsNoTracking().ToListAsync();
            return books;
        }

        [HttpGet]
        [Route("/get-book/{bookId:int}")]
        public async Task<ActionResult<Book>> GetBookAsync(int bookId)
        {
            var bookToShow = await _db.Books.AnyAsync(x => x.BookId == bookId);
            if(!bookToShow)
            {
                return NotFound("Book doesn't exist.");
            }
            var result = await _db.Books.Include(x => x.Author).FirstAsync(x => x.BookId == bookId);
            return Ok(result);
        }

        [HttpPost]
        [Route("/add-book")]
        public async Task<ActionResult> AddBookAsync(Book book)
        {
            var authorExist = await _db.Authors.AnyAsync(x => x.AuthorId == book.AuthorId);
            if (!authorExist)
            {
                return NotFound("Author doesn't exist.");
            }

            var bookExist = await _db.Books.AnyAsync(x => x.Title == book.Title);
            if (bookExist)
            {
                return BadRequest($"Book already exist.");
            }

            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();
            _db.Entry(book).State = EntityState.Detached;
            return Ok("Book created");
        }

        [HttpPut]
        [Route("/update-book/{bookId:int}")]
        public async Task<ActionResult> UpdateBookASync(Book book, int bookId)
        {
            if(book.BookId != bookId)
            {
                return BadRequest("Id's are not equal.");
            }

            var bookExist = await _db.Books.AnyAsync(x => x.BookId == bookId);
            if(!bookExist)
            {
                return NotFound("Book doesn't exist.");
            }
            _db.Books.Update(book);
            _db.Entry(book).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok("Book updated.");
        }

        [HttpDelete]
        [Route("/delte-book/{bookId:int}")]
        public async Task<ActionResult> DeleteBookAsync(int bookId)
        {
            var bookToDelete = await _db.Books.FindAsync(bookId);
            if(bookToDelete == null)
            {
                return NotFound("Book doesn't exist.");
            }
            _db.Books.Remove(bookToDelete);
            await _db.SaveChangesAsync();
            return Ok("Book deleted.");
        }
    }
}
