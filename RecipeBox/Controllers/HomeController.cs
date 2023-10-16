using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBox.Controllers
{
    public class HomeController : Controller
    {
      private readonly RecipeBoxContext _db;

      public HomeController(RecipeBoxContext db)
      {
        _db = db;
      }

      [HttpGet("/")]
      public ActionResult Index()
      {
        Author[] authors = _db.Authors.ToArray();
        Recipe[] recipes = _db.Recipes.ToArray();
        Tag[] tags = _db.Tags.ToArray();
        Dictionary<string, object[]> model = new Dictionary<string, object[]>();
        model.Add("authors", authors);
        model.Add("recipes", recipes);
        model.Add("tags", tags);
        return View(model);
      }

    }
}