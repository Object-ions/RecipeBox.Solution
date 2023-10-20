using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Author
    {
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Author has to be giving a name")]
        public string Name { get; set; }
        public List<Recipe> Recipes { get; set; }
        public ApplicationUser User { get; set; } 
        public string UserId { get; set; }
    }
}