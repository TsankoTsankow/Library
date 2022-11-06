using Library.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AddBookViewModel //: IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        //[Range(typeof(decimal), "0.0", "10.0", ConvertValueInInvariantCulture = true)]
        public decimal Rating { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrEmpty(Title))
        //    {
        //        yield return new ValidationResult("Title cannot be empty");
        //    }
        //    if (string.IsNullOrEmpty(Author))
        //    {
        //        yield return new ValidationResult("Author cannot be empty");
        //    }
        //    if (ConfirmedPassword != Password)
        //    {
        //        yield return new ValidationResult("Passowrds do not match");
        //    }
        //}
    }
}
