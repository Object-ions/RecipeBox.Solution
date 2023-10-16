using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Recipe
  {
    public int RecipeId { get; set; }
    public string Name { get; set; }
    public int Rate { get; set; }
    public string Ingredient { get; set; }
    public string Instruction { get; set; }
    public string Measure { get; set; }
    public string Intro { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public List<RecipeTag> JoinEntities { get; }
  }
}