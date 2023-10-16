using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Models
{
  public class Tag
    {
        public int TagId { get; set; }
        [Required(ErrorMessage = "Creating a tag requires a name!")]
        public string Name { get; set; }
        public List<RecipeTag> JoinEntities { get;}
    }
}