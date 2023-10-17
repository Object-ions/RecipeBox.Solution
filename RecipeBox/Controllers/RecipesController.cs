using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeBox.Controllers
{
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;

    public RecipesController(RecipeBoxContext db)
    {
      _db = db;
    }

    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Recipe recipe)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
        return View(recipe);
      }
      else
      {
        _db.Recipes.Add(recipe);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
  }
}