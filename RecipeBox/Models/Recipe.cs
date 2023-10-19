using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public int RecipeId { get; set; }
    [Required(ErrorMessage = "Creating a recipe requires a name!")]
    public string Name { get; set; }
    public int Rate { get; set; }
    [Required(ErrorMessage = "Ingredients are required")]
    public string Ingredient { get; set; }
    [Required(ErrorMessage = "Instructions are required")]
    public string Instruction { get; set; }
    [Required(ErrorMessage = "An intro is required")]
    public string Intro { get; set; }
    // Note: this is a condition to add a recipe
    [Range(1, int.MaxValue, ErrorMessage = "error")]
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public List<RecipeTag> JoinEntities { get; }
    public ApplicationUser User { get; set; } 
  }
}