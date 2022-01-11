using System.ComponentModel.DataAnnotations;
using WebApiAuthors.Models;

namespace WebApiAutores.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        public string Name { get; set; }

        public IEnumerable<Book> Books { get; set; } 
    }
}
