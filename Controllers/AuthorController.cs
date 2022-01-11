using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.Data;
using WebApiAutores.Models;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : Controller
    {
        private readonly AppDBContext _db;

        public AuthorController(AppDBContext dBContext)
        {
            _db = dBContext;       
        }

        [HttpGet]
        [Route("/get-authors")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAutorsAsync()
        {
            var authors = await _db.Authors.AsNoTracking().Include(x => x.Books).ToListAsync();
            return authors;
        }

        [HttpGet]
        [Route("/get-author/{authorId}")]
        public async Task<Author> GetAuthorAsync(int authorId)
        { 
            var author = await _db.Authors.FindAsync(authorId);
            return author;
        }

        [HttpPost]
        [Route("/add-author")]
        public async Task<ActionResult> AddAuthorAsync(Author author)
        {
            await _db.Authors.AddAsync(author);
            await _db.SaveChangesAsync();
            _db.Entry(author).State = EntityState.Detached;
            return Ok("Author created.");
        }

        [HttpPut]
        [Route("/update-author/{authorId}")]
        public async Task<ActionResult> UpdateAuthorAsync(Author author, int authorId)
        {
            
            if (author.AuthorId != authorId)
            {
                return BadRequest("Id's are not equal.");
            }

            var result = await _db.Authors.AnyAsync(x => x.AuthorId == authorId);
            if (!result)
            {
                return NotFound("Author doesnt exist.");
            }

            _db.Authors.Update(author);
            _db.Entry(author).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok("Author updated.");
        }

        [HttpDelete]
        [Route("/delete-author/{authorId}")]
        public async Task<ActionResult> DeleteAuthorAsync(int authorId)
        {
            var result = await _db.Authors.FindAsync(authorId);
            if( result == null)
            {
                return NotFound("Author doesn't exist.");
            }
            _db.Authors.Remove(result);
            await _db.SaveChangesAsync();
            return Ok("Author deleted.");
        }
    }
}
