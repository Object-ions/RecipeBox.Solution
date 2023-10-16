using System.Collections.Generic;

namespace RecipeBox.Models
{
  public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}