using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.Data;
using WebApiAutores.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorController : Controller
    {
        private readonly AppDBContext _db;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(AppDBContext dBContext, ILogger<AuthorController> logger)
        {
            _db = dBContext;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 10)]
        [Route("/get-authors")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Author>>> GetAutorsAsync()
        {
            var authors = await _db.Authors.AsNoTracking().Include(x => x.Books).ToListAsync();
            return authors;
        }

        [HttpGet]
        [Route("/get-author/{authorId:int}")]
        public async Task<ActionResult<Author>> GetAuthorAsync(int authorId)
        { 
            var authorExist = await _db.Authors.AnyAsync(x => x.AuthorId == authorId);
            if(!authorExist)
            {
                _logger.LogError("Author id doesn't exist.");
                return BadRequest("Author doesn't exist.");
            }
            var author = await _db.Authors.FindAsync(authorId);
            return author;
        }

        [HttpPost]
        [Route("/add-author")]
        public async Task<ActionResult> AddAuthorAsync(Author author)
        {
            var exist = await _db.Authors.AnyAsync(x => x.Name == author.Name);
            if(exist)
            {
                return BadRequest($"Author {author.Name} doesn't exist.");
            }
            await _db.Authors.AddAsync(author);
            await _db.SaveChangesAsync();
            _db.Entry(author).State = EntityState.Detached;
            return Ok("Author created.");
        }

        [HttpPut]
        [Route("/update-author/{authorId:int}")]
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
        [Route("/delete-author/{authorId:int}")]
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
