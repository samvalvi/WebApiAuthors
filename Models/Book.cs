using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Models;

namespace WebApiAuthors.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "A title is required.")]
        public string Title { get; set; }
        
        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
