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
    [Required(ErrorMessage = "Measurements are required")]
    public string Measure { get; set; }
    [Required(ErrorMessage = "An intro is required")]
    public string Intro { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Your recipe must have an author first! Have you created an author?")]
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public List<RecipeTag> JoinEntities { get; }
  }
}